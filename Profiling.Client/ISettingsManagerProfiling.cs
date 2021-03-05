using System;
using Profiling.Common;

namespace Profiling.Client
{
	public interface ISettingsManagerProfiling : IDisposable
	{
		event EventHandler SettingChanged;

		ProfilingModeSettings ProfilingModeSettings { get; }
	}

	public class SettingsManagerProfilingStub : ISettingsManagerProfiling
	{
		public event EventHandler SettingChanged;
		public ProfilingModeSettings ProfilingModeSettings
		{
			get
			{
				return new ProfilingModeSettings {ProfilingModeLevel = ProfilingModeLevel.DetectSlowness};				
			}
		}

		public void Dispose()
		{
			
		}

		private void OnSettingChanged()
		{
			SettingChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
