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

        CreateMap<DependentAllergyRequestDTO, DependentAllergy>();
        CreateMap<DependentAllergy, DependentAllergyResponseDTO>()
            .ForMember(
                dest => dest.AllergyId,
                opt => opt.MapFrom(src => src.Allergy.Id))
            .ForMember(
                dest => dest.AllergyName,
                opt => opt.MapFrom(src => src.Allergy.Name));


        CreateMap<FamilyGroupRequestDTO, FamilyGroup>();
        CreateMap<FamilyGroup, FamilyGroupResponseDTO>();

        CreateMap<AllergyRequestDTO, Allergy>();
        CreateMap<Allergy, AllergyResponseDTO>();

        CreateMap<VaccineRequestDTO, Vaccine>();
        CreateMap<Vaccine, VaccineResponseDTO>()
            .ForMember(dest => dest.VaccineCategoryId,
                opt => opt.MapFrom(src => src.VaccineCategory_Vaccines
                    .FirstOrDefault() != null
                    ? src.VaccineCategory_Vaccines.FirstOrDefault().VaccineCategoryId
                    : 0))
            .ForMember(
                dest => dest.VaccineCategoryName,
                opt => opt.MapFrom(src => src.VaccineCategory_Vaccines
                    .FirstOrDefault() != null
                    && src.VaccineCategory_Vaccines.FirstOrDefault().VaccineCategory != null
                    ? src.VaccineCategory_Vaccines.FirstOrDefault().VaccineCategory.Name
                    : string.Empty))
            .ForMember(
                dest => dest.VaccineCategoryDescription,
                opt => opt.MapFrom(src => src.VaccineCategory_Vaccines
                    .FirstOrDefault() != null
                    && src.VaccineCategory_Vaccines.FirstOrDefault().VaccineCategory.Description != null
                    ? src.VaccineCategory_Vaccines.FirstOrDefault().VaccineCategory.Description
                    : string.Empty));

        CreateMap<VaccineCategoryRequestDTO, VaccineCategory>();
        CreateMap<VaccineCategory, VaccineCategoryResponseDTO>();


        CreateMap<VaccineCategory_VaccineRequestDTO, VaccineCategory_Vaccine>();
        CreateMap<VaccineCategory_Vaccine, VaccineCategory_VaccineResponseDTO>()
            .ForMember(
                dest => dest.VaccineCategoryId,
                opt => opt.MapFrom(src => src.VaccineCategory.Id))
            .ForMember(
                dest => dest.VaccineCategoryName,
                opt => opt.MapFrom(src => src.VaccineCategory.Name))
            .ForMember(
                dest => dest.VaccineCategoryDescription,
                opt => opt.MapFrom(src => src.VaccineCategory.Description));
        CreateMap<VaccineCategory_Vaccine, VaccineCategoryResponseDTO>()
            .ForMember(
                dest => dest.Id, 
                opt => opt.MapFrom(src => src.VaccineCategory.Id))
            .ForMember(
                dest => dest.Name, 
                opt => opt.MapFrom(src => src.VaccineCategory.Name))
            .ForMember(
                dest => dest.Description, 
                opt => opt.MapFrom(src => src.VaccineCategory.Description));
    }
}
