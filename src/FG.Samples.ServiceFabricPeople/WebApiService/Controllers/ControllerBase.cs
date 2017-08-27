using System;
using System.Threading.Tasks;
using Application;
using FG.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiService.Diagnostics;

namespace WebApiService.Controllers
{
	public class ControllerBase : Controller, ILoggableController
	{
		public IWebApiLogger Logger { get; }
		public IDisposable RequestLoggingContext { get; set; }
						
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			var valuesController = context.Controller as ILoggableController;
			if (context?.Exception != null)
			{
				valuesController?.Logger.RecieveWebApiRequestFailed(context.HttpContext.Request.GetUri(), context.HttpContext.Request.ToString(), ServiceRequestContext.Current?[ServiceRequestContextWrapperServiceFabricPeople.ServiceRequestContextKeys.CorrelationId], ServiceRequestContext.Current?[ServiceRequestContextWrapperServiceFabricPeople.ServiceRequestContextKeys.UserId], context.Exception);
			}
			
			valuesController?.RequestLoggingContext?.Dispose();
			base.OnActionExecuted(context);
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var valuesController = context.Controller as ILoggableController;
			if (valuesController != null)
			{
				valuesController.RequestLoggingContext = valuesController.Logger.RecieveWebApiRequest(
					requestUri: context.HttpContext.Request.GetUri(),
					payload: "",
					correlationId: ServiceRequestContext.Current?[ServiceRequestContextWrapperServiceFabricPeople.ServiceRequestContextKeys.CorrelationId],
					userId: ServiceRequestContext.Current?[ServiceRequestContextWrapperServiceFabricPeople.ServiceRequestContextKeys.UserId]);
			}
			base.OnActionExecuting(context);
		}

		public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var valuesController = context.Controller as ILoggableController;
			if (valuesController != null)
			{
				valuesController.RequestLoggingContext = valuesController.Logger.RecieveWebApiRequest(
					requestUri: context.HttpContext.Request.GetUri(),
					payload: "",
					correlationId: ServiceRequestContext.Current?[ServiceRequestContextWrapperServiceFabricPeople.ServiceRequestContextKeys.CorrelationId],
					userId: ServiceRequestContext.Current?[ServiceRequestContextWrapperServiceFabricPeople.ServiceRequestContextKeys.UserId]);
			}
			
			return base.OnActionExecutionAsync(context, next);
		}
	}
}