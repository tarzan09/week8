using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBindingDemoF
{
    public partial class MainForm : Form
    {
        private ProductViewModel productVM;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            labelCount.Text = "";
            labelProductLegend.Text = string.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n",
                                        "Total Cost:",
                                        "SubTotal:",
                                        "PST:",
                                        "GST:",
                                        "Total:");
            productVM = new ProductViewModel(0.05m, 0.07m);
            setBindings();
            setupDataGridView();
        }

        private void setBindings()
        {
            textBoxSku.DataBindings.Add("Text", productVM, "DisplayProduct.Sku");

            textBoxDescription.DataBindings.Add("Text", productVM, "DisplayProduct.Description");

            textBox3.DataBindings.Add("Text", productVM, "DisplayProduct.Quantity",
                                            true, DataSourceUpdateMode.OnValidation, "0", "#,##0;(#,##0);0");

            textBoxCost.DataBindings.Add("Text", productVM, "DisplayProduct.Cost",
                                          true, DataSourceUpdateMode.OnValidation,
                                          "0.00", "#,##0.00;(#,##0.00);0.00");
            textBox1.DataBindings.Add("Text", productVM, "DisplayProduct.SellPrice",
                             true, DataSourceUpdateMode.OnValidation, "0.00", "#,##0.00;(#,##0.00);0.00");

            textBox2.DataBindings.Add("Text", productVM, "DisplayProduct.ExtensionPrice",
                                          true, DataSourceUpdateMode.OnValidation, "0.00", "#,##0.00;(#,##0.00);0.00");

            checkBoxTaxable.DataBindings.Add("Checked", productVM, "DisplayProduct.IsTaxable");

            labelCount.DataBindings.Add("Text", productVM, "Count");

            labelTotals.DataBindings.Add("Text", productVM, "Totals");

            dataGridViewProducts.AutoGenerateColumns = false;
            dataGridViewProducts.DataSource = productVM.ProductsSource;
        }

        private void setupDataGridView()
        {
            // configure for readonly 
            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProducts.MultiSelect = false;
            dataGridViewProducts.AllowUserToAddRows = false;
            dataGridViewProducts.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewProducts.AllowUserToOrderColumns = false;
            dataGridViewProducts.AllowUserToResizeColumns = false;
            dataGridViewProducts.AllowUserToResizeRows = false;
            dataGridViewProducts.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            dataGridViewProducts.RowHeadersVisible = false;

            // add columns

            DataGridViewTextBoxColumn id = new DataGridViewTextBoxColumn();
            id.Name = "id";
            id.DataPropertyName = "ProductId";
            id.HeaderText = "Id";
            id.Width = 40;
            id.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            id.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            id.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewProducts.Columns.Add(id);

            DataGridViewTextBoxColumn sku = new DataGridViewTextBoxColumn();
            sku.Name = "sku";
            sku.DataPropertyName = "Sku";
            sku.HeaderText = "Sku";
            sku.Width = 90;
            sku.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewProducts.Columns.Add(sku);

            DataGridViewTextBoxColumn desc = new DataGridViewTextBoxColumn();
            desc.Name = "desc";
            desc.DataPropertyName = "Description";
            desc.HeaderText = "Description";
            desc.Width = 220;
            desc.SortMode = DataGridViewColumnSortMode.NotSortable;
            desc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewProducts.Columns.Add(desc);

            DataGridViewTextBoxColumn qty = new DataGridViewTextBoxColumn();
            qty.Name = "qty";
            qty.DataPropertyName = "Quantity";
            qty.HeaderText = "Quantity";
            qty.Width = 60;
            qty.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            qty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            qty.DefaultCellStyle.Format = "N0";
            qty.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewProducts.Columns.Add(qty);

            DataGridViewTextBoxColumn cost = new DataGridViewTextBoxColumn();
            cost.Name = "cost";
            cost.DataPropertyName = "Cost";
            cost.HeaderText = "Cost";
            cost.Width = 80;
            cost.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            cost.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            cost.DefaultCellStyle.Format = "N2";
            cost.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewProducts.Columns.Add(cost);

            DataGridViewCheckBoxColumn tax = new DataGridViewCheckBoxColumn();
            tax.Name = "taxable";
            tax.DataPropertyName = "IsTaxable";
            tax.HeaderText = "Taxable";
            tax.Width = 60;
            tax.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewProducts.Columns.Add(tax);
        }

        private void dataGridViewProducts_SelectionChanged(object sender, EventArgs e)
        {
            int index = dataGridViewProducts.CurrentRow.Index;

            //Need to unbox product
            Product product = (Product)productVM.ProductsSource[index];
            productVM.DisplayProduct = product;

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            int index = dataGridViewProducts.CurrentRow.Index;
            Product product = productVM.DisplayProduct;

            //If there is no ProductId, then we know this is a new product
            //Otherwise, it is a an existing product we are updating
            if (product.ProductId == default)
            {
                //Mimic database assigning ID
                product.ProductId = productVM.ProductsSource.Count + 1;
                productVM.ProductsSource.Add(product);

                //Select the last row, which is where we added the item
                dataGridViewProducts.Rows[dataGridViewProducts.Rows.Count - 1].Selected = true;
            }
            else
            {
                productVM.ProductsSource[index] = product;
            }

        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            //Reset the list so that it doesn't have an incorrect selection
            dataGridViewProducts.ClearSelection();

            Product product = new Product(productVM.GstRate, productVM.PstRate);
            productVM.DisplayProduct = product;

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
