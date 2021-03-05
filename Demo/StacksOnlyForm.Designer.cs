namespace Profiling.Demo
{
	partial class StacksOnlyForm
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
			this.stackControl1 = new Profiling.WindowsForms.StackControl();
			((System.ComponentModel.ISupportInitialize)(this.stackControl1)).BeginInit();
			this.SuspendLayout();
			// 
			// stackControl1
			// 
			this.stackControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stackControl1.Location = new System.Drawing.Point(0, 0);
			this.stackControl1.Name = "stackControl1";
			this.stackControl1.Size = new System.Drawing.Size(1330, 620);
			this.stackControl1.TabIndex = 0;
			// 
			// StacksOnlyForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1330, 620);
			this.Controls.Add(this.stackControl1);
			this.Name = "StacksOnlyForm";
			this.Text = "StacksOnlyForm";
			((System.ComponentModel.ISupportInitialize)(this.stackControl1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private WindowsForms.StackControl stackControl1;
	}
}