using CipatBarzel.Hubs;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

string? connctionString = builder.Configuration.GetConnectionString("server=LAPTOP-GGTATKCJ\\SQLEXPRESS;" +
                                                   "initial catalog= CipatBarzel ;" +
                                                   "user id=sa ;" +
                                                   "password=1234 ; " +
                                                   "TrustServerCertificate=Yes");
builder.Services.AddDbContext<CipatBarzel>(options => )

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



app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<RealTime>("/rt");

app.Run();
