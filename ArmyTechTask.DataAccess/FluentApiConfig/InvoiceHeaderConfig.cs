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
    public class InvoiceHeaderConfig : IEntityTypeConfiguration<InvoiceHeader>
    {


        public void Configure(EntityTypeBuilder<InvoiceHeader> builder)
        {
            builder.Property(e => e.CustomerName).HasDefaultValueSql("('')");

            builder.Property(e => e.Invoicedate).HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.Branch)
                        .WithMany(p => p.InvoiceHeader)
                        .HasForeignKey(d => d.BranchId)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_InvoiceHeader_Branches");

            builder.HasOne(d => d.Cashier)
                        .WithMany(p => p.InvoiceHeader)
                        .HasForeignKey(d => d.CashierId)
                        .HasConstraintName("FK_InvoiceHeader_Cashier");
        }
    }
}
