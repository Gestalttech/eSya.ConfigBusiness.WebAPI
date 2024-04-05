using eSya.ConfigBusiness.WebAPI.Filters;
using Microsoft.Extensions.Configuration;
using DL_ConfigBusiness = eSya.ConfigBusiness.DL.Entities;
using eSya.ConfigBusiness.IF;
using eSya.ConfigBusiness.DL.Repository;
using Microsoft.AspNetCore.Mvc;
using eSya.ConfigBusiness.WebAPI.Utility;
using System.Globalization;
using eSya.ConfigBusiness.DL.Localization;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

DL_ConfigBusiness.eSyaEnterprise._connString = builder.Configuration.GetConnectionString("dbConn_eSyaEnterprise");

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ApikeyAuthAttribute>();
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<HttpAuthAttribute>();
}); builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CultureAuthAttribute>();
});
//Localization

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
                   //new CultureInfo(name:"en-IN"),
                    new CultureInfo(name:"en-US"),
                    new CultureInfo(name:"ar-EG"),
                };
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(culture: supportedCultures[0], uiCulture: supportedCultures[0]);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

});

builder.Services.AddLocalization();


//localization
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();
builder.Services.AddScoped<IRulesRepository, RulesRepository>();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Localization

var supportedCultures = new[] { /*"en-IN", */ "en-US", "ar-EG" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);
//Localization

app.UseAuthorization();

app.MapControllers();

app.Run();

//Abdul Changes