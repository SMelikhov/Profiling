using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Profiling.Client;
using Profiling.Common;
using Profiling.Common.Sampling;

namespace Profiling.Demo
{
	public partial class TestForm : Form
	{
		private readonly UILatencyManager _latency;

		public TestForm()
		{
			InitializeComponent();
			_latency = new UILatencyManager(SynchronizationContext.Current, false, new SettingsManagerProfilingStub(), Thread.CurrentThread); //service app lifetime
		}

		private void OnStackForm(object sender, EventArgs e)
		{
			using (var form = new StacksForm())
			{
				form.ShowDialog();
			}
		}

		private void OnStart(object sender, EventArgs e)
		{
			_latency.Start(new ProfilingSettings
			{
				CollectLatency = true,
				StartCollectingBeforeThreshold = true,
				LatencySnapshotPeriod = 20,
				LatencyThreshold = 100,
				ProfilingLevel = ProfilingLevel.ProfilingStacksOnly
			}); //can be started and stopped
		}

		private void OnStop(object sender, EventArgs e)
		{
			_latency.Stop();
		}

		private void OnSleep(object sender, EventArgs e)
		{
			Thread.Sleep(1000);
		}

		private void OnGet(object sender, EventArgs e)
		{
			using (var form = new StacksOnlyForm(_latency.GetStacks()))
			{
				form.ShowDialog();
			}
		}

		private void OnGetFrozen(object sender, EventArgs e)
		{
			var frozen = _latency.GetFrozenAggregatedItem();
			if(frozen == null || frozen.Stacks == null)
				return;

			using (var form = new StacksOnlyForm( frozen.Stacks.Select(c => new ProfilingStack(c)).ToArray()))
			{
				form.ShowDialog();
			}
		}

		private void OnSleepInOtherThread(object sender, EventArgs e)
		{
			using (var tracker = new StackTracker(new Stacks(100, 20,
				true, 10000)))
			{
				var thread = new Thread(() =>
				{
					using (tracker.RegisterThread(Thread.CurrentThread))
					{
						Thread.Sleep(1000);
					}
				});

				thread.Start();
				thread.Join();
				tracker.Parse();

				using (var form = new StacksOnlyForm(tracker.GetStacks()))
				{
					form.ShowDialog();
				}
			}
		}

		private async void OnAsync(object sender, EventArgs e)
		{
			using (var tracker = new StackTracker(new Stacks(100, 20,
				true, 10000)))
			{
				using (tracker.RegisterThread(Thread.CurrentThread))
				{
					var someTask = Task<int>.Factory.StartNew(() => ExecuteAsync(tracker));
					await someTask;
				}

				tracker.Parse();
				using (var form = new StacksOnlyForm(tracker.GetStacks()))
				{
					form.ShowDialog();
				}
			}
		}

		public static int ExecuteAsync(StackTracker tracker)
		{
			using (tracker.RegisterThread(Thread.CurrentThread))
			{
				Thread.Sleep(1000);
			}

			return 0;
		}

		private void OnSleepInOtherThread2(object sender, EventArgs e)
		{
			var thread = new Thread(() =>
			{
				using (_latency.RegisterThread())
				{
					Thread.Sleep(1000000000);
				}
			});

			thread.Start();
		}


	}
}
