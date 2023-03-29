using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBindingDemoF
{
    class ProductViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public readonly decimal GstRate;
        public readonly decimal PstRate;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ProductViewModel(decimal gstRate, decimal pstRate)
        {
            GstRate = gstRate;
            PstRate = pstRate;
            products = DataGenerator.CreateProducts(GstRate, PstRate);
            ProductsSource = new BindingSource();
            ProductsSource.DataSource = products;
            ProductsSource.ListChanged += ProductSource_ListChanged;
            DisplayProduct = new Product(GstRate, PstRate);
        }

        private Product displayProduct;
        public Product DisplayProduct
        {
            get { return displayProduct; }
            set
            {
                //Create a copy of the product as a temporary placeholder
                //so that the original product doesn't get updated until we hit save
                displayProduct = new Product(GstRate, PstRate)
                {
                    ProductId = value.ProductId,
                    Sku = value.Sku,
                    Description = value.Description,
                    Quantity = value.Quantity,
                    SellPrice = value.SellPrice,
                    Cost = value.Cost,
                    IsTaxable = value.IsTaxable
                };
                OnPropertyChanged();
            }
        }
        private readonly ProductList products;
        public BindingSource ProductsSource { get; }

        //Can't access count directly when databinding due to a limitation in the framework.
        //So, we'll use this property to pass it through.
        public int Count
        {
            get
            {
                return products.Count;
            }
        }

        public string Totals
        {
            get
            {
                return string.Format("{0,15:N2}\r\n{1,15:N2}\r\n{2,15:N2}\r\n{3,15:N2}\r\n{4,15:N2}\r\n",
                                                   products.TotalCost,
                                                   products.SubTotal,
                                                   products.TotalPst,
                                                   products.TotalGst,
                                                   products.Total);
            }
        }

        //If the list has changed, that means something was either added, updated, or deleted.
        //When this happens, we need to raise events to tell the controls that there has been an
        //update so that they know to refresh their values.
        private void ProductSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            OnPropertyChanged("Count");
            OnPropertyChanged("Totals");
        }

    }
}
