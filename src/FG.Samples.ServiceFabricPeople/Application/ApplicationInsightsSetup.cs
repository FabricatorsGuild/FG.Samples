namespace Application
{
	public static class ApplicationInsightsSetup
	{
		public static void Setup(ApplicationInsightsSettingsProvider settingsProvider)
		{
			Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = settingsProvider.InstrumentationKey;
		}
	}
}