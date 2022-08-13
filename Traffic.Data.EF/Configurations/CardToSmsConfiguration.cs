using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traffic.Data.EF.Extensions;
using Traffic.Data.Entities;

namespace Traffic.Data.EF.Configurations
{
    public class CardToSmsConfiguration : DbEntityConfiguration<CardToSms>
    {
        public override void Configure(EntityTypeBuilder<CardToSms> entity)
        {
            entity.HasMany(c => c.SmsLogs).WithOne(p => p.CardToSms).HasForeignKey(s => s.CardToSmsId);
            entity.Property(c => c.PCID).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.PostCode).HasColumnType("VARCHAR2(64 CHAR)");
            entity.Property(c => c.ContractNumber).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.PhoneNumber).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.SendingStatus).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.CreatedBy).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.UpdatedBy).HasColumnType("VARCHAR2(32 CHAR)");
            entity.HasIndex(c => c.SendingStatus);
        }
    }
}
