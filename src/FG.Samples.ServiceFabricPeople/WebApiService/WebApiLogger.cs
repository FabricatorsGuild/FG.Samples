/*******************************************************************************************
*  This class is autogenerated from the class WebApiLogger
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;
using System.Collections.Generic;
using WebApiService.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Runtime.Remoting.Messaging;


namespace WebApiService
{
	internal sealed class WebApiLogger : IWebApiLogger
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


		private readonly System.Fabric.StatelessServiceContext _context;
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

		public WebApiLogger(
			System.Fabric.StatelessServiceContext context)
		{
			_context = context;
			
            _telemetryClient = new Microsoft.ApplicationInsights.TelemetryClient();
            _telemetryClient.Context.User.Id = Environment.UserName;
            _telemetryClient.Context.Session.Id = Guid.NewGuid().ToString();
            _telemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();

		}

		public void ActivatingController(
			string correlationId,
			string userId)
		{
			WebApiServiceEventSource.Current.ActivatingController(
				_context, 
				correlationId, 
				userId
			);
			_telemetryClient.TrackEvent(
	            nameof(ActivatingController),
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
	                {"ServiceName", _context.ServiceName.ToString()},
                    {"ServiceTypeName", _context.ServiceTypeName},
                    {"ReplicaOrInstanceId", _context.InstanceId.ToString()},
                    {"PartitionId", _context.PartitionId.ToString()},
                    {"ApplicationName", _context.CodePackageActivationContext.ApplicationName},
                    {"ApplicationTypeName", _context.CodePackageActivationContext.ApplicationTypeName},
                    {"NodeName", _context.NodeContext.NodeName},
                    {"CorrelationId", correlationId},
                    {"UserId", userId}
	            });
    
		}



		public void StartGetAll(
			)
		{
			WebApiServiceEventSource.Current.StartGetAll(
				_context
			);

			var getAllOperationHolder = _telemetryClient.StartOperation<RequestTelemetry>("getAll");
	       getAllOperationHolder.Telemetry.Properties.Add("ServiceName", _context.ServiceName.ToString());
			getAllOperationHolder.Telemetry.Properties.Add("ServiceTypeName", _context.ServiceTypeName);
			getAllOperationHolder.Telemetry.Properties.Add("ReplicaOrInstanceId", _context.InstanceId.ToString());
			getAllOperationHolder.Telemetry.Properties.Add("PartitionId", _context.PartitionId.ToString());
			getAllOperationHolder.Telemetry.Properties.Add("ApplicationName", _context.CodePackageActivationContext.ApplicationName);
			getAllOperationHolder.Telemetry.Properties.Add("ApplicationTypeName", _context.CodePackageActivationContext.ApplicationTypeName);
			getAllOperationHolder.Telemetry.Properties.Add("NodeName", _context.NodeContext.NodeName);
	       OperationHolder.StartOperation(getAllOperationHolder);
    
		}



		public void StopGetAll(
			)
		{
			WebApiServiceEventSource.Current.StopGetAll(
				_context
			);

			var getAllOperationHolder = OperationHolder.StopOperation();
			_telemetryClient.StopOperation(getAllOperationHolder);
			getAllOperationHolder.Dispose();
    
		}




        public System.IDisposable RecieveWebApiRequest(
			System.Uri requestUri,
			string payload,
			string correlationId,
			string userId)
		{
		    return new ScopeWrapper(new IDisposable[]
		    {

                ScopeWrapperWithAction.Wrap(() =>
		        {
			WebApiServiceEventSource.Current.StartRecieveWebApiRequest(
				_context, 
				requestUri, 
				payload, 
				correlationId, 
				userId
			);
    
		            return new ScopeWrapperWithAction(() =>
		            {
			WebApiServiceEventSource.Current.StopRecieveWebApiRequest(
				_context, 
				requestUri, 
				payload, 
				correlationId, 
				userId
			);
    
		            });
		        }),


                ScopeWrapperWithAction.Wrap(() =>
		        {

			            var recieveWebApiRequestOperationHolder = _telemetryClient.StartOperation<RequestTelemetry>(requestUri.ToString() ?? "recieveWebApiRequest");
			            recieveWebApiRequestOperationHolder.Telemetry.Properties.Add("ServiceName", _context.ServiceName.ToString());
			recieveWebApiRequestOperationHolder.Telemetry.Properties.Add("ServiceTypeName", _context.ServiceTypeName);
			recieveWebApiRequestOperationHolder.Telemetry.Properties.Add("ReplicaOrInstanceId", _context.InstanceId.ToString());
			recieveWebApiRequestOperationHolder.Telemetry.Properties.Add("PartitionId", _context.PartitionId.ToString());
			recieveWebApiRequestOperationHolder.Telemetry.Properties.Add("ApplicationName", _context.CodePackageActivationContext.ApplicationName);
			recieveWebApiRequestOperationHolder.Telemetry.Properties.Add("ApplicationTypeName", _context.CodePackageActivationContext.ApplicationTypeName);
			recieveWebApiRequestOperationHolder.Telemetry.Properties.Add("NodeName", _context.NodeContext.NodeName);
			recieveWebApiRequestOperationHolder.Telemetry.Properties.Add("RequestUri", requestUri.ToString());
			recieveWebApiRequestOperationHolder.Telemetry.Properties.Add("Payload", payload);
			recieveWebApiRequestOperationHolder.Telemetry.Properties.Add("CorrelationId", correlationId);
			recieveWebApiRequestOperationHolder.Telemetry.Properties.Add("UserId", userId);
    
		            return new ScopeWrapperWithAction(() =>
		            {

			            _telemetryClient.StopOperation<RequestTelemetry>(recieveWebApiRequestOperationHolder);
    
		            });
		        }),


		    });
		}





		public void RecieveWebApiRequestFailed(
			System.Uri requestUri,
			string payload,
			string correlationId,
			string userId,
			System.Exception exception)
		{
			WebApiServiceEventSource.Current.RecieveWebApiRequestFailed(
				_context, 
				requestUri, 
				payload, 
				correlationId, 
				userId, 
				exception
			);
			_telemetryClient.TrackException(
	            exception,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "RecieveWebApiRequestFailed" },
	                {"ServiceName", _context.ServiceName.ToString()},
                    {"ServiceTypeName", _context.ServiceTypeName},
                    {"ReplicaOrInstanceId", _context.InstanceId.ToString()},
                    {"PartitionId", _context.PartitionId.ToString()},
                    {"ApplicationName", _context.CodePackageActivationContext.ApplicationName},
                    {"ApplicationTypeName", _context.CodePackageActivationContext.ApplicationTypeName},
                    {"NodeName", _context.NodeContext.NodeName},
                    {"RequestUri", requestUri.ToString()},
                    {"Payload", payload},
                    {"CorrelationId", correlationId},
                    {"UserId", userId},
                    {"Message", exception.Message},
                    {"Source", exception.Source},
                    {"ExceptionTypeName", exception.GetType().FullName},
                    {"Exception", exception.AsJson()}
	            });
    
		}



	}
}
