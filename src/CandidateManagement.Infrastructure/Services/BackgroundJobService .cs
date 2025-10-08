using CandidateManagement.Application.Interfaces;
using CandidateManagement.Application.Services;
using CandidateManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CandidateManagement.Infrastructure.Services;

public class BackgroundJobService : IBackgroundJobService
{
    private readonly IServiceProvider _serviceProvider;

    public BackgroundJobService(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void EnqueueVerificationSearch(Guid verificationId, string fullName)
    {
        Task.Run(async () =>
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                await ProcessVerificationSearchAsync(scope.ServiceProvider, verificationId, fullName);
            }
            catch (Exception ex)
            {
            }
        });
    }

    private async Task ProcessVerificationSearchAsync(IServiceProvider serviceProvider, Guid verificationId, string fullName)
    {
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        var verification = await unitOfWork.VerificationRepository.GetByIdAsync(verificationId);
        if (verification == null)
        {
            return;
        }

        try
        {

            var searchTasks = new List<Task>
            {
                SearchCandidatesAsync(verification, fullName, unitOfWork.CandidateRepository),
                SearchEmployeesAsync(verification, fullName, unitOfWork.EmployeeRepository)
            };

            await Task.WhenAll(searchTasks);

            verification.CompleteVerification();
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            verification.MarkAsFailed();
            await unitOfWork.SaveChangesAsync();
        }
    }

    private async Task SearchCandidatesAsync(Verification verification, string fullName, ICandidateRepository candidateRepository)
    {
        var candidates = await candidateRepository.SearchByNameAsync(fullName);
        verification.AddFoundCandidates(candidates.Select(c => c.Id).ToList());
    }

    private async Task SearchEmployeesAsync(Verification verification, string fullName, IEmployeeRepository employeeRepository)
    {
        var employees = await employeeRepository.SearchByNameAsync(fullName); 
        verification.AddFoundEmployees(employees.Select(e => e.Id).ToList());
    }
}