using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CandidateManagement.Domain.Entities;

namespace CandidateManagement.Infrastructure.Data.Configurations
{
    public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.ToTable("Candidates");

            builder.HasKey(c => c.Id);

            builder.OwnsOne(c => c.CandidateData, candidateData =>
            {
                candidateData.OwnsOne(cd => cd.FullName, fullName =>
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

                candidateData.OwnsOne(cd => cd.Email, email =>
                {
                    email.Property(e => e.Value)
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnName("Email");
                });

                candidateData.OwnsOne(cd => cd.PhoneNumber, phoneNumber =>
                {
                    phoneNumber.Property(p => p.Value)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PhoneNumber");
                });

                candidateData.Property(cd => cd.Country)
                    .HasMaxLength(100);

                candidateData.Property(cd => cd.DateOfBirth)
                    .HasColumnType("date");

                candidateData.OwnsMany(cd => cd.SocialNetworks, sn =>
                {
                    sn.WithOwner().HasForeignKey("CandidateId");
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

            builder.HasOne<WorkingGroup>()
                .WithMany()
                .HasForeignKey(c => c.WorkingGroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.CreatedByUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.WorkingGroupId);
            builder.HasIndex(c => c.WorkSchedule);
        }
    }
}