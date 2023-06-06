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


        public Form3(object[] row) : this()
        {

            dataGridView1.Rows.Add(row);

            TotalCostTextBox();
        }

        public void AddRowToCart(object[] row)
        {

            dataGridView1.Rows.Add(row);
        }


        private void Form3_Load(object sender, EventArgs e)
        {

        }

        public void TotalCostTextBox()
        {
            string ConnectionString2 = @"Data Source=HQ-W10-L111\SQLEXPRESS; Initial Catalog=AdventureWorks2022; User ID = Kamish; Password = Welcome123!; TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(ConnectionString2);
            connection.Open();

            SqlCommand command1 = new SqlCommand("SELECT ListPrice * @Quantity FROM Production.Product WHERE Name = @Name", connection);
            command1.Parameters.AddWithValue("@Name", Form2.productSel);
            command1.Parameters.AddWithValue("@Quantity", Form2.qtySel);

            DataTable dt3 = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(command1);

            adapter.Fill(dt3);

            if (dt3.Rows.Count > 0)
            {
                decimal costTotal = Convert.ToDecimal(dt3.Rows[0].ItemArray[0]);

                textBox1.Text = costTotal.ToString();


            }

            connection.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ConnectionString3 = @"Data Source=HQ-W10-L111\SQLEXPRESS; Initial Catalog=AdventureWorks2022; User ID = Kamish; Password = Welcome123!; TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(ConnectionString3);
            connection.Open();

            SqlCommand command3 = new SqlCommand("SELECT TOP 1 SalesOrderID FROM Sales.SalesOrderDetail ORDER BY SalesOrderID DESC", connection);
            DataTable dt4 = new DataTable(); 

            SqlDataAdapter adapter = new SqlDataAdapter(command3);

            adapter.Fill(dt4);

            int SalesID =  Convert.ToInt32(dt4.Rows[0].ItemArray[0]);


            SqlCommand command2 = new SqlCommand("INSERT INTO Sales.SalesOrderDetail(SalesOrderID, OrderQty, ProductID, SpecialOfferID, " +
                "UnitPrice, UnitPriceDiscount) VALUES(@SalesID, @Quantity, (SELECT ProductID FROM Production.Product WHERE Name = @Name), 1, (SELECT ListPrice FROM Production.Product WHERE Name = @Name), 0.00) ", connection);
            command2.Parameters.AddWithValue("@SalesID", SalesID + 1);
            command2.Parameters.AddWithValue("@Name", Form2.productSel);
            command2.Parameters.AddWithValue("@Quantity", Form2.qtySel);
            command2.CommandTimeout = 400;
           

            command2.ExecuteNonQuery();

            MessageBox.Show("Orders table updated");

        }
    }
}
