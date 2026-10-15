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
                opt => opt.MapFrom(src => src.FamilyGroup.Name))
            .ForMember(
                dest => dest.AllergyId,
                opt => opt.MapFrom(src => src.DependentAllergies
                    .FirstOrDefault() != null
                    ? src.DependentAllergies.FirstOrDefault().AllergyId
                    : 0))
            .ForMember(
                dest => dest.AllergyName,
                opt => opt.MapFrom(src => src.DependentAllergies
                    .FirstOrDefault() != null
                    && src.DependentAllergies.FirstOrDefault().Allergy != null
                    ? src.DependentAllergies.FirstOrDefault().Allergy.Name
                    : string.Empty));

        CreateMap<AllergyDependentRequestDTO, DependentAllergy>();
        CreateMap<DependentAllergy, DependentAllergyResponseDTO>()
            .ForMember(
                dest => dest.AllergyName,
                opt => opt.MapFrom(src => src.Allergy.Name));

        CreateMap<FamilyGroupRequestDTO, FamilyGroup>();
        CreateMap<FamilyGroup, FamilyGroupResponseDTO>();

        CreateMap<AllergyRequestDTO, Allergy>();
        CreateMap<Allergy, AllergyResponseDTO>();
    }
}
