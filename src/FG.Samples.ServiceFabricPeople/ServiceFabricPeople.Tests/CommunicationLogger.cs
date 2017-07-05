/*******************************************************************************************
*  This class is autogenerated from the class CommunicationLogger
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;
using System.Collections.Generic;
using ServiceFabricPeople.Tests;


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


		private readonly string _testrun;

		public CommunicationLogger(
			string testrun)
		{
			_testrun = testrun;
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
				_testrun, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			ServiceFabricPeopleTestsEventSource.Current.StopRecieveActorMessage(
				_testrun, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader
			);
    
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
				_testrun, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader, 
				ex
			);
    
		}


		public void FailedToGetActorMethodName(
			FG.ServiceFabric.Actors.Remoting.Runtime.ActorMessageHeaders actorMessageHeaders,
			System.Exception ex)
		{
			ServiceFabricPeopleTestsEventSource.Current.FailedToGetActorMethodName(
				_testrun, 
				actorMessageHeaders, 
				ex
			);
    
		}


		public void FailedToReadActorMessageHeaders(
			Microsoft.ServiceFabric.Services.Remoting.ServiceRemotingMessageHeaders serviceRemotingMessageHeaders,
			System.Exception ex)
		{
			ServiceFabricPeopleTestsEventSource.Current.FailedToReadActorMessageHeaders(
				_testrun, 
				serviceRemotingMessageHeaders, 
				ex
			);
    
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
				_testrun, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			ServiceFabricPeopleTestsEventSource.Current.StopRecieveServiceMessage(
				_testrun, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader
			);
    
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
				_testrun, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader, 
				ex
			);
    
		}


		public void FailedToGetServiceMethodName(
			System.Uri requestUri,
			int interfaceId,
			int methodId,
			System.Exception ex)
		{
			ServiceFabricPeopleTestsEventSource.Current.FailedToGetServiceMethodName(
				_testrun, 
				requestUri, 
				interfaceId, 
				methodId, 
				ex
			);
    
		}


		public void StartRequestContext(
			System.Collections.Generic.IEnumerable<FG.ServiceFabric.Services.Remoting.FabricTransport.ServiceRequestHeader> headers)
		{
			ServiceFabricPeopleTestsEventSource.Current.StartRequestContext(
				_testrun, 
				headers
			);
    
		}


		public void StopRequestContext(
			System.Collections.Generic.IEnumerable<FG.ServiceFabric.Services.Remoting.FabricTransport.ServiceRequestHeader> headers)
		{
			ServiceFabricPeopleTestsEventSource.Current.StopRequestContext(
				_testrun, 
				headers
			);
    
		}


		public void FailedRequestContext(
			System.Collections.Generic.IEnumerable<FG.ServiceFabric.Services.Remoting.FabricTransport.ServiceRequestHeader> headers,
			System.Exception exception)
		{
			ServiceFabricPeopleTestsEventSource.Current.FailedRequestContext(
				_testrun, 
				headers, 
				exception
			);
    
		}


		public void FailedToReadCustomServiceMessageHeader(
			Microsoft.ServiceFabric.Services.Remoting.ServiceRemotingMessageHeaders serviceRemotingMessageHeaders,
			System.Exception ex)
		{
			ServiceFabricPeopleTestsEventSource.Current.FailedToReadCustomServiceMessageHeader(
				_testrun, 
				serviceRemotingMessageHeaders, 
				ex
			);
    
		}


		public void EnumeratingPartitions(
			System.Uri serviceUri)
		{
			ServiceFabricPeopleTestsEventSource.Current.EnumeratingPartitions(
				_testrun, 
				serviceUri
			);
    
		}


		public void FailedToEnumeratePartitions(
			System.Uri serviceUri,
			System.Exception ex)
		{
			ServiceFabricPeopleTestsEventSource.Current.FailedToEnumeratePartitions(
				_testrun, 
				serviceUri, 
				ex
			);
    
		}


		public void EnumeratedExistingPartitions(
			System.Uri serviceUri,
			System.Collections.Generic.IEnumerable<System.Fabric.ServicePartitionInformation> partitions)
		{
			ServiceFabricPeopleTestsEventSource.Current.EnumeratedExistingPartitions(
				_testrun, 
				serviceUri, 
				partitions
			);
    
		}


		public void EnumeratedAndCachedPartitions(
			System.Uri serviceUri,
			System.Collections.Generic.IEnumerable<System.Fabric.ServicePartitionInformation> partitions)
		{
			ServiceFabricPeopleTestsEventSource.Current.EnumeratedAndCachedPartitions(
				_testrun, 
				serviceUri, 
				partitions
			);
    
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
				_testrun, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			ServiceFabricPeopleTestsEventSource.Current.StopCallActor(
				_testrun, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader
			);
    
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
				_testrun, 
				requestUri, 
				actorMethodName, 
				actorMessageHeaders, 
				customServiceRequestHeader, 
				ex
			);
    
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
				_testrun, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			ServiceFabricPeopleTestsEventSource.Current.StopCallService(
				_testrun, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader
			);
    
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
				_testrun, 
				requestUri, 
				serviceMethodName, 
				serviceMessageHeaders, 
				customServiceRequestHeader, 
				ex
			);
    
		}


		public void ServiceClientFailed(
			System.Uri requestUri,
			FG.ServiceFabric.Services.Remoting.FabricTransport.CustomServiceRequestHeader customServiceRequestHeader,
			System.Exception ex)
		{
			ServiceFabricPeopleTestsEventSource.Current.ServiceClientFailed(
				_testrun, 
				requestUri, 
				customServiceRequestHeader, 
				ex
			);
    
		}


	}
}
