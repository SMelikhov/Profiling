using System.Collections.Generic;
using System.Linq;

namespace Profiling.Common.Sampling
{
	public sealed class ProfilingStackFilter
	{
		public enum FilterType
		{
			None = 0,
			Event = 1,
			ExceptKnownDelays = 2,
			TagsOnly = 3,
			PotentialProblems = 4,
		}

		private readonly List<string> _notIssues = new List<string>
			{
				"ProfilingResultViewControl",
				"System.Windows.Forms.Control.CreateHandle",
				"OMS.Client.FormBase.DataBind"
			};

		private readonly List<string> _event = new List<string>
			{
				"System.Windows.Forms.Timer.TimerNativeWindow.WndProc",
				"DataContextSubscription.Publish",
				"DataContextBase.ApplyChangedToDataContext",
				"DataContextBase.ExternalChangesHandler",
				"MainForm.Timer_Tick",
				"void OnIdle",
				"IdleQueue.RunIdle",
				"ESAgent.DispatchEventQueue",
				"ESAgent.FireDomainObjectChangedEvents",
				"System.Windows.Forms.Timer.TimerNativeWindow.WndProc"
			};

		private readonly List<string> _potentialProblems = new List<string>
		{
			"EvalHelper.",
			"CompiledEvaluator.Eval",
			"Amazonia.Management.ExpressionEvaluator.ExpressionEvaluator`1.EvaluateExpression",
		};

		private FilterType _filterType;
		private readonly ProfilingStack[] _stacks;
		private readonly int _totaltimeThreashold = 30;
		private readonly List<ProfilingStack> _result = new List<ProfilingStack>();
		private readonly List<string> _tags = new List<string>();

		public ProfilingStackFilter(ProfilingStack[] stacks, List<string> notIssuesExceptions, string tags, int? totaltimeThreashold = null)
		{
			_stacks = stacks;
			if (totaltimeThreashold.HasValue)
				_totaltimeThreashold = totaltimeThreashold.Value;

			if (!string.IsNullOrEmpty(tags))
				_tags.Add(tags);

			if(notIssuesExceptions != null)
				_notIssues.AddRange(notIssuesExceptions);
		}

		public ProfilingStack[] GetFiltered(FilterType filterType)
		{
			_filterType = filterType;

			if (_stacks.Any())
				Parse();

			return _result.ToArray();
		}

		private void Parse()
		{
			foreach (var stack in _stacks)
			{
				if (stack.Stacks != null)
				{
					foreach (var item in stack.Stacks)
					{
						ResetFiltered(item);
					}
				}
			}

			foreach (var stack in _stacks)
			{
				var isFiltered = IsFiltered(stack);
				switch (_filterType)
				{
					case FilterType.ExceptKnownDelays:
						if (!isFiltered)
							_result.Add(stack);
						break;

					case FilterType.Event:
					case FilterType.PotentialProblems:
					case FilterType.TagsOnly:
						if (isFiltered)
							_result.Add(stack);
						break;
				}

			}
		}

		private void ResetFiltered(ProfilingStackItem item)
		{
			item.Filtered = false;
			foreach (var profilingStackItem in item.Children)
			{
				ResetFiltered(profilingStackItem);
			}
		}

		private bool IsFiltered(ProfilingStack stack)
		{
			if (stack.Stacks != null)
			{
				foreach (var profilingStack in stack.Stacks)
				{
					if (IsFiltered(profilingStack, stack))
						return true;
				}
			}
			return false;
		}

		private bool IsFiltered(ProfilingStackItem item, ProfilingStack profilingStack)
		{
			switch (_filterType)
			{
				case FilterType.ExceptKnownDelays:
					if (_notIssues.Any(c => item.Stack.Contains(c)))
					{
						return true;
					}
					break;

				case FilterType.Event:
					if (_event.Any(c => item.Stack.Contains(c)) && item.TotalTime >= _totaltimeThreashold)
					{
						item.Filtered = true;
						return true;
					}
					break;

				case FilterType.PotentialProblems:
					if (_potentialProblems.Any(c => item.Stack.Contains(c)) && item.TotalTime >= _totaltimeThreashold)
					{
						item.Filtered = true;
						return true;
					}
					break;

				case FilterType.TagsOnly:
					if (_tags.Any(c => profilingStack.AdditionalInfo.Contains(c)))
					{
						return true;
					}
					break;
			}

			foreach (var stack in item.Children)
			{
				if (IsFiltered(stack, profilingStack))
					return true;
			}

			return false;
		}
	}
}
