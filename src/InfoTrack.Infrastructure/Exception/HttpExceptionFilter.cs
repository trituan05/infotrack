using FluentValidation;
using InfoTrack.Commons.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace InfoTrack.Infrastructure.Exception
{
    internal class HttpExceptionFilter : IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<HttpExceptionFilter>>();
            return new InnerFilter(logger);
        }

        private class InnerFilter : IActionFilter, IOrderedFilter
        {
            private readonly ILogger<HttpExceptionFilter> _logger;

            public int Order => int.MaxValue;

            public InnerFilter(ILogger<HttpExceptionFilter> logger)
            {
                _logger = logger;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                if (context.Exception == null)
                    return;

                if (context.Exception is InfoTrackException applicationException)
                {
                    if (applicationException.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        _logger.LogError(applicationException.InnerException ?? applicationException, "An error has occurred");
                    }
                    else
                    {
                        _logger.LogWarning(applicationException, "The normal response could not be returned due to an error: {message}", applicationException.Message);
                    }

                    var responseModel = new ExceptionResponseModel
                    {
                        ErrorCode = applicationException.ErrorCode,
                        Message = applicationException.Message
                    };

                    context.Result = new JsonResult(responseModel)
                    {
                        StatusCode = (int)applicationException.StatusCode
                    };
                }
                else if(context.Exception is ValidationException fluentValidationException)
                {
                    _logger.LogError(fluentValidationException, "An validation has occurred");

                    var responseModels = fluentValidationException.Errors
                        .Select(s => new ExceptionResponseModel
                        {
                            ErrorCode = s.ErrorCode,
                            Message = s.ErrorMessage
                        });

                    context.Result = new JsonResult(responseModels)
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                }
                else
                {
                    _logger.LogError(context.Exception, "An unexpetced error has occurred");
                    context.Result = new JsonResult(new ExceptionResponseModel { ErrorCode = "500", Message = context.Exception.Message })
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }

                context.Exception = null;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
            }
        }
    }
}
