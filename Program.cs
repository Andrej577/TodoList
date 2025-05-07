var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();

// Zeabur daje port preko ENV
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // može ostati isključen jer Zeabur koristi HTTPS
}

app.UseHttpsRedirection(); // uklonjeno!

app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
