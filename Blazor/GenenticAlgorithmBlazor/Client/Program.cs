using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GenenticAlgorithmBlazor.Client;
using GenenticAlgorithmBlazor.Client.Controllers;
using GenenticAlgorithmBlazor.Client.Interfaces;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ISimulationRequest, SimulationRequestController>();
builder.Services.AddScoped<DLLTestController, DLLTestController>();
builder.Services.AddScoped(cc => new ConsoleController());
builder.Services.AddScoped(uic => new UserInterfaceController());

await builder.Build().RunAsync();
