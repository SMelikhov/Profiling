using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Profiling.Common.InstanceData
{
	public static class MemoryProfiling
	{
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

#pragma warning disable 169
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private class MEMORYSTATUSEX
		{
			public uint dwLength;
			public uint dwMemoryLoad;
			public ulong ullTotalPhys;
			public ulong ullAvailPhys;
			public ulong ullTotalPageFile;
			public ulong ullAvailPageFile;
			public ulong ullTotalVirtual;
			public ulong ullAvailVirtual;
			public ulong ullAvailExtendedVirtual;
			public MEMORYSTATUSEX()
			{
				this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
			}
		}
#pragma warning restore 169

		public struct MemoryStatus
		{
			public float Total;
			public float Available;
			public float Usage;
		}

		public sealed class MemoryInfo
		{
			public float PrivateBytes64 { get; set; }
			public float VirtualMemorySize64 { get; set; }
			public float WorkingSet64 { get; set; }
			public int Threads { get; set; }
		}

		public static MemoryStatus GetAvailable()
		{
			try
			{
				var msex = new MEMORYSTATUSEX();
				GlobalMemoryStatusEx(msex);
				return new MemoryStatus
				{
					Total = (float)msex.ullTotalPhys / 1024 / 1024,
					Available = (float)msex.ullAvailPhys / 1024 / 1024,
					Usage = (float)(msex.ullTotalPhys - msex.ullAvailPhys) / 1024 / 1024
				};
			}
			catch (Exception)
			{
				//do nothing by design
			}
			return new MemoryStatus();
		}

		public static MemoryInfo GetMemoryInfo()
		{
			try
			{
				using (var process = Process.GetCurrentProcess())
				{
					return new MemoryInfo
					{
						VirtualMemorySize64 = (float)process.VirtualMemorySize64 / 1024 / 1024,
						PrivateBytes64 = (float)process.PrivateMemorySize64 / 1024 / 1024,
						WorkingSet64 = (float)process.WorkingSet64 / 1024 / 1024,
						Threads = process.Threads.Count,
					};
				}
			}
			catch (Exception)
			{
				//do nothing by design
			}
			return new MemoryInfo();
		}

	}
}
