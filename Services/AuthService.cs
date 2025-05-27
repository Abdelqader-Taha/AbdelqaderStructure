using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrphanSystem.Data;
using OrphanSystem.Helpers;
using OrphanSystem.Models.DTOs.Auth;
using OrphanSystem.Models.Entities;
using OrphanSystem.Extensions;
using OrphanSystem.Null;

namespace OrphanSystem.Services;

public interface IAuthService
{
    Task<Response<LoginResponseDTO>> Login(LoginFormDTO form);
    Task<Response<string>> Register(RegisterFormDTO form);
    Task ResetPassword(Guid userId, ResetPasswordFormDTO form);
}

public class AuthService : BaseService, IAuthService
{
    public AuthService(MasterDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

        public async Task<Response<string>> Register(RegisterFormDTO form)
        {
            await ValidateRegister(form);
            var user = await CreateUser(form);
            var token = JwtToken.GenToken(user.Id, user.StaticRole.ToRoleString());
            return new Response<string>(token, null, 200);
        }

        private async Task ValidateRegister(RegisterFormDTO form)
        {
            var emailOrPhoneIsTaken = await _context.Users.AnyAsync(u =>
                (form.Email == u.Email) ||
                (form.PhoneCountryCode == u.PhoneCountryCode && form.Phone == u.Phone));
            if (emailOrPhoneIsTaken)
                ErrResponseThrower.Unauthorized();
        }

    private async Task<User> CreateUser(RegisterFormDTO form)
    {
        using var hmac = new HMACSHA512();
        var user = new User
        {
            Name = form.Name,
            Email = form.Email,
            Phone = form.Phone,
            PhoneCountryCode = form.PhoneCountryCode,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(form.Password)),
            PasswordSalt = hmac.Key,
            StaticRole = form.Role ?? StaticRole.User 
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<Response<LoginResponseDTO>> Login(LoginFormDTO form)
{
    try
    {
        User user;
        if (form.Email != null)
        {
            user = await _context.Users
                .Where(e => e.Email == form.Email)
                .FirstOrDefaultAsync();
        }
        else
        {
            user = await _context.Users
                .Where(e => e.PhoneCountryCode == form.PhoneCountryCode && e.Phone == form.Phone)
                .FirstOrDefaultAsync();
        }

        if (user == null)
            return new Response<LoginResponseDTO>(null, "User not found or unauthorized.", 401);

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var dtoHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(form.Password));
        if (!user.PasswordHash.SequenceEqual(dtoHash))
            return new Response<LoginResponseDTO>(null, "Password is incorrect.", 401);

        var token = JwtToken.GenToken(user.Id, user.StaticRole.ToRoleString());

        _context.Update(user);
        await _context.SaveChangesAsync();

        var responseDto = new LoginResponseDTO(user.Id, user.StaticRole.ToRoleString(), token, user.Name);
        return new Response<LoginResponseDTO>(responseDto, null, 200);
    }
    catch (Exception ex)
    {
        return new Response<LoginResponseDTO>(null, "An unexpected error occurred.", 500);
    }
}



    public async Task ResetPassword(Guid userId, ResetPasswordFormDTO form)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            ErrResponseThrower.NotFound("USER_NOT_FOUND");

        using var hmacOld = new HMACSHA512(user.PasswordSalt);
        var dtoOldPassHash = hmacOld.ComputeHash(Encoding.UTF8.GetBytes(form.OldPassword));
        if (!user.PasswordHash.SequenceEqual(dtoOldPassHash))
            ErrResponseThrower.Unauthorized("WRONG_PASSWORD");

        using var hmacNew = new HMACSHA512();
        user.PasswordHash = hmacNew.ComputeHash(Encoding.UTF8.GetBytes(form.NewPassword));
        user.PasswordSalt = hmacNew.Key;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
