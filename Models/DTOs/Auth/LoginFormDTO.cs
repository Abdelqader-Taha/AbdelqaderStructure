using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace OrphanSystem.Models.DTOs.Auth;

public class LoginFormDTO
{
    [EmailAddress] [MaxLength(128)] public string? Email { get; set; }

    [MaxLength(10)] public string? PhoneCountryCode { get; set; }
    [Phone] [MaxLength(20)] public string? Phone { get; set; }

    [Required] [MinLength(6)] public string Password { get; set; }
}

public class LoginFormFluentValidator : AbstractValidator<LoginFormDTO>
{
    public LoginFormFluentValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().NotEmpty()
            .When(x => x.Phone == null);
        RuleFor(x => x.Phone)
            .NotNull().NotEmpty()
            .When(x => x.Email == null);
        RuleFor(x => x.PhoneCountryCode)
            .NotNull().NotEmpty()
            .When(x => x.Email == null);
    }
}
public class LoginResponseDTO
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }

    public LoginResponseDTO(Guid userId, string role, string token, string name)
    {
        UserId = userId;
        Role = role;
        Token = token;
        Name = name;
    }
}
