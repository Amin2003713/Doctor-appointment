namespace AppointmentPlanner.Shared.AuthModels;

public sealed record RegisterRequest(string Email , string Password , string FullName);