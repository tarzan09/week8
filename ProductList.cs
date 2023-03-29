using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBindingDemoF
{
    class ProductList : List<Product>
    {
        public decimal TotalCost
        {
            get
            {
                decimal totalCost = 0.0m;
                foreach (Product product in this)
                {
                    totalCost += product.ExtensionCost;
                }
                return totalCost;
            }
        }

        public decimal SubTotal
        {
            get
            {
                decimal subTotal = 0.0m;
                foreach (Product product in this)
                {
                    subTotal += product.ExtensionPrice;
                }
                return subTotal;
            }
        }

        public decimal TotalPst
        {
            get
            {
                decimal totalPst = 0.0m;
                foreach (Product product in this)
                {
                    totalPst += product.IsTaxable ? product.SellPrice * product.PstRate : 0.0m;
                }
                return totalPst;
            }
        }

        public decimal TotalGst
        {
            get
            {
                decimal totalGst = 0.0m;
                foreach (Product product in this)
                {
                    totalGst += product.SellPrice * product.GstRate;
                }
                return totalGst;
            }
        }
        public decimal Total
        {
            get
            {
                return SubTotal + TotalGst + TotalPst;
            }
        }
    }
}
