using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GenenticAlgorithmBlazor.Client;
using GenenticAlgorithmBlazor.Client.Controllers;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(cc => new ConsoleController());
builder.Services.AddScoped(uic => new UserInterfaceController());
builder.Services.AddScoped(dt => new DLLTestController());
await builder.Build().RunAsync();
