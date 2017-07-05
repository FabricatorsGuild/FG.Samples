using FG.ServiceFabric.Diagnostics;
using FG.ServiceFabric.Services.Remoting.Runtime.Client;

namespace ServiceFabricPeople.Tests
{
	public interface ICommunicationLogger :
		IActorServiceCommunicationLogger,
		IPartitionHelperLogger,
		IActorClientLogger,
		IServiceClientLogger
	{
	}
}