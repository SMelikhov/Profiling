using Profiling.WindowsForms;

namespace Profiling.Demo
{
	partial class StacksForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;



		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this._stackControl = new Profiling.WindowsForms.StackControl();
			this._profilingManagerControl = new Profiling.WindowsForms.ProfilingManagerControl();
			this._latencyComponent = new Profiling.WindowsForms.LatencyControl();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.sleepTime = new System.Windows.Forms.DateTimePicker();
			this._gcPause = new System.Windows.Forms.Button();
			this.sleep = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._stackControl)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1241, 441);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this._stackControl);
			this.tabPage1.Controls.Add(this._profilingManagerControl);
			this.tabPage1.Controls.Add(this._latencyComponent);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1233, 415);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Stacks";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// _stackControl
			// 
			this._stackControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._stackControl.Location = new System.Drawing.Point(3, 61);
			this._stackControl.Name = "_stackControl";
			this._stackControl.Size = new System.Drawing.Size(1227, 351);
			this._stackControl.TabIndex = 0;
			// 
			// _profilingManagerControl
			// 
			this._profilingManagerControl.Dock = System.Windows.Forms.DockStyle.Top;
			this._profilingManagerControl.Location = new System.Drawing.Point(3, 30);
			this._profilingManagerControl.MinimumSize = new System.Drawing.Size(660, 31);
			this._profilingManagerControl.Name = "_profilingManagerControl";
			this._profilingManagerControl.Size = new System.Drawing.Size(1227, 31);
			this._profilingManagerControl.TabIndex = 7;
			// 
			// _latencyComponent
			// 
			this._latencyComponent.Dock = System.Windows.Forms.DockStyle.Top;
			this._latencyComponent.Location = new System.Drawing.Point(3, 3);
			this._latencyComponent.Name = "_latencyComponent";
			this._latencyComponent.Size = new System.Drawing.Size(1227, 27);
			this._latencyComponent.TabIndex = 6;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.sleepTime);
			this.tabPage2.Controls.Add(this._gcPause);
			this.tabPage2.Controls.Add(this.sleep);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(1233, 415);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Test Page";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// sleepTime
			// 
			this.sleepTime.CustomFormat = "HH:mm:ss";
			this.sleepTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.sleepTime.Location = new System.Drawing.Point(104, 28);
			this.sleepTime.Name = "sleepTime";
			this.sleepTime.Size = new System.Drawing.Size(85, 20);
			this.sleepTime.TabIndex = 27;
			this.sleepTime.Value = new System.DateTime(2016, 5, 4, 0, 0, 1, 0);
			// 
			// button3
			// 
			this._gcPause.Location = new System.Drawing.Point(18, 84);
			this._gcPause.Name = "_gcPause";
			this._gcPause.Size = new System.Drawing.Size(75, 23);
			this._gcPause.TabIndex = 26;
			this._gcPause.Text = "GC Pause";
			this._gcPause.UseVisualStyleBackColor = true;
			this._gcPause.Click += new System.EventHandler(this.OnGcPause);
			// 
			// sleep
			// 
			this.sleep.Location = new System.Drawing.Point(18, 25);
			this.sleep.Name = "sleep";
			this.sleep.Size = new System.Drawing.Size(75, 23);
			this.sleep.TabIndex = 26;
			this.sleep.Text = "Sleep";
			this.sleep.UseVisualStyleBackColor = true;
			this.sleep.Click += new System.EventHandler(this.sleep_Click);
			// 
			// StacksForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1241, 441);
			this.Controls.Add(this.tabControl1);
			this.Name = "StacksForm";
			this.Text = "Form1";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._stackControl)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private LatencyControl _latencyComponent;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private StackControl _stackControl;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.DateTimePicker sleepTime;
		private System.Windows.Forms.Button sleep;
		private ProfilingManagerControl _profilingManagerControl;
		private System.Windows.Forms.Button _gcPause;
	}
}

