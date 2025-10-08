using CandidateManagement.Domain.Interfaces;

namespace CandidateManagement.Domain.Entities;

public class Employee : IAggregateRoot
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public CandidateData CandidateData { get; private set; }
    public DateTime HireDate { get; private set; }

    private Employee() { }

    public Employee(Candidate candidate, DateTime hireDate)
    {
        CandidateData = candidate.CandidateData;
        HireDate = hireDate;
    }

    public void UpdateHireDate(DateTime newDate) => HireDate = newDate;
}