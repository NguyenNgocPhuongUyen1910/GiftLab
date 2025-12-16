using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GiftLab.Data.Entities;

public partial class Customer
{
    [Key]
    public int CustomerID { get; set; }

    [StringLength(255)]
    public string? FullName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Birthday { get; set; }

    [StringLength(255)]
    public string? Avatar { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    [StringLength(150)]
    public string? Email { get; set; }

    [StringLength(12)]
    [Unicode(false)]
    public string? Phone { get; set; }

    public int? LocationID { get; set; }

    public int? District { get; set; }

    public int? Ward { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [StringLength(50)]
    public string? Password { get; set; }

    public bool? Active { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
