using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CrewMate.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Serilog;
using Serilog.Sinks.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80);
});

DotNetEnv.Env.Load();
builder.Configuration.AddEnvironmentVariables();

//foreach (var key in builder.Configuration.AsEnumerable().Where(k => k.Value != null).ToList())
//{
//    var value = Environment.GetEnvironmentVariable(key.Value);
//    if (value != null)
//    {
//        builder.Configuration[key.Key] = value;
//    }
//}

builder.Configuration["ConnectionStrings:PostgreConnection"] = Environment.GetEnvironmentVariable("DATABASE_SETTING");
var postgreString = builder.Configuration["ConnectionStrings:PostgreConnection"];
var postgreConnection = Environment.GetEnvironmentVariable("DATABASE_SETTING");
var connectionStringTest = builder.Configuration.GetConnectionString("PostgreConnection");

Console.WriteLine($"Database Connection: {postgreString}");
Console.WriteLine($"Postgre Connection: {postgreConnection}");
Console.WriteLine($"Connection String: {connectionStringTest}");

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("PostgreConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Identity Configuration
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
});

//Swagger API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

//Bypass Email Sender
builder.Services.AddSingleton<IEmailSender, Microsoft.AspNetCore.Identity.UI.Services.NoOpEmailSender>();

//Authentication, Session, Authorization
builder.Services.AddSession();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

//Set Up Logging
var columnWriters = new Dictionary<string, ColumnWriterBase>
{
    { "message", new RenderedMessageColumnWriter() },
    { "message_template", new MessageTemplateColumnWriter() },
    { "level", new LevelColumnWriter() },
    { "timestamp", new TimestampColumnWriter() },
    { "exception", new ExceptionColumnWriter() },
    { "properties", new LogEventSerializedColumnWriter() }
};

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.PostgreSQL(
        connectionString: builder.Configuration.GetConnectionString("PostgreConnection"),
        tableName: "Logs",
        needAutoCreateTable: true,
        columnOptions: columnWriters
    )
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();
