using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace RecipeBook
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            label1.Text = "Name";
            label2.Text = "Description";
            label3.Text = "Instructions";
            label4.Text = "Dish type";
            button1.Text = "Insert";
        }
        public string cs = @"Data Source=(LocalDB)\mssqllocaldb;AttachDbFilename=C:\Users\user\C#\RecipeBook\RecipeBook\Recipe.mdf;Integrated Security=True";
        public SqlConnection myConnection = default(SqlConnection);
        public SqlCommand myCommand = default(SqlCommand);

        private void Form2_Load(object sender, EventArgs e)
        {
            myConnection = new SqlConnection(cs);

            myConnection.Open();
            myCommand = new SqlCommand("Select * from dish_type", myConnection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(myCommand);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            comboBox1.DataSource = dataSet.Tables[0];
            comboBox1.DisplayMember = "dish_name";
            comboBox1.ValueMember = "dish_id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                myConnection = new SqlConnection(cs);
                myCommand = new SqlCommand("INSERT INTO recipe ( recipe_name,recipe_desc,recipe_instruct, dish_id) VALUES ( @recipe_name,@recipe_desc,@recipe_instruct, @dish_id)", myConnection);
                myConnection.Open();
                myCommand.Parameters.AddWithValue("@recipe_name", textBox1.Text);
                myCommand.Parameters.AddWithValue("@recipe_desc", textBox2.Text);
                myCommand.Parameters.AddWithValue("@recipe_instruct", textBox3.Text);
                if (comboBox1.SelectedIndex == 0)
                {
                    myCommand.Parameters.AddWithValue("@dish_id", 1);
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    myCommand.Parameters.AddWithValue("@dish_id", 2);
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    myCommand.Parameters.AddWithValue("@dish_id", 3);
                }
                else { myCommand.Parameters.AddWithValue("@dish_id", 4); }
                myCommand.ExecuteNonQuery();
                myConnection.Close();

                MessageBox.Show("Insert successful!");
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
