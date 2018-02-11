using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core2WebUI.Core.RabbitMQ;

namespace Core2WebUI.Filters
{
    public class PageEntryLogRabbitMQAttribute : ActionFilterAttribute
    {
        private readonly PageEntryLogPublisher _pageEntryLogPublisher;
        public PageEntryLogRabbitMQAttribute(PageEntryLogPublisher pageEntryLogPublisher)
        {
            _pageEntryLogPublisher = pageEntryLogPublisher;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _pageEntryLogPublisher.PageEntryLogPublish();
            base.OnActionExecuting(context);
        }
    }
}
