global using Models;
global using Repository;
global using Services;
global using _Helper;
global using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using HarrysRestro.Repository;
using Microsoft.AspNetCore.Identity;
using System.Net;
using HarrysRestro.Services;
using KingofCurries.Models;
using Stripe;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddLocalization(option => { option.ResourcesPath = "Resources"; });
builder.Services.AddMvc().AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

builder.Services.AddScoped<ICustomerDataAccessLayer,CustomerDataAccessLayer>(); 
builder.Services.AddScoped<IBankHolidayRepository,BankHolidayRepository>(); 
builder.Services.AddScoped<IDeliveryChargesRepository,DeliveryChargesRepository>(); 

builder.Services.AddScoped<IUserRepository,UserRespository>(); 
builder.Services.AddScoped<IHomeRepository,HomeRepository>();
builder.Services.AddScoped<IFreeItemsRepository, FreeItemsRepository>();
builder.Services.AddScoped<ISubItemsRepository, SubItemsRepository>();
builder.Services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<IDeliveryChargesRepository, DeliveryChargesRepository>();
builder.Services.AddScoped<IBankHolidayRepository, BankHolidayRepository>();
builder.Services.AddScoped<ICompanyProfileRepository, CompanyProfileRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));








builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
   {
       options.LoginPath = new PathString("/Home/Index");
       options.LogoutPath = new PathString("/Home/Index");
       options.ExpireTimeSpan = TimeSpan.FromDays(30);
   });

builder.Services.AddMvc()
        .AddSessionStateTempDataProvider();
builder.Services.AddSession();

//builder.Services.AddQuartz(q =>
//{
//    q.UseMicrosoftDependencyInjectionScopedJobFactory();
//    var jobKey = new JobKey("DemoJob");
//    q.AddJob<CronsEvery15SecQuards>(opts => opts.WithIdentity(jobKey));

//    q.AddTrigger(opts => opts
//        .ForJob(jobKey)
//        .WithIdentity("DemoJob-trigger")
//        .WithCronSchedule("0/15 * * * * ?"));

//});

//builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  

   
}
else
{

    app.UseStatusCodePages(async context =>
    {
        var check = context.HttpContext.Response.StatusCode;
        if (context.HttpContext.Response.StatusCode == 404)
        {
            context.HttpContext.Response.Redirect("/Error404");
        }
        else if (context.HttpContext.Response.StatusCode == 500)
        {
            context.HttpContext.Response.Redirect("/Error500");
        }
    });
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

var Supportedlanguage = new[] {"en","es"};
var opcioneslocalizacion = new RequestLocalizationOptions().SetDefaultCulture(Supportedlanguage[0])
    .AddSupportedCultures(Supportedlanguage)
    .AddSupportedCultures(Supportedlanguage);
app.UseRequestLocalization(opcioneslocalizacion);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
