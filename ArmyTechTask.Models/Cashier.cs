// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.EntityFrameworkCore;

namespace ArmyTechTask.Models
{
    public partial class Cashier
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string CashierName { get; set; }

        [Column("BranchID")]
        public int BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        [InverseProperty(nameof(Models.Branches.Cashier))]
        public virtual Branches Branch { get; set; }

        [InverseProperty("Cashier")]
        public virtual ICollection<InvoiceHeader> InvoiceHeader { get; set; }
    }
}