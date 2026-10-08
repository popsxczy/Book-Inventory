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

namespace WindowsFormsApp2
{
    public partial class member : Form
    {
        public member()
        {
            InitializeComponent();
            load();
        }

        SqlConnection con = new SqlConnection("Data Source = POPSXCZY\\SQLEXPRESS ;Initial Catalog = slibrary;Integrated Security = True");
        SqlCommand cmd;
        SqlDataReader dr;
        string sql;
        bool Mode = true;
        string id;


        public void load()
        {
            sql = "SELECT * FROM member";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();

            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr[0], dr[1], dr[2], dr[3]);
            }

            con.Close();
        }





        private void member_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = txtname.Text;
            string address = txtaddress.Text;
            string phone = txtphone.Text;



            if (Mode == true)
            {


                using (var cmd = new SqlCommand("INSERT INTO member (name,address,phone) VALUES (@name,@address,@phone)", con))
                {
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar, 200).Value = name;
                    cmd.Parameters.Add("@address", SqlDbType.NVarChar, 500).Value = address;

                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar, 50).Value = phone;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Member Created");

                txtname.Clear();
                txtaddress.Clear();
                txtphone.Clear();

                con.Close();
                load();
                txtname.Focus();

            }

            else
            {
                sql = "Update member set name= @name, address = @address, phone = @phone  where id = @id";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Member Updated");
                txtname.Clear();

                con.Close();
                load();

                txtname.Focus();


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           txtaddress.Clear();
            txtname.Clear();
            txtphone.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
