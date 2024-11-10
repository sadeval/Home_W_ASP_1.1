using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapPost("/api/greeting", async (HttpContext context) =>
{
    try
    {
        var greetingRequest = await context.Request.ReadFromJsonAsync<GreetingRequest>();

        if (greetingRequest == null || string.IsNullOrWhiteSpace(greetingRequest.Name))
        {
            context.Response.StatusCode = 400; 
            await context.Response.WriteAsync("Invalid request. 'name' is required.");
            return;
        }

        string greeting = $"Hello, {greetingRequest.Name}!";

        context.Response.ContentType = "text/plain";

        await context.Response.WriteAsync(greeting);
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500; 
        await context.Response.WriteAsync("An error occurred processing your request.");
    }
});

app.Run();

public class GreetingRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
