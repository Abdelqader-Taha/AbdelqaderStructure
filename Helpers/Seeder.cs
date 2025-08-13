using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AbdelqaderStructure.Data;
using AbdelqaderStructure.Models.Entities;

using System.Security.Cryptography;
using System.Text;


namespace AbdelqaderStructure.Helpers;

public class Seeder
{
    private MasterDbContext _masterDbContext;

    public Seeder(MasterDbContext masterDbContext)
    {
        _masterDbContext = masterDbContext;
    }

    public async Task SeedSuperAdmin(string email)
    {
        if (!await _masterDbContext.Users.AnyAsync(e => e.Email == email))
        {
            var user = new User
            {
                StaticRole = StaticRole.SuperAdmin,
                Email = email,
                Phone = "0000000000",
                PhoneCountryCode = "+964",
                Name = "SuperAdmin",
                IsProtected = true,
            };



            using var hmac = new HMACSHA512();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("SuperAdmin"));
            user.PasswordSalt = hmac.Key;

            await _masterDbContext.Users.AddAsync(user);
        }


        await _masterDbContext.SaveChangesAsync();

    }



}
