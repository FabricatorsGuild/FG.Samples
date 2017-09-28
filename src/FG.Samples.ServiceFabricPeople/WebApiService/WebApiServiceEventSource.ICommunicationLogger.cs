/*******************************************************************************************
*  This class is autogenerated from the class CommunicationLogger
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace WebApiService
{
	internal sealed partial class WebApiServiceEventSource
	{

		private const int EnumeratingPartitionsEventId = 201;

		[Event(EnumeratingPartitionsEventId, Level = EventLevel.LogAlways, Message = "Enumerating Partitions {7}", Keywords = Keywords.Communication)]
		private void EnumeratingPartitions(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string serviceUri)
		{
			WriteEvent(
				EnumeratingPartitionsEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				serviceUri);
		}

		[NonEvent]
		public void EnumeratingPartitions(
			System.Fabric.StatelessServiceContext context, 
			System.Uri serviceUri)
		{
			if (this.IsEnabled())
			{
				EnumeratingPartitions(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					serviceUri.ToString());
			}
		}


		private const int FailedToEnumeratePartitionsEventId = 402;

		[Event(FailedToEnumeratePartitionsEventId, Level = EventLevel.LogAlways, Message = "{8}", Keywords = Keywords.Communication)]
		private void FailedToEnumeratePartitions(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string serviceUri, 
			string message, 
			string source, 
			string exceptionTypeName, 
			string exception)
		{
			WriteEvent(
				FailedToEnumeratePartitionsEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				serviceUri, 
				message, 
				source, 
				exceptionTypeName, 
				exception);
		}

		[NonEvent]
		public void FailedToEnumeratePartitions(
			System.Fabric.StatelessServiceContext context, 
			System.Uri serviceUri, 
			System.Exception ex)
		{
			if (this.IsEnabled())
			{
				FailedToEnumeratePartitions(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					serviceUri.ToString(), 
					ex.Message, 
					ex.Source, 
					ex.GetType().FullName, 
					ex.AsJson());
			}
		}


		private const int EnumeratedExistingPartitionsEventId = 603;

		[Event(EnumeratedExistingPartitionsEventId, Level = EventLevel.LogAlways, Message = "Enumerated Existing Partitions {7} {8}", Keywords = Keywords.Communication)]
		private void EnumeratedExistingPartitions(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string serviceUri, 
			string partitions)
		{
			WriteEvent(
				EnumeratedExistingPartitionsEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				serviceUri, 
				partitions);
		}

		[NonEvent]
		public void EnumeratedExistingPartitions(
			System.Fabric.StatelessServiceContext context, 
			System.Uri serviceUri, 
			System.Collections.Generic.IEnumerable<System.Fabric.ServicePartitionInformation> partitions)
		{
			if (this.IsEnabled())
			{
				EnumeratedExistingPartitions(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					serviceUri.ToString(), 
					partitions.ToString());
			}
		}


		private const int EnumeratedAndCachedPartitionsEventId = 804;

		[Event(EnumeratedAndCachedPartitionsEventId, Level = EventLevel.LogAlways, Message = "Enumerated And Cached Partitions {7} {8}", Keywords = Keywords.Communication)]
		private void EnumeratedAndCachedPartitions(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string serviceUri, 
			string partitions)
		{
			WriteEvent(
				EnumeratedAndCachedPartitionsEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				serviceUri, 
				partitions);
		}

		[NonEvent]
		public void EnumeratedAndCachedPartitions(
			System.Fabric.StatelessServiceContext context, 
			System.Uri serviceUri, 
			System.Collections.Generic.IEnumerable<System.Fabric.ServicePartitionInformation> partitions)
		{
			if (this.IsEnabled())
			{
				EnumeratedAndCachedPartitions(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					serviceUri.ToString(), 
					partitions.ToString());
			}
		}


		private const int StartCallActorEventId = 1005;

		[Event(StartCallActorEventId, Level = EventLevel.LogAlways, Message = "Start Call Actor {7} {8} {9} {10} {11} {12} {13}", Keywords = Keywords.Communication, Opcode = EventOpcode.Start, Task = Tasks.CallActor)]
		private void StartCallActor(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string requestUri, 
			string actorMethodName, 
			int interfaceId, 
			int methodId, 
			string actorId, 
			string userId, 
			string correlationId)
		{
			WriteEvent(
				StartCallActorEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				requestUri, 
				actorMethodName, 
				interfaceId, 
				methodId, 
				actorId, 
				userId, 
				correlationId);
		}

		[NonEvent]
		public void StartCallActor(
			System.Fabric.StatelessServiceContext context, 
			System.Uri requestUri, 
			string actorMethodName, 
			FG.ServiceFabric.Actors.Remoting.Runtime.ActorMessageHeaders actorMessageHeaders, 
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader)
		{
			if (this.IsEnabled())
			{
				StartCallActor(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					requestUri.ToString(), 
					actorMethodName, 
					(actorMessageHeaders?.InterfaceId ?? 0), 
					(actorMessageHeaders?.MethodId ?? 0), 
					actorMessageHeaders?.ActorId.ToString(), 
					customServiceRequestHeader?.GetHeader("userId"), 
					customServiceRequestHeader?.GetHeader("correlationId"));
			}
		}


		private const int StopCallActorEventId = 1206;

		[Event(StopCallActorEventId, Level = EventLevel.LogAlways, Message = "Stop Call Actor {7} {8} {9} {10} {11} {12} {13}", Keywords = Keywords.Communication, Opcode = EventOpcode.Stop, Task = Tasks.CallActor)]
		private void StopCallActor(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string requestUri, 
			string actorMethodName, 
			int interfaceId, 
			int methodId, 
			string actorId, 
			string userId, 
			string correlationId)
		{
			WriteEvent(
				StopCallActorEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				requestUri, 
				actorMethodName, 
				interfaceId, 
				methodId, 
				actorId, 
				userId, 
				correlationId);
		}

		[NonEvent]
		public void StopCallActor(
			System.Fabric.StatelessServiceContext context, 
			System.Uri requestUri, 
			string actorMethodName, 
			FG.ServiceFabric.Actors.Remoting.Runtime.ActorMessageHeaders actorMessageHeaders, 
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader)
		{
			if (this.IsEnabled())
			{
				StopCallActor(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					requestUri.ToString(), 
					actorMethodName, 
					(actorMessageHeaders?.InterfaceId ?? 0), 
					(actorMessageHeaders?.MethodId ?? 0), 
					actorMessageHeaders?.ActorId.ToString(), 
					customServiceRequestHeader?.GetHeader("userId"), 
					customServiceRequestHeader?.GetHeader("correlationId"));
			}
		}


		private const int CallActorFailedEventId = 1407;

		[Event(CallActorFailedEventId, Level = EventLevel.LogAlways, Message = "{14}", Keywords = Keywords.Communication)]
		private void CallActorFailed(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string requestUri, 
			string actorMethodName, 
			int interfaceId, 
			int methodId, 
			string actorId, 
			string userId, 
			string correlationId, 
			string message, 
			string source, 
			string exceptionTypeName, 
			string exception)
		{
			WriteEvent(
				CallActorFailedEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				requestUri, 
				actorMethodName, 
				interfaceId, 
				methodId, 
				actorId, 
				userId, 
				correlationId, 
				message, 
				source, 
				exceptionTypeName, 
				exception);
		}

		[NonEvent]
		public void CallActorFailed(
			System.Fabric.StatelessServiceContext context, 
			System.Uri requestUri, 
			string actorMethodName, 
			FG.ServiceFabric.Actors.Remoting.Runtime.ActorMessageHeaders actorMessageHeaders, 
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader, 
			System.Exception ex)
		{
			if (this.IsEnabled())
			{
				CallActorFailed(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					requestUri.ToString(), 
					actorMethodName, 
					(actorMessageHeaders?.InterfaceId ?? 0), 
					(actorMessageHeaders?.MethodId ?? 0), 
					actorMessageHeaders?.ActorId.ToString(), 
					customServiceRequestHeader?.GetHeader("userId"), 
					customServiceRequestHeader?.GetHeader("correlationId"), 
					ex.Message, 
					ex.Source, 
					ex.GetType().FullName, 
					ex.AsJson());
			}
		}


		private const int StartCallServiceEventId = 1608;

		[Event(StartCallServiceEventId, Level = EventLevel.LogAlways, Message = "Start Call Service {7} {8} {9} {10} {11} {12}", Keywords = Keywords.Communication, Opcode = EventOpcode.Start, Task = Tasks.CallService)]
		private void StartCallService(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string requestUri, 
			string serviceMethodName, 
			int InterfaceId, 
			int MethodId, 
			string userId, 
			string correlationId)
		{
			WriteEvent(
				StartCallServiceEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				requestUri, 
				serviceMethodName, 
				InterfaceId, 
				MethodId, 
				userId, 
				correlationId);
		}

		[NonEvent]
		public void StartCallService(
			System.Fabric.StatelessServiceContext context, 
			System.Uri requestUri, 
			string serviceMethodName,
			Microsoft.ServiceFabric.Services.Remoting.V1.ServiceRemotingMessageHeaders serviceMessageHeaders, 
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader)
		{
			if (this.IsEnabled())
			{
				StartCallService(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					requestUri.ToString(), 
					serviceMethodName, 
					(serviceMessageHeaders?.InterfaceId ?? 0), 
					(serviceMessageHeaders?.MethodId ?? 0), 
					customServiceRequestHeader?.GetHeader("userId"), 
					customServiceRequestHeader?.GetHeader("correlationId"));
			}
		}


		private const int StopCallServiceEventId = 1809;

		[Event(StopCallServiceEventId, Level = EventLevel.LogAlways, Message = "Stop Call Service {7} {8} {9} {10} {11} {12}", Keywords = Keywords.Communication, Opcode = EventOpcode.Stop, Task = Tasks.CallService)]
		private void StopCallService(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string requestUri, 
			string serviceMethodName, 
			int InterfaceId, 
			int MethodId, 
			string userId, 
			string correlationId)
		{
			WriteEvent(
				StopCallServiceEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				requestUri, 
				serviceMethodName, 
				InterfaceId, 
				MethodId, 
				userId, 
				correlationId);
		}

		[NonEvent]
		public void StopCallService(
			System.Fabric.StatelessServiceContext context, 
			System.Uri requestUri, 
			string serviceMethodName,
			Microsoft.ServiceFabric.Services.Remoting.V1.ServiceRemotingMessageHeaders serviceMessageHeaders, 
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader)
		{
			if (this.IsEnabled())
			{
				StopCallService(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					requestUri.ToString(), 
					serviceMethodName, 
					(serviceMessageHeaders?.InterfaceId ?? 0), 
					(serviceMessageHeaders?.MethodId ?? 0), 
					customServiceRequestHeader?.GetHeader("userId"), 
					customServiceRequestHeader?.GetHeader("correlationId"));
			}
		}


		private const int CallServiceFailedEventId = 2010;

		[Event(CallServiceFailedEventId, Level = EventLevel.LogAlways, Message = "{13}", Keywords = Keywords.Communication)]
		private void CallServiceFailed(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string requestUri, 
			string serviceMethodName, 
			int InterfaceId, 
			int MethodId, 
			string userId, 
			string correlationId, 
			string message, 
			string source, 
			string exceptionTypeName, 
			string exception)
		{
			WriteEvent(
				CallServiceFailedEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				requestUri, 
				serviceMethodName, 
				InterfaceId, 
				MethodId, 
				userId, 
				correlationId, 
				message, 
				source, 
				exceptionTypeName, 
				exception);
		}

		[NonEvent]
		public void CallServiceFailed(
			System.Fabric.StatelessServiceContext context, 
			System.Uri requestUri, 
			string serviceMethodName,
			Microsoft.ServiceFabric.Services.Remoting.V1.ServiceRemotingMessageHeaders serviceMessageHeaders, 
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader, 
			System.Exception ex)
		{
			if (this.IsEnabled())
			{
				CallServiceFailed(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					requestUri.ToString(), 
					serviceMethodName, 
					(serviceMessageHeaders?.InterfaceId ?? 0), 
					(serviceMessageHeaders?.MethodId ?? 0), 
					customServiceRequestHeader?.GetHeader("userId"), 
					customServiceRequestHeader?.GetHeader("correlationId"), 
					ex.Message, 
					ex.Source, 
					ex.GetType().FullName, 
					ex.AsJson());
			}
		}


		private const int ServiceClientFailedEventId = 2211;

		[Event(ServiceClientFailedEventId, Level = EventLevel.LogAlways, Message = "{10}", Keywords = Keywords.Communication)]
		private void ServiceClientFailed(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string requestUri, 
			string userId, 
			string correlationId, 
			string message, 
			string source, 
			string exceptionTypeName, 
			string exception)
		{
			WriteEvent(
				ServiceClientFailedEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				requestUri, 
				userId, 
				correlationId, 
				message, 
				source, 
				exceptionTypeName, 
				exception);
		}

		[NonEvent]
		public void ServiceClientFailed(
			System.Fabric.StatelessServiceContext context, 
			System.Uri requestUri, 
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader, 
			System.Exception ex)
		{
			if (this.IsEnabled())
			{
				ServiceClientFailed(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					requestUri.ToString(), 
					customServiceRequestHeader?.GetHeader("userId"), 
					customServiceRequestHeader?.GetHeader("correlationId"), 
					ex.Message, 
					ex.Source, 
					ex.GetType().FullName, 
					ex.AsJson());
			}
		}


		private const int StartRequestContextEventId = 2412;

		[Event(StartRequestContextEventId, Level = EventLevel.LogAlways, Message = "Start Request Context {7}", Keywords = Keywords.Communication, Opcode = EventOpcode.Start)]
		private void StartRequestContext(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string headers)
		{
			WriteEvent(
				StartRequestContextEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				headers);
		}

		[NonEvent]
		public void StartRequestContext(
			System.Fabric.StatelessServiceContext context, 
			System.Collections.Generic.IEnumerable<FG.ServiceFabric.Services.Remoting.FabricTransport.ServiceRequestHeader> headers)
		{
			if (this.IsEnabled())
			{
				StartRequestContext(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					headers.ToString());
			}
		}


		private const int StopRequestContextEventId = 2613;

		[Event(StopRequestContextEventId, Level = EventLevel.LogAlways, Message = "Stop Request Context {7}", Keywords = Keywords.Communication, Opcode = EventOpcode.Stop)]
		private void StopRequestContext(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string headers)
		{
			WriteEvent(
				StopRequestContextEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				headers);
		}

		[NonEvent]
		public void StopRequestContext(
			System.Fabric.StatelessServiceContext context, 
			System.Collections.Generic.IEnumerable<FG.ServiceFabric.Services.Remoting.FabricTransport.ServiceRequestHeader> headers)
		{
			if (this.IsEnabled())
			{
				StopRequestContext(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					headers.ToString());
			}
		}


		private const int FailedRequestContextEventId = 2814;

		[Event(FailedRequestContextEventId, Level = EventLevel.LogAlways, Message = "{8}", Keywords = Keywords.Communication)]
		private void FailedRequestContext(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			string headers, 
			string message, 
			string source, 
			string exceptionTypeName, 
			string exception)
		{
			WriteEvent(
				FailedRequestContextEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				headers, 
				message, 
				source, 
				exceptionTypeName, 
				exception);
		}

		[NonEvent]
		public void FailedRequestContext(
			System.Fabric.StatelessServiceContext context, 
			System.Collections.Generic.IEnumerable<FG.ServiceFabric.Services.Remoting.FabricTransport.ServiceRequestHeader> headers, 
			System.Exception exception)
		{
			if (this.IsEnabled())
			{
				FailedRequestContext(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					headers.ToString(), 
					exception.Message, 
					exception.Source, 
					exception.GetType().FullName, 
					exception.AsJson());
			}
		}


		private const int FailedToReadCustomServiceMessageHeaderEventId = 3015;

		[Event(FailedToReadCustomServiceMessageHeaderEventId, Level = EventLevel.LogAlways, Message = "{9}", Keywords = Keywords.Communication)]
		private void FailedToReadCustomServiceMessageHeader(
			string serviceName, 
			string serviceTypeName, 
			long replicaOrInstanceId, 
			Guid partitionId, 
			string applicationName, 
			string applicationTypeName, 
			string nodeName, 
			int InterfaceId, 
			int MethodId, 
			string message, 
			string source, 
			string exceptionTypeName, 
			string exception)
		{
			WriteEvent(
				FailedToReadCustomServiceMessageHeaderEventId, 
				serviceName, 
				serviceTypeName, 
				replicaOrInstanceId, 
				partitionId, 
				applicationName, 
				applicationTypeName, 
				nodeName, 
				InterfaceId, 
				MethodId, 
				message, 
				source, 
				exceptionTypeName, 
				exception);
		}

		[NonEvent]
		public void FailedToReadCustomServiceMessageHeader(
			System.Fabric.StatelessServiceContext context,
			Microsoft.ServiceFabric.Services.Remoting.V1.ServiceRemotingMessageHeaders serviceRemotingMessageHeaders, 
			System.Exception ex)
		{
			if (this.IsEnabled())
			{
				FailedToReadCustomServiceMessageHeader(
					context.ServiceName.ToString(), 
					context.ServiceTypeName, 
					context.InstanceId, 
					context.PartitionId, 
					context.CodePackageActivationContext.ApplicationName, 
					context.CodePackageActivationContext.ApplicationTypeName, 
					context.NodeContext.NodeName, 
					(serviceRemotingMessageHeaders?.InterfaceId ?? 0), 
					(serviceRemotingMessageHeaders?.MethodId ?? 0), 
					ex.Message, 
					ex.Source, 
					ex.GetType().FullName, 
					ex.AsJson());
			}
		}


	}
}