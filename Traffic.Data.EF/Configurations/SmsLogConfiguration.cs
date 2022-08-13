using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traffic.Data.EF.Extensions;
using Traffic.Data.Entities;

namespace Traffic.Data.EF.Configurations
{
    public class SmsLogConfiguration : DbEntityConfiguration<SmsLog>
    {
        public override void Configure(EntityTypeBuilder<SmsLog> entity)
        {
            entity.Property(c => c.MessageId).HasColumnType("VARCHAR2(128 CHAR)");
            entity.HasIndex(c => new { c.MessageId }).IsUnique();
            entity.Property(c => c.SmsStatus).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.SmsProvider).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.CreatedBy).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.UpdatedBy).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.SmsSendingTime).HasColumnType("VARCHAR2(32 CHAR)");
        }
    }
}
