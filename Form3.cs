using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp5
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            dataGridView1.Columns.Add("Product", "Product");
            dataGridView1.Columns.Add("Quantity", "Quantity");
        }


        public Form3(List<string> row) : this()
        {

            AddRowToCart(row);

            
        }

        public void AddRowToCart(List<string> row)
        {

            foreach (var item in row) 
            {
                string name = item.Split(':').First().ToString();
                string quantity = item.Split(':').Last().ToString();

                object[] realrow = new object[2];
                realrow[0] = name;
                realrow[1] = quantity;

                dataGridView1.Rows.Add(realrow);
                TotalCostTextBox();
            }

            
        }


        private void Form3_Load(object sender, EventArgs e)
        {
            //foreach(var item in Form2.itemsCart)
            //{
                
            //}
        }

        public void TotalCostTextBox()
        {
            string ConnectionString2 = @"Data Source=HQ-W10-L111\SQLEXPRESS; Initial Catalog=AdventureWorks2022; User ID = Kamish; Password = Welcome123!; TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(ConnectionString2);
            connection.Open();

            decimal totcost = 0;

           foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                if (row.Cells[0].Value != null & row.Cells[1].Value != null)
                {
                    Form2.productSel = row.Cells["Product"].Value.ToString();
                    Form2.qtySel = Convert.ToInt32(row.Cells["Quantity"].Value);

                    SqlCommand command1 = new SqlCommand("SELECT ListPrice * @Quantity FROM Production.Product WHERE Name = @Name", connection);
                    command1.Parameters.AddWithValue("@Name", Form2.productSel);
                    command1.Parameters.AddWithValue("@Quantity", Form2.qtySel);

                    DataTable dt3 = new DataTable();

                    SqlDataAdapter adapter = new SqlDataAdapter(command1);

                    adapter.Fill(dt3);

                    if (dt3.Rows.Count > 0)
                    {
                        decimal costTotal = Convert.ToDecimal(dt3.Rows[0].ItemArray[0]);

                        totcost += costTotal;





                    }

                    textBox1.Text = totcost.ToString();
                }

                



            }

            connection.Close();




        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ConnectionString3 = @"Data Source=HQ-W10-L111\SQLEXPRESS; Initial Catalog=AdventureWorks2022; User ID = Kamish; Password = Welcome123!; TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(ConnectionString3);
            connection.Open();

            foreach(DataGridViewRow row1 in dataGridView1.Rows)
            {
                if (row1.Cells[0].Value != null & row1.Cells[1].Value != null)
                {
                    
                    Form2.productSel = row1.Cells["Product"].Value.ToString();
                    Form2.qtySel = Convert.ToInt32(row1.Cells["Quantity"].Value);

                    SqlCommand command3 = new SqlCommand("SELECT TOP 1 SalesOrderID FROM Sales.SalesOrderDetail ORDER BY SalesOrderID DESC", connection);
                    DataTable dt4 = new DataTable();

                    SqlDataAdapter adapter = new SqlDataAdapter(command3);

                    adapter.Fill(dt4);

                    int SalesID = Convert.ToInt32(dt4.Rows[0].ItemArray[0]);


                    SqlCommand command2 = new SqlCommand("INSERT INTO Sales.SalesOrderDetail(SalesOrderID, OrderQty, ProductID, SpecialOfferID, " +
                        "UnitPrice, UnitPriceDiscount) VALUES(@SalesID, @Quantity, (SELECT ProductID FROM Production.Product WHERE Name = @Name), 1, (SELECT ListPrice FROM Production.Product WHERE Name = @Name), 0.00) ", connection);
                    command2.Parameters.AddWithValue("@SalesID", SalesID + 1);
                    command2.Parameters.AddWithValue("@Name", Form2.productSel);
                    command2.Parameters.AddWithValue("@Quantity", Form2.qtySel);
                    command2.CommandTimeout = 400;


                    command2.ExecuteNonQuery();

                    

                }

                    

            }

            MessageBox.Show("Orders table updated");


            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
