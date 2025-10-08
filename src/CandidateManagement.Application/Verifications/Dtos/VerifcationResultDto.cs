namespace CandidateManagement.Application.Verifications.Dtos;
public record VerificationResultDto
{
    public Guid VerificationId { get; init; }
    public string SearchedFullName { get; init; }
    public string Status { get; init; }
    public List<Guid> FoundCandidateIds { get; init; } = new();
    public List<Guid> FoundEmployeeIds { get; init; } = new();
    public DateTime StartedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
}