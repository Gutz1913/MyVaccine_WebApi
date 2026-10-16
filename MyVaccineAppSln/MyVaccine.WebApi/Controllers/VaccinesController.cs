using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class VaccinesController : ControllerBase
{
    private readonly IVaccineService _vaccineService;
    private readonly IValidator<VaccineRequestDTO> _validator;

    public VaccinesController(IVaccineService vaccineService, IValidator<VaccineRequestDTO> validator)
    {
        _vaccineService = vaccineService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vaccines = await _vaccineService.GetAll();

        return Ok(vaccines);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vaccine = await _vaccineService.GetById(id);
        return Ok(vaccine);
    }

    [HttpPost]
    public async Task<IActionResult> Create(VaccineRequestDTO vaccineDto)
    {
        var validationResult = await _validator.ValidateAsync(vaccineDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var vaccine = await _vaccineService.Add(vaccineDto);

        return Ok(vaccine);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, VaccineRequestDTO vaccineDto)
    {
        var vaccine = await _vaccineService.Update(id, vaccineDto);
        if (vaccine == null)
        {
            return NotFound();
        }
        return Ok(vaccine);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vaccine = await _vaccineService.Delete(id);
        if (vaccine == null)
        {
            return NotFound();
        }
        return Ok(vaccine);
    }
}