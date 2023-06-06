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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WinFormsApp5
{
    public partial class Form2 : Form
    {
        public static string prodSel;
        public static string qtySel;
        public static String productSel;
        private List<CartItem> cart = new List<CartItem>();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=HQ-W10-L111\SQLEXPRESS; Initial Catalog=AdventureWorks2022; User ID = Kamish; Password = Welcome123!; TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(ConnectionString);

            connection.Open();

            SqlCommand cmd = new SqlCommand("SELECT Top 25 Name from Production.Product WHERE ListPrice != 0", connection);

            DataTable dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.ValueMember = "Name";

            dataGridView1.DataSource = dt;

            dataGridView1.Visible = true;



            connection.Close();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            prodSel = comboBox1.SelectedValue.ToString();

            
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].Index < dataGridView1.Rows.Count)
            {

                productSel = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                textBox1.Visible = true;
            }
            else
            {
                productSel = null;
                textBox1.Visible = false;
            }


            string ConnectionString2 = @"Data Source=HQ-W10-L111\SQLEXPRESS; Initial Catalog=AdventureWorks2022; User ID = Kamish; Password = Welcome123!; TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(ConnectionString2);
            connection.Open();

            if (prodSel == null)
            {
                prodSel = "LL Road Seat Assembly";
            }


            SqlCommand command2 = new SqlCommand("SELECT Name from Production.Product WHERE Name = @Name", connection);
            command2.Parameters.AddWithValue("Name", prodSel);

            DataTable dt2 = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(command2);

            adapter.Fill(dt2);

            dataGridView1.DataSource = dt2;

            dataGridView1.Visible = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            qtySel = textBox1.Text;


            CartItem item = new CartItem(productSel, qtySel);


            cart.Add(item);


            productSel = null;
            qtySel = null;


            
            MessageBox.Show("Item added to cart!");


            Form3 cartForm = new Form3(cart);
            cartForm.ShowDialog();

        }
    }

    public class CartItem
    {
        public string Product { get; set; }
        public string Quantity { get; set; }

        public CartItem(string product, string quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }

}
