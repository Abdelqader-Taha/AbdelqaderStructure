using AbdelqaderStructure.Data;
using AbdelqaderStructure.Extensions;
using AbdelqaderStructure.Helpers;
using AbdelqaderStructure.Models.DTOs.TeacherDTOs;
using AbdelqaderStructure.Models.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;




//   Note : This code  is for Guide how to use this structure u can remove it after u learn it .


namespace AbdelqaderStructure.Services.HowToUse
{
    public interface ITeacherService
    {
        Task<Response<PagedList<TeacherDTO>>> GetAll(TeacherFilter filter);
        Task<Response<TeacherDTO>> GetById(Guid Id);
        Task<Response<TeacherDTO>> Create(CreateTeacherFrom form);
        Task<Response<TeacherDTO>> Update(Guid Id, UpdateTeacherFrom form);
        Task<Response<string>> Delete(Guid Id);
    }

    public class TeacherService : BaseService, ITeacherService
    {
        public TeacherService(MasterDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Response<PagedList<TeacherDTO>>> GetAll(TeacherFilter filter)
        {
            var teachers = _context.Teachers
                .WhereBaseFilter(filter)    
                .OrderByCreationDate()
               .Select(t => new TeacherDTO
               {
                   Id = t.Id,
                   FullName = t.FullName,
                   Phone = t.Phone,
                   Email = t.Email,
                   Location = t.Location,
                   School = t.School,
                   Gender = t.Gender,
                   YearsOfTeach = t.YearsOfTeach,
                   Note = t.Note,
                   Percentage = t.Percentage,
                   ImgUrl = t.ImgUrl,
                   CreatedAt = t.CreatedAt,
                   UpdatedAt = t.UpdatedAt,
                   
               }) ;
            var pagedTeachers = await teachers.Paginate(filter);
            return new Response<PagedList<TeacherDTO>>(pagedTeachers, null, 200);
        }

        public async Task<Response<TeacherDTO>> GetById(Guid Id)
        {
            var teacher = await _context.Teachers
               .AsNoTracking()
               .FirstOrDefaultAsync(t => t.Id == Id && !t.IsDeleted);

            if (teacher == null)
            {
                return new Response<TeacherDTO>(null, "Teacher not found", 404);
            }
            var teacherdto = new TeacherDTO
            {

                Id = teacher.Id,
                FullName = teacher.FullName,
                Phone = teacher.Phone,
                Email = teacher.Email,
                Location = teacher.Location,
                School = teacher.School,
                Gender = teacher.Gender,
                YearsOfTeach = teacher.YearsOfTeach,
                Note = teacher.Note,
                Percentage = teacher.Percentage,
                ImgUrl = teacher.ImgUrl,
                CreatedAt = teacher.CreatedAt,
                UpdatedAt = teacher.UpdatedAt

            };
            return new Response<TeacherDTO>(teacherdto, null, 200);
        }

        public async Task<Response<TeacherDTO>> Create(CreateTeacherFrom form)
        {
            if (form == null)
            {
                return new Response<TeacherDTO>(null, " Form cannot be null", 400);
            }

            var teacher = new Teacher
            {
                Id = Guid.NewGuid(), 
                FullName = form.FullName,
                Phone = form.Phone,
                Email = form.Email,
                Location = form.Location,
                School = form.School,
                Gender = form.Gender,
                YearsOfTeach = form.YearsOfTeach,
                Note = form.Note,
                Percentage = form.Percentage,
                ImgUrl = form.ImgUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow

            };
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            var teacherDto = new TeacherDTO
            {
                Id = teacher.Id,
                FullName = teacher.FullName,
                Phone = teacher.Phone,
                Email = teacher.Email,
                Location = teacher.Location,
                School = teacher.School,
                Gender = teacher.Gender,
                YearsOfTeach = teacher.YearsOfTeach,
                Note = teacher.Note,
                Percentage = teacher.Percentage,
                ImgUrl = teacher.ImgUrl,
                CreatedAt = teacher.CreatedAt,
                UpdatedAt = teacher.UpdatedAt

            };
            return new Response<TeacherDTO>(teacherDto, "Teacher created successfully", 201);
        }
        public async Task<Response<TeacherDTO>> Update(Guid Id ,UpdateTeacherFrom form)
        {
            if (form == null)
                return new Response<TeacherDTO>(null,"Form cannot be null",400);

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.Id == form.Id && !t.IsDeleted);

            if (teacher == null)
                return new Response<TeacherDTO>(null,"Teacher Not Found Or Deleted",404);

            teacher.FullName = form.FullName ?? teacher.FullName;
            teacher.Phone = form.Phone ?? teacher.Phone;
            teacher.Email = form.Email ?? teacher.Email;
            teacher.Location = form.Location ?? teacher.Location;
            teacher.School = form.School ?? teacher.School;
            teacher.Gender = form.Gender ?? teacher.Gender;
            teacher.YearsOfTeach = form.YearsOfTeach ?? teacher.YearsOfTeach;
            teacher.Note = form.Note ?? teacher.Note;
            teacher.Percentage = form.Percentage ?? teacher.Percentage;
            teacher.ImgUrl = form.ImgUrl ?? teacher.ImgUrl;
            teacher.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            var teacherDto = new TeacherDTO
            {
                Id = teacher.Id,
                FullName = teacher.FullName,
                Phone = teacher.Phone,
                Email = teacher.Email,
                Location = teacher.Location,
                School = teacher.School,
                Gender = teacher.Gender,
                YearsOfTeach = teacher.YearsOfTeach,
                Note = teacher.Note,
                Percentage = teacher.Percentage,
                ImgUrl = teacher.ImgUrl,
                CreatedAt = teacher.CreatedAt,
                UpdatedAt = teacher.UpdatedAt
            };

            return new Response<TeacherDTO>(teacherDto, null, 200);

        }
        public async Task<Response<string>> Delete(Guid Id)
        {
           
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.Id == Id && !t.IsDeleted);

            if (teacher == null)
                return new Response<string>(null, "Teacher not found or already deleted", 404);

            await teacher.Delete(_context);
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();

            return new Response<string>("Teacher deleted successfully.", null, 200);
        }

    }
}
