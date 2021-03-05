using System;

namespace Profiling.Common.Sampling
{
	public sealed class GcItem
	{
		private const int MaxDifferenceInGen = 10;

		public int Generation0 { get; }

		public int Generation1 { get; }

		public int Generation2 { get; }

		public GcItem()
			: this(GC.CollectionCount(0), GC.CollectionCount(1),GC.CollectionCount(2))
		{
		}

		private GcItem(int generation0, int generation1, int generation2)
		{
			Generation0 = generation0;
			Generation1 = generation1;
			Generation2 = generation2;
		}

		public GcItem CreateDif()
		{
			return new GcItem( 
				GC.CollectionCount(0) - Generation0, 
				GC.CollectionCount(1) - Generation1, 
				GC.CollectionCount(2) - Generation2);
		}

		public bool NeedCollectAvailableMemory()
		{
			return Generation0 > MaxDifferenceInGen;
		}

		public string GetGCString()
		{
			return string.Format("[GC0={0} GC1={1} GC2={2}]",  Generation0, Generation1, Generation2);			
		}


	}
}
