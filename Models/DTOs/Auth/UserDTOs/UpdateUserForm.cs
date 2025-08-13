using System.ComponentModel.DataAnnotations;
using AbdelqaderStructure.Models.DTOs;
using AbdelqaderStructure.Models.Entities;

namespace AbdelqaderStructure.Models.DTOs.Auth.UserDTOs
{
    public class UpdateUserForm:BaseUpdateDTO
    {
        public StaticRole? StaticRole { get; set; }

        [MaxLength(320)] public string? Email { get; set; }

        [MaxLength(16)] public string? Phone { get; set; }
        [MaxLength(8)] public string? PhoneCountryCode { get; set; }
        [MaxLength(128)] public string? Name { get; set; }
       // [MaxLength(128)] public string? UserName { get; set; }
    }
}
