using System;

namespace Profiling.Common.Tracing
{
	public sealed class ProfilingThreadContext
	{
		private ProfilingThreadContext()
		{
		}

		public int ParentOperationHash
		{
			get;
			private set;
		}

		public int ProfilingLevel
		{
			get;
			private set;
		}


		//perf opt. ThreadStatic faster than CallContext
		[ThreadStatic]
		private static ProfilingThreadContext _current;

		/// <summary>
		/// Gets the current thread context.
		/// </summary>
		public static ProfilingThreadContext Current
		{
			get
			{
				var context = _current;
				if (context == null)
				{
					context = new ProfilingThreadContext();
					_current = context;
				}

				return context;
			}
		}


		public ProfilingThreadScope CreateProfilingOperationScope(int profilingOperationHash)
		{
			ProfilingLevel++;

			var storedValue = ParentOperationHash;
			ParentOperationHash = profilingOperationHash;

			return new ProfilingThreadScope(() =>
			{
				ProfilingLevel--;
				ParentOperationHash = storedValue;
			});
		}



	}
}
