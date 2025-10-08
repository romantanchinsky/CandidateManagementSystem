using CandidateManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CandidateManagement.Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(e => e.Id);

        builder.OwnsOne(e => e.CandidateData, data =>
        {
            data.OwnsOne(d => d.FullName, fullName =>
            {
                fullName.Property(fn => fn.FirstName).HasColumnName("FirstName").HasMaxLength(50);
                fullName.Property(fn => fn.LastName).HasColumnName("LastName").HasMaxLength(50);
                fullName.Property(fn => fn.Patronymic).HasColumnName("Patronymic").HasMaxLength(50);
            });

            data.OwnsOne(d => d.Email, email =>
            {
                email.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Email");
            });
            data.OwnsOne(d => d.PhoneNumber, phoneNumber =>
            {
                phoneNumber.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("PhoneNumber");
            });
            data.Property(d => d.Country).HasMaxLength(100);
            data.Property(d => d.DateOfBirth).HasColumnType("date");

            data.OwnsMany(d => d.SocialNetworks, sn =>
            {
                sn.WithOwner().HasForeignKey("EmployeeId");
                sn.Property(s => s.Username)
                    .IsRequired()
                    .HasMaxLength(100);
                sn.Property(s => s.Type)
                    .IsRequired()
                    .HasMaxLength(50);
                sn.Property(s => s.DateAdded)
                    .IsRequired();
            sn.HasKey("Id");
            });
        });

        builder.Property(e => e.HireDate).IsRequired();
    }
}