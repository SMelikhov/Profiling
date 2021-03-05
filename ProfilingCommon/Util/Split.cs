using System.Collections.Generic;

namespace Profiling.Common.Util
{
	public static class Split
	{
		public static IEnumerable<IEnumerable<T>> SplitBy<T>(this IEnumerable<T> source, int count)
		{
			var result = new List<T>();
			foreach (var item in source)
			{
				result.Add(item);
				if (result.Count < count)
					continue;

				yield return result;
				result = new List<T>();
			}

			if (result.Count > 0)
				yield return result;

			yield break;
		}

		public static IEnumerable<T> SplitJoin<T>(this IEnumerable<IEnumerable<T>> source)
		{
			foreach (var ts in source)
				if (ts != null)
					foreach (var item in ts)
						yield return item;
		}
	}
}
