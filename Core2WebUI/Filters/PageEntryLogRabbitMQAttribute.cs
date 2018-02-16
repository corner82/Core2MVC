using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core2WebUI.Core.RabbitMQ;
using Core2WebUI.Entities.Log;
using Microsoft.AspNetCore.Http;
using Core2WebUI.Core.Extensions.Session;
using Core2WebUI.Core.Utills;

namespace Core2WebUI.Filters
{
    public class PageEntryLogRabbitMQAttribute : ActionFilterAttribute
    {
        private readonly PageEntryLogPublisher _pageEntryLogPublisher;
        private  HttpContext _httpContext;
        private readonly RemoteAddressFinder _remoteAdressFinder;
        private PageAccessLogModel _pageAccessLogModel;
        public PageEntryLogRabbitMQAttribute(PageEntryLogPublisher pageEntryLogPublisher,
                                            RemoteAddressFinder remoteAdressFinder,
                                            PageAccessLogModel pageAccesslogModel)
        {
            _pageEntryLogPublisher = pageEntryLogPublisher;
            _remoteAdressFinder = remoteAdressFinder;
            _pageAccessLogModel = pageAccesslogModel;
        }

        public override void OnActionExecuting(ActionExecutingContext context)

        {
            _httpContext = context.HttpContext;
            var headers = context.HttpContext.Request.Headers;
            var host = context.HttpContext.Request.Host;
            var routeValues = context.ActionDescriptor.RouteValues;


            _pageAccessLogModel.UserAgent = headers.Where(x => x.Key == "HeaderUserAgent").FirstOrDefault().Value;
            _pageAccessLogModel.Host = _httpContext.Request.Host.Host;
            _pageAccessLogModel.Port = Convert.ToInt32(_httpContext.Request.Host.Port);
            _pageAccessLogModel.Controller = routeValues.Where(x => x.Key == "controller").FirstOrDefault().Value;
            _pageAccessLogModel.Action = routeValues.Where(x => x.Key == "action").FirstOrDefault().Value;
            _pageAccessLogModel.SessionID = _httpContext.Session.Id;
            _pageAccessLogModel.Method = _httpContext.Request.Method;
            _pageAccessLogModel.UserName = _httpContext.Session.GetUserName();
            _pageAccessLogModel.UserPrivateKey = _httpContext.Session.GetUserPrivateKey();
            _pageAccessLogModel.UserPublicKey = _httpContext.Session.GetUserPublicKey();
            _pageAccessLogModel.UserToken = _httpContext.Session.GetHmacToken();
            _pageAccessLogModel.UserIP = _remoteAdressFinder.GetRequestIP();

            
            var controller = context.Controller.ToString();
            var actionProperties = context.ActionDescriptor.Properties;
            var idsplayName = context.ActionDescriptor.DisplayName;
            var actionConstratints = context.ActionDescriptor.ActionConstraints;
            var attribuetRouteInfo = context.ActionDescriptor.AttributeRouteInfo;
            var test = context.ActionDescriptor.Parameters;
            var queryString = context.HttpContext.Request.QueryString;
            var method = context.HttpContext.Request.Method;
            var path = context.HttpContext.Request.Path;
            
            _pageEntryLogPublisher.PageEntryLogPublish(context);
            base.OnActionExecuting(context);
        }
    }
}
