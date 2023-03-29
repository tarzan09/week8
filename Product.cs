using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBindingDemoF
{
    class Product
    {
        public decimal PstRate { get; }
        public decimal GstRate { get; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public decimal SellPrice { get; set; }
        public decimal Cost { get; set; }
        public decimal ExtensionCost { get { return Quantity * Cost; } }
        public decimal ExtensionPrice { get { return Quantity * SellPrice; } }
        public bool IsTaxable { get; set; }
        public Product(decimal pstRate, decimal gstRate)
        {
            PstRate = pstRate;
            GstRate = gstRate;
        }
    }
}