using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ChinookDatabase.DataModel
{
    [DebuggerDisplay("InvoiceLineId = {InvoiceLineId}")]
    public class InvoiceLine
    {
        [Key]
        public int InvoiceLineId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int TrackId { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [RelatedTo(ForeignKey = "CustomerId")]
        public Customer Customer { get; set; }

        [RelatedTo(ForeignKey = "TrackId")]
        public Track Track { get; set; }
    }
}
