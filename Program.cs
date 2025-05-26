using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrphanSystem.Data.Interceptors;
using OrphanSystem.Data;
using OrphanSystem.Extensions;
using OrphanSystem.Helpers;


#region Setup
#if SET_LEGACY_TIMESTAMP
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
#endif
/*
#if USE_FIREBASE
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs/Firebase/ServiceAccount.json"))
});
#endif*/
#endregion
 

#region App Builder


var builder = WebApplication.CreateBuilder(args);
ConfigProvider.config = builder.Configuration;

builder.Services.AddDbContext<MasterDbContext>(opt =>
{
    opt.AddInterceptors(new DateSetterInterceptor());
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowSpecificOrigins",
        builder => builder
            .WithOrigins(
                "http://localhost:6969", "http://127.0.0.1:6969", "http://100.42.190.178:6969",

                "http://localhost:3000", "http://127.0.0.1:3000", "http://100.42.190.178:3000",
                "http://localhost:3001", "http://127.0.0.1:3001", "http://100.42.190.178:3001",
                "http://localhost:3002", "http://127.0.0.1:3002", "http://100.42.190.178:3002",

                "http://localhost:8000", "http://127.0.0.1:8000", "http://100.42.190.178:8000",
                "http://localhost:8001", "http://127.0.0.1:8001", "http://100.42.190.178:8001",
                "http://localhost:8002", "http://127.0.0.1:8002", "http://100.42.190.178:8002",

                "http://localhost:8080", "http://127.0.0.1:8080", "http://100.42.190.178:8080",
                "http://localhost:8081", "http://127.0.0.1:8081", "http://100.42.190.178:8081",
                "http://localhost:8082", "http://127.0.0.1:8082", "http://100.42.190.178:8082"
                )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("status", "Content-Disposition", "Access-Control-Allow-Origin")
    );
});

builder.Services.AddAuthConfig();
builder.Services.AddAutoMapperConfig();
builder.Services.AddSignalRConfig();
builder.Services.AddSwaggerConfig();

builder.Services.AddServices();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCustomRouting();
builder.Services.AddCustomControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddTmpStuff();
builder.Services.AddControllersWithViews();
builder.Services.AddLocalizationConfig();

#endregion


#region App
var app = builder.Build();

app.UseCustomSwagger();
app.UseHttpsRedirection();
app.UseHsts();
app.UseCors("AllowSpecificOrigins");
app.UseRouting();
app.UseErrResponseExceptionHandler();
app.UseAuth();
app.UseStaticFiles();
/*
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<MasterHub>("/hubs/master");
    endpoints.MapControllers();
});*/

// Seeder

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
    var seeder = new Seeder(dataContext);
    
    await seeder.SeedSuperAdmin("Abdelqader@Abdelqader.com");
    if (app.Environment.IsDevelopment())
    {
        bool ignoreExistingRecords = false;
        //await seeder.SeedCountriesCities(ignoreExistingRecords);
    }
}



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
#endregion