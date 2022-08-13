using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traffic.Data.EF.Extensions;
using Traffic.Data.Entities;

namespace Traffic.Data.EF.Configurations
{
    public class CardDeliveryInfoConfiguration : DbEntityConfiguration<CardDeliveryInfo>
    {
        public override void Configure(EntityTypeBuilder<CardDeliveryInfo> entity)
        {
            entity.HasMany(d => d.CardDeliveryStatus).WithOne(p => p.CardDeliveryInfo).HasForeignKey(s => s.CardDeliveryInfoId);
            entity.HasIndex(c => new { c.PCID, c.PostCode }).IsUnique();
            entity.Property(c => c.PCID).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.OrderId).HasColumnType("VARCHAR2(64 CHAR)");
            entity.Property(c => c.PostCode).HasColumnType("VARCHAR2(64 CHAR)");
            entity.Property(c => c.LastStatusCode).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.AcceptancePostCode).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.LastDestinationPostCode).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.ReceiverProvinceId).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.ReceiverName).HasMaxLength(128);
            entity.Property(c => c.ReceiverTel).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.CreatedBy).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.UpdatedBy).HasColumnType("VARCHAR2(32 CHAR)");
        }
    }
}
