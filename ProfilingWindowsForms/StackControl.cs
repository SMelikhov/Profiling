using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Profiling.Client;
using Profiling.Common;
using Profiling.Common.DTO;
using Profiling.Common.Sampling;

namespace Profiling.WindowsForms
{
	public partial class StackControl : UserControl, ISupportInitialize
	{
		private ProfilingStack[] _stacks = new ProfilingStack[0];

		private ILatencyComponent _latencyComponent = new LatencyComponentStub();
		protected readonly List<string> NotIssueExceptions = new List<string>();
		protected string Tags = null;
		private const string TagsOnly = "Tags Only";
		public StackControl()
		{
			InitializeComponent();

			cmbOrderBy.Items.Add(new OrderByComboboxItem { Text = "Total Time", Value = OrderBy.TotalTime });
			cmbOrderBy.Items.Add(new OrderByComboboxItem { Text = "Create Date", Value = OrderBy.Date });

			filterType.Items.Add(new FilterTypeComboboxItem { Text = "Show All", Value = ProfilingStackFilter.FilterType.None });
			filterType.Items.Add(new FilterTypeComboboxItem { Text = "Event Only", Value = ProfilingStackFilter.FilterType.Event });
			filterType.Items.Add(new FilterTypeComboboxItem { Text = "Except Known Delays", Value = ProfilingStackFilter.FilterType.ExceptKnownDelays });
			filterType.Items.Add(new FilterTypeComboboxItem { Text = TagsOnly, Value = ProfilingStackFilter.FilterType.TagsOnly });
			filterType.Items.Add(new FilterTypeComboboxItem { Text = "Likely Problems", Value = ProfilingStackFilter.FilterType.PotentialProblems });

			cmbOrderBy.SelectedIndex = 0;
			filterType.SelectedIndex = 0;

			this.cmbOrderBy.SelectedIndexChanged += new System.EventHandler(this.cmbOrderBy_SelectedIndexChanged);
			this.filterType.SelectedIndexChanged += new System.EventHandler(this.filterType_SelectedIndexChanged);
			this.latencyDividedCh.CheckedChanged += new System.EventHandler(this.latencyDiveded_CheckedChanged);
		}

		public void SetUpLatency(ILatencyComponent component)
		{
			_latencyComponent = component;
		}

		public void RefreshStacks(ProfilingStack[] stacks, bool? latencyDivided = null)
		{
			if (latencyDivided.HasValue)
				latencyDividedCh.Checked = latencyDivided.Value;

			using (_latencyComponent.Postpone())
			using (Dim())
			{
				_stacks = stacks;
				BuildTree();
			}
		}

		public void BuildTree()
		{
			if (filterType.SelectedItem == null || cmbOrderBy.SelectedItem == null)
				return;

			BuildTree(
				new FilterContext(simplifyStackCh.Checked,
					latencyDividedCh.Checked,
					((FilterTypeComboboxItem)filterType.SelectedItem).Value,
					showOnlyFrozen.Checked, ((OrderByComboboxItem)cmbOrderBy.SelectedItem).Value));
		}

		private void BuildTree(FilterContext filterContext)
		{
			_treeView.Nodes.Clear();
			var filtered = _stacks;

			if (filterContext.FilterType != ProfilingStackFilter.FilterType.None)
			{
				var filter = new ProfilingStackFilter(filtered, NotIssueExceptions, Tags);
				filtered = filter.GetFiltered(filterContext.FilterType);
			}

			if (filterContext.OnlyFrozen)
			{
				filtered = filtered.Where(c => c.StackType == StackType.Frozen).ToArray();
			}

			if (!filterContext.LatencyDiveded)
			{
				var merger =
						new ProfilingStackMerger(filtered.Where(c => c.StackType == StackType.Normal)
							.SelectMany(c => c.Stacks)
							.ToArray(), filterContext.SimplifyStack);

				foreach (var stack in merger.GetRoots())
				{
					BuildRootNode(new[] { new StackItem(stack, null, StackType.Normal, null) }, merger.SimplifyStack);
				}
			}
			else
			{
				var items = filtered.SelectMany(c => c.Stacks.Select(x => new StackItem(x, c.CreateDate.ToLocalTime(), c.StackType, c)));

				if (filterContext.OrderBy == OrderBy.TotalTime)
					items = items.OrderByDescending(c => c.TotalTime);
				else
					items = items.OrderByDescending(c => c.CreateDate);

				BuildRootNode(items.ToArray(), filterContext.SimplifyStack);
			}

			SetUpCount();

			if(_treeView.Nodes.Count == 1)
				_treeView.Nodes[0].Expand();

			if (filterContext.FilterType == ProfilingStackFilter.FilterType.Event || filterContext.FilterType == ProfilingStackFilter.FilterType.PotentialProblems)
			{
				HighlightFilteredNodes(_treeView.Nodes);
			}

			if (filtered.Any() && filtered[0].StackType == StackType.Compare)
			{
				HighlightCompareNodes(_treeView.Nodes, true);
			}
		}

		public void SetUpCount()
		{
			countLbl.Text = string.Format("Count = {0}", _treeView.Nodes.Count);
		}

		private void BuildRootNode(StackItem[] stacks, bool simplifyStack)
		{
			_treeView.Nodes.AddRange(stacks.Select(root =>
			{
				var rootNode = CreateNode(root.Item, root.CreateDate, root);
				AddChildNode(root.Item, rootNode, simplifyStack, root);
				return rootNode;
			}).ToArray());

			Application.DoEvents();
		}

		private void AddChildNode(ProfilingStackItem parent, TreeNode parentNode, bool simplifyStack, StackItem root)
		{
			if (simplifyStack && parent.Children.Count == 1 && parent.Children.First().Children.Count > 0)
			{
				AddChildNode(parent.Children.First(), parentNode, true, root);
			}
			else
			{
				foreach (var child in parent.Children)
				{
					var childNode = CreateNode(child, null, root);
					parentNode.Nodes.Add(childNode);
					AddChildNode(child, childNode, simplifyStack, root);	
				}
			}
		}

		private TreeNode CreateNode(ProfilingStackItem item, DateTime? createDate, StackItem root)
		{
			var details = string.Format("[{0} Stack count] {1}{2}{3}{4}{5}",
				item.HitCount,
				item.Filtered ? "[#Filtered#]" : string.Empty,
				item.DifferencePercent != 0 ? "[#Compare#]" : string.Empty,
				item.Stack,
				Environment.NewLine,
				root.Root?.AdditionalInfo ?? string.Empty);

			var stacks = item.Stack.Split(new[] { " in " }, StringSplitOptions.None);

			var name = string.Format("[{0}% : {1} ms : {2}{3}{4}] {5}",
				item.Persent,
				item.TotalTime,
				TimeSpan.FromMilliseconds(item.TotalTime),
				(createDate.HasValue ? " : " + createDate.Value.ToString("dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture) : ""),
				(root.StackType == StackType.Compare ? string.Format(" : Difference {0}% : Old elapsed {1} ms", Math.Round(item.DifferencePercent, 2), item.OldTotalTime) : ""),
				stacks[0]

				);

			var node = new TreeNode(name);
			node.Tag = details;
			if (root.StackType == StackType.Frozen)
			{
				node.ForeColor = Color.Red;
			}
			else if (root.StackType == StackType.Compare)
			{
				if (item.DifferencePercent > 0)
				{
					node.ForeColor = Color.Green;
				}
				else if (item.DifferencePercent < 0)
				{
					node.ForeColor = Color.Red;
				}
			}
			else if (root.StackType == StackType.AllStacks)
			{
				node.ForeColor = Color.DarkSlateBlue;
			}

			return node;
		}

		private void simplifyStack_CheckedChanged(object sender, EventArgs e)
		{
			RebuildTree();
		}

		private void latencyDiveded_CheckedChanged(object sender, EventArgs e)
		{
			RebuildTree();
		}

		private void cmbOrderBy_SelectedIndexChanged(object sender, EventArgs e)
		{
			RebuildTree();
		}

		private void showOnlyFrozen_CheckedChanged(object sender, EventArgs e)
		{
			RebuildTree();
		}

		private void filterType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((FilterTypeComboboxItem) filterType.SelectedItem).Value != ProfilingStackFilter.FilterType.TagsOnly)
			{
				RebuildTree();
			}
		}

		private void RebuildTree()
		{
			using (_latencyComponent.Postpone())
			using (Dim())
			{
				BuildTree();
			}
		}

		private void exportBtn_Click(object sender, EventArgs e)
		{
			using (_latencyComponent.Postpone())
			{
				var latency = new ProfilingLatencyDTO
				{
					Stacks = _stacks.Select(c => c.ToDto()).ToArray()
				};

				using (var dialog = new SaveFileDialog())
				{
					dialog.Filter = "Oms files (*.oms)|*.oms|All files (*.*)|*.*";
					if (dialog.ShowDialog() == DialogResult.OK)
					{
						var formatter = new XmlSerializer(typeof(ProfilingLatencyDTO));
						using (var stream = dialog.OpenFile())
						{
							formatter.Serialize(stream, latency);
						}
					}
				}
			}
		}

		private void importBtn_Click(object sender, EventArgs e)
		{
			using (_latencyComponent.Postpone())
			{
				using (var dialog = new OpenFileDialog())
				{
					dialog.Filter = "Oms files (*.oms)|*.oms|All files (*.*)|*.*";
					dialog.Multiselect = true;
					if (dialog.ShowDialog() == DialogResult.OK)
					{
						var formatter = new XmlSerializer(typeof(ProfilingLatencyDTO));
						var stacks = new List<ProfilingStack>();
						foreach (var filename in dialog.FileNames)
						{
							using (Stream stream = new FileStream(filename, FileMode.Open))
							{
								var latencyDto = (ProfilingLatencyDTO)formatter.Deserialize(stream);
								stacks.AddRange(latencyDto.Stacks.Select(c => new ProfilingStack(c)));
							}
						}
						RefreshStacks(stacks.ToArray());
					}
				}
			}
		}


		private void Find()
		{
			using (_latencyComponent.Postpone())
			using (Dim())
			{
				BuildTree();
				if (!string.IsNullOrEmpty(findText.Text))
					FindNodeInHierarchy(_treeView.Nodes, findText.Text);

				SetUpCount();
			}
		}

		private void _treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				using (_latencyComponent.Postpone())
				{
					_treeView.SelectedNode = e.Node;
					contextMenuStrip1.Show(e.Location);
				}
			}
		}

		private void createNewCallGraphToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (_latencyComponent.Postpone())
			using (Dim())
			{
				var search = _treeView.SelectedNode.Text;
				var index = search.IndexOf("at", StringComparison.Ordinal);
				if (index != -1)
				{
					search = search.Substring(index, search.Length - index);
				}
				BuildTree();

				var result = BuildNewGraph(_treeView.Nodes, search);
				_treeView.Nodes.Clear();
				_treeView.Nodes.AddRange(result.ToArray());

				SetUpCount();
			}
		}

		private void removeKnownIssueToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (_latencyComponent.Postpone())
			using (Dim())
			{
				var search = _treeView.SelectedNode.Text;
				var index = search.IndexOf("at", StringComparison.Ordinal);
				if (index != -1)
				{
					search = search.Substring(index, search.Length - index);
					NotIssueExceptions.Add(search);
				}
				BuildTree();

				SetUpCount();
			}
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var stacks = GetPlainStack(_treeView.SelectedNode, 0);
			
			Clipboard.SetText(string.Join(Environment.NewLine, stacks));
		}

		private List<string> GetPlainStack(TreeNode parent, int level)
		{
			var list = new List<string>();
			list.Add(new string(' ', level) + parent.Text);
			foreach (TreeNode node in parent.Nodes)
			{
				list.AddRange(GetPlainStack(node, level+1));
			}
			return list;
		}

		private bool FindNodeInHierarchy(TreeNodeCollection nodes, string searchValue)
		{
			bool result = false;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i].Text.ToUpper().Contains(searchValue.ToUpper()))
				{
					nodes[i].BackColor = Color.Yellow;
					result = true;
				}

				nodes[i].Expand();
				var childResult = FindNodeInHierarchy(nodes[i].Nodes, searchValue);
				result = result || childResult;
				if (!childResult)
				{
					nodes[i].Collapse();
				}

			}
			return result;
		}

		private bool HighlightFilteredNodes(TreeNodeCollection nodes)
		{
			bool result = false;
			for (int i = 0; i < nodes.Count; i++)
			{
				Application.DoEvents();
				if (((string)nodes[i].Tag ?? string.Empty).Contains("[#Filtered#]"))
				{
					nodes[i].BackColor = Color.OrangeRed;
					result = true;
				}

				nodes[i].Expand();
				var childResult = HighlightFilteredNodes(nodes[i].Nodes);
				result = result || childResult;
				if (!childResult)
				{
					nodes[i].Collapse();
				}

			}
			return result;
		}

		private bool HighlightCompareNodes(TreeNodeCollection nodes, bool parentResult)
		{
			bool result = false;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (((string)nodes[i].Tag ?? string.Empty).Contains("[#Compare#]"))
				{
					result = true;
				}

				if (parentResult && result)
				{
					nodes[i].Expand();
				}
				var childResult = HighlightCompareNodes(nodes[i].Nodes, result);
				result = result || childResult;
				if (!childResult)
				{
					nodes[i].Collapse();
				}

			}
			return result;
		}

		private List<TreeNode> BuildNewGraph(TreeNodeCollection nodes, string searchValue)
		{
			var result = new List<TreeNode>();
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i].Text.ToUpper().Contains(searchValue.ToUpper()))
				{
					nodes[i].BackColor = Color.Yellow;
					result.Add(nodes[i]);
				}

				result.AddRange(BuildNewGraph(nodes[i].Nodes, searchValue));
			}

			return result;
		}

		private void findText_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				Find();
			}
		}

		private void findTags_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				Tags = findTags.Text;
				if(string.IsNullOrEmpty(Tags))
					return;

				filterType.SelectedIndex = filterType.FindStringExact(TagsOnly);

				RebuildTree();			
			}
		}

		private void _treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			using (_latencyComponent.Postpone())
			{
				var node = e.Node;
				if (node != null)
				{
					var text = node.Tag as string;
					richTextBox1.Text = text ?? node.Name;
				}
			}
		}

		public virtual IDisposable Dim()
		{
			return null;
		}


		public virtual void BeginInit()
		{
			
		}

		public virtual void EndInit()
		{

		}


	}

	internal class StackItem
	{
		private readonly ProfilingStackItem _item;
		private readonly DateTime? _createDate;
		private readonly StackType _stackType;
		private readonly ProfilingStack _root;

		public StackItem(ProfilingStackItem item, DateTime? createDate, StackType stackType, ProfilingStack root)
		{
			_item = item;
			_createDate = createDate;
			_stackType = stackType;
			_root = root;
		}

		public long HitCount
		{
			get { return Item.HitCount; }
		}

		public long TotalTime
		{
			get { return Item.TotalTime; }
		}

		public ProfilingStackItem Item
		{
			get { return _item; }
		}

		public DateTime? CreateDate
		{
			get { return _createDate; }
		}

		public StackType StackType
		{
			get { return _stackType; }
		}

		public ProfilingStack Root
		{
			get { return _root; }
		}
	}

	enum OrderBy
	{
		TotalTime,
		Date
	}

	class OrderByComboboxItem
	{
		public string Text { get; set; }
		public OrderBy Value { get; set; }

		public override string ToString()
		{
			return Text;
		}
	}

	class FilterTypeComboboxItem
	{
		public string Text { get; set; }
		public ProfilingStackFilter.FilterType Value { get; set; }

		public override string ToString()
		{
			return Text;
		}
	}

	class FilterContext
	{
		private readonly bool _simplifyStack;
		private readonly bool _latencyDiveded;
		private readonly ProfilingStackFilter.FilterType _filterType;
		private readonly bool _onlyFrozen;
		private readonly OrderBy _orderBy;

		public FilterContext(bool simplifyStack, bool latencyDiveded, ProfilingStackFilter.FilterType filterType, bool onlyFrozen,
												 OrderBy orderBy)
		{
			_simplifyStack = simplifyStack;
			_latencyDiveded = latencyDiveded;
			_filterType = filterType;
			_onlyFrozen = onlyFrozen;
			_orderBy = orderBy;

			if (onlyFrozen)
			{
				_latencyDiveded = true;
				_filterType = ProfilingStackFilter.FilterType.None;
			}
		}

		public bool SimplifyStack
		{
			get { return _simplifyStack; }
		}

		public bool LatencyDiveded
		{
			get { return _latencyDiveded; }
		}

		public ProfilingStackFilter.FilterType FilterType
		{
			get { return _filterType; }
		}

		public bool OnlyFrozen
		{
			get { return _onlyFrozen; }
		}

		public OrderBy OrderBy
		{
			get { return _orderBy; }
		}
	}
}
