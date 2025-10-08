using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CandidateManagement.Domain.Entities;

namespace CandidateManagement.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion(
                    login => login.ToLower(),
                    value => value
                );

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.OwnsOne(u => u.FullName, fullName =>
            {
                fullName.Property(fn => fn.FirstName)
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnName("FirstName");

                fullName.Property(fn => fn.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("LastName");

                fullName.Property(fn => fn.Patronymic)
                    .HasMaxLength(50)
                    .HasColumnName("Patronymic");
            });

            builder.HasOne<WorkingGroup>()
                .WithMany()
                .HasForeignKey(u => u.WorkingGroupId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(u => u.Login)
                .IsUnique()
                .HasDatabaseName("IX_Users_Login");

            builder.HasIndex(u => u.WorkingGroupId)
                .HasDatabaseName("IX_Users_WorkGroupId");
        }
    }
}