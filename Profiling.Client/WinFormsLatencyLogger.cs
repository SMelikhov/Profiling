using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Profiling.Common;
using Profiling.Common.DTO;
using Profiling.Common.Sampling;
using Profiling.Common.Tracing;
using Profiling.Common.Util;

namespace Profiling.Client
{
	public sealed class WinFormsLatencyLogger : IWinFormsLatencyLogger
	{
		private volatile bool _isDisposed;
		private readonly LatencyInterval _interval = new LatencyInterval();

		private volatile int _max;
		private volatile int _avg;
		private volatile int _min = int.MaxValue;
		private volatile int _count;
		private volatile int _elapsed;
		private readonly ISettingsManagerProfiling _settingsManager;

		private readonly SynchronizationContext _context;
		private readonly bool _isBookmark;
		private readonly Thread _uiThread;
		private volatile IStackTracker _tracker = new StackTrackerStub();
		private readonly DelayExecuter _delayExecuter;
		private DateTime _start;
		private ProfilingSettings _lastSettings;
		private readonly IProfilingSpecific _profilingSpecific = ProfilingSpecificProvider.GetProfilingSpecific();
		private readonly ConcurrentDictionary<string, string> _tags = new ConcurrentDictionary<string, string>();
		private volatile List<string> _listTags = new List<string>();

		public WinFormsLatencyLogger(SynchronizationContext context, ProfilingSettings settings, bool isBookmark, Thread uiThread, ISettingsManagerProfiling settingsManager)
		{
			_context = context;
			_isBookmark = isBookmark;
			_uiThread = uiThread;
			_settingsManager = settingsManager;
			_settingsManager.SettingChanged += SettingsManagerOnSettingChanged;
			var samplingInterval = TimeSpan.FromMilliseconds(10);
			_delayExecuter = new DelayExecuter(samplingInterval, StartProbe);
			ReloadTracker(settings);
		}

		public void Start()
		{
			if (_isDisposed)
				throw new ObjectDisposedException("WinFormsLatencyLogger");

			_delayExecuter.StartOnce();
			_start = DateTime.UtcNow;
		}

		public void Reset()
		{
			_max = _avg = _count = _elapsed = 0;
			_interval.Clear();
			_min = int.MaxValue;
			_start = DateTime.UtcNow;
		}

		public void DisposeStacks()
		{
			AssignNewTraker(new StackTrackerStub());
		}

		public void CollectStacks(int uiThreshold, int uiLatencySnapshotPeriod, bool collectAllStacks)
		{
			AssignNewTraker(new StackTracker(new Stacks(uiThreshold, uiLatencySnapshotPeriod, collectAllStacks, ProfilingSettings.UIFreezeThresholdDefault)));
		}

		public void CopyStacks(IStackTracker stackTracker)
		{
			_tracker.CopyStacks(stackTracker);
		}

		public IDisposable Postpone()
		{
			return _tracker.Postpone();
		}

		public void Dispose()
		{
			_isDisposed = true;

			DisposeStacks();
			_delayExecuter.Dispose();
			_settingsManager.SettingChanged -= SettingsManagerOnSettingChanged;
		}

		public void AddTag(string tag)
		{
			if (!tag.StartsWith("UI", StringComparison.OrdinalIgnoreCase))
				return;

			var tagEntries = tag.Split(new[] {"="}, StringSplitOptions.RemoveEmptyEntries);
			if (tagEntries.Length == 2)
			{
				_tags[tagEntries[0]] = tag;
				_listTags = _tags.Select(c => c.Value).ToList();
			}
		}

		public int Max
		{
			get { return _max; }
		}

		public int Min
		{
			get { return _min; }
		}

		public int Avg
		{
			get { return _avg; }
		}

		public int Count
		{
			get { return _count; }
		}

		public int Elapsed
		{
			get { return _elapsed; }
		}

		public string Message
		{
			get
			{
				var end = DateTime.UtcNow;

				return string.Format("Elapsed Time: {0} [Max: {1}ms  |  Avg: {2}ms  |  Session Elapsed Time: {3}] ",
					Elapsed.ToString("N0"),
														 Max.ToString("N0"),
														 Avg.ToString("N0"),
														(end - _start).ToString(@"dd\:hh\:mm\:ss"))
							 + _interval.GetAboveMessageItem();
			}
		}

		public LatencyIntervalDTO GetLatencyInterval()
		{
			return _interval.GetDto();
		}

		public List<Tuple<DateTime, int>> GetLatencyThresholds()
		{
			return _interval.GetLatencyThresholds();
		}

		public ProfilingStack[] GetStacks()
		{
			return _tracker.GetStacks();
		}

		public bool TryGetFrozenStack(out ProfilingStack stack)
		{
			return _tracker.TryGetFrozenLogEntry(_uiThread, out stack);
		}

		private void StartProbe()
		{
			if (_isDisposed)
				return;

			var entry = _tracker.CreateLogEntry(_uiThread);
			try
			{
				_context.Post(EndProbe, entry);
			}
			catch (Exception)
			{
				_tracker.RemoveLogEntry(_uiThread);
				_delayExecuter.StartOnce();
			}
		}

		private void EndProbe(object obj)
		{
			var entry = (ILogEntry)obj;
			if (_isDisposed)
				return;

			try
			{
				entry.Stop();
				_tracker.RemoveLogEntry(_uiThread, _listTags);

				_count++;

				var elapsed = (int)entry.ElapsedMilliseconds;

				if (elapsed > _max)
					_max = elapsed;

				if (elapsed < _min)
					_min = elapsed;

				_elapsed += elapsed;

				_avg = _elapsed / CountNotZero;

				_interval.Increment(elapsed);
			}
			finally
			{
				_delayExecuter.StartOnce();
			}
		}

		private int CountNotZero
		{
			get { return _count == 0 ? 1 : _count; }
		}

		private void SettingsManagerOnSettingChanged(object sender, EventArgs e)
		{
			if (_isBookmark || _isDisposed)
				return;

			var newSettings = ProfilingSettings.CreateInstance(_settingsManager.ProfilingModeSettings);
			if (Equals(newSettings, _lastSettings))
				return;

			ReloadTracker(newSettings);
		}

		private void ReloadTracker(ProfilingSettings settings)
		{
			if (settings.CollectLatency && _tracker.IsFake && _profilingSpecific.CanCollectStacks(settings))
				AssignNewTraker(new StackTracker(new Stacks(settings.LatencyThreshold, settings.LatencySnapshotPeriod, settings.StartCollectingBeforeThreshold, settings.UIFreezeThreshold)));
			else
				AssignNewTraker(new StackTrackerStub());

			_lastSettings = new ProfilingSettings(settings);
		}

		private void AssignNewTraker(IStackTracker tracker)
		{
			_tracker?.Dispose();
			_tracker = tracker;
		}
	}
}
