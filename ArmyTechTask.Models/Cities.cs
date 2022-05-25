﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.EntityFrameworkCore;

namespace ArmyTechTask.Models
{
    public partial class Cities
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string CityName { get; set; }

        [InverseProperty("City")]
        public virtual ICollection<Branches> Branches { get; set; }
    }
}