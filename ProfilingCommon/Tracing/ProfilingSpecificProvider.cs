using System;
using System.Collections.Generic;

namespace Profiling.Common.Tracing
{
	public static class ProfilingSpecificProvider
	{
		private static IProfilingSpecific _profilingSpecific;

		static ProfilingSpecificProvider()
		{
			Initialize(new ProfilingSpecificStub());
		}

		public static void Initialize(IProfilingSpecific provider)
		{
			_profilingSpecific = provider;
		}

		public static IProfilingSpecific GetProfilingSpecific()
		{
			return _profilingSpecific;
		}
	}

	public interface IProfilingSpecific
	{
		string ParseAdditionalInfo(int operation, string additionalInfo);
		void StartCollectTrafficStatistics();
		void StopCollectTrafficStatistics();
		void Error(string message);
		void Debug(string message);
		void Info(string message);
		void Error(string message, Exception ex);
		bool CanCollectStacks(ProfilingSettings settings);
		bool StubMode();
		List<string> GetAdditionalTagsForStacks();
		void WriteDiagnosticInfo(string line);
	}

	public sealed class ProfilingSpecificStub : IProfilingSpecific
	{
		public string ParseAdditionalInfo(int operation, string additionalInfo)
		{
			return additionalInfo;
		}

		public void StartCollectTrafficStatistics()
		{
		}

		public void StopCollectTrafficStatistics()
		{
		}

		public void Error(string message)
		{			
		}

		public void Debug(string message)
		{
		}

		public void Info(string message)
		{
		}

		public void Error(string message, Exception ex)
		{
		}

		public bool CanCollectStacks(ProfilingSettings settings)
		{
			return true;
		}

		public bool StubMode()
		{
			return false;
		}

		public List<string> GetAdditionalTagsForStacks()
		{
			return new List<string>();
		}

		public void WriteDiagnosticInfo(string line)
		{
		}
	}
}