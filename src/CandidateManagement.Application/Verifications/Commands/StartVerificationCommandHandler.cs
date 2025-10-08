using CandidateManagement.Application.Interfaces;
using CandidateManagement.Application.Services;
using CandidateManagement.Domain.Entities;
using MediatR;

namespace CandidateManagement.Application.Verifications.Commands;

public class StartVerificationCommandHandler : IRequestHandler<StartVerificationCommand, Guid>
{
    private readonly IVerificationRepository _verificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBackgroundJobService _backgroundJobService;
    public StartVerificationCommandHandler(
        IVerificationRepository verificationRepository,
        IUnitOfWork unitOfWork,
        IBackgroundJobService backgroundJobService)
    {
        _verificationRepository = verificationRepository;
        _unitOfWork = unitOfWork;
        _backgroundJobService = backgroundJobService;
    }

    public async Task<Guid> Handle(StartVerificationCommand request, CancellationToken cancellationToken)
    {
        var verification = Verification.StartVerification(request.FullName, request.CurrentUserId);

        await _verificationRepository.AddAsync(verification);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _backgroundJobService.EnqueueVerificationSearch(verification.Id, request.FullName);

        return verification.Id;
    }
}