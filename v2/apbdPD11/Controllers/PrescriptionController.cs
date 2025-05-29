using apbd_cw11.DTOs;
using apbd_cw11.Exceptions;
using apbd_cw11.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_cw11.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionController(IPrescriptionService _prescriptionService) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatient([FromRoute] int id)
    {
        try
        {
            var result = await _prescriptionService.GetPatient(id);
            return Ok(result);
        }
        catch (MyExceptionWhenNotFound e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> PostPrescription(AddPrescriptionRequestDto prescription)
    {
        try
        {
            await _prescriptionService.AddPrescription(prescription);
            return Created("", null);
        }
        catch (MyExceptionWhenNotFound e)
        {
            return NotFound(e.Message);
        }
        catch (MyExceptionWhenConflict e)
        {
            return Conflict(e.Message);
        }
    }
}