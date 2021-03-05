using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Profiling.Demo
{
	public partial class StacksForm : Form
	{
		public StacksForm()
		{
			InitializeComponent();

			_profilingManagerControl.SetUpLatency(_latencyComponent);
			_profilingManagerControl.SetUpStackControl(_stackControl);
			_stackControl.SetUpLatency(_latencyComponent);
		}

		private void sleep_Click(object sender, EventArgs e)
		{
			Thread.Sleep(new TimeSpan(sleepTime.Value.Hour, sleepTime.Value.Minute, sleepTime.Value.Second));
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			_latencyComponent.Start();
		}

		protected override void Dispose(bool disposing)
		{
			_latencyComponent?.Stop();
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			_latencyComponent?.Stop();
		}

		private void OnGcPause(object sender, EventArgs e)
		{
			var thread = new Thread(() =>
			{
				Thread.Sleep(100);
				for (int i = 0; i < 4; i++)
				{
					var array = CreateMemoryPressure(0);
					//_list.Add(array);
					for (int j = 0; j < 5; j++)
					{
						GC.Collect(2);
						GC.Collect(1);
						GC.Collect(0);

						GC.Collect();
					}

				}
			});
			thread.Start();
			thread.Join();
		}

		private object CreateMemoryPressure(int level)
		{
			if (level >= 5000)
				return null;

			var array = new List<object>();

			for (int j = 0; j < 5000; j++)
			{
				var obj = j == 1 ? CreateMemoryPressure(level + 1) : new object();

				if (obj != null)
					array.Add(obj);
			}

			return array;
		}
	}
}
