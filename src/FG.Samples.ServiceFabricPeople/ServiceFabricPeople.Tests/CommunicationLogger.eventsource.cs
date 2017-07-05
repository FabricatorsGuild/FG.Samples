/*******************************************************************************************
*  This class is autogenerated from the class CommunicationLogger
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;
using System.Collections.Generic;
using ServiceFabricPeople.Tests;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Runtime.Remoting.Messaging;


namespace ServiceFabricPeople.Tests
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


		private readonly bool _autogenerated;
		private readonly string _machineName;
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
			bool autogenerated,
			string machineName)
		{
			_autogenerated = autogenerated;
			_machineName = machineName;
			
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
			ServiceFabricPeopleTestsEventSource.Current.StartRecieveActorMessage(
				_autogenerated, 
				_machineName, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			ServiceFabricPeopleTestsEventSource.Current.StopRecieveActorMessage(
				_autogenerated, 
				_machineName, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader
			);
    
		            });
		        }),


                ScopeWrapperWithAction.Wrap(() =>
		        {

			            var recieveActorMessageOperationHolder = _telemetryClient.StartOperation<RequestTelemetry>(customServiceRequestHeader.ToString() ?? "recieveActorMessage");
			            recieveActorMessageOperationHolder.Telemetry.Properties.Add("Autogenerated", _autogenerated.ToString());
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("MachineName", Environment.MachineName);
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("RequestUri", requestUri.ToString());
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("ActorMethodName", actorMethodName);
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("ActorMessageHeaders", actorMessageHeaders.ToString());
			recieveActorMessageOperationHolder.Telemetry.Properties.Add("CustomServiceRequestHeader", customServiceRequestHeader.ToString());
    
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
			ServiceFabricPeopleTestsEventSource.Current.RecieveActorMessageFailed(
				_autogenerated, 
				_machineName, 
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
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
                    {"RequestUri", requestUri.ToString()},
                    {"ActorMethodName", actorMethodName},
                    {"ActorMessageHeaders", actorMessageHeaders.ToString()},
                    {"CustomServiceRequestHeader", customServiceRequestHeader.ToString()},
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
			ServiceFabricPeopleTestsEventSource.Current.FailedToGetActorMethodName(
				_autogenerated, 
				_machineName, 
				actorMessageHeaders, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "FailedToGetActorMethodName" },
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
                    {"ActorMessageHeaders", actorMessageHeaders.ToString()},
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
			ServiceFabricPeopleTestsEventSource.Current.FailedToReadActorMessageHeaders(
				_autogenerated, 
				_machineName, 
				serviceRemotingMessageHeaders, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "FailedToReadActorMessageHeaders" },
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
                    {"ServiceRemotingMessageHeaders", serviceRemotingMessageHeaders.ToString()},
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
			ServiceFabricPeopleTestsEventSource.Current.StartRecieveServiceMessage(
				_autogenerated, 
				_machineName, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			ServiceFabricPeopleTestsEventSource.Current.StopRecieveServiceMessage(
				_autogenerated, 
				_machineName, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader
			);
    
		            });
		        }),


                ScopeWrapperWithAction.Wrap(() =>
		        {

			            var recieveServiceMessageOperationHolder = _telemetryClient.StartOperation<RequestTelemetry>(customServiceRequestHeader.ToString() ?? "recieveServiceMessage");
			            recieveServiceMessageOperationHolder.Telemetry.Properties.Add("Autogenerated", _autogenerated.ToString());
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("MachineName", Environment.MachineName);
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("RequestUri", requestUri.ToString());
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("ServiceMethodName", serviceMethodName);
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("ServiceMessageHeaders", serviceMessageHeaders.ToString());
			recieveServiceMessageOperationHolder.Telemetry.Properties.Add("CustomServiceRequestHeader", customServiceRequestHeader.ToString());
    
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
			ServiceFabricPeopleTestsEventSource.Current.RecieveServiceMessageFailed(
				_autogenerated, 
				_machineName, 
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
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
                    {"RequestUri", requestUri.ToString()},
                    {"ServiceMethodName", serviceMethodName},
                    {"ServiceMessageHeaders", serviceMessageHeaders.ToString()},
                    {"CustomServiceRequestHeader", customServiceRequestHeader.ToString()},
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
			ServiceFabricPeopleTestsEventSource.Current.FailedToGetServiceMethodName(
				_autogenerated, 
				_machineName, 
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
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
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
			ServiceFabricPeopleTestsEventSource.Current.StartRequestContext(
				_autogenerated, 
				_machineName, 
				headers
			);

			var requestContextOperationHolder = _telemetryClient.StartOperation<RequestTelemetry>("requestContext");
	       requestContextOperationHolder.Telemetry.Properties.Add("Autogenerated", _autogenerated.ToString());
			requestContextOperationHolder.Telemetry.Properties.Add("MachineName", Environment.MachineName);
			requestContextOperationHolder.Telemetry.Properties.Add("Headers", headers.ToString());
	       OperationHolder.StartOperation(requestContextOperationHolder);
    
		}



		public void StopRequestContext(
			System.Collections.Generic.IEnumerable<FG.ServiceFabric.Services.Remoting.FabricTransport.ServiceRequestHeader> headers)
		{
			ServiceFabricPeopleTestsEventSource.Current.StopRequestContext(
				_autogenerated, 
				_machineName, 
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
			ServiceFabricPeopleTestsEventSource.Current.FailedRequestContext(
				_autogenerated, 
				_machineName, 
				headers, 
				exception
			);
			_telemetryClient.TrackException(
	            exception,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "FailedRequestContext" },
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
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
			ServiceFabricPeopleTestsEventSource.Current.FailedToReadCustomServiceMessageHeader(
				_autogenerated, 
				_machineName, 
				serviceRemotingMessageHeaders, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "FailedToReadCustomServiceMessageHeader" },
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
                    {"ServiceRemotingMessageHeaders", serviceRemotingMessageHeaders.ToString()},
                    {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}



		public void EnumeratingPartitions(
			System.Uri serviceUri)
		{
			ServiceFabricPeopleTestsEventSource.Current.EnumeratingPartitions(
				_autogenerated, 
				_machineName, 
				serviceUri
			);
			_telemetryClient.TrackEvent(
	            nameof(EnumeratingPartitions),
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
                    {"ServiceUri", serviceUri.ToString()}
	            });
    
		}



		public void FailedToEnumeratePartitions(
			System.Uri serviceUri,
			System.Exception ex)
		{
			ServiceFabricPeopleTestsEventSource.Current.FailedToEnumeratePartitions(
				_autogenerated, 
				_machineName, 
				serviceUri, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "FailedToEnumeratePartitions" },
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
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
			ServiceFabricPeopleTestsEventSource.Current.EnumeratedExistingPartitions(
				_autogenerated, 
				_machineName, 
				serviceUri, 
				partitions
			);
			_telemetryClient.TrackEvent(
	            nameof(EnumeratedExistingPartitions),
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
                    {"ServiceUri", serviceUri.ToString()},
                    {"Partitions", partitions.ToString()}
	            });
    
		}



		public void EnumeratedAndCachedPartitions(
			System.Uri serviceUri,
			System.Collections.Generic.IEnumerable<System.Fabric.ServicePartitionInformation> partitions)
		{
			ServiceFabricPeopleTestsEventSource.Current.EnumeratedAndCachedPartitions(
				_autogenerated, 
				_machineName, 
				serviceUri, 
				partitions
			);
			_telemetryClient.TrackEvent(
	            nameof(EnumeratedAndCachedPartitions),
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
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
			ServiceFabricPeopleTestsEventSource.Current.StartCallActor(
				_autogenerated, 
				_machineName, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			ServiceFabricPeopleTestsEventSource.Current.StopCallActor(
				_autogenerated, 
				_machineName, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader
			);
    
		            });
		        }),


                ScopeWrapperWithAction.Wrap(() =>
		        {

			            var callActorOperationHolder = _telemetryClient.StartOperation<DependencyTelemetry>(customServiceRequestHeader.ToString() ?? "callActor");
			            callActorOperationHolder.Telemetry.Properties.Add("Autogenerated", _autogenerated.ToString());
			callActorOperationHolder.Telemetry.Properties.Add("MachineName", Environment.MachineName);
			callActorOperationHolder.Telemetry.Properties.Add("RequestUri", requestUri.ToString());
			callActorOperationHolder.Telemetry.Properties.Add("ActorMethodName", actorMethodName);
			callActorOperationHolder.Telemetry.Properties.Add("ActorMessageHeaders", actorMessageHeaders.ToString());
			callActorOperationHolder.Telemetry.Properties.Add("CustomServiceRequestHeader", customServiceRequestHeader.ToString());
    
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
			ServiceFabricPeopleTestsEventSource.Current.CallActorFailed(
				_autogenerated, 
				_machineName, 
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
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
                    {"RequestUri", requestUri.ToString()},
                    {"ActorMethodName", actorMethodName},
                    {"ActorMessageHeaders", actorMessageHeaders.ToString()},
                    {"CustomServiceRequestHeader", customServiceRequestHeader.ToString()},
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
			ServiceFabricPeopleTestsEventSource.Current.StartCallService(
				_autogenerated, 
				_machineName, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			ServiceFabricPeopleTestsEventSource.Current.StopCallService(
				_autogenerated, 
				_machineName, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader
			);
    
		            });
		        }),


                ScopeWrapperWithAction.Wrap(() =>
		        {

			            var callServiceOperationHolder = _telemetryClient.StartOperation<DependencyTelemetry>(customServiceRequestHeader.ToString() ?? "callService");
			            callServiceOperationHolder.Telemetry.Properties.Add("Autogenerated", _autogenerated.ToString());
			callServiceOperationHolder.Telemetry.Properties.Add("MachineName", Environment.MachineName);
			callServiceOperationHolder.Telemetry.Properties.Add("RequestUri", requestUri.ToString());
			callServiceOperationHolder.Telemetry.Properties.Add("ServiceMethodName", serviceMethodName);
			callServiceOperationHolder.Telemetry.Properties.Add("ServiceMessageHeaders", serviceMessageHeaders.ToString());
			callServiceOperationHolder.Telemetry.Properties.Add("CustomServiceRequestHeader", customServiceRequestHeader.ToString());
    
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
			ServiceFabricPeopleTestsEventSource.Current.CallServiceFailed(
				_autogenerated, 
				_machineName, 
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
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
                    {"RequestUri", requestUri.ToString()},
                    {"ServiceMethodName", serviceMethodName},
                    {"ServiceMessageHeaders", serviceMessageHeaders.ToString()},
                    {"CustomServiceRequestHeader", customServiceRequestHeader.ToString()},
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
			ServiceFabricPeopleTestsEventSource.Current.ServiceClientFailed(
				_autogenerated, 
				_machineName, 
				requestUri, 
				customServiceRequestHeader, 
				ex
			);
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "ServiceClientFailed" },
	                {"Autogenerated", _autogenerated.ToString()},
                    {"MachineName", Environment.MachineName},
                    {"RequestUri", requestUri.ToString()},
                    {"CustomServiceRequestHeader", customServiceRequestHeader.ToString()},
                    {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}



	}
}
