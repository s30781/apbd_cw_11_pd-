using apbd_cw11.Data;
using apbd_cw11.DTOs;
using apbd_cw11.Exceptions;
using apbd_cw11.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw11.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly MyDbContext _context;

    public PrescriptionService(MyDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetPatientDto> GetPatient(int id)
    {
        var patient = await _context.Patients
            .Where(p => p.IdPatient == id)
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync();

        if (patient == null)
            throw new MyExceptionWhenNotFound("Patient not found");

        return new GetPatientDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthday = patient.Birthday,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName
                    },
                    Medicaments = p.PrescriptionMedicaments
                        .Select(pm => new GetMedicamentDto
                        {
                            IdMedicament = pm.IdMedicament,
                            Name = pm.Medicament.Name,
                            Description = pm.Medicament.Description,
                            Dose = pm.Dose
                        }).ToList()
                }).ToList()
        };
    }
    
    public async Task AddPrescription(AddPrescriptionRequestDto dto)
    {
        if (dto.DueDate < dto.Date)
            throw new MyExceptionWhenConflict("The due date of the perscription is earlier than the date");

        if (dto.Medicaments.Count > 10)
            throw new MyExceptionWhenConflict("Too much medicaments");

        var patient = await _context.Patients
            .FirstOrDefaultAsync(p =>
                p.FirstName == dto.Patient.FirstName &&
                p.LastName == dto.Patient.LastName &&
                p.Birthday.Equals(dto.Patient.Birthday));

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                Birthday = dto.Patient.Birthday
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        var doctor = await _context.Doctors.FindAsync(dto.IdDoctor);
        if (doctor == null)
            throw new MyExceptionWhenNotFound("Prescription doctor not found");

        var medicaments = await _context.Medicament
            .Where(m => dto.Medicaments.Select(x => x.IdMedicament).Contains(m.IdMedicament))
            .ToListAsync();

        if (medicaments.Count != dto.Medicaments.Count)
            throw new MyExceptionWhenNotFound("At least one medicament was not found");

        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            IdDoctor = dto.IdDoctor,
            IdPatient = patient.IdPatient,
            PrescriptionMedicaments = dto.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Details
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
    }

    
}