using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GiftLab.Data.Entities;

public partial class Order
{
    [Key]
    public int OrderID { get; set; }

    public int? CustomerID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ShipDate { get; set; }

    public int? TransactionStatusID { get; set; }

    public bool? Deleted { get; set; }

    public bool? Paid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PaymentData { get; set; }

    public int? PaymentID { get; set; }

    public string? Note { get; set; }

    [ForeignKey("CustomerID")]
    [InverseProperty("Orders")]
    public virtual Customer? Customer { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("TransactionStatusID")]
    [InverseProperty("Orders")]
    public virtual TransacStatus? TransactionStatus { get; set; }
}
