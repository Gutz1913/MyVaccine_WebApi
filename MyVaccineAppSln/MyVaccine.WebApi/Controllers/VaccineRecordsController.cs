using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VaccineRecordsController : ControllerBase
{
    private readonly IVaccineRecordService _vaccineRecordService;
    private readonly IValidator<VaccineRecordRequestDTO> _validator;

    public VaccineRecordsController(IVaccineRecordService vaccineRecordService, IValidator<VaccineRecordRequestDTO> validator)
    {
        _vaccineRecordService = vaccineRecordService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _vaccineRecordService.GetAll();

        return Ok(records);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _vaccineRecordService.GetById(id);
        return Ok(record);
    }

    [HttpGet("get-records-by-userid/{userId}")]
    public async Task<IActionResult> GetRecordsByUserId(int userId)
    {
        var records = await _vaccineRecordService.GetRecordsByUserId(userId);
        return Ok(records);
    }

    [HttpPost]
    public async Task<IActionResult> Create(VaccineRecordRequestDTO recordDto)
    {
        var validationResult = await _validator.ValidateAsync(recordDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var record = await _vaccineRecordService.Add(recordDto);

        return Ok(record);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, VaccineRecordRequestDTO recordDto)
    {
        var record = await _vaccineRecordService.Update(id, recordDto);
        if (record == null)
        {
            return NotFound();
        }
        return Ok(record);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await _vaccineRecordService.Delete(id);
        if (record == null)
        {
            return NotFound();
        }
        return Ok(record);
    }
}

