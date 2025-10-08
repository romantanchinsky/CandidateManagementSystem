using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CandidateManagement.Domain.Entities;

namespace CandidateManagement.Infrastructure.Data.Configurations
{
    public class WorkGroupConfiguration : IEntityTypeConfiguration<WorkingGroup>
    {
        public void Configure(EntityTypeBuilder<WorkingGroup> builder)
        {
            builder.ToTable("WorkGroups");
            builder.HasKey(wg => wg.Id);

            builder.Property(wg => wg.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(wg => wg.Participants)
                .WithOne()
                .HasForeignKey(u => u.WorkingGroupId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(wg => wg.Candidates)
                .WithOne()
                .HasForeignKey(c => c.WorkingGroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(wg => wg.Name)
                .IsUnique()
                .HasDatabaseName("IX_WorkGroups_Name");
        }
    }
}