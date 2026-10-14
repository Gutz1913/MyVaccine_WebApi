using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class DependentService : IDependentService
{
    private readonly IBaseRepository<Dependent> _dependentRepository;
    private readonly IMapper _mapper;
    public DependentService(IBaseRepository<Dependent> dependentRepository, IMapper mapper)
    {
        _dependentRepository = dependentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DependentResponseDTO>> GetAll()
    {
        var dependents = await _dependentRepository
            .GetAll()
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .AsNoTracking()
            .ToListAsync();

        var response = _mapper.Map<IEnumerable<DependentResponseDTO>>(dependents);

        return response;
    }

    public async Task<DependentResponseDTO> GetById(int id)
    {
        var dependent = await _dependentRepository
            .FindByAsNoTracking(d => d.Id == id)
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .FirstOrDefaultAsync();

        var response = _mapper.Map<DependentResponseDTO>(dependent);

        return response;
    }

    public async Task<IEnumerable<DependentResponseDTO>> GetDependentsByUserId(int userId)
    {
        var dependents = await _dependentRepository
            .FindByAsNoTracking(d => d.UserId == userId)
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .ToListAsync();

        var response = _mapper.Map<IEnumerable<DependentResponseDTO>>(dependents);

        return response;
    }

    public async Task<DependentResponseDTO> Add(DependentRequestDTO request)
    {
        var dependent = new Dependent
        {
            Name = request.Name,
            BirthDate = request.BirthDate,
            UserId = request.UserId,
            FamilyGroupId = request.FamilyGroupId
        };

        await _dependentRepository.Add(dependent);
        var dependentWithUserAndFamilyGroup = await _dependentRepository
            .FindBy(d => d.Id == dependent.Id)
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .FirstOrDefaultAsync();

        var response = _mapper.Map<DependentResponseDTO>(dependentWithUserAndFamilyGroup);

        return response;
    }

    public async Task<DependentResponseDTO> Update(int id, DependentRequestDTO request)
    {
        var dependent = await _dependentRepository
            .FindBy(d => d.Id == id)
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .FirstOrDefaultAsync();

        dependent.Name = request.Name;
        dependent.BirthDate = request.BirthDate;
        dependent.UserId = request.UserId;
        dependent.FamilyGroupId = request.FamilyGroupId;

        await _dependentRepository.Update(dependent);
        var response = _mapper.Map<DependentResponseDTO>(dependent);

        return response;
    }

    public async Task<DependentResponseDTO> Delete(int id)
    {
        var dependent = await _dependentRepository
            .FindBy(d => d.Id == id)
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .FirstOrDefaultAsync();

        await _dependentRepository.Delete(dependent);
        var response = _mapper.Map<DependentResponseDTO>(dependent);

        return response;
    }
}
