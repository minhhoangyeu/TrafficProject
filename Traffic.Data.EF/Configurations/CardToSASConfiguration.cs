using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traffic.Data.EF.Extensions;
using Traffic.Data.Entities;

namespace Traffic.Data.EF.Configurations
{
    public class CardToSASConfiguration : DbEntityConfiguration<CardToSAS>
    {
        public override void Configure(EntityTypeBuilder<CardToSAS> entity)
        {
            entity.Property(c => c.PostCode).HasColumnType("VARCHAR2(64 CHAR)").IsRequired();
            entity.Property(c => c.PCID).HasColumnType("VARCHAR2(32 CHAR)").IsRequired();
            entity.Property(c => c.CreatedBy).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.UpdatedBy).HasColumnType("VARCHAR2(32 CHAR)");
        }
    }
}
