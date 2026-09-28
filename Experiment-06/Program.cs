var builder = WebApplication.CreateBuilder(args);

// Add MVC Controllers and Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

// Custom MVC Route Rules
app.MapControllerRoute(
    name: "category_route",
    pattern: "Products/Category/{category}",
    defaults: new { controller = "Product", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();
