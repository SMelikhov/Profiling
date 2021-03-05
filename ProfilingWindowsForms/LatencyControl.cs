using System;
using System.Threading;
using System.Windows.Forms;
using Profiling.Client;
using Profiling.Common;
using Profiling.Common.Sampling;
using Timer = System.Windows.Forms.Timer;

namespace Profiling.WindowsForms
{
	public partial class LatencyControl : UserControl, ILatencyComponent
	{
		private readonly Control _control;
		private IWinFormsLatencyLogger _logger = new WinFormsLatencyLoggerStub();
		private ISettingsManagerProfiling _settingsManagerProfiling = new SettingsManagerProfilingStub();
		private readonly Timer _timer;

		public LatencyControl()
		{
			_control = this;
			_timer = new Timer();
			_timer.Tick += _timer_Tick;
			_timer.Interval = 1000;
			InitializeComponent();
		}

		public void SetUp(ISettingsManagerProfiling settingsManagerProfiling)
		{
			_settingsManagerProfiling = settingsManagerProfiling;
		}

		public IDisposable Postpone()
		{
			return _logger.Postpone();
		}

		public void Reset()
		{
			_logger.Reset();
		}

		public ProfilingStack[] GetStacks()
		{
			return _logger.GetStacks();
		}

		public void CollectStacks(int uiThreshold, int uiLatencySnapshotPeriod, bool collectAllStacks)
		{
			_logger.CollectStacks(uiThreshold, uiLatencySnapshotPeriod, collectAllStacks);
		}

		public void DisposeStacks()
		{
			_logger.DisposeStacks();
		}

		public void Start()
		{
			Stop();
			_logger = new WinFormsLatencyLogger(SynchronizationContext.Current, new ProfilingSettings { CollectLatency = true }, true, Thread.CurrentThread, _settingsManagerProfiling);
			_logger.Start();
			_timer.Start();
		}

		public void Stop()
		{
			_timer.Stop();
			_logger.Dispose();
			_logger = new WinFormsLatencyLoggerStub();
		}

		private void Redraw()
		{
			var str = _logger.Message;

			label1.Text = str;
		}

		void _timer_Tick(object sender, EventArgs e)
		{
			Redraw();
		}
	}
}
