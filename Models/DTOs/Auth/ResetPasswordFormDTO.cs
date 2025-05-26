using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace OrphanSystem.Models.DTOs.Auth;

public class ResetPasswordFormDTO
{
    [Required]
    [StringLength(30, MinimumLength = 6)]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 6)]
    public string NewPassword { get; set; }
}