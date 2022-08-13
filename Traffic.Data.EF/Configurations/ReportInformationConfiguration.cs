using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traffic.Data.EF.Extensions;
using Traffic.Data.Entities;

namespace Traffic.Data.EF.Configurations
{
    public class ReportInformationConfiguration : DbEntityConfiguration<ReportInformation>
    {
        public override void Configure(EntityTypeBuilder<ReportInformation> entity)
        {
            entity.Property(c => c.ReportCode).HasColumnType("VARCHAR2(50 CHAR)");
            entity.HasIndex(c => new { c.ReportCode }).IsUnique();
            entity.Property(c => c.ReportName).HasColumnType("VARCHAR2(100 CHAR)");
            entity.Property(c => c.Description).HasColumnType("VARCHAR2(250 CHAR)");
            entity.Property(c => c.Period).HasColumnType("VARCHAR2(20 CHAR)");
            entity.Property(c => c.RefreshButton).HasDefaultValue(true);
            entity.Property(c => c.DownloadButton).HasDefaultValue(true);
            entity.Property(c => c.TotalStoreProcedureName).HasColumnType("VARCHAR2(100 CHAR)");
            entity.Property(c => c.DetailStoreProcedureName).HasColumnType("VARCHAR2(100 CHAR)");

            entity.Property(c => c.CreatedBy).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.UpdatedBy).HasColumnType("VARCHAR2(32 CHAR)");
        }
    }
}
