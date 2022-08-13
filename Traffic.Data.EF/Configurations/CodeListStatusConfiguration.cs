using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traffic.Data.EF.Extensions;
using Traffic.Data.Entities;

namespace Traffic.Data.EF.Configurations
{
    public class CodeListStatusConfiguration : DbEntityConfiguration<CodeListStatus>
    {
        public override void Configure(EntityTypeBuilder<CodeListStatus> entity)
        {
            entity.Property(c => c.Group).HasColumnType("VARCHAR2(512 CHAR)");
            entity.Property(c => c.Code).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.Name).HasMaxLength(512);
            entity.Property(c => c.CreatedBy).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.UpdatedBy).HasColumnType("VARCHAR2(32 CHAR)");
        }
    }
}
