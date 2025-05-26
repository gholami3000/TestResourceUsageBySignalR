using TestResourceUsageBySignalR;
using TestResourceUsageBySignalR.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add HttpClient as a service
builder.Services.AddHttpClient<HomeController>();
// Add services to the container.
builder.Services.AddControllersWithViews();
// Add SignalR services
builder.Services.AddSignalR();
var app = builder.Build();
app.UseMiddleware<RequestCounterMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapHub<ChatHub>("/chathub"); // Map the SignalR hub
app.MapHub<SystemHub2>("/systemhub2");
app.MapHub<SystemHub>("/systemhub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
