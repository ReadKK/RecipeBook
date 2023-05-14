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
using System.Collections.ObjectModel;

namespace RecipeBook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        //public string cs = @"Data Source=(LocalDB)\mssqllocaldb;AttachDbFilename=C:\Users\user\C#\RecipeBook\RecipeBook\Recipe.mdf;Integrated Security=True";
        public string cs = @"Data Source=(LocalDB)\mssqllocaldb;AttachDbFilename=|DataDirectory|\Recipe.mdf;Integrated Security=True";
        public SqlConnection myConnection = default(SqlConnection);
        public SqlCommand myCommand = default(SqlCommand);


        private void Form1_Load(object sender, EventArgs e)
        {
            panel5.Height = button1.Height;
            panel5.Top = button1.Top;
            panel1.Controls.Add(UserControl3.Instance);
            UserControl3.Instance.Dock = DockStyle.Fill;
            UserControl3.Instance.BringToFront();
        }


       

        private void button1_Click_1(object sender, EventArgs e)
        {
            panel5.Height = button1.Height;
            panel5.Top = button1.Top;

            if (!panel1.Controls.Contains(UserControl3.Instance))
            {
                panel1.Controls.Add(UserControl3.Instance);
                UserControl3.Instance.Dock = DockStyle.Fill;
                UserControl3.Instance.BringToFront();
            }
            else
                UserControl3.Instance.BringToFront();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel5.Height = button2.Height;
            panel5.Top = button2.Top;

            if (!panel1.Controls.Contains(UserControl1.Instance))
            {
                panel1.Controls.Add(UserControl1.Instance);
                UserControl1.Instance.Dock = DockStyle.Fill;
                UserControl1.Instance.BringToFront();
            }
            else
                UserControl1.Instance.BringToFront();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel5.Height = button3.Height;
            panel5.Top = button3.Top;

            if (!panel1.Controls.Contains(UserControl2.Instance))
            {
                panel1.Controls.Add(UserControl2.Instance);
                UserControl2.Instance.Dock = DockStyle.Fill;
                UserControl2.Instance.BringToFront();
            }
            else
                UserControl2.Instance.BringToFront();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

    }
    
}
