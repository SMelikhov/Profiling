using System.Linq;

namespace Profiling.Common.Sampling
{
	public sealed class ProfilingStackDifference
	{
		private readonly decimal _difference;

		public ProfilingStackDifference(ProfilingStackItem[] rootsNew, ProfilingStackItem[] rootsOld, decimal difference)
		{
			_difference = difference;

			foreach (var newRoot in rootsNew)
			{
				var oldRoot = rootsOld.FirstOrDefault(c => c.Stack == newRoot.Stack);
				Parse(newRoot, oldRoot);
			}
		}

		private void Parse(ProfilingStackItem newRoot, ProfilingStackItem oldRoot)
		{
			if (newRoot.TotalTime > oldRoot.TotalTime)
			{
				if (oldRoot.TotalTime != 0)
				{
					var dif = ((decimal)newRoot.TotalTime - oldRoot.TotalTime) / oldRoot.TotalTime * 100;
					if (dif > _difference)
						newRoot.DifferencePercent = -dif;
				}
			}
			else
			{
				if (newRoot.TotalTime != 0)
				{
					var dif = ((decimal)oldRoot.TotalTime - newRoot.TotalTime) / newRoot.TotalTime * 100;
					if (dif > _difference)
						newRoot.DifferencePercent = dif;
				}
			}

			newRoot.OldTotalTime = oldRoot.TotalTime;

			foreach (var newChild in newRoot.Children)
			{
				var oldChild = oldRoot.Find(newChild.Stack);

				if (oldChild != null)
					Parse(newChild, oldChild);
			}
		}
	}
}
