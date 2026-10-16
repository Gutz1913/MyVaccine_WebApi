using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class VaccineCategoryService : IVaccineCategoryService
{
    private readonly IBaseRepository<VaccineCategory> _vaccineCategoryRepository;
    private readonly IBaseRepository<Vaccine> _vaccineRepository;
    private readonly IBaseRepository<VaccineCategory_Vaccine> _vaccineCategory_VaccineRepository;
    private readonly IMapper _mapper;

    public VaccineCategoryService(
        IBaseRepository<VaccineCategory> vaccineCategoryRepository,
        IBaseRepository<Vaccine> vaccineRepository, IBaseRepository<VaccineCategory_Vaccine> vaccineCategory_VaccineRepository,
        IMapper mapper)
    {
        _vaccineCategoryRepository = vaccineCategoryRepository;
        _vaccineRepository = vaccineRepository;
        _vaccineCategory_VaccineRepository = vaccineCategory_VaccineRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VaccineCategoryResponseDTO>> GetAll()
    {
        var categories = await _vaccineCategoryRepository
            .GetAll()
            .AsNoTracking()
            .ToListAsync();

        var response = _mapper.Map<IEnumerable<VaccineCategoryResponseDTO>>(categories);

        return response;
    }

    public async Task<VaccineCategoryResponseDTO> GetById(int id)
    {
        var category = await _vaccineCategoryRepository
            .FindByAsNoTracking(vc => vc.Id == id)
            .FirstOrDefaultAsync();

        var response = _mapper.Map<VaccineCategoryResponseDTO>(category);

        return response;
    }

    public async Task<IEnumerable<VaccineCategoryResponseDTO>> GetVaccinesByCategoryId(int vaccineCategoryId)
    {
        var vaccines = await _vaccineCategoryRepository
            .FindByAsNoTracking(vc => vc.Id == vaccineCategoryId)
            .Include(vc => vc.VaccineCategory_Vaccines)
            .ThenInclude(vc => vc.Vaccine)
            .ToListAsync();

        var response = _mapper.Map<IEnumerable<VaccineCategoryResponseDTO>>(vaccines);

        return response;
    }

    public async Task<VaccineCategoryResponseDTO> Add(VaccineCategoryRequestDTO request)
    {
        var category = new VaccineCategory
        {
            Name = request.Name,
            Description = request.Description
        };

        await _vaccineCategoryRepository.Add(category);

        var response = _mapper.Map<VaccineCategoryResponseDTO>(category);

        return response;
    }

    public async Task<VaccineCategoryResponseDTO> Update(int id, VaccineCategoryRequestDTO request)
    {
        var category = await _vaccineCategoryRepository
            .FindBy(vc => vc.Id == id)
            .FirstOrDefaultAsync();

        category.Name = request.Name;
        category.Description = request.Description;


        await _vaccineCategoryRepository.Update(category);
        var response = _mapper.Map<VaccineCategoryResponseDTO>(category);

        return response;
    }

    public async Task<VaccineCategoryResponseDTO> Delete(int id)
    {
        var category = await _vaccineCategoryRepository
            .FindBy(vc => vc.Id == id)
            .FirstOrDefaultAsync();

        await _vaccineCategoryRepository.Delete(category);
        var response = _mapper.Map<VaccineCategoryResponseDTO>(category);

        return response;
    }

    public async Task RegisterVaccineAsync(VaccineCategory_VaccineRequestDTO request)
    {
        var category = await _vaccineCategoryRepository
            .FindBy(vc => vc.Id == request.VaccineCategoryId)
            .FirstOrDefaultAsync();

        if (category == null)
            throw new KeyNotFoundException($"Categoría con ID: {request.VaccineCategoryId} no encontrada.");

        var vaccine = await _vaccineRepository
            .FindBy(v => v.Id == request.VaccineId)
            .FirstOrDefaultAsync();

        if (vaccine == null)
            throw new KeyNotFoundException($"Vacuna con ID: {request.VaccineId} no encontrada.");

        var vaccineCategory_Vaccine = new VaccineCategory_Vaccine()
        {
            VaccineCategoryId = request.VaccineCategoryId,
            VaccineId = request.VaccineId,
            RegisteredAt = DateTime.UtcNow
        };

        await _vaccineCategory_VaccineRepository.Add(vaccineCategory_Vaccine);
        _mapper.Map<VaccineCategory_VaccineResponseDTO>(vaccineCategory_Vaccine);
    }
}
