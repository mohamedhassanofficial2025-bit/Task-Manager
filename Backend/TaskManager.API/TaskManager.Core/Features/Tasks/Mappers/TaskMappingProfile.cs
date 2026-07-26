using AutoMapper;
using TaskManager.Core.Entities;
using TaskManager.Core.Features.Tasks.DTOs;

namespace TaskManager.Core.Features.Tasks.Mappers;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        // Entity -> Response DTO
        CreateMap<TaskItem, TaskResponseDto>();

        // Create DTO -> Entity
        CreateMap<CreateTaskDto, TaskItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Project, opt => opt.Ignore());

        // Update DTO -> Entity (no Status — handled by UpdateTaskStatusDto)
        CreateMap<UpdateTaskDto, TaskItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectId, opt => opt.Ignore())
            .ForMember(dest => dest.Project, opt => opt.Ignore());
    }
}
