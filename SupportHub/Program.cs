using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configura los servicios
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Welcome/Login"; // Ruta para la p�gina de inicio de sesi�n
        options.LogoutPath = "/Welcome/Logout"; // Ruta para la p�gina de logout
    });

var app = builder.Build();

// Configura el middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Aseg�rate de que este middleware est� aqu�
app.UseAuthorization();

app.MapGet("/", (context) =>
{
    context.Response.Redirect("/Welcome/Login");
    return Task.CompletedTask;
});

// Mapea las p�ginas Razor
app.MapRazorPages();

app.Run();
