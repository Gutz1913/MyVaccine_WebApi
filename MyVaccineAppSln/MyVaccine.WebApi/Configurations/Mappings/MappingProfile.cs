using AutoMapper;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Configurations.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DependentRequestDTO, Dependent>();
        CreateMap<Dependent, DependentResponseDTO>()
            .ForMember(
                dest => dest.UserFullName, 
                opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            .ForMember(
                dest => dest.FamilyGroupName, 
                opt => opt.MapFrom(src => src.FamilyGroup.Name));

        CreateMap<FamilyGroupRequestDTO, FamilyGroup>();
        CreateMap<FamilyGroup, FamilyGroupResponseDTO>();

        CreateMap<AllergyRequestDTO, Allergy>();
        CreateMap<Allergy, AllergyResponseDTO>();
    }
}
