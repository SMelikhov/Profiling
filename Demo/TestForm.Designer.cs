namespace Profiling.Demo
{
	partial class TestForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._stackFormBtn = new System.Windows.Forms.Button();
			this._startBtn = new System.Windows.Forms.Button();
			this._stopBtn = new System.Windows.Forms.Button();
			this._sleepBtn = new System.Windows.Forms.Button();
			this._getBtn = new System.Windows.Forms.Button();
			this._sleepInOtherThread = new System.Windows.Forms.Button();
			this._asyncBtn = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _stackFormBtn
			// 
			this._stackFormBtn.Location = new System.Drawing.Point(29, 38);
			this._stackFormBtn.Name = "_stackFormBtn";
			this._stackFormBtn.Size = new System.Drawing.Size(75, 23);
			this._stackFormBtn.TabIndex = 0;
			this._stackFormBtn.Text = "Stacks Form";
			this._stackFormBtn.UseVisualStyleBackColor = true;
			this._stackFormBtn.Click += new System.EventHandler(this.OnStackForm);
			// 
			// _startBtn
			// 
			this._startBtn.Location = new System.Drawing.Point(167, 38);
			this._startBtn.Name = "_startBtn";
			this._startBtn.Size = new System.Drawing.Size(75, 23);
			this._startBtn.TabIndex = 0;
			this._startBtn.Text = "Start";
			this._startBtn.UseVisualStyleBackColor = true;
			this._startBtn.Click += new System.EventHandler(this.OnStart);
			// 
			// _stopBtn
			// 
			this._stopBtn.Location = new System.Drawing.Point(426, 38);
			this._stopBtn.Name = "_stopBtn";
			this._stopBtn.Size = new System.Drawing.Size(75, 23);
			this._stopBtn.TabIndex = 0;
			this._stopBtn.Text = "Stop";
			this._stopBtn.UseVisualStyleBackColor = true;
			this._stopBtn.Click += new System.EventHandler(this.OnStop);
			// 
			// _sleepBtn
			// 
			this._sleepBtn.Location = new System.Drawing.Point(248, 38);
			this._sleepBtn.Name = "_sleepBtn";
			this._sleepBtn.Size = new System.Drawing.Size(75, 23);
			this._sleepBtn.TabIndex = 0;
			this._sleepBtn.Text = "Sleep";
			this._sleepBtn.UseVisualStyleBackColor = true;
			this._sleepBtn.Click += new System.EventHandler(this.OnSleep);
			// 
			// _getBtn
			// 
			this._getBtn.Location = new System.Drawing.Point(329, 38);
			this._getBtn.Name = "_getBtn";
			this._getBtn.Size = new System.Drawing.Size(75, 23);
			this._getBtn.TabIndex = 0;
			this._getBtn.Text = "Get";
			this._getBtn.UseVisualStyleBackColor = true;
			this._getBtn.Click += new System.EventHandler(this.OnGet);
			// 
			// _sleepInOtherThread
			// 
			this._sleepInOtherThread.Location = new System.Drawing.Point(29, 113);
			this._sleepInOtherThread.Name = "_sleepInOtherThread";
			this._sleepInOtherThread.Size = new System.Drawing.Size(158, 23);
			this._sleepInOtherThread.TabIndex = 27;
			this._sleepInOtherThread.Text = "Sleep in other thread";
			this._sleepInOtherThread.UseVisualStyleBackColor = true;
			this._sleepInOtherThread.Click += new System.EventHandler(this.OnSleepInOtherThread);
			// 
			// _asyncBtn
			// 
			this._asyncBtn.Location = new System.Drawing.Point(207, 113);
			this._asyncBtn.Name = "_asyncBtn";
			this._asyncBtn.Size = new System.Drawing.Size(158, 23);
			this._asyncBtn.TabIndex = 27;
			this._asyncBtn.Text = "Async";
			this._asyncBtn.UseVisualStyleBackColor = true;
			this._asyncBtn.Click += new System.EventHandler(this.OnAsync);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(29, 142);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(158, 23);
			this.button1.TabIndex = 27;
			this.button1.Text = "Sleep in other thread2";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnSleepInOtherThread2);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(329, 67);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 0;
			this.button2.Text = "Get Frozen";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.OnGetFrozen);
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(525, 262);
			this.Controls.Add(this._asyncBtn);
			this.Controls.Add(this.button1);
			this.Controls.Add(this._sleepInOtherThread);
			this.Controls.Add(this.button2);
			this.Controls.Add(this._getBtn);
			this.Controls.Add(this._sleepBtn);
			this.Controls.Add(this._stopBtn);
			this.Controls.Add(this._startBtn);
			this.Controls.Add(this._stackFormBtn);
			this.Name = "TestForm";
			this.Text = "TestForm";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button _stackFormBtn;
		private System.Windows.Forms.Button _startBtn;
		private System.Windows.Forms.Button _stopBtn;
		private System.Windows.Forms.Button _sleepBtn;
		private System.Windows.Forms.Button _getBtn;
		private System.Windows.Forms.Button _sleepInOtherThread;
		private System.Windows.Forms.Button _asyncBtn;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}