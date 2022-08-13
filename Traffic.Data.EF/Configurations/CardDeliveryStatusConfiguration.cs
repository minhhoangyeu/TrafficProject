using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traffic.Data.EF.Extensions;
using Traffic.Data.Entities;

namespace Traffic.Data.EF.Configurations
{
    public class CardDeliveryStatusConfiguration : DbEntityConfiguration<CardDeliveryStatus>
    {
        public override void Configure(EntityTypeBuilder<CardDeliveryStatus> entity)
        {
            entity.Property(c => c.StatusCode).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.DestinationPostCode).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.CreatedBy).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.UpdatedBy).HasColumnType("VARCHAR2(32 CHAR)");
        }
    }
}
