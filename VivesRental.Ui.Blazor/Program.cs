using System;
using System.Net.Http;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using VivesRental.Ui.Blazor;
using VivesRental.Ui.Blazor.Security;
using VivesRental.Ui.Blazor.Stores;
using VivesRental.Ui.Blazor.Services;
using VivesRental.Ui.Blazor.Sdk;
using VivesRental.Ui.Blazor.Sdk.DelegatingHandlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// API base URL from wwwroot/appsettings.json (key: ApiSettings:BaseUrl)
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? builder.HostEnvironment.BaseAddress;

// Static resources client (existing)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Local storage
builder.Services.AddBlazoredLocalStorage();

// Token store + auth state provider
builder.Services.AddScoped<ITokenStore, LocalStorageTokenStore>();
builder.Services.AddScoped<TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<TokenAuthenticationStateProvider>());
// Authorization handler + named API HttpClient
builder.Services.AddTransient<AuthorizationHandler>();
builder.Services.AddHttpClient("VivesRentalApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<AuthorizationHandler>();

// SDKs
builder.Services.AddScoped<IdentitySdkService>();
builder.Services.AddScoped<ProductSdkService>();
builder.Services.AddScoped<ArticleSdkService>();
builder.Services.AddScoped<CustomerSdkService>();
builder.Services.AddScoped<OrderSdkService>();
builder.Services.AddScoped<OrderLineSdkService>();
builder.Services.AddScoped<ArticleReservationSdkService>(); // <-- added

await builder.Build().RunAsync();
