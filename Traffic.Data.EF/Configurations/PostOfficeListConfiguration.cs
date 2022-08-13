using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traffic.Data.EF.Extensions;
using Traffic.Data.Entities;

namespace Traffic.Data.EF.Configurations
{
    public class PostOfficeListConfiguration : DbEntityConfiguration<PostOfficeList>
    {
        public override void Configure(EntityTypeBuilder<PostOfficeList> entity)
        {
            entity.Property(c => c.PostOfficeCode).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.CenterCode).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.ProvinceCode).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.PostOfficeType).HasColumnType("VARCHAR2(32 CHAR)");
        }
    }
}
