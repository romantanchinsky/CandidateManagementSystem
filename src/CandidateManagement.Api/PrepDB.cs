using CandidateManagement.Application.Services;
using CandidateManagement.Domain.Entities;
using CandidateManagement.Domain.Enums;
using CandidateManagement.Domain.ValueObjects;
using CandidateManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CandidateManagement.Api;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
    {
        using var servicesScope = app.ApplicationServices.CreateScope();
        SeedData(servicesScope.ServiceProvider.GetService<ApplicationDbContext>(),
            servicesScope.ServiceProvider.GetService<IPasswordHasher>(),
            isProduction);
    }

    private static void SeedData(ApplicationDbContext context,
        IPasswordHasher passwordHasher,
        bool isProduction)
    {
        if (isProduction)
        {
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($" --> Could not run migrations: {ex.Message}");
            }
        }
        if (!context.Users.Any())
            {
                var admin = new User(
                        Role.Admin,
                        new FullName(
                            "Alexander",
                            "Nevsky",
                            null
                        ),
                        "Admin",
                        passwordHasher.HashPassword("Admin")
                    );

                var hrs = new User[] {
                new User(
                    Role.HR,
                    new FullName(
                        "hr1FristName",
                        "hr1LastName",
                        null
                    ),
                    "hr1login",
                    passwordHasher.HashPassword("hr1Password")
                ),
                new User(
                    Role.HR,
                    new FullName(
                        "hr2FristName",
                        "hr2LastName",
                        null
                    ),
                    "hr2login",
                    passwordHasher.HashPassword("hr2Password")
                ),
                new User(
                    Role.HR,
                    new FullName(
                        "hr3FristName",
                        "hr3LastName",
                        null
                    ),
                    "hr3login",
                    passwordHasher.HashPassword("hr3Password")
                ),
                new User(
                    Role.HR,
                    new FullName(
                        "hr4FristName",
                        "hr4LastName",
                        null
                    ),
                    "hr4login",
                    passwordHasher.HashPassword("hr4Password")
                )
            };


                var workingGroups = new WorkingGroup[] {
                new WorkingGroup(
                    "groupOne"
                ),
                new WorkingGroup(
                    "groupTwo"
                )
            };
                hrs[0].AssignToWorkingGroup(workingGroups[0].Id);
                hrs[1].AssignToWorkingGroup(workingGroups[0].Id);
                var candidates = new Candidate[]{
                new Candidate(
                    new CandidateData(
                        new FullName
                        (
                            "candidate1Firstname",
                            "candidate1LastName",
                            "candidate1Patronymic"
                        ),
                        new Email("candidate1@mail.ru"),
                        new PhoneNumber("12335555"),
                        "Country1",
                        DateTime.UtcNow
                    ),
                    WorkSchedule.Hybrid,
                    workingGroups[0].Id,
                    hrs[0].Id
                ),
                new Candidate(
                    new CandidateData(
                        new FullName
                        (
                            "candidate2Firstname",
                            "candidate2LastName",
                            "candidate2Patronymic"
                        ),
                        new Email("candidate2@mail.ru"),
                        new PhoneNumber("12335555"),
                        "Country1",
                        DateTime.UtcNow
                    ),
                    WorkSchedule.Hybrid,
                    workingGroups[0].Id,
                    hrs[1].Id
                ),
                new Candidate(
                    new CandidateData(
                        new FullName
                        (
                            "candidate3Firstname",
                            "candidate3LastName",
                            "candidate3Patronymic"
                        ),
                        new Email("candidate3@mail.ru"),
                        new PhoneNumber("12335555"),
                        "Country1",
                        DateTime.UtcNow
                    ),
                    WorkSchedule.Hybrid,
                    workingGroups[1].Id,
                    hrs[2].Id
                ),
                new Candidate(
                    new CandidateData(
                        new FullName
                        (
                            "candidate4Firstname",
                            "candidate4LastName",
                            "candidate4Patronymic"
                        ),
                        new Email("candidate4@mail.ru"),
                        new PhoneNumber("12335555"),
                        "Country1",
                        DateTime.UtcNow
                    ),
                    WorkSchedule.Hybrid,
                    workingGroups[1].Id,
                    hrs[3].Id
                ),
                new Candidate(
                    new CandidateData(
                        new FullName
                        (
                            "candidate5Firstname",
                            "candidate5LastName",
                            "candidate5Patronymic"
                        ),
                        new Email("candidate5@mail.ru"),
                        new PhoneNumber("12335555"),
                        "Country1",
                        DateTime.UtcNow
                    ),
                    WorkSchedule.Hybrid,
                    workingGroups[0].Id,
                    hrs[0].Id
                ),
                new Candidate(
                    new CandidateData(
                        new FullName
                        (
                            "candidate6Firstname",
                            "candidate6LastName",
                            "candidate6Patronymic"
                        ),
                        new Email("candidate6@mail.ru"),
                        new PhoneNumber("12335555"),
                        "Country2",
                        DateTime.UtcNow
                    ),
                    WorkSchedule.Hybrid,
                    workingGroups[1].Id,
                    hrs[2].Id
                )
            };
                int count = 0;
                foreach (var c in candidates)
                {
                    c.CandidateData.AddSocialNetwork($"username{count}", $"type{count % 3}");
                }

                Console.WriteLine("--> Seeding WorkingGroups");
                context.WorkingGroups.AddRange(
                    workingGroups
                );
                Console.WriteLine("--> Seeding Users");
                context.Users.Add(
                    admin
                );
                context.Users.AddRange(
                    hrs
                );
                context.Candidates.AddRange(
                    candidates
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
    }
}