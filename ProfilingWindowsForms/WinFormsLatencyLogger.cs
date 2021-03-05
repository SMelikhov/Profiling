using System;
using System.Threading;
using System.Windows.Forms;
using Profiling.Common;
using Profiling.Common.DTO;
using Profiling.Common.Sampling;
using Profiling.Common.Util;

namespace Profiling.WindowsForms
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

		private readonly Control _parentControl;
		private readonly bool _isBookmark;
		private readonly Thread _uiThread;
		private readonly TimeSpan _samplingInterval;
		private volatile IStackTracker _tracker = new StackTrackerStub();
		private readonly DelayExecuter _delayExecuter;
		private DateTime _start;
		private ProfilingSettings _lastSettings;

		public WinFormsLatencyLogger(Control control, ProfilingSettings settings, bool isBookmark, Thread uiThread, ISettingsManagerProfiling settingsManager)
		{
			_parentControl = control;
			_isBookmark = isBookmark;
			_uiThread = uiThread;
			_settingsManager = settingsManager;
			_settingsManager.SettingChanged += SettingsManagerOnSettingChanged;
			_samplingInterval = TimeSpan.FromMilliseconds(10);
			_delayExecuter = new DelayExecuter(_samplingInterval, StartProbe);
			ReloadTracker(settings);
		}

		public void Start()
		{
			if (_isDisposed)
				throw new ObjectDisposedException("WinFormsLatencyLogger");

			_delayExecuter.StartOnce();
			_start = DateTime.Now;
		}

		public void Reset()
		{
			_max = _avg = _count = _elapsed = 0;
			_interval.Clear();
			_min = int.MaxValue;
			_start = DateTime.Now;
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
				var end = DateTime.Now;

				return string.Format("Elapsed Time: {2} [Max: {0}  |  Avg: {1}ms] ",
														 Max.ToString("N0"),
														 Avg.ToString("N0"),
														 (end - _start).ToString(@"mm\:ss"))
							 + _interval.GetAboveMessageItem();
			}
		}

		public LatencyIntervalDTO GetLatencyInterval()
		{
			return _interval.GetDto();
		}

		public ProfilingStack[] GetStacks()
		{
			return _tracker.GetStacks();
		}

		public bool TryGetFrozenStack(out ProfilingStack stack)
		{
			return _tracker.TryGetFrozenLogEntry(_uiThread, out stack);
		}

		public bool TryGetStacksForAllThreads(out ProfilingStack stack)
		{
			return _tracker.TryGetStacksForAllThreads(out stack);
		}

		private void StartProbe()
		{
			if (_isDisposed)
				return;

			var entry = _tracker.CreateLogEntry(_uiThread);
			try
			{
				_parentControl.BeginInvoke(new Action<ILogEntry>(EndProbe), new object[] { entry });
			}
			catch (Exception)
			{
				_tracker.RemoveLogEntry(_uiThread);
				_delayExecuter.StartOnce();
			}

		}

		private void EndProbe(ILogEntry entry)
		{
			if (_isDisposed)
				return;

			try
			{
				entry.Stop();
				_tracker.RemoveLogEntry(_uiThread);

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

			if (Equals(_settingsManager.ProfilingModeSettings, _lastSettings))
				return;

			ReloadTracker(ProfilingSettings.CreateInstance(_settingsManager.ProfilingModeSettings));
		}

		private void ReloadTracker(ProfilingSettings settings)
		{
			if (settings.CollectLatency && _tracker.IsFake)
				AssignNewTraker(new StackTracker(new Common.Sampling.Stacks(settings.LatencyThreshold, settings.LatencySnapshotPeriod, settings.StartCollectingBeforeThreshold, settings.UIFreezeThreshold)));
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
