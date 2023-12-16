using ExPaper.SharedMethods.Lib.Extensions;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services;
using ExPaper.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


builder.AddSeriLog();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IGlobalSettingService, GlobalSettingService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();


Urls.AuthApiBase = builder.Configuration["ServiceUrls:AuthAPI"];

Urls.GlobalSettingsApiBase = builder.Configuration["ServiceUrls:OcelotGateway"];
Urls.OrganisationApiBase = builder.Configuration["ServiceUrls:OcelotGateway"];
Urls.FolderApiBase = builder.Configuration["ServiceUrls:OcelotGateway"];
Urls.FolderTabApiBase = builder.Configuration["ServiceUrls:OcelotGateway"];
Urls.DocumentApiBase = builder.Configuration["ServiceUrls:OcelotGateway"];
Urls.UserApiBase = builder.Configuration["ServiceUrls:OcelotGateway"];


builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGlobalSettingService, GlobalSettingService>();
builder.Services.AddScoped<IOrganisationService, OrganisationService>();
builder.Services.AddScoped<IFolderService, FolderService>();
builder.Services.AddScoped<IFolderTabService, FolderTabService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IFileService, FileService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(100);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Use(async (context, next) =>
//{
//    context.Response.Headers.Add("Content-Security-Policy", "object-src 'self' http://192.168.175.15/webdav/");
//    await next();
//});


app.Run();
