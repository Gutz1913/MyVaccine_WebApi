using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FamilyGroupsController : ControllerBase
{
    private readonly IFamilyGroupService _familyGroupService;
    private readonly IValidator<FamilyGroupRequestDTO> _validator;

    public FamilyGroupsController(IFamilyGroupService familyGroupService, IValidator<FamilyGroupRequestDTO> validator)
    {
        _familyGroupService = familyGroupService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var familyGroups = await _familyGroupService.GetAll();

        return Ok(familyGroups);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var familyGroup = await _familyGroupService.GetById(id);
        return Ok(familyGroup);
    }

    [HttpPost]
    public async Task<IActionResult> Create(FamilyGroupRequestDTO familyGroupDto)
    {
        var validationResult = await _validator.ValidateAsync(familyGroupDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var familyGroup = await _familyGroupService.Add(familyGroupDto);

        return Ok(familyGroup);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, FamilyGroupRequestDTO familyGroupDto)
    {
        var familyGroup = await _familyGroupService.Update(id, familyGroupDto);
        if (familyGroup == null)
        {
            return NotFound();
        }
        return Ok(familyGroup);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var familyGroup = await _familyGroupService.Delete(id);
        if (familyGroup == null)
        {
            return NotFound();
        }
        return Ok(familyGroup);
    }
}
