using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UI.Web;
using UI.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<UI.Web.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.ConfigureDependencyInjection();
builder.Services.AddServices();

AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    Console.WriteLine((e.ExceptionObject as Exception).Message, "Unhandled exception");
};

TaskScheduler.UnobservedTaskException += (sender, e) =>
{
    Console.WriteLine(e.Exception.Message, "Unobserved task exception");
    e.SetObserved();
};

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7162")
});

await builder.Build().RunAsync();