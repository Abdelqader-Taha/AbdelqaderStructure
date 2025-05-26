using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrphanSystem.Extensions;
using OrphanSystem.Models.DTOs;
using OrphanSystem.Models.Entities;


namespace OrphanSystem.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<UserFormDTO, User>();
        CreateMap<UserUpdateDTO, User>()
            .IgnoreNullAndEmptyGuids();

       

        CreateMap<Notification, NotificationDTO>();
        CreateMap<Notification, NotificationDTOSimplified>();
        CreateMap<NotificationForm, Notification>();
        

        
        CreateMap<Role,RoleDTO>();
        CreateMap<RoleFormDTO,Role>();
        CreateMap<RoleUpdateDTO,Role>()
            .IgnoreNullAndEmptyGuids();

        CreateMap<Permission, PermissionDTO>();


    }
}