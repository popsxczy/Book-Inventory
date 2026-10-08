using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("Data Source = POPSXCZY\\SQLEXPRESS ;Initial Catalog = slibrary;Integrated Security = True");
        SqlCommand cmd;



        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username, userpassword;

           username = txtuser.Text;
            userpassword = txtpass.Text;

            try
            {
                String querry = "SELECT * FROM logindb WHERE username = '" + username + "' AND password = '" + userpassword + "' ";
                SqlDataAdapter sda = new SqlDataAdapter(querry, con);

                DataTable dtbl = new DataTable();
                sda.Fill(dtbl);

                if (dtbl.Rows.Count > 0)
                {
                    username = txtuser.Text;
                    userpassword = txtpass.Text;

                   Main cat = new Main();
                    cat.Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Invalid Username or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtpass.Clear();
                    txtuser.Clear();
                    txtuser.Focus();
                }
            }         
            catch
            {
                MessageBox.Show("Error");
            }
            finally
            {
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtpass.Clear();
            txtuser.Clear();
            txtuser.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           Application.Exit();
        }
    }
}
