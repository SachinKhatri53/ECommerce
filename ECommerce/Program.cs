using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Data;
using ECommerce.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ECommerceContext") ?? throw new InvalidOperationException("Connection string 'ECommerceContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Custom Injection
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<ShippingService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<UserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddTransient<IPaypalService, PaypalService>();

//------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
// custom
app.UseSession();
/*app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name:"default",
        pattern:"{controller:Checkout}"
        );
});*/
// ------
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
 pattern: "{controller=Home}/{action=Index}/{id?}");
/*pattern: "{controller=Deal}/{action=Index}/{id?}");*/

app.Run();
