using FG.ServiceFabric.Services.Remoting.FabricTransport;

namespace Application
{
	public class ServiceFabricPeopleContext : FG.ServiceFabric.Services.Remoting.FabricTransport.ServiceRequestContextWrapper
	{
		public static ServiceFabricPeopleContext Current => new ServiceFabricPeopleContext();

		private ServiceFabricPeopleContext() { }
		public ServiceFabricPeopleContext(string correlationId, string userId, string authToken, string tenantId)
		{
			if (ServiceRequestContext.Current == null) return;

			this.CorrelationId = correlationId;
			this.UserId = userId;
			this.RequestUri = null;
			this.AuthToken = authToken;
			this.TenantId = tenantId;
		}

		public string AuthToken
		{
			get { return ServiceRequestContext.Current?[ServiceRequestContextKeys.AuthToken]; }
			set
			{
				if (ServiceRequestContext.Current != null)
				{
					ServiceRequestContext.Current[ServiceRequestContextKeys.AuthToken] = value;
				}
			}
		}

		public string TenantId
		{
			get { return ServiceRequestContext.Current?[ServiceRequestContextKeys.TenantId]; }
			set
			{
				if (ServiceRequestContext.Current != null)
				{
					ServiceRequestContext.Current[ServiceRequestContextKeys.TenantId] = value;
				}
			}
		}

		private static class ServiceRequestContextKeys
		{
			public const string AuthToken = "authToken";
			public const string TenantId = "tenantId";
		}
	}
}