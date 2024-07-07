using InfoTrack.Commons.Exception;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InfoTrack.Infrastructure.Validation
{
    public class ValidationErrorResponse : ExceptionResponseModel
    {
        public ModelStateDictionary? ValidationResult { get; set; }
    }
}
