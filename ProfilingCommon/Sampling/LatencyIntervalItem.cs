namespace Profiling.Common.Sampling
{
	internal sealed class LatencyIntervalItem
	{
		private readonly int _from;
		private readonly int _to;
		private volatile int _above;

		public LatencyIntervalItem(int from, int to)
		{
			_from = @from;
			_to = to;
		}


		public void Clear()
		{
			_above = 0;
		}

		public void Increment(int elapsed)
		{
			if (elapsed > _from && (elapsed < _to || _to == -1))
				_above++;
		}

		public string GetAboveMessageItem()
		{
			var secFrom = _from / 1000;
			var isSec = secFrom > 0;
			return (_above > 0 ? string.Format("   [>{1}{2}: {0} hint]", _above,
				(isSec ? secFrom : _from), (isSec ? "s" : "ms")

				) : string.Empty);
		}

		public int Above
		{
			get { return _above; }
		}
	}
}
