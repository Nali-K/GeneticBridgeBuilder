using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using GeneticDesignerWebAssembly;
using GeneticDesignerWebAssembly.Controllers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton(cc => new ConsoleController());
builder.Services.AddSingleton(uic => new UserInterfaceController());
builder.Services.AddSingleton(dt => new DLLTestController());
await builder.Build().RunAsync();
