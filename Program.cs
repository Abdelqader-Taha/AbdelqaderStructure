using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using AbdelqaderStructure.Data.Interceptors;
using AbdelqaderStructure.Data;
using AbdelqaderStructure.Extensions;
using AbdelqaderStructure.Helpers;
using AbdelqaderStructure.Extensions;
using AbdelqaderStructure.Helpers;

#region App Builder

var builder = WebApplication.CreateBuilder(args);
ConfigProvider.config = builder.Configuration;

builder.Services.AddDbContext<MasterDbContext>(opt =>
{
    opt.AddInterceptors(new DateSetterInterceptor());
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});


// if  u want to edit the project services u find  it in the Extensions Folder /ServiceCollectionExtensions class 
builder.Services.AddProjectServices();

#endregion


#region App
var app = builder.Build();

app.UseCustomSwagger();
app.UseHttpsRedirection();
app.UseHsts();
app.UseCors("AllowAllOriginsWithLimitedMethods");
app.UseRouting();
app.UseErrResponseExceptionHandler();
app.UseAuth();
app.UseStaticFiles();


// Seeder

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
    var seeder = new Seeder(dataContext);

    await seeder.SeedSuperAdmin("SuperAdmin@SuperAdmin.com");
    if (app.Environment.IsDevelopment())
    {
        bool ignoreExistingRecords = false;
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
#endregion