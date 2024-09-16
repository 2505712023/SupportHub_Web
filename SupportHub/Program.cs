var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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

// Map Razor Pages
app.MapRazorPages();

app.Run();
