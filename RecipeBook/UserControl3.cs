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
    public partial class UserControl3 : UserControl
    {
        private static UserControl3 _instace;

        public static UserControl3 Instance
        {
            get
            {
                if (_instace == null)
                    _instace = new UserControl3();
                return _instace;
            }
        }
        public UserControl3()
        {
            InitializeComponent();
            label5.Text = "If there is a Recipe you like add its ID under!!!";
            //label1.Text = "Required ingredients for the recipe and the required amount to realise it(grams/mililiters)";

            myConnection = new SqlConnection(cs);
        }

        public string cs = @"Data Source=(LocalDB)\mssqllocaldb;AttachDbFilename=|DataDirectory|\Recipe.mdf;Integrated Security=True";
        public SqlConnection myConnection = default(SqlConnection);
        public SqlCommand myCommand = default(SqlCommand);


        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            decimal d;
            

                if (textBox3.Text != "")
                {

                    if (decimal.TryParse(textBox3.Text, out d))
                    {
                        myConnection.Open();
                        myCommand = new SqlCommand("Select recipe_instruct from recipe where recipe_id=@showinst", myConnection);
                        myCommand.Parameters.AddWithValue("@showinst", textBox3.Text);
                        SqlDataReader re = myCommand.ExecuteReader();
                        //myCommand.ExecuteNonQuery();                 
                        if (re.Read())
                        {
                            textBox2.Text = re["recipe_instruct"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid ID", "Stop!");
                        }
                        myConnection.Close();
                        myConnection.Open();
                        myCommand = new SqlCommand("SELECT ingredients.ingredient_name, available_ingredients.avl_ingredient_quantity from recipe" +
                    " JOIN available_ingredients ON recipe.recipe_id=available_ingredients.recipe_id" +
                    " JOIN ingredients ON ingredients.ingredient_id=available_ingredients.ingredient_id where recipe.recipe_id=@showingr", myConnection);
                        myCommand.Parameters.AddWithValue("@showingr", textBox3.Text);
                        SqlDataReader re2 = myCommand.ExecuteReader();

                        while (re2.Read())
                        {

                            listBox1.Items.Add(re2.GetString(0) + " " + re2.GetValue(1));

                        }
                        myConnection.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a number", "Stop!");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You need to enter a recipe ID first.", "Wait!");
                }
            
            
        }
    }
}
