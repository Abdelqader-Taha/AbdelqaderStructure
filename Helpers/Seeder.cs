using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using OrphanSystem.Data;
using OrphanSystem.Models.Entities;


namespace OrphanSystem.Helpers;

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
                StaticRole = StaticRole.SUPER_ADMIN,
                Email = email,
                Phone = "7727633485",
                PhoneCountryCode = "+964",
                Name = "Abdelqader",
            };



            using var hmac = new HMACSHA512();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("string"));
            user.PasswordSalt = hmac.Key;

            await _masterDbContext.Users.AddAsync(user);
        }

        
        await _masterDbContext.SaveChangesAsync();

    }



}
