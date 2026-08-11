using AutoMapper;
using TaskManager.Core.Entities;
using TaskManager.Core.Features.Projects.DTOs;

namespace TaskManager.Core.Features.Projects.Mappers;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        // Entity -> Response DTO
        CreateMap<Project, ProjectResponseDto>();

        // Create DTO -> Entity
        CreateMap<CreateProjectDto, Project>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Tasks, opt => opt.Ignore());

        // Update DTO -> Entity
        CreateMap<UpdateProjectDto, Project>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Tasks, opt => opt.Ignore());
    }
}
