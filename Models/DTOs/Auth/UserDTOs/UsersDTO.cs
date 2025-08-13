using System.ComponentModel.DataAnnotations;
using AbdelqaderStructure.Models.DTOs;
using AbdelqaderStructure.Models.Entities;

namespace AbdelqaderStructure.Models.DTOs.Auth.UserDTOs
{
    public class UsersDTO : BaseDTO
    {
        // Note there  is Two UsersDTO do not use the other one Its for the auth 
        // Note  this dto for the get and update and edit 
        public StaticRole? StaticRole { get; set; }

        [MaxLength(320)] public string? Email { get; set; }

        [MaxLength(16)] public string? Phone { get; set; }
        [MaxLength(8)] public string? PhoneCountryCode { get; set; }
        [MaxLength(128)] public string? Name { get; set; }
       // [MaxLength(128)] public string? UserName { get; set; }

    }

    public class Filter : BaseFilter
    {
        [MaxLength(128)] public string? Name { get; set; }

        [MaxLength(320)] public string? Email { get; set; }

    }
}
