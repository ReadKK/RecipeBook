using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RecipeBook
{
    public partial class UserControl1 : UserControl
    {
        private static UserControl1 _instace;

        public static UserControl1 Instance
        {
            get
            {
                if (_instace == null)
                    _instace = new UserControl1();
                return _instace;
            }
        }
        public UserControl1()
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
            button3.Text = "Clear the list";

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
        public string cs = @"Data Source=(LocalDB)\mssqllocaldb;AttachDbFilename=|DataDirectory|\Recipe.mdf;Integrated Security=True";
        public SqlConnection myConnection = default(SqlConnection);
        public SqlCommand myCommand = default(SqlCommand);
        public SqlCommand avIng = default(SqlCommand);
        //________________________________________________________________________________________________________________
        private void button1_Click_1(object sender, EventArgs e)
        {
            string str;
            string[] str2;
            try
            {
                myConnection = new SqlConnection(cs);
                avIng = new SqlCommand("INSERT INTO available_ingredients ( avl_ingredient_quantity,recipe_id, ingredient_id) VALUES ( @b,@c, @d) ", myConnection);

                string sql = @"INSERT INTO  recipe ( recipe_name,recipe_desc,recipe_instruct, dish_id) VALUES ( @recipe_name,@recipe_desc,@recipe_instruct, @dish_id);SELECT Scope_Identity()";
                SqlCommand myCommand = new SqlCommand(sql, myConnection);



                myConnection.Open();
                myCommand.Parameters.AddWithValue("@recipe_name", textBox1.Text);
                myCommand.Parameters.AddWithValue("@recipe_desc", textBox2.Text);
                myCommand.Parameters.AddWithValue("@recipe_instruct", textBox3.Text);
                myCommand.Parameters.AddWithValue("@dish_id", comboBox1.SelectedValue);

                int newID = (int)(decimal)myCommand.ExecuteScalar();


                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    avIng.Parameters.Clear();
                    str = listBox1.Items[i].ToString();
                    str2 = str.Split(';');
                    avIng.Parameters.AddWithValue("@b", str2[2]);
                    avIng.Parameters.AddWithValue("@c", newID);
                    avIng.Parameters.AddWithValue("@d", str2[0]);
                    avIng.ExecuteNonQuery();
                }
                myConnection.Close();

                MessageBox.Show("You have successfully added a new recipe in your book.","Good job!!!");
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Dispose();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            listBox1.Items.Clear();
        }
        //________________________________________________________________________________________________________________
        private void button2_Click_1(object sender, EventArgs e)
        {
            decimal d;
            bool ch;
            string str;
            string[] str2;
            if (decimal.TryParse(textBox4.Text, out d))
            {
                if (int.Parse(textBox4.Text) > 0)
                {
                    ch = true;
                    for (int j = 0; j < listBox1.Items.Count; j++)
                    {
                        str = listBox1.Items[j].ToString();
                        str2 = str.Split(';');
                        if (comboBox2.SelectedValue.ToString() == str2[0])
                        {
                            ch = false;
                            break;
                        }

                    }
                    if (ch == true)
                    {
                        string s = comboBox2.SelectedValue + ";" + comboBox2.Text + ";" + textBox4.Text;
                        listBox1.Items.Add(s);
                    }
                    else
                    {
                        MessageBox.Show("Can't insert the same ingredient");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Value cannot be zero or below!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number");
                return;
            }
        }
        //________________________________________________________________________________________________________________
        private void button3_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
