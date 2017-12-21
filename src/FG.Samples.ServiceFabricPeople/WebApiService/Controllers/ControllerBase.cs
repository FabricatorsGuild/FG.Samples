using System;
using System.Fabric;
using System.Globalization;
using System.Threading.Tasks;
using Application;
using FG.Common.Utils;
using FG.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiService.Diagnostics;

namespace WebApiService.Controllers
{
	public class ControllerBase : Controller, ILoggableController, IDisposable
	{
		private readonly ServiceFabricPeopleContext _contextScope;

		public ICommunicationLogger ServicesCommunicationLogger { get; protected set; }
		public IWebApiLogger Logger { get; private set; }

		public IDisposable RequestLoggingContext { get; set; }

		protected ServiceFabricPeopleContext ContextScope => _contextScope;

		public ControllerBase(StatelessServiceContext context)
		{
			var correlationId = Guid.NewGuid().ToString();
			_contextScope = new ServiceFabricPeopleContext(
				correlationId: correlationId,
				userId: "",
				authToken: "",
				tenantId: "");

			Logger = new WebApiLogger(context);
		    ServicesCommunicationLogger = new CommunicationLogger(context);

			Logger.ActivatingController(ContextScope.CorrelationId, ContextScope.UserId);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			_contextScope.Dispose();
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			var valuesController = context.Controller as ILoggableController;
			if (context?.Exception != null)
			{
				var contextWrapper = ServiceFabricPeopleContext.Current;
				valuesController?.Logger.RecieveWebApiRequestFailed(
					context.HttpContext.Request.GetUri(), 
					context.HttpContext.Request.ToString(),
					contextWrapper.CorrelationId,
					contextWrapper.UserId, 
					context.Exception);
			}
			
			valuesController?.RequestLoggingContext?.Dispose();
			base.OnActionExecuted(context);
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			_contextScope.UserId = GetUserName();
			_contextScope.AuthToken = $"{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}|{_contextScope.UserId}|{_contextScope.CorrelationId}".ToBase64();
			_contextScope.TenantId = GetTenantId();

			var valuesController = context.Controller as ILoggableController;
			if (valuesController != null)
			{
				var contextWrapper = ServiceFabricPeopleContext.Current;
				valuesController.RequestLoggingContext = valuesController.Logger.RecieveWebApiRequest(
					requestUri: context.HttpContext.Request.GetUri(),
					payload: "",
					correlationId: contextWrapper.CorrelationId,
					userId: contextWrapper.UserId);
			}
			base.OnActionExecuting(context);
		}

		public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			_contextScope.UserId = GetUserName();
			_contextScope.AuthToken = $"{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}|{_contextScope.UserId}|{_contextScope.CorrelationId}".ToBase64();
			_contextScope.TenantId = GetTenantId();

			var valuesController = context.Controller as ILoggableController;
			if (valuesController != null)
			{
				var contextWrapper = ServiceFabricPeopleContext.Current;
				valuesController.RequestLoggingContext = valuesController.Logger.RecieveWebApiRequest(
					requestUri: context.HttpContext.Request.GetUri(),
					payload: "",
					correlationId: contextWrapper.CorrelationId,
					userId: contextWrapper.UserId);
			}
			
			return base.OnActionExecutionAsync(context, next);
		}

		private string GetUserName()
		{
			if (this.HttpContext?.Request.Headers.ContainsKey("userName") ?? false)
			{
				return this.HttpContext.Request.Headers["userName"];
			}

			if (this.HttpContext?.Request.Query.ContainsKey("userName") ?? false)
			{
				return this.HttpContext.Request.Query["userName"];
			}

			return @"domain\john_doe";
		}

		private string GetTenantId()
		{
			if (this.HttpContext?.Request.Headers.ContainsKey("tenantId") ?? false)
			{
				return this.HttpContext.Request.Headers["tenantId"];
			}

			if (this.HttpContext?.Request.Query.ContainsKey("tenantId") ?? false)
			{
				return this.HttpContext.Request.Query["tenantId"];
			}

			return @"3DCDA8A1-4A16-4B99-9881-F0566FCA3F2D";
		}
	}
}