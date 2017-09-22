using System.Fabric;
using FG.ServiceFabric.Utils;

namespace Application
{
	public class ApplicationInsightsSettingsProvider : SettingsProviderBase
	{
		private ApplicationInsightsSettingsProvider(ServiceContext context) : base(context)
		{
			Configure()
				.FromSettings("ApplicationInsights", "Key");
		}

		public static ApplicationInsightsSettingsProvider FromServiceFabricContext(ServiceContext context)
		{
			return new ApplicationInsightsSettingsProvider(context);
		}

		public string InstrumentationKey => base["ApplicationInsights.Key"];
	}
}