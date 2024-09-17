namespace dotnet_calculator.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _apiKey = configuration["ApiKey"]; // Store your API key in configuration
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;

            if (context.Request.Headers.TryGetValue("X-Api-Key", out var apiKey) && apiKey == _apiKey)
            {
                await _next(context);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("error unauthorized: ");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
        }
    }
}
