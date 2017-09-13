/*******************************************************************************************
*  This class is autogenerated from the class CommunicationLogger
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;
using System.Collections.Generic;
using PersonActor.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Runtime.Remoting.Messaging;


namespace PersonActor
{
	internal sealed class CommunicationLogger : ICommunicationLogger
	{
	    private sealed class ScopeWrapper : IDisposable
        {
            private readonly IEnumerable<IDisposable> _disposables;

            public ScopeWrapper(IEnumerable<IDisposable> disposables)
            {
                _disposables = disposables;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (disposing)
                {
                    foreach (var disposable in _disposables)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

	    private sealed class ScopeWrapperWithAction : IDisposable
        {
            private readonly Action _onStop;

            internal static IDisposable Wrap(Func<IDisposable> wrap)
            {
                return wrap();
            }

            public ScopeWrapperWithAction(Action onStop)
            {
                _onStop = onStop;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _onStop?.Invoke();
                }
            }
        }


		private readonly FG.ServiceFabric.Diagnostics.ActorOrActorServiceDescription _actor;
		private readonly Microsoft.ApplicationInsights.TelemetryClient _telemetryClient;

        public sealed class OperationHolder
        {
            public static void StartOperation(IOperationHolder<RequestTelemetry> aiOperationHolder)
            {
                OperationHolder.Current = new OperationHolder() {AIOperationHolder = aiOperationHolder};
            }

            public static IOperationHolder<RequestTelemetry> StopOperation()
            {
                var aiOperationHolder = OperationHolder.Current.AIOperationHolder;
                OperationHolder.Current = null;

                return aiOperationHolder;
            }

            private IOperationHolder<RequestTelemetry> AIOperationHolder { get; set; }

            private static readonly string ContextKey = Guid.NewGuid().ToString();

            public static OperationHolder Current
            {
                get { return (OperationHolder)CallContext.LogicalGetData(ContextKey); }
                internal set
                {
                    if (value == null)
                    {
                        CallContext.FreeNamedDataSlot(ContextKey);
                    }
                    else
                    {
                        CallContext.LogicalSetData(ContextKey, value);
                    }
                }
            }
        }

		public CommunicationLogger(
			FG.ServiceFabric.Diagnostics.ActorOrActorServiceDescription actor)
		{
			_actor = actor;
			
            _telemetryClient = new Microsoft.ApplicationInsights.TelemetryClient();
            _telemetryClient.Context.User.Id = Environment.UserName;
            _telemetryClient.Context.Session.Id = Guid.NewGuid().ToString();
            _telemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();

		}


        public System.IDisposable RecieveActorMessage(
			System.Uri requestUri,
			string actorMethodName,
			FG.ServiceFabric.Actors.Remoting.Runtime.ActorMessageHeaders actorMessageHeaders,
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader)
		{
		    return new ScopeWrapper(new IDisposable[]
		    {

                ScopeWrapperWithAction.Wrap(() =>
		        {
			PersonActorEventSource.Current.StartRecieveActorMessage(
				_actor, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			PersonActorEventSource.Current.StopRecieveActorMessage(
				_actor, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader
			);
    
		            });
		        }),


                ScopeWrapperWithAction.Wrap(() =>
		        {

			            var recieveActorMessageOperationHolder = _telemetryClient.StartOperation<RequestTelemetry>(requestUri.ToString() ?? "recieveActorMessage");
			            recieveActorMessageOperationHolder.Telemetry.Properties.Add("ActorType", _actor.ActorType.ToString());
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("ApplicationTypeName", _actor.ApplicationTypeName);
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("ApplicationName", _actor.ApplicationName);
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("ServiceTypeName", _actor.ServiceTypeName);
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("ServiceName", _actor.ServiceName);
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("PartitionId", _actor.PartitionId.ToString());
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString());
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("NodeName", _actor.NodeName);
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("RequestUri", requestUri.ToString());
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("ActorMethodName", actorMethodName);
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("InterfaceId", (actorMessageHeaders?.InterfaceId ?? 0).ToString());
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("MethodId", (actorMessageHeaders?.MethodId ?? 0).ToString());
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("ActorId", actorMessageHeaders?.ActorId.ToString());
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("UserId", customServiceRequestHeader?.GetHeader("userId"));
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("CorrelationId", customServiceRequestHeader?.GetHeader("correlationId"));
    
		            return new ScopeWrapperWithAction(() =>
		            {

			            _telemetryClient.StopOperation<RequestTelemetry>(recieveActorMessageOperationHolder);
    
		            });
		        }),


		    });
		}





		public void RecieveActorMessageFailed(
			System.Uri requestUri,
			string actorMethodName,
			FG.ServiceFabric.Actors.Remoting.Runtime.ActorMessageHeaders actorMessageHeaders,
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader,
			System.Exception ex)
		{
			PersonActorEventSource.Current.RecieveActorMessageFailed(
				_actor, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "RecieveActorMessageFailed" },
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"RequestUri", requestUri.ToString()},
                    {"ActorMethodName", actorMethodName},
                    {"InterfaceId", (actorMessageHeaders?.InterfaceId ?? 0).ToString()},
                    {"MethodId", (actorMessageHeaders?.MethodId ?? 0).ToString()},
                    {"ActorId", actorMessageHeaders?.ActorId.ToString()},
                    {"UserId", customServiceRequestHeader?.GetHeader("userId")},
                    {"CorrelationId", customServiceRequestHeader?.GetHeader("correlationId")},
                    {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}



		public void FailedToGetActorMethodName(
			FG.ServiceFabric.Actors.Remoting.Runtime.ActorMessageHeaders actorMessageHeaders,
			System.Exception ex)
		{
			PersonActorEventSource.Current.FailedToGetActorMethodName(
				_actor, 
				actorMessageHeaders, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "FailedToGetActorMethodName" },
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"InterfaceId", (actorMessageHeaders?.InterfaceId ?? 0).ToString()},
                    {"MethodId", (actorMessageHeaders?.MethodId ?? 0).ToString()},
                    {"ActorId", actorMessageHeaders?.ActorId.ToString()},
                    {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}



		public void FailedToReadActorMessageHeaders(
			Microsoft.ServiceFabric.Services.Remoting.ServiceRemotingMessageHeaders serviceRemotingMessageHeaders,
			System.Exception ex)
		{
			PersonActorEventSource.Current.FailedToReadActorMessageHeaders(
				_actor, 
				serviceRemotingMessageHeaders, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "FailedToReadActorMessageHeaders" },
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ActorId", _actor.ActorId.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"InterfaceId", (serviceRemotingMessageHeaders?.InterfaceId ?? 0).ToString()},
                    {"MethodId", (serviceRemotingMessageHeaders?.MethodId ?? 0).ToString()},
                    {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}




        public System.IDisposable RecieveServiceMessage(
			System.Uri requestUri,
			string serviceMethodName,
			Microsoft.ServiceFabric.Services.Remoting.ServiceRemotingMessageHeaders serviceMessageHeaders,
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader)
		{
		    return new ScopeWrapper(new IDisposable[]
		    {

                ScopeWrapperWithAction.Wrap(() =>
		        {
			PersonActorEventSource.Current.StartRecieveServiceMessage(
				_actor, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			PersonActorEventSource.Current.StopRecieveServiceMessage(
				_actor, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader
			);
    
		            });
		        }),


                ScopeWrapperWithAction.Wrap(() =>
		        {

			            var recieveServiceMessageOperationHolder = _telemetryClient.StartOperation<RequestTelemetry>(requestUri.ToString() ?? "recieveServiceMessage");
			            recieveServiceMessageOperationHolder.Telemetry.Properties.Add("ActorType", _actor.ActorType.ToString());
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("ActorId", _actor.ActorId.ToString());
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("ApplicationTypeName", _actor.ApplicationTypeName);
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("ApplicationName", _actor.ApplicationName);
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("ServiceTypeName", _actor.ServiceTypeName);
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("ServiceName", _actor.ServiceName);
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("PartitionId", _actor.PartitionId.ToString());
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString());
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("NodeName", _actor.NodeName);
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("RequestUri", requestUri.ToString());
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("ServiceMethodName", serviceMethodName);
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("InterfaceId", (serviceMessageHeaders?.InterfaceId ?? 0).ToString());
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("MethodId", (serviceMessageHeaders?.MethodId ?? 0).ToString());
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("UserId", customServiceRequestHeader?.GetHeader("userId"));
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("CorrelationId", customServiceRequestHeader?.GetHeader("correlationId"));
    
		            return new ScopeWrapperWithAction(() =>
		            {

			            _telemetryClient.StopOperation<RequestTelemetry>(recieveServiceMessageOperationHolder);
    
		            });
		        }),


		    });
		}





		public void RecieveServiceMessageFailed(
			System.Uri requestUri,
			string serviceMethodName,
			Microsoft.ServiceFabric.Services.Remoting.ServiceRemotingMessageHeaders serviceMessageHeaders,
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader,
			System.Exception ex)
		{
			PersonActorEventSource.Current.RecieveServiceMessageFailed(
				_actor, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "RecieveServiceMessageFailed" },
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ActorId", _actor.ActorId.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"RequestUri", requestUri.ToString()},
                    {"ServiceMethodName", serviceMethodName},
                    {"InterfaceId", (serviceMessageHeaders?.InterfaceId ?? 0).ToString()},
                    {"MethodId", (serviceMessageHeaders?.MethodId ?? 0).ToString()},
                    {"UserId", customServiceRequestHeader?.GetHeader("userId")},
                    {"CorrelationId", customServiceRequestHeader?.GetHeader("correlationId")},
                    {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}



		public void FailedToGetServiceMethodName(
			System.Uri requestUri,
			int interfaceId,
			int methodId,
			System.Exception ex)
		{
			PersonActorEventSource.Current.FailedToGetServiceMethodName(
				_actor, 
				requestUri, 
				interfaceId, 
				methodId, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "FailedToGetServiceMethodName" },
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ActorId", _actor.ActorId.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"RequestUri", requestUri.ToString()},
                    {"InterfaceId", interfaceId.ToString()},
                    {"MethodId", methodId.ToString()},
                    {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}



		public void StartRequestContext(
			System.Collections.Generic.IEnumerable<FG.ServiceFabric.Services.Remoting.FabricTransport.ServiceRequestHeader> headers)
		{
			PersonActorEventSource.Current.StartRequestContext(
				_actor, 
				headers
			);

			var requestContextOperationHolder = _telemetryClient.StartOperation<RequestTelemetry>("requestContext");
	       requestContextOperationHolder.Telemetry.Properties.Add("ActorType", _actor.ActorType.ToString());
			requestContextOperationHolder.Telemetry.Properties.Add("ActorId", _actor.ActorId.ToString());
			requestContextOperationHolder.Telemetry.Properties.Add("ApplicationTypeName", _actor.ApplicationTypeName);
			requestContextOperationHolder.Telemetry.Properties.Add("ApplicationName", _actor.ApplicationName);
			requestContextOperationHolder.Telemetry.Properties.Add("ServiceTypeName", _actor.ServiceTypeName);
			requestContextOperationHolder.Telemetry.Properties.Add("ServiceName", _actor.ServiceName);
			requestContextOperationHolder.Telemetry.Properties.Add("PartitionId", _actor.PartitionId.ToString());
			requestContextOperationHolder.Telemetry.Properties.Add("ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString());
			requestContextOperationHolder.Telemetry.Properties.Add("NodeName", _actor.NodeName);
			requestContextOperationHolder.Telemetry.Properties.Add("Headers", headers.ToString());
	       OperationHolder.StartOperation(requestContextOperationHolder);
    
		}



		public void StopRequestContext(
			System.Collections.Generic.IEnumerable<FG.ServiceFabric.Services.Remoting.FabricTransport.ServiceRequestHeader> headers)
		{
			PersonActorEventSource.Current.StopRequestContext(
				_actor, 
				headers
			);

			var requestContextOperationHolder = OperationHolder.StopOperation();
			_telemetryClient.StopOperation(requestContextOperationHolder);
			requestContextOperationHolder.Dispose();
    
		}



		public void FailedRequestContext(
			System.Collections.Generic.IEnumerable<FG.ServiceFabric.Services.Remoting.FabricTransport.ServiceRequestHeader> headers,
			System.Exception exception)
		{
			PersonActorEventSource.Current.FailedRequestContext(
				_actor, 
				headers, 
				exception
			);
			_telemetryClient.TrackException(
	            exception,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "FailedRequestContext" },
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ActorId", _actor.ActorId.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"Headers", headers.ToString()},
                    {"Message", exception.Message},
                    {"Source", exception.Source},
                    {"ExceptionTypeName", exception.GetType().FullName},
                    {"Exception", exception.AsJson()}
	            });
    
		}



		public void FailedToReadCustomServiceMessageHeader(
			Microsoft.ServiceFabric.Services.Remoting.ServiceRemotingMessageHeaders serviceRemotingMessageHeaders,
			System.Exception ex)
		{
			PersonActorEventSource.Current.FailedToReadCustomServiceMessageHeader(
				_actor, 
				serviceRemotingMessageHeaders, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "FailedToReadCustomServiceMessageHeader" },
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ActorId", _actor.ActorId.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"InterfaceId", (serviceRemotingMessageHeaders?.InterfaceId ?? 0).ToString()},
                    {"MethodId", (serviceRemotingMessageHeaders?.MethodId ?? 0).ToString()},
                    {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}



		public void EnumeratingPartitions(
			System.Uri serviceUri)
		{
			PersonActorEventSource.Current.EnumeratingPartitions(
				_actor, 
				serviceUri
			);
			_telemetryClient.TrackEvent(
	            nameof(EnumeratingPartitions),
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ActorId", _actor.ActorId.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"ServiceUri", serviceUri.ToString()}
	            });
    
		}



		public void FailedToEnumeratePartitions(
			System.Uri serviceUri,
			System.Exception ex)
		{
			PersonActorEventSource.Current.FailedToEnumeratePartitions(
				_actor, 
				serviceUri, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "FailedToEnumeratePartitions" },
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ActorId", _actor.ActorId.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"ServiceUri", serviceUri.ToString()},
                    {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}



		public void EnumeratedExistingPartitions(
			System.Uri serviceUri,
			System.Collections.Generic.IEnumerable<System.Fabric.ServicePartitionInformation> partitions)
		{
			PersonActorEventSource.Current.EnumeratedExistingPartitions(
				_actor, 
				serviceUri, 
				partitions
			);
			_telemetryClient.TrackEvent(
	            nameof(EnumeratedExistingPartitions),
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ActorId", _actor.ActorId.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"ServiceUri", serviceUri.ToString()},
                    {"Partitions", partitions.ToString()}
	            });
    
		}



		public void EnumeratedAndCachedPartitions(
			System.Uri serviceUri,
			System.Collections.Generic.IEnumerable<System.Fabric.ServicePartitionInformation> partitions)
		{
			PersonActorEventSource.Current.EnumeratedAndCachedPartitions(
				_actor, 
				serviceUri, 
				partitions
			);
			_telemetryClient.TrackEvent(
	            nameof(EnumeratedAndCachedPartitions),
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ActorId", _actor.ActorId.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"ServiceUri", serviceUri.ToString()},
                    {"Partitions", partitions.ToString()}
	            });
    
		}




        public System.IDisposable CallActor(
			System.Uri requestUri,
			string actorMethodName,
			FG.ServiceFabric.Actors.Remoting.Runtime.ActorMessageHeaders actorMessageHeaders,
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader)
		{
		    return new ScopeWrapper(new IDisposable[]
		    {

                ScopeWrapperWithAction.Wrap(() =>
		        {
			PersonActorEventSource.Current.StartCallActor(
				_actor, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			PersonActorEventSource.Current.StopCallActor(
				_actor, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader
			);
    
		            });
		        }),


                ScopeWrapperWithAction.Wrap(() =>
		        {

			            var callActorOperationHolder = _telemetryClient.StartOperation<DependencyTelemetry>(requestUri.ToString() ?? "callActor");
			            callActorOperationHolder.Telemetry.Properties.Add("ActorType", _actor.ActorType.ToString());
			callActorOperationHolder.Telemetry.Properties.Add("ApplicationTypeName", _actor.ApplicationTypeName);
			callActorOperationHolder.Telemetry.Properties.Add("ApplicationName", _actor.ApplicationName);
			callActorOperationHolder.Telemetry.Properties.Add("ServiceTypeName", _actor.ServiceTypeName);
			callActorOperationHolder.Telemetry.Properties.Add("ServiceName", _actor.ServiceName);
			callActorOperationHolder.Telemetry.Properties.Add("PartitionId", _actor.PartitionId.ToString());
			callActorOperationHolder.Telemetry.Properties.Add("ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString());
			callActorOperationHolder.Telemetry.Properties.Add("NodeName", _actor.NodeName);
			callActorOperationHolder.Telemetry.Properties.Add("RequestUri", requestUri.ToString());
			callActorOperationHolder.Telemetry.Properties.Add("ActorMethodName", actorMethodName);
			callActorOperationHolder.Telemetry.Properties.Add("InterfaceId", (actorMessageHeaders?.InterfaceId ?? 0).ToString());
			callActorOperationHolder.Telemetry.Properties.Add("MethodId", (actorMessageHeaders?.MethodId ?? 0).ToString());
			callActorOperationHolder.Telemetry.Properties.Add("ActorId", actorMessageHeaders?.ActorId.ToString());
			callActorOperationHolder.Telemetry.Properties.Add("UserId", customServiceRequestHeader?.GetHeader("userId"));
			callActorOperationHolder.Telemetry.Properties.Add("CorrelationId", customServiceRequestHeader?.GetHeader("correlationId"));
    
		            return new ScopeWrapperWithAction(() =>
		            {

			            _telemetryClient.StopOperation<DependencyTelemetry>(callActorOperationHolder);
    
		            });
		        }),


		    });
		}





		public void CallActorFailed(
			System.Uri requestUri,
			string actorMethodName,
			FG.ServiceFabric.Actors.Remoting.Runtime.ActorMessageHeaders actorMessageHeaders,
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader,
			System.Exception ex)
		{
			PersonActorEventSource.Current.CallActorFailed(
				_actor, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "CallActorFailed" },
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"RequestUri", requestUri.ToString()},
                    {"ActorMethodName", actorMethodName},
                    {"InterfaceId", (actorMessageHeaders?.InterfaceId ?? 0).ToString()},
                    {"MethodId", (actorMessageHeaders?.MethodId ?? 0).ToString()},
                    {"ActorId", actorMessageHeaders?.ActorId.ToString()},
                    {"UserId", customServiceRequestHeader?.GetHeader("userId")},
                    {"CorrelationId", customServiceRequestHeader?.GetHeader("correlationId")},
                    {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}




        public System.IDisposable CallService(
			System.Uri requestUri,
			string serviceMethodName,
			Microsoft.ServiceFabric.Services.Remoting.ServiceRemotingMessageHeaders serviceMessageHeaders,
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader)
		{
		    return new ScopeWrapper(new IDisposable[]
		    {

                ScopeWrapperWithAction.Wrap(() =>
		        {
			PersonActorEventSource.Current.StartCallService(
				_actor, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			PersonActorEventSource.Current.StopCallService(
				_actor, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader
			);
    
		            });
		        }),


                ScopeWrapperWithAction.Wrap(() =>
		        {

			            var callServiceOperationHolder = _telemetryClient.StartOperation<DependencyTelemetry>(requestUri.ToString() ?? "callService");
			            callServiceOperationHolder.Telemetry.Properties.Add("ActorType", _actor.ActorType.ToString());
			callServiceOperationHolder.Telemetry.Properties.Add("ActorId", _actor.ActorId.ToString());
			callServiceOperationHolder.Telemetry.Properties.Add("ApplicationTypeName", _actor.ApplicationTypeName);
			callServiceOperationHolder.Telemetry.Properties.Add("ApplicationName", _actor.ApplicationName);
			callServiceOperationHolder.Telemetry.Properties.Add("ServiceTypeName", _actor.ServiceTypeName);
			callServiceOperationHolder.Telemetry.Properties.Add("ServiceName", _actor.ServiceName);
			callServiceOperationHolder.Telemetry.Properties.Add("PartitionId", _actor.PartitionId.ToString());
			callServiceOperationHolder.Telemetry.Properties.Add("ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString());
			callServiceOperationHolder.Telemetry.Properties.Add("NodeName", _actor.NodeName);
			callServiceOperationHolder.Telemetry.Properties.Add("RequestUri", requestUri.ToString());
			callServiceOperationHolder.Telemetry.Properties.Add("ServiceMethodName", serviceMethodName);
			callServiceOperationHolder.Telemetry.Properties.Add("InterfaceId", (serviceMessageHeaders?.InterfaceId ?? 0).ToString());
			callServiceOperationHolder.Telemetry.Properties.Add("MethodId", (serviceMessageHeaders?.MethodId ?? 0).ToString());
			callServiceOperationHolder.Telemetry.Properties.Add("UserId", customServiceRequestHeader?.GetHeader("userId"));
			callServiceOperationHolder.Telemetry.Properties.Add("CorrelationId", customServiceRequestHeader?.GetHeader("correlationId"));
    
		            return new ScopeWrapperWithAction(() =>
		            {

			            _telemetryClient.StopOperation<DependencyTelemetry>(callServiceOperationHolder);
    
		            });
		        }),


		    });
		}





		public void CallServiceFailed(
			System.Uri requestUri,
			string serviceMethodName,
			Microsoft.ServiceFabric.Services.Remoting.ServiceRemotingMessageHeaders serviceMessageHeaders,
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader,
			System.Exception ex)
		{
			PersonActorEventSource.Current.CallServiceFailed(
				_actor, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "CallServiceFailed" },
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ActorId", _actor.ActorId.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"RequestUri", requestUri.ToString()},
                    {"ServiceMethodName", serviceMethodName},
                    {"InterfaceId", (serviceMessageHeaders?.InterfaceId ?? 0).ToString()},
                    {"MethodId", (serviceMessageHeaders?.MethodId ?? 0).ToString()},
                    {"UserId", customServiceRequestHeader?.GetHeader("userId")},
                    {"CorrelationId", customServiceRequestHeader?.GetHeader("correlationId")},
                    {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}



		public void ServiceClientFailed(
			System.Uri requestUri,
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader,
			System.Exception ex)
		{
			PersonActorEventSource.Current.ServiceClientFailed(
				_actor, 
				requestUri, 
				customServiceRequestHeader, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "ServiceClientFailed" },
	                {"ActorType", _actor.ActorType.ToString()},
                    {"ActorId", _actor.ActorId.ToString()},
                    {"ApplicationTypeName", _actor.ApplicationTypeName},
                    {"ApplicationName", _actor.ApplicationName},
                    {"ServiceTypeName", _actor.ServiceTypeName},
                    {"ServiceName", _actor.ServiceName},
                    {"PartitionId", _actor.PartitionId.ToString()},
                    {"ReplicaOrInstanceId", _actor.ReplicaOrInstanceId.ToString()},
                    {"NodeName", _actor.NodeName},
                    {"RequestUri", requestUri.ToString()},
                    {"UserId", customServiceRequestHeader?.GetHeader("userId")},
                    {"CorrelationId", customServiceRequestHeader?.GetHeader("correlationId")},
                    {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}



	}
}
