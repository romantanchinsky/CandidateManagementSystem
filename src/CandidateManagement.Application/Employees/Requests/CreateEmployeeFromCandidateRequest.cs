namespace CandidateManagement.Application.Employees.Requests;

public sealed record CreateEmployeeFromCandidateRequest(
    Guid CandidateId
);