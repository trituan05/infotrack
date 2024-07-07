using Microsoft.Extensions.DependencyInjection;

namespace InfoTrack.Infrastructure.Exception
{
    public static class Extensions
    {
        public static IMvcBuilder AddExceptionFilter(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddMvcOptions(o => o.Filters.Add(new HttpExceptionFilter()));
            return mvcBuilder;
        }
    }
}
