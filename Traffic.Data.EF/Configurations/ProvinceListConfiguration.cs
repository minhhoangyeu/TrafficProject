using Microsoft.EntityFrameworkCore;
using Traffic.Data.EF.Extensions;
using Traffic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Data.EF.Configurations
{
    public class ProvinceListConfiguration : DbEntityConfiguration<ProvinceList>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProvinceList> entity)
        {
            entity.Property(p => p.ProvinceCode).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(p => p.ProvinceName).HasMaxLength(32);
        }
    }
}
