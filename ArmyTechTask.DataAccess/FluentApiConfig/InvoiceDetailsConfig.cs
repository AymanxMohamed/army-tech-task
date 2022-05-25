using ArmyTechTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyTechTask.DataAccess.FluentApiConfig
{
    public class InvoiceDetailsConfig : IEntityTypeConfiguration<InvoiceDetails>
    {
        public void Configure(EntityTypeBuilder<InvoiceDetails> builder)
        {
            builder.Property(e => e.ItemName).HasDefaultValueSql("('')");

            builder.HasOne(d => d.InvoiceHeader)
                .WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.InvoiceHeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetails_InvoiceHeader");
        }

    }
}
