using System.ComponentModel.DataAnnotations;

namespace AbdelqaderStructure.Models.DTOs.TeacherDTOs
{
    public class CreateTeacherFrom:BaseFormDTO
    {
        [Required, MaxLength(150)]
        public string FullName { get; set; } = default!;

        [MaxLength(30)]
        public string? Phone { get; set; }

        [MaxLength(200), EmailAddress]
        public string? Email { get; set; }

        [MaxLength(150)]
        public string? Location { get; set; }

        [MaxLength(150)]
        public string? School { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; }

        public int? YearsOfTeach { get; set; }

        public string? Note { get; set; }

        public float? Percentage { get; set; }

        public string? ImgUrl { get; set; }

    }
}
