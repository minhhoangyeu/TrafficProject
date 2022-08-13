using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traffic.Data.EF.Extensions;
using Traffic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Data.EF.Configurations
{
    public class CardKafkaHistoryConfiguration : DbEntityConfiguration<CardKafkaHistory>
    {
        public override void Configure(EntityTypeBuilder<CardKafkaHistory> entity)
        {
            entity.HasIndex(c => c.CardDeliveryInfoId);
            entity.Property(c => c.PCID).HasColumnType("VARCHAR2(32 CHAR)");
            entity.Property(c => c.PostCode).HasColumnType("VARCHAR2(64 CHAR)");
            entity.Property(c => c.Id_CUID).HasColumnType("VARCHAR2(64 CHAR)");
            entity.Property(c => c.DeliveryStatusID).HasColumnType("VARCHAR2(20 CHAR)");
            entity.Property(c => c.DeliveryStatusCaption).HasColumnType("VARCHAR2(200 CHAR)");
            entity.Property(c => c.CardType).HasColumnType("VARCHAR2(20 CHAR)");
        }
    }
}
