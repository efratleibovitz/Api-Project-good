using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;




namespace WebApiShop.Middlewares
{
    public class RatingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RatingMiddleware> _logger;

        public RatingMiddleware(RequestDelegate next, ILogger<RatingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext, IRatingService ratingService)
        {
            Rating rating = new Rating();
            rating.Host = httpContext.Request.Host.Value;
            rating.Method = httpContext.Request.Method;
            rating.Path = httpContext.Request.Path;
            rating.Referer = httpContext.Request.Headers.Referer;
            rating.UserAgent = httpContext.Request.Headers.UserAgent;
            rating.RecordDate = DateTime.Now;
            await ratingService.AddRating(rating);
            await _next(httpContext);
        }
    
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RatingExtensions
    {
        public static IApplicationBuilder UseRating(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RatingMiddleware>();
        }
    }
}
