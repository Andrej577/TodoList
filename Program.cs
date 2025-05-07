var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();

// Zeabur daje port preko ENV
var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // app.UseHsts(); // može ostati isključen jer Zeabur koristi HTTPS
}

// ❌ NE treba HTTPS redirection jer Zeabur reverse proxy radi to
// app.UseHttpsRedirection(); // uklonjeno!

app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
