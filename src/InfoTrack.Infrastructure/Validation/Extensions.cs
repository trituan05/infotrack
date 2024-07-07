using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;

namespace InfoTrack.Infrastructure.Validation
{
    public static class Extensions
    {
        public static IMvcBuilder ConfigureValidation(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddFluentValidation(x => x.RunDefaultMvcValidationAfterFluentValidationExecutes = false);
            mvcBuilder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new BadRequestObjectResult(new ValidationErrorResponse 
                    { 
                        ErrorCode = "400",
                        Message = "Validation Error",
                        ValidationResult = context.ModelState
                    });

                    result.ContentTypes.Add(MediaTypeNames.Application.Json);
                    return result;
                };
            });

            ValidatorOptions.Global.PropertyNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;

            return mvcBuilder;
        }
    }
}
