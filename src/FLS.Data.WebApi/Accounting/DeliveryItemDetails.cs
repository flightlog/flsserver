using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Accounting
{
    public class DeliveryItemDetails : FLSBaseData
    {
        public Guid DeliveryItemId { get; set; }

        [Required]
        public int Position { get; set; }

        [Required]
        [StringLength(50)]
        public string ArticleNumber { get; set; }

        [StringLength(250)]
        public string ItemText { get; set; }

        [StringLength(250)]
        public string AdditionalInformation { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public int DiscountInPercent { get; set; }

        [Required]
        public string UnitType { get; set; }

        public override Guid Id
        {
            get { return DeliveryItemId; }
            set { DeliveryItemId = value; }
        }

        public override string ToString()
        {
            return $"Pos: {Position} Product: {ArticleNumber} Text: {ItemText} Qty: {Quantity} of type: {UnitType} Discount: {DiscountInPercent}% Add. Info: {AdditionalInformation}";
        }
    }
}
