using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configura los servicios
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Welcome/Login"; // Ruta para la página de inicio de sesión
        options.LogoutPath = "/Welcome/Logout"; // Ruta para la página de logout
    });

var app = builder.Build();

// Configura el middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Asegúrate de que este middleware está aquí
app.UseAuthorization();

app.MapGet("/", (context) =>
{
    context.Response.Redirect("/Welcome/Login");
    return Task.CompletedTask;
});

// Mapea las páginas Razor
app.MapRazorPages();

app.Run();
