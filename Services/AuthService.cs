using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using AbdelqaderStructure.Data;
using AbdelqaderStructure.Extensions;
using AbdelqaderStructure.Helpers;
using AbdelqaderStructure.Models.DTOs;
using AbdelqaderStructure.Models.DTOs.Auth;
using AbdelqaderStructure.Models.Entities;
using AbdelqaderStructure.Null;
using AbdelqaderStructure.Models.DTOs.Auth.UserDTOs;
using System.Security.Cryptography;
using System.Text;

namespace AbdelqaderStructure.Services;

public interface IAuthService
{
    Task<Response<LoginResponseDTO>> Login(LoginFormDTO form);
    Task<Response<string>> Register(RegisterFormDTO form);
    Task ResetPassword(Guid userId, ResetPasswordFormDTO form);
    Task<Response<PagedList<UsersDTO>>> GetAll(Filter filter);
    Task<Response<string>> Update(UpdateUserForm form);
    Task<Response<string>> Delete(Guid id);
}

public class AuthService : BaseService, IAuthService
{
    public AuthService(MasterDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<Response<PagedList<UsersDTO>>> GetAll(Filter filter)
    {
        var query = _context.Users
            .WhereBaseFilter(filter)
            .OrderByCreationDate();

        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            query = query.Where(u => u.Email.Contains(filter.Email));
        }

        var result = await query
            .ProjectTo<UsersDTO>(_mapper.ConfigurationProvider)
            .Paginate(filter);

        return new Response<PagedList<UsersDTO>>(result, null, 200);
    }


    public async Task<Response<string>> Update(UpdateUserForm form)
    {
        // Check if email is used by another user
        var emailExists = await _context.Users
            .AnyAsync(u => u.Email == form.Email && u.Id != form.Id);

        if (emailExists)
        {
            return new Response<string>(null, "This email is already in use by another user.", 400);
        }

        // Check if phone is used
        var phoneExists = await _context.Users
            .AnyAsync(u =>
                u.Phone == form.Phone &&
                u.PhoneCountryCode == form.PhoneCountryCode &&
                u.Id != form.Id);

        if (phoneExists)
        {
            return new Response<string>(null, "This phone number is already in use by another user.", 400);
        }

        try
        {
            await _context.UpdateWithMapperOrException<User, UpdateUserForm>(form, _mapper);
            return new Response<string>("User updated successfully.", null, 200);
        }
        catch (DbUpdateException)
        {
            return new Response<string>(null, "Database error while updating user. Please try again.", 500);
        }
        catch (Exception)
        {
            return new Response<string>(null, "An unexpected error occurred during update.", 500);
        }
    }



    public async Task<Response<string>> Delete(Guid id)
    {
        var user = await _context.Users
            .Where(u => u.Id == id && !u.IsDeleted)
            .FirstOrDefaultAsync();

        if (user == null)
            return new Response<string>(null, "User Not Found Or Deleted", 404);

        if (user.IsProtected)
            return new Response<string>(null, "This user is protected and cannot be deleted.", 403);

        await user.Delete(_context);
        _context.Update(user);
        await _context.SaveChangesAsync();

        return new Response<string>("User soft-deleted successfully.", null, 200);
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
            StaticRole = form.Role ?? StaticRole.User, // Default to User if no role is provided
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
