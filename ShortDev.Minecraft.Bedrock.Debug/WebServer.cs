using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ShortDev.Minecraft.Bedrock.Debug
{
    internal static class WebServer
    {
        public static void Run()
        {
            Task.Run(() =>
            {
                var builder = WebApplication.CreateBuilder();
                builder.Services.AddControllers();
                var app = builder.Build();

                app.MapGet("/", () => "Hello World!");
                app.MapControllers();

                app.Run("http://localhost:3000");
            });
        }

        public static event Action<string>? OnDataReceived;
        public static void TriggerDataReceived(string arg)
            => OnDataReceived?.Invoke(arg);
    }

    [Controller]
    public sealed class RouteController : Controller
    {
        [HttpPost, Route("/test")]
        public IActionResult Test()
        {
            //WebServer.TriggerDataReceived(body);
            return Ok();
        }

        [HttpPost, Route("/updateData")]
        public IActionResult UpdateData()
        {
            // WebServer.TriggerDataReceived(body);
            return Ok();
        }
    }
}
