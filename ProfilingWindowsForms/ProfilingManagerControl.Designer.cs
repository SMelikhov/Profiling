namespace Profiling.WindowsForms
{
	partial class ProfilingManagerControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._btnRefresh = new System.Windows.Forms.Button();
			this.collactAllStacks = new System.Windows.Forms.CheckBox();
			this.period = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.uiThreshold = new System.Windows.Forms.NumericUpDown();
			this._btnStacksCancel = new System.Windows.Forms.Button();
			this._btcCollectStacks = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.period)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiThreshold)).BeginInit();
			this.SuspendLayout();
			// 
			// _btnRefresh
			// 
			this._btnRefresh.Location = new System.Drawing.Point(567, 3);
			this._btnRefresh.Name = "_btnRefresh";
			this._btnRefresh.Size = new System.Drawing.Size(75, 23);
			this._btnRefresh.TabIndex = 39;
			this._btnRefresh.Text = "Refresh";
			this._btnRefresh.UseVisualStyleBackColor = true;
			this._btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// collactAllStacks
			// 
			this.collactAllStacks.AutoSize = true;
			this.collactAllStacks.Checked = true;
			this.collactAllStacks.CheckState = System.Windows.Forms.CheckState.Checked;
			this.collactAllStacks.Location = new System.Drawing.Point(453, 8);
			this.collactAllStacks.Name = "collactAllStacks";
			this.collactAllStacks.Size = new System.Drawing.Size(108, 17);
			this.collactAllStacks.TabIndex = 46;
			this.collactAllStacks.Text = "Collect All Stacks";
			this.collactAllStacks.UseVisualStyleBackColor = true;
			// 
			// period
			// 
			this.period.Location = new System.Drawing.Point(387, 6);
			this.period.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
			this.period.Name = "period";
			this.period.Size = new System.Drawing.Size(59, 20);
			this.period.TabIndex = 45;
			this.period.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(345, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 13);
			this.label4.TabIndex = 43;
			this.label4.Text = "Period:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(182, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 13);
			this.label3.TabIndex = 44;
			this.label3.Text = "UI Latency Threshold:";
			// 
			// uiThreshold
			// 
			this.uiThreshold.Location = new System.Drawing.Point(294, 6);
			this.uiThreshold.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.uiThreshold.Name = "uiThreshold";
			this.uiThreshold.Size = new System.Drawing.Size(47, 20);
			this.uiThreshold.TabIndex = 42;
			this.uiThreshold.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
			// 
			// _btnStacksCancel
			// 
			this._btnStacksCancel.Location = new System.Drawing.Point(101, 3);
			this._btnStacksCancel.Name = "_btnStacksCancel";
			this._btnStacksCancel.Size = new System.Drawing.Size(75, 23);
			this._btnStacksCancel.TabIndex = 41;
			this._btnStacksCancel.Text = "Cancel";
			this._btnStacksCancel.UseVisualStyleBackColor = true;
			this._btnStacksCancel.Click += new System.EventHandler(this.collectStacksCancel_Click);
			// 
			// _btcCollectStacks
			// 
			this._btcCollectStacks.Location = new System.Drawing.Point(3, 3);
			this._btcCollectStacks.Name = "_btcCollectStacks";
			this._btcCollectStacks.Size = new System.Drawing.Size(95, 23);
			this._btcCollectStacks.TabIndex = 40;
			this._btcCollectStacks.Text = "Collect Stacks";
			this._btcCollectStacks.UseVisualStyleBackColor = true;
			this._btcCollectStacks.Click += new System.EventHandler(this.collectStacks_Click);
			// 
			// ProfilingManagerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._btnRefresh);
			this.Controls.Add(this.collactAllStacks);
			this.Controls.Add(this.period);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.uiThreshold);
			this.Controls.Add(this._btnStacksCancel);
			this.Controls.Add(this._btcCollectStacks);
			this.MinimumSize = new System.Drawing.Size(660, 31);
			this.Name = "ProfilingManagerControl";
			this.Size = new System.Drawing.Size(660, 31);
			((System.ComponentModel.ISupportInitialize)(this.period)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiThreshold)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _btnRefresh;
		private System.Windows.Forms.CheckBox collactAllStacks;
		private System.Windows.Forms.NumericUpDown period;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown uiThreshold;
		private System.Windows.Forms.Button _btnStacksCancel;
		private System.Windows.Forms.Button _btcCollectStacks;
	}
}
