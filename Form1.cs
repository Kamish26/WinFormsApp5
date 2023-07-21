using Microsoft.SqlServer.Server;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using System;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.DataFormats;

namespace WinFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text;
            string pswd = textBox2.Text;

            string ConnectionString = @"Data Source=HQ-W10-L111\SQLEXPRESS; Initial Catalog=AdventureWorks2022; User ID = Kamish2; Password = Welcome123!; TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(ConnectionString);

            connection.Open();

            SqlCommand command = new SqlCommand("EXEC GetUserCred @user, @pswd ", connection);
            command.Parameters.AddWithValue("@user", user);
            command.Parameters.AddWithValue("@pswd", pswd);



            SqlDataReader reader = command.ExecuteReader();


            if (reader.HasRows)
            {
                Form2 secondForm = new Form2();
                secondForm.Show();
            }

            else
            {
                MessageBox.Show("Username or password invalid");
            }

            reader.Close();

            connection.Close();


        }
    }
}