var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//a�ade el servicio de sesiones 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5); //la sesi�n durar� 5 minutos
    options.Cookie.HttpOnly = true; //evita que la cookie de sesi�n sea accesible desde JavaScrip en el cliente
    options.Cookie.IsEssential = true; //marca la sesi�n como escencial para que no sea bloqueada
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Define a route for the default home page
app.MapGet("/", (context) =>
{
    context.Response.Redirect("/Welcome/Login");
    return Task.CompletedTask;
});

app.UseSession();
// Map Razor Pages
app.MapRazorPages();

app.Run();
