namespace CandidateManagement.Application.Services;

public interface IBackgroundJobService
{
    void EnqueueVerificationSearch(Guid verificationId, string fullName);
    
}