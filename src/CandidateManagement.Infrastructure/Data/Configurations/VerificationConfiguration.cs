using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CandidateManagement.Domain.Entities;
using System.Text.Json;

namespace CandidateManagement.Infrastructure.Data.Configurations;

public class VerificationConfiguration : IEntityTypeConfiguration<Verification>
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions();
    public void Configure(EntityTypeBuilder<Verification> builder)
    {
        builder.ToTable("Verifications");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.SearchedFullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.Status)
            .HasConversion(new EnumToStringConverter<VerificationStatus>())
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(v => v.FoundCandidateIds)
            .HasColumnType("jsonb");

        builder.Property(v => v.FoundEmployeeIds)
            .HasColumnType("jsonb");

        builder.Property(v => v.StartedAt)
            .IsRequired();

        builder.Property(v => v.CompletedAt)
            .IsRequired(false);

        builder.HasIndex(v => v.Status)
            .HasDatabaseName("IX_Verifications_Status");

        builder.HasIndex(v => v.StartedAt)
            .HasDatabaseName("IX_Verifications_StartedAt");

        builder.HasIndex(v => new { v.Status, v.StartedAt })
            .HasDatabaseName("IX_Verifications_Status_StartedAt");
    }
}