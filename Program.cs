using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using CEM.Components;
using CEM.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Kết nối CSDL
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddDbContext<QlbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DBConnection")));

// Cấu hình Identity





// Thêm dịch vụ Authorization

// Cần thiết để truy cập HTTP Context
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// Thêm dịch vụ cho ứng dụng
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
       
    });

builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddRadzenComponents();
builder.Services.AddBlazoredLocalStorage();
// Thêm các dịch vụ cần thiết
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<DataService>();
builder.Services.AddSingleton<MinIOService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
// Cấu hình Authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

var app = builder.Build();

// Cấu hình pipeline yêu cầu HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts(); // HSTS cho môi trường sản xuất
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Cấu hình Authentication
app.UseAuthentication();
app.UseAuthorization();

// Định tuyến cho API Controller
app.MapControllers();

// Định tuyến cho Razor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
