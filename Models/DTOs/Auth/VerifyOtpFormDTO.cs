using System.ComponentModel.DataAnnotations;

namespace AbdelqaderStructure.Models.DTOs.Auth;

public class VerifyOtpFormDTO
{
    [Required] public Guid Id { get; set; }

    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string OTP { get; set; }
}