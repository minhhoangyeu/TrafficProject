using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traffic.Data.EF.Extensions;
using Traffic.Data.Entities;

namespace Traffic.Data.EF.Configurations
{
    public class OperationStatusHistoryConfiguration : DbEntityConfiguration<OperationStatusHistory>
    {
        public override void Configure(EntityTypeBuilder<OperationStatusHistory> entity)
        {
            entity.Property(c => c.IsActive).HasDefaultValue(true);
            entity.Property(c => c.PREVIOUS_OPERATION_STATUS).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.Reason).HasMaxLength(512);
            entity.Property(c => c.Address).HasMaxLength(512);
            entity.Property(c => c.Note).HasMaxLength(512);
            entity.Property(c => c.CreatedBy).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.UpdatedBy).HasColumnType("VARCHAR2(32 CHAR)");
        }
    }
}
