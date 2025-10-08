using CandidateManagement.Application.Auth.Dtos;
using CandidateManagement.Application.Candidates.Commands;
using CandidateManagement.Application.Candidates.Dtos;
using CandidateManagement.Application.Candidates.Requests;
using CandidateManagement.Application.Employees.Dtos;
using CandidateManagement.Application.Users.Commands;
using CandidateManagement.Application.Users.Dtos;
using CandidateManagement.Application.Users.Requests;
using CandidateManagement.Application.WorkingGroups.Commands;
using CandidateManagement.Domain.Entities;
using CandidateManagement.Domain.ValueObjects;
using Mapster;

namespace CandidateManagement.Application.Mapping;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateWorkingGroupCommand, WorkingGroup>()
            .MapWith(src => new WorkingGroup(
                src.Name
            ));

        config.NewConfig<User, TokenClaims>()
            .Map(dest => dest.UserId, src => src.Id)
            .Map(dest => dest.UserRole, src => src.Role);

        config.NewConfig<Candidate, CandidateReadDto>()
            .Map(dest => dest.FirstName,
                src => src.CandidateData.FullName.FirstName)
            .Map(dest => dest.LastName,
                src => src.CandidateData.FullName.LastName)
            .Map(dest => dest.Patronymic,
                src => src.CandidateData.FullName.Patronymic)
            .Map(dest => dest.Email,
                src => src.CandidateData.Email)
            .Map(dest => dest.PhoneNumber,
                src => src.CandidateData.PhoneNumber)
            .Map(dest => dest.Country,
                src => src.CandidateData.Country)
            .Map(dest => dest.DateOfBirth,
                src => src.CandidateData.DateOfBirth)
            .Map(dest => dest.SocialNetworks,
                src => src.CandidateData.SocialNetworks);

        // config.NewConfig<UpdateCandidateCommand, CandidateData>()
        //     .MapWith(scr => new CandidateData(
        //         new FullName(scr.FirstName, scr.LastName, scr.Patronymic),
        //         new Email(scr.Email),
        //         new PhoneNumber(scr.PhoneNumber),
        //         scr.Country,
        //         scr.DateOfBirth));

        config.NewConfig<CreateHrRequest, CreateHrCommand>()
            .Map(dest => dest.FullName,
                src => new FullName(src.FirstName,
                    src.LastName,
                    src.Patronymic));

        config.NewConfig<User, UserReadDto>()
            .Map(dest => dest.FirstName,
                src => src.FullName.FirstName)
            .Map(dest => dest.LastName,
                src => src.FullName.LastName)
            .Map(dest => dest.Patronymic,
                src => src.FullName.Patronymic);

        config.NewConfig<CreateCandidateRequest, CreateCandidateCommand>()
            .Map(dest => dest.FullName,
                src => new FullName(src.FirstName,
                    src.LastName,
                    src.Patronymic))
            .Map(dest => dest.Email,
                src => new Email(src.Email))
            .Map(dest => dest.PhoneNumber,
                src => new PhoneNumber(src.PhoneNumber));

        config.NewConfig<UpdateCandidateRequest, UpdateCandidateCommand>()
            .Map(dest => dest.FullName,
                src => new FullName(
                    src.FirstName,
                    src.LastName,
                    src.Patronymic))
            .Map(dest => dest.Email,
                src => new Email(src.Email))
            .Map(dest => dest.PhoneNumber,
                src => new PhoneNumber(src.PhoneNumber));

        config.NewConfig<Employee, EmployeeReadDto>()
            .Map(dest => dest.FirstName,
                src => src.CandidateData.FullName.FirstName)
            .Map(dest => dest.LastName,
                src => src.CandidateData.FullName.LastName)
            .Map(dest => dest.Patronymic,
                src => src.CandidateData.FullName.Patronymic)
            .Map(dest => dest.Email,
                src => src.CandidateData.Email)
            .Map(dest => dest.PhoneNumber,
                src => src.CandidateData.PhoneNumber)
            .Map(dest => dest.Country,
                src => src.CandidateData.Country)
            .Map(dest => dest.DateOfBirth,
                src => src.CandidateData.DateOfBirth)
            .Map(dest => dest.SocialNetworks,
                src => src.CandidateData.SocialNetworks);
    }
}