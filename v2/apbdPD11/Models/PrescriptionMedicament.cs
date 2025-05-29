﻿namespace apbd_cw11.Models;

public class PrescriptionMedicament
{
    public int IdPrescription { get; set; }
    public int IdMedicament { get; set; }

    public int Dose { get; set; }
    public string Details { get; set; }

    public Prescription Prescription { get; set; }
    public Medicament Medicament { get; set; }
}