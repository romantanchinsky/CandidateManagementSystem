using CandidateManagement.Application.Interfaces;
using CandidateManagement.Domain.Entities;
using CandidateManagement.Domain.Enums;
using MediatR;

namespace CandidateManagement.Application.WorkingGroups.Commands
{
    public sealed class UpdateWorkingGroupMembersCommandHandler : IRequestHandler<UpdateWorkingGroupMembersCommand, Unit>
    {
        private readonly IWorkingGroupRepository _workingGroupRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWorkingGroupMembersCommandHandler(
            IWorkingGroupRepository workGroupRepository,
            IUserRepository userRepository,
            ICandidateRepository candidateRepository,
            IUnitOfWork unitOfWork)
        {
            _workingGroupRepository = workGroupRepository;
            _userRepository = userRepository;
            _candidateRepository = candidateRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateWorkingGroupMembersCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var workingGroup = await _workingGroupRepository.GetByIdAsync(request.WorkingGroupId) ?? throw new NotFoundDomainException($"Work group with id: {request.WorkingGroupId} not found");
                if (!await _userRepository.IsAdmin(request.CurrentUserId))
                {
                    throw new AccessDeniedDomainException("Only administrators can edit work group participants");
                }

                var currentMembers = workingGroup.Participants.ToList();
                var newMembers = await _userRepository.GetUsersByIdsAsync(request.UserIds);

                var usersToAdd = newMembers.Where(newUser => 
                    !currentMembers.Any(current => current.Id == newUser.Id)).ToList();
                
                var usersToRemove = currentMembers.Where(current => 
                    !request.UserIds.Contains(current.Id)).ToList();

                foreach (var userToAdd in usersToAdd)
                {
                    await ProcessUserAdditionAsync(userToAdd, workingGroup.Id);
                }
                foreach (var userToRemove in usersToRemove)
                {
                    await ProcessUserRemovalAsync(userToRemove, workingGroup.Id);
                }

                UpdateWorkGroupMembers(workingGroup, newMembers);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        private async Task ProcessUserRemovalAsync(User user, Guid workGroupId)
        {
            if (user.IsHR())
            {
                await MigrateCandidatesToOtherHRAsync(user.Id, workGroupId);
            }
            user.RemoveFromWorkGroup();
        }

        private async Task ProcessUserAdditionAsync(User user, Guid workGroupId)
        {
            if (user.Role != Role.HR)
            {
                throw new UserDomainException($"User {user.Login} with role: {user.Role} cannot be assigned to work group");
            }

            user.AssignToWorkingGroup(workGroupId);
            var candidates = await _candidateRepository.GetByCreatorAsync(user.Id);
            foreach (var candidate in candidates)
            {
                candidate.AssignToWorkingGroup(workGroupId);
            }
        }

        private void UpdateWorkGroupMembers(WorkingGroup workingGroup, List<User> newMembers)
        {
            foreach (var currentMember in workingGroup.Participants.ToList())
            {
                workingGroup.RemoveParticipant(currentMember);
            }

            foreach (var newMember in newMembers)
            {
                workingGroup.AddParticipant(newMember);
            }
        }

        private async Task MigrateCandidatesToOtherHRAsync(Guid hrUserId, Guid sourceWorkGroupId)
        {
            var candidates = await _candidateRepository.GetByCreatorAsync(hrUserId);
            
            if (!candidates.Any())
            {
                return;
            }

            var otherHR = await _userRepository.GetOtherHRInWorkGroupAsync(sourceWorkGroupId, hrUserId);
            
            if (otherHR == null)
            {
                await AssignCandidatesToAdminAsync(candidates, sourceWorkGroupId);
                return;
            }

            foreach (var candidate in candidates)
            {
                candidate.ChangeCreatedByUser(otherHR.Id);
            }
        }

        private async Task AssignCandidatesToAdminAsync(List<Candidate> candidates, Guid workGroupId)
        {
            var admin = await _userRepository.GetAdminUserAsync();
            foreach (var candidate in candidates)
            {
                candidate.ChangeCreatedByUser(admin.Id);
            }
        }
    }
}