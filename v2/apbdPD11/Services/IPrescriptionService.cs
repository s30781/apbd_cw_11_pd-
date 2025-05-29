using apbd_cw11.DTOs;

namespace apbd_cw11.Services;

public interface IPrescriptionService
{
    Task AddPrescription(AddPrescriptionRequestDto prescriptionRequestDto);
    Task<GetPatientDto> GetPatient(int id);
}