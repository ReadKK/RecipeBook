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
            label5.Text = "Ingredients";
            label6.Text = "Ing.id/Name/quantity";
            label7.Text = "grams/milliliters";
            button1.Text = "Add recipe";
            button2.Text = "Add to the list";
        }
        public string cs = @"Data Source=(LocalDB)\mssqllocaldb;AttachDbFilename=C:\Users\user\C#\RecipeBook\RecipeBook\Recipe.mdf;Integrated Security=True";
        public SqlConnection myConnection = default(SqlConnection);
        public SqlCommand myCommand = default(SqlCommand);
        public SqlCommand avIng = default(SqlCommand);
        

        private void Form2_Load(object sender, EventArgs e)
        {
            myConnection = new SqlConnection(cs);

            myConnection.Open();
            myCommand = new SqlCommand("Select * from dish_type", myConnection);
            SqlDataAdapter dataAdapter1 = new SqlDataAdapter(myCommand);
            DataSet dataSet1 = new DataSet();
            dataAdapter1.Fill(dataSet1);
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            comboBox1.DisplayMember = "dish_name";
            comboBox1.ValueMember = "dish_id";
            comboBox1.DataSource = dataSet1.Tables[0];
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            

            myConnection.Open();
            myCommand = new SqlCommand("Select * from ingredients", myConnection);
            SqlDataAdapter dataAdapter2 = new SqlDataAdapter(myCommand);
            DataSet dataSet2 = new DataSet();
            dataAdapter2.Fill(dataSet2);
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            comboBox2.DisplayMember = "ingredient_name";
            comboBox2.ValueMember = "ingredient_id";
            comboBox2.DataSource = dataSet2.Tables[0];
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                myConnection = new SqlConnection(cs);
                //myCommand = new SqlCommand("INSERT INTO recipe ( recipe_name,recipe_desc,recipe_instruct, dish_id) VALUES ( @recipe_name,@recipe_desc,@recipe_instruct, @dish_id)", myConnection);
                avIng = new SqlCommand("INSERT INTO available_ingredients ( avl_ingredient_quantity,recipe_id, ingredient_id) VALUES ( @b,@c, @d) ", myConnection);
                
                string sql = @"INSERT INTO  recipe ( recipe_name,recipe_desc,recipe_instruct, dish_id) VALUES ( @recipe_name,@recipe_desc,@recipe_instruct, @dish_id);SELECT Scope_Identity()";
                SqlCommand myCommand= new SqlCommand(sql,myConnection);
                
                

                myConnection.Open();
                myCommand.Parameters.AddWithValue("@recipe_name", textBox1.Text);
                myCommand.Parameters.AddWithValue("@recipe_desc", textBox2.Text);
                myCommand.Parameters.AddWithValue("@recipe_instruct", textBox3.Text);
                myCommand.Parameters.AddWithValue("@dish_id", comboBox1.SelectedValue);

                int newID = (int)(decimal)myCommand.ExecuteScalar();

                avIng.Parameters.AddWithValue("@b", textBox4.Text);
                avIng.Parameters.AddWithValue("@c", newID);
                avIng.Parameters.AddWithValue("@d", comboBox2.SelectedValue);
                avIng.ExecuteNonQuery();
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

        private void button2_Click(object sender, EventArgs e)
        {
            decimal d;
            if (decimal.TryParse(textBox4.Text, out d))
            {
                string s =comboBox2.SelectedValue+"; "+ comboBox2.Text + "; " + textBox4.Text;
                listBox1.Items.Add(s);
            }
            else {
                MessageBox.Show("Please enter a valid number");
                return;
            }
        }
    }
}
