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
        public static int qtySel;
        public static string productSel;
      
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

            try
            {
                dataGridView1.DataSource = dt2;
                dataGridView2.Visible = false;

                dataGridView1.Visible = true;
            }

            catch
            {
                dataGridView1.Visible = false;

                dataGridView2.DataSource = dt2;

                dataGridView2.Visible = true;
            }


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




        }

        private void button1_Click(object sender, EventArgs e)
        {

            qtySel = Convert.ToInt32(textBox1.Text);

            string selProduct = comboBox1.SelectedValue.ToString();
            int selQty = int.Parse(textBox1.Text);

            object[] row = { selProduct, selQty };


            Form3 form3 = Application.OpenForms.OfType<Form3>().FirstOrDefault();

            if (form3 != null)
            {
                form3.AddRowToCart(row);
            }

            else
            {
                MessageBox.Show("Item Added to Cart");
                form3 = new Form3(row);
                form3.Show();
            }

            
            


        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0 && dataGridView2.SelectedRows[0].Index < dataGridView2.Rows.Count)
            {

                productSel = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();

                textBox1.Visible = true;



            }
            else
            {
                productSel = null;
                textBox1.Visible = false;
            }

            
        }
    }

   

}
