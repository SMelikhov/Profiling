using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Profiling.Common;
using Profiling.Common.Sampling;

namespace Profiling.Demo
{
	public partial class StacksOnlyForm : Form
	{
		public StacksOnlyForm(ProfilingStack[] stacks )
		{
			InitializeComponent();

			stackControl1.RefreshStacks(stacks);
		}
	}
}
