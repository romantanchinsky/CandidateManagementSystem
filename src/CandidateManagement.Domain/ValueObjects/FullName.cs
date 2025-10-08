namespace CandidateManagement.Domain.ValueObjects;

public sealed record FullName
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Patronymic { get; init; }

    public FullName(string firstName, string lastName, string patronymic)
    {
        FirstName = firstName ?? throw new ArgumentException("FirstName required", nameof(firstName));;
        LastName = lastName ?? throw new ArgumentException("Lastname required", nameof(lastName));
        Patronymic = patronymic;
    }

    public override string ToString()
    {
        return Patronymic is null
            ? $"{FirstName} {LastName}"
            : $"{FirstName} {LastName} {Patronymic}";
    }
}