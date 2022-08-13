using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traffic.Data.EF.Extensions;
using Traffic.Data.Entities;

namespace Traffic.Data.EF.Configurations
{
    public class CardToDeliveryConfiguration : DbEntityConfiguration<CardToDelivery>
    {
        public override void Configure(EntityTypeBuilder<CardToDelivery> entity)
        {
            entity.HasMany(c => c.OperationStatusHistories).WithOne(p => p.CardToDelivery).HasForeignKey(s => s.CardToDeliveryId);
            entity.Property(c => c.PCID).HasColumnType("VARCHAR2(32 CHAR)").IsRequired();
            entity.HasIndex(c => c.PCID).IsUnique();
            entity.Property(c => c.CardType).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.CreatedBy).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.UpdatedBy).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.OperationStatus).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.CardStatus).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.ContractNumber).HasColumnType("VARCHAR2(32 CHAR)");
            entity.HasIndex(c => c.OperationStatus);
        }
    }
}
