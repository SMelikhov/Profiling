using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Profiling.WindowsForms
{
	public partial class ProfilingManagerControl : UserControl
	{
		public ProfilingManagerControl()
		{
			InitializeComponent();
		}

		public ProfilingManagerControl(StackControl stackControl, LatencyControl latencyControl) : this()
		{
			_stackControl = stackControl;
			_latencyControl = latencyControl;
		}

		public void SetUpLatency(LatencyControl latencyControl)
		{
			_latencyControl = latencyControl;
		}

		public void SetUpStackControl(StackControl stackControl)
		{
			_stackControl = stackControl;
		}

		private StackControl _stackControl;
		private LatencyControl _latencyControl;

		private void collectStacks_Click(object sender, EventArgs e)
		{
			_btcCollectStacks.Enabled = false;
			_latencyControl.CollectStacks((int)uiThreshold.Value, (int)period.Value, collactAllStacks.Checked);
		}
		private void collectStacksCancel_Click(object sender, EventArgs e)
		{
			_latencyControl.DisposeStacks();
			_btcCollectStacks.Enabled = true;
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			_btnRefresh.Enabled = false;
			_stackControl.RefreshStacks(_latencyControl.GetStacks());
			_btnRefresh.Enabled = true;
		}
	}
}
