/*******************************************************************************************
*  This class is autogenerated from the class ActorLogger
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace PersonActor
{
	internal sealed partial class PersonActorEventSource
	{

		private const int ActorActivatedEventId = 2001;

		[Event(ActorActivatedEventId, Level = EventLevel.LogAlways, Message = "Actor Activated {9}", Keywords = Keywords.Actor)]
		private void ActorActivated(
			string actorType, 
			string actorId, 
			string applicationTypeName, 
			string applicationName, 
			string serviceTypeName, 
			string serviceName, 
			Guid partitionId, 
			long replicaOrInstanceId, 
			string nodeName, 
			bool firstActivation)
		{
			WriteEvent(
				ActorActivatedEventId, 
				actorType, 
				actorId, 
				applicationTypeName, 
				applicationName, 
				serviceTypeName, 
				serviceName, 
				partitionId, 
				replicaOrInstanceId, 
				nodeName, 
				firstActivation);
		}

		[NonEvent]
		public void ActorActivated(
			FG.ServiceFabric.Diagnostics.ActorOrActorServiceDescription actor, 
			bool firstActivation)
		{
			if (this.IsEnabled())
			{
				ActorActivated(
					actor.ActorType.ToString(), 
					actor.ActorId.ToString(), 
					actor.ApplicationTypeName, 
					actor.ApplicationName, 
					actor.ServiceTypeName, 
					actor.ServiceName, 
					actor.PartitionId, 
					actor.ReplicaOrInstanceId, 
					actor.NodeName, 
					firstActivation);
			}
		}


		private const int ActorDeactivatedEventId = 4002;

		[Event(ActorDeactivatedEventId, Level = EventLevel.LogAlways, Message = "Actor Deactivated", Keywords = Keywords.Actor)]
		private void ActorDeactivated(
			string actorType, 
			string actorId, 
			string applicationTypeName, 
			string applicationName, 
			string serviceTypeName, 
			string serviceName, 
			Guid partitionId, 
			long replicaOrInstanceId, 
			string nodeName)
		{
			WriteEvent(
				ActorDeactivatedEventId, 
				actorType, 
				actorId, 
				applicationTypeName, 
				applicationName, 
				serviceTypeName, 
				serviceName, 
				partitionId, 
				replicaOrInstanceId, 
				nodeName);
		}

		[NonEvent]
		public void ActorDeactivated(
			FG.ServiceFabric.Diagnostics.ActorOrActorServiceDescription actor)
		{
			if (this.IsEnabled())
			{
				ActorDeactivated(
					actor.ActorType.ToString(), 
					actor.ActorId.ToString(), 
					actor.ApplicationTypeName, 
					actor.ApplicationName, 
					actor.ServiceTypeName, 
					actor.ServiceName, 
					actor.PartitionId, 
					actor.ReplicaOrInstanceId, 
					actor.NodeName);
			}
		}


		private const int StartReadStateEventId = 6003;

		[Event(StartReadStateEventId, Level = EventLevel.LogAlways, Message = "Start Read State {9}", Keywords = Keywords.Actor, Opcode = EventOpcode.Start, Task = Tasks.ReadState)]
		private void StartReadState(
			string actorType, 
			string actorId, 
			string applicationTypeName, 
			string applicationName, 
			string serviceTypeName, 
			string serviceName, 
			Guid partitionId, 
			long replicaOrInstanceId, 
			string nodeName, 
			string stateName)
		{
			WriteEvent(
				StartReadStateEventId, 
				actorType, 
				actorId, 
				applicationTypeName, 
				applicationName, 
				serviceTypeName, 
				serviceName, 
				partitionId, 
				replicaOrInstanceId, 
				nodeName, 
				stateName);
		}

		[NonEvent]
		public void StartReadState(
			FG.ServiceFabric.Diagnostics.ActorOrActorServiceDescription actor, 
			string stateName)
		{
			if (this.IsEnabled())
			{
				StartReadState(
					actor.ActorType.ToString(), 
					actor.ActorId.ToString(), 
					actor.ApplicationTypeName, 
					actor.ApplicationName, 
					actor.ServiceTypeName, 
					actor.ServiceName, 
					actor.PartitionId, 
					actor.ReplicaOrInstanceId, 
					actor.NodeName, 
					stateName);
			}
		}


		private const int StopReadStateEventId = 8004;

		[Event(StopReadStateEventId, Level = EventLevel.LogAlways, Message = "Stop Read State {9}", Keywords = Keywords.Actor, Opcode = EventOpcode.Stop, Task = Tasks.ReadState)]
		private void StopReadState(
			string actorType, 
			string actorId, 
			string applicationTypeName, 
			string applicationName, 
			string serviceTypeName, 
			string serviceName, 
			Guid partitionId, 
			long replicaOrInstanceId, 
			string nodeName, 
			string stateName)
		{
			WriteEvent(
				StopReadStateEventId, 
				actorType, 
				actorId, 
				applicationTypeName, 
				applicationName, 
				serviceTypeName, 
				serviceName, 
				partitionId, 
				replicaOrInstanceId, 
				nodeName, 
				stateName);
		}

		[NonEvent]
		public void StopReadState(
			FG.ServiceFabric.Diagnostics.ActorOrActorServiceDescription actor, 
			string stateName)
		{
			if (this.IsEnabled())
			{
				StopReadState(
					actor.ActorType.ToString(), 
					actor.ActorId.ToString(), 
					actor.ApplicationTypeName, 
					actor.ApplicationName, 
					actor.ServiceTypeName, 
					actor.ServiceName, 
					actor.PartitionId, 
					actor.ReplicaOrInstanceId, 
					actor.NodeName, 
					stateName);
			}
		}


		private const int StartWriteStateEventId = 10005;

		[Event(StartWriteStateEventId, Level = EventLevel.LogAlways, Message = "Start Write State {9}", Keywords = Keywords.Actor, Opcode = EventOpcode.Start, Task = Tasks.WriteState)]
		private void StartWriteState(
			string actorType, 
			string actorId, 
			string applicationTypeName, 
			string applicationName, 
			string serviceTypeName, 
			string serviceName, 
			Guid partitionId, 
			long replicaOrInstanceId, 
			string nodeName, 
			string stateName)
		{
			WriteEvent(
				StartWriteStateEventId, 
				actorType, 
				actorId, 
				applicationTypeName, 
				applicationName, 
				serviceTypeName, 
				serviceName, 
				partitionId, 
				replicaOrInstanceId, 
				nodeName, 
				stateName);
		}

		[NonEvent]
		public void StartWriteState(
			FG.ServiceFabric.Diagnostics.ActorOrActorServiceDescription actor, 
			string stateName)
		{
			if (this.IsEnabled())
			{
				StartWriteState(
					actor.ActorType.ToString(), 
					actor.ActorId.ToString(), 
					actor.ApplicationTypeName, 
					actor.ApplicationName, 
					actor.ServiceTypeName, 
					actor.ServiceName, 
					actor.PartitionId, 
					actor.ReplicaOrInstanceId, 
					actor.NodeName, 
					stateName);
			}
		}


		private const int StopWriteStateEventId = 12006;

		[Event(StopWriteStateEventId, Level = EventLevel.LogAlways, Message = "Stop Write State {9}", Keywords = Keywords.Actor, Opcode = EventOpcode.Stop, Task = Tasks.WriteState)]
		private void StopWriteState(
			string actorType, 
			string actorId, 
			string applicationTypeName, 
			string applicationName, 
			string serviceTypeName, 
			string serviceName, 
			Guid partitionId, 
			long replicaOrInstanceId, 
			string nodeName, 
			string stateName)
		{
			WriteEvent(
				StopWriteStateEventId, 
				actorType, 
				actorId, 
				applicationTypeName, 
				applicationName, 
				serviceTypeName, 
				serviceName, 
				partitionId, 
				replicaOrInstanceId, 
				nodeName, 
				stateName);
		}

		[NonEvent]
		public void StopWriteState(
			FG.ServiceFabric.Diagnostics.ActorOrActorServiceDescription actor, 
			string stateName)
		{
			if (this.IsEnabled())
			{
				StopWriteState(
					actor.ActorType.ToString(), 
					actor.ActorId.ToString(), 
					actor.ApplicationTypeName, 
					actor.ApplicationName, 
					actor.ServiceTypeName, 
					actor.ServiceName, 
					actor.PartitionId, 
					actor.ReplicaOrInstanceId, 
					actor.NodeName, 
					stateName);
			}
		}


		private const int ActorHostInitializationFailedEventId = 14007;

		[Event(ActorHostInitializationFailedEventId, Level = EventLevel.LogAlways, Message = "{9}", Keywords = Keywords.Actor)]
		private void ActorHostInitializationFailed(
			string actorType, 
			string actorId, 
			string applicationTypeName, 
			string applicationName, 
			string serviceTypeName, 
			string serviceName, 
			Guid partitionId, 
			long replicaOrInstanceId, 
			string nodeName, 
			string message, 
			string source, 
			string exceptionTypeName, 
			string exception)
		{
			WriteEvent(
				ActorHostInitializationFailedEventId, 
				actorType, 
				actorId, 
				applicationTypeName, 
				applicationName, 
				serviceTypeName, 
				serviceName, 
				partitionId, 
				replicaOrInstanceId, 
				nodeName, 
				message, 
				source, 
				exceptionTypeName, 
				exception);
		}

		[NonEvent]
		public void ActorHostInitializationFailed(
			FG.ServiceFabric.Diagnostics.ActorOrActorServiceDescription actor, 
			System.Exception ex)
		{
			if (this.IsEnabled())
			{
				ActorHostInitializationFailed(
					actor.ActorType.ToString(), 
					actor.ActorId.ToString(), 
					actor.ApplicationTypeName, 
					actor.ApplicationName, 
					actor.ServiceTypeName, 
					actor.ServiceName, 
					actor.PartitionId, 
					actor.ReplicaOrInstanceId, 
					actor.NodeName, 
					ex.Message, 
					ex.Source, 
					ex.GetType().FullName, 
					ex.AsJson());
			}
		}


	}
}