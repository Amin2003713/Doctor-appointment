﻿namespace Api.Endpoints.Models.Prescriptions;

public enum PrescriptionStatus
{
    Draft     = 0,
    Issued    = 1,
    Dispensed = 2,
    Cancelled = 3
}