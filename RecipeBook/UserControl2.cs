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
    public partial class UserControl2 : UserControl
    {
        private static UserControl2 _instace;

        public static UserControl2 Instance
        {
            get
            {
                if (_instace == null)
                    _instace = new UserControl2();
                return _instace;
            }
        }
        public UserControl2()
        {
            InitializeComponent();
            label1.Text = "Ingredients";
            label2.Text = "grams/milileters";
            label3.Text = "Ing.id/Name/quantity";
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(186, 85, 54);
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(104, 130, 158);
            dataGridView1.RowsDefaultCellStyle.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(255, 255, 255);

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11.0f, FontStyle.Bold);
            //dataGridView1.ColumnHeadersDefaultCellStyle;
            dataGridView1.RowsDefaultCellStyle.Font = new Font("Times New Roman", 9.0f);
            //dataGridView1.RowsDefaultCellStyle.

            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.RowTemplate.Height = 90;
            dataGridView1.EnableHeadersVisualStyles = true;


            myConnection = new SqlConnection(cs);

            myConnection.Open();
            myCommand = new SqlCommand("Select * from ingredients", myConnection);
            SqlDataAdapter dataAdapter2 = new SqlDataAdapter(myCommand);
            DataSet dataSet2 = new DataSet();
            dataAdapter2.Fill(dataSet2);
            myCommand.ExecuteNonQuery();

            myCommand = new SqlCommand("update ingredients set ingredient_at_hand=0; ", myConnection);
            myCommand.ExecuteNonQuery();
            myCommand = new SqlCommand("Delete from search ", myConnection);
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            comboBox1.DisplayMember = "ingredient_name";
            comboBox1.ValueMember = "ingredient_id";
            comboBox1.DataSource = dataSet2.Tables[0];
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        public string cs = @"Data Source=(LocalDB)\mssqllocaldb;AttachDbFilename=|DataDirectory|\Recipe.mdf;Integrated Security=True";
        public SqlConnection myConnection = default(SqlConnection);
        public SqlCommand myCommand = default(SqlCommand);
        SqlDataAdapter adapt;

        private void DisplayData()
        {
            myConnection.Open();

            myCommand = new SqlCommand("Select count(ingredient_at_hand) from ingredients where ingredient_at_hand>0", myConnection);
            int a = (Int32)myCommand.ExecuteScalar();
            myCommand = new SqlCommand("TRUNCATE TABLE search ", myConnection);
            myCommand.ExecuteNonQuery();
            myCommand = new SqlCommand("insert into search(search_name, search_count) SELECT recipe_name, count(recipe_name) as countable from recipe" +
                " JOIN available_ingredients ON recipe.recipe_id=available_ingredients.recipe_id" +
                " JOIN ingredients ON ingredients.ingredient_id=available_ingredients.ingredient_id where ingredients.ingredient_at_hand>0 and available_ingredients.avl_ingredient_quantity<=ingredients.ingredient_at_hand" +
                " group by(recipe.recipe_name)", myConnection);
            myCommand.ExecuteNonQuery();
            adapt = new SqlDataAdapter("SELECT recipe_id as ID, recipe_name as Recipe, recipe_desc as Description, dish_type.dish_name as Type from recipe" +
                " join dish_type on dish_type.dish_id=recipe.dish_id where recipe_name in(select search_name from search where search_count=@k)", myConnection);
            adapt.SelectCommand.Parameters.AddWithValue("@k", a);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            myConnection.Close();
            dataGridView1.ClearSelection();
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (int i = 0; i <= dataGridView1.Columns.Count - 1; i++)
            {
                // Store Auto Sized Widths:
                int colw = dataGridView1.Columns[i].Width;

                // Remove AutoSizing:
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                // Set Width to calculated AutoSize value:
                dataGridView1.Columns[i].Width = colw;
            }
        }
        //________________________________________________________________________________________________________________
        private void button2_Click(object sender, EventArgs e)
        {
            decimal d;
            bool ch;
            string str;
            string[] str2;
            if (decimal.TryParse(textBox1.Text, out d))
            {
                if (int.Parse(textBox1.Text) > 0)
                {
                    ch = true;
                    for (int j = 0; j < listBox1.Items.Count; j++)
                    {
                        str = listBox1.Items[j].ToString();
                        str2 = str.Split(';');
                        if (comboBox1.SelectedValue.ToString() == str2[0])
                        {
                            ch = false;
                            break;
                        }

                    }
                    if (ch == true)
                    {
                        string s = comboBox1.SelectedValue + ";" + comboBox1.Text + ";" + textBox1.Text;
                        listBox1.Items.Add(s);
                        myConnection.Open();
                        myCommand = new SqlCommand("update ingredients set ingredient_at_hand=@b where ingredient_id=@a ", myConnection);
                        myCommand.Parameters.AddWithValue("@a", comboBox1.SelectedValue);
                        myCommand.Parameters.AddWithValue("@b", textBox1.Text);
                        myCommand.ExecuteNonQuery();
                        myConnection.Close();


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
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                myConnection = new SqlConnection(cs);
                DisplayData();
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
        //________________________________________________________________________________________________________________
        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            myConnection.Open();
            myCommand = new SqlCommand("update ingredients set ingredient_at_hand=0; ", myConnection);
            myCommand.ExecuteNonQuery();
            myCommand = new SqlCommand("TRUNCATE TABLE search ", myConnection);
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }

    } 
}
