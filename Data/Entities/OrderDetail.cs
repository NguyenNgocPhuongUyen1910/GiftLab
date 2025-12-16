using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GiftLab.Data.Entities;

public partial class OrderDetail
{
    [Key]
    public int OrderDetailID { get; set; }

    public int? OrderID { get; set; }

    public int? ProductID { get; set; }

    public int? OrderNumber { get; set; }

    public int? Quantity { get; set; }

    public int? Discount { get; set; }

    public int? Total { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ShipDate { get; set; }

    [ForeignKey("OrderID")]
    [InverseProperty("OrderDetails")]
    public virtual Order? Order { get; set; }
}
