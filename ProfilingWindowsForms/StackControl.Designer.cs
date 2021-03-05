using System.Windows.Forms;

namespace Profiling.WindowsForms
{
	partial class StackControl
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
			this.components = new System.ComponentModel.Container();
			this.importBtn = new System.Windows.Forms.Button();
			this.exportBtn = new System.Windows.Forms.Button();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.createNewCallGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeKnownIssueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.simplifyStackCh = new System.Windows.Forms.CheckBox();
			this.latencyDividedCh = new System.Windows.Forms.CheckBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.countLbl = new System.Windows.Forms.Label();
			this._treeView = new System.Windows.Forms.TreeView();
			this.label2 = new System.Windows.Forms.Label();
			this.findText = new System.Windows.Forms.TextBox();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbOrderBy = new System.Windows.Forms.ComboBox();
			this.showOnlyFrozen = new System.Windows.Forms.CheckBox();
			this.filterType = new System.Windows.Forms.ComboBox();
			this.findTags = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.contextMenuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// importBtn
			// 
			this.importBtn.Location = new System.Drawing.Point(1007, 9);
			this.importBtn.Name = "importBtn";
			this.importBtn.Size = new System.Drawing.Size(71, 23);
			this.importBtn.TabIndex = 33;
			this.importBtn.Text = "Import";
			this.importBtn.UseVisualStyleBackColor = true;
			this.importBtn.Click += new System.EventHandler(this.importBtn_Click);
			// 
			// exportBtn
			// 
			this.exportBtn.Location = new System.Drawing.Point(937, 9);
			this.exportBtn.Name = "exportBtn";
			this.exportBtn.Size = new System.Drawing.Size(71, 23);
			this.exportBtn.TabIndex = 34;
			this.exportBtn.Text = "Export";
			this.exportBtn.UseVisualStyleBackColor = true;
			this.exportBtn.Click += new System.EventHandler(this.exportBtn_Click);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewCallGraphToolStripMenuItem,
            this.removeKnownIssueToolStripMenuItem,
            this.copyToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(194, 70);
			// 
			// createNewCallGraphToolStripMenuItem
			// 
			this.createNewCallGraphToolStripMenuItem.Name = "createNewCallGraphToolStripMenuItem";
			this.createNewCallGraphToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.createNewCallGraphToolStripMenuItem.Text = "Create New Call Graph";
			this.createNewCallGraphToolStripMenuItem.Click += new System.EventHandler(this.createNewCallGraphToolStripMenuItem_Click);
			// 
			// removeKnownIssueToolStripMenuItem
			// 
			this.removeKnownIssueToolStripMenuItem.Name = "removeKnownIssueToolStripMenuItem";
			this.removeKnownIssueToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.removeKnownIssueToolStripMenuItem.Text = "Remove Known Issue";
			this.removeKnownIssueToolStripMenuItem.Click += new System.EventHandler(this.removeKnownIssueToolStripMenuItem_Click);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.copyToolStripMenuItem.Text = "Copy to Clipboard";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// simplifyStackCh
			// 
			this.simplifyStackCh.AutoSize = true;
			this.simplifyStackCh.Location = new System.Drawing.Point(10, 12);
			this.simplifyStackCh.Name = "simplifyStackCh";
			this.simplifyStackCh.Size = new System.Drawing.Size(92, 17);
			this.simplifyStackCh.TabIndex = 18;
			this.simplifyStackCh.Text = "Simplify Stack";
			this.simplifyStackCh.UseVisualStyleBackColor = true;
			this.simplifyStackCh.CheckedChanged += new System.EventHandler(this.simplifyStack_CheckedChanged);
			// 
			// latencyDividedCh
			// 
			this.latencyDividedCh.AutoSize = true;
			this.latencyDividedCh.Checked = true;
			this.latencyDividedCh.CheckState = System.Windows.Forms.CheckState.Checked;
			this.latencyDividedCh.Location = new System.Drawing.Point(108, 12);
			this.latencyDividedCh.Name = "latencyDividedCh";
			this.latencyDividedCh.Size = new System.Drawing.Size(169, 17);
			this.latencyDividedCh.TabIndex = 19;
			this.latencyDividedCh.Text = "Show Latency Divided Stacks";
			this.latencyDividedCh.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Location = new System.Drawing.Point(10, 39);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.countLbl);
			this.splitContainer1.Panel1.Controls.Add(this._treeView);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.findText);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
			this.splitContainer1.Panel2.Controls.Add(this.label1);
			this.splitContainer1.Size = new System.Drawing.Size(1068, 545);
			this.splitContainer1.SplitterDistance = 465;
			this.splitContainer1.SplitterWidth = 8;
			this.splitContainer1.TabIndex = 39;
			// 
			// countLbl
			// 
			this.countLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.countLbl.AutoSize = true;
			this.countLbl.Location = new System.Drawing.Point(987, 441);
			this.countLbl.Name = "countLbl";
			this.countLbl.Size = new System.Drawing.Size(0, 13);
			this.countLbl.TabIndex = 43;
			this.countLbl.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// _treeView
			// 
			this._treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._treeView.ContextMenuStrip = this.contextMenuStrip1;
			this._treeView.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._treeView.Location = new System.Drawing.Point(3, 3);
			this._treeView.Name = "_treeView";
			this._treeView.Size = new System.Drawing.Size(1062, 427);
			this._treeView.TabIndex = 18;
			this._treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._treeView_AfterSelect);
			this._treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._treeView_NodeMouseClick);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 439);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(124, 13);
			this.label2.TabIndex = 42;
			this.label2.Text = "Find methods in call tree:";
			// 
			// findText
			// 
			this.findText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.findText.Location = new System.Drawing.Point(128, 436);
			this.findText.Name = "findText";
			this.findText.Size = new System.Drawing.Size(440, 20);
			this.findText.TabIndex = 39;
			this.findText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.findText_KeyDown);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBox1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.richTextBox1.Location = new System.Drawing.Point(3, 3);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(1062, 8);
			this.richTextBox1.TabIndex = 41;
			this.richTextBox1.Text = "";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(-113, -158);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(119, 13);
			this.label1.TabIndex = 40;
			this.label1.Text = "Find method in call tree:";
			// 
			// cmbOrderBy
			// 
			this.cmbOrderBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbOrderBy.FormattingEnabled = true;
			this.cmbOrderBy.Location = new System.Drawing.Point(283, 11);
			this.cmbOrderBy.Name = "cmbOrderBy";
			this.cmbOrderBy.Size = new System.Drawing.Size(121, 21);
			this.cmbOrderBy.TabIndex = 43;
			// 
			// showOnlyFrozen
			// 
			this.showOnlyFrozen.AutoSize = true;
			this.showOnlyFrozen.Location = new System.Drawing.Point(416, 13);
			this.showOnlyFrozen.Name = "showOnlyFrozen";
			this.showOnlyFrozen.Size = new System.Drawing.Size(82, 17);
			this.showOnlyFrozen.TabIndex = 40;
			this.showOnlyFrozen.Text = "Only Frozen";
			this.showOnlyFrozen.UseVisualStyleBackColor = true;
			this.showOnlyFrozen.CheckedChanged += new System.EventHandler(this.showOnlyFrozen_CheckedChanged);
			// 
			// filterType
			// 
			this.filterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.filterType.FormattingEnabled = true;
			this.filterType.Location = new System.Drawing.Point(504, 10);
			this.filterType.Name = "filterType";
			this.filterType.Size = new System.Drawing.Size(121, 21);
			this.filterType.TabIndex = 43;
			// 
			// findTags
			// 
			this.findTags.Location = new System.Drawing.Point(690, 11);
			this.findTags.Name = "findTags";
			this.findTags.Size = new System.Drawing.Size(242, 20);
			this.findTags.TabIndex = 44;
			this.findTags.KeyDown += new System.Windows.Forms.KeyEventHandler(this.findTags_KeyDown);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(631, 14);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(57, 13);
			this.label3.TabIndex = 44;
			this.label3.Text = "Find Tags:";
			// 
			// StackControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label3);
			this.Controls.Add(this.findTags);
			this.Controls.Add(this.filterType);
			this.Controls.Add(this.cmbOrderBy);
			this.Controls.Add(this.showOnlyFrozen);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.latencyDividedCh);
			this.Controls.Add(this.importBtn);
			this.Controls.Add(this.simplifyStackCh);
			this.Controls.Add(this.exportBtn);
			this.Name = "StackControl";
			this.Size = new System.Drawing.Size(1088, 587);
			this.contextMenuStrip1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button importBtn;
		private System.Windows.Forms.Button exportBtn;
		private System.Windows.Forms.CheckBox simplifyStackCh;
		protected System.Windows.Forms.CheckBox latencyDividedCh;
		protected System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem createNewCallGraphToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer1;
		protected System.Windows.Forms.TreeView _treeView;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		protected System.Windows.Forms.TextBox findText;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Label countLbl;
		private System.Windows.Forms.ComboBox cmbOrderBy;
		private System.Windows.Forms.CheckBox showOnlyFrozen;
		private System.Windows.Forms.ComboBox filterType;
		private System.Windows.Forms.ToolStripMenuItem removeKnownIssueToolStripMenuItem;
		private ToolStripMenuItem copyToolStripMenuItem;
		protected TextBox findTags;
		private Label label3;
	}
}
