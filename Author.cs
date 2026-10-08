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
using System.Xml.Linq;

namespace WindowsFormsApp2
{
    public partial class Author : Form
    {
        public Author()
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
            sql = "SELECT * FROM author";
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



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = txtname.Text;
            string address = txtaddress.Text;
            string phone = txtphone.Text;

    

            if (Mode == true)
            {
                
              
                using (var cmd = new SqlCommand(
                    "INSERT INTO author (name,address,phone) VALUES (@name,@address,@phone)", con))
                {
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar, 200).Value = name;
                    cmd.Parameters.Add("@address", SqlDbType.NVarChar, 500).Value = address;
                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar, 20).Value = phone;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                MessageBox.Show("Author Created");

                txtname.Clear();
                txtaddress.Clear();
                txtphone.Clear();

                con.Close();  
                load();
                txtname.Focus();

            }

            else
            {
                sql = "Update author set name= @name, address = @address, phone = @phone  where id = @id";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 200).Value = name;
                cmd.Parameters.Add("@address", SqlDbType.NVarChar, 500).Value = address;
                cmd.Parameters.Add("@phone", SqlDbType.NVarChar, 20).Value = phone;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = int.Parse(id); 
                cmd.ExecuteNonQuery();
                MessageBox.Show("Author Updated");
                txtname.Clear();
           
                con.Close();
                load();
               
                txtname.Focus();


            }



        }

        private void Author_Load(object sender, EventArgs e)
        {

        }

        public void getid(string id)
        {
            sql = "SELECT * FROM author WHERE id = '" + id + "'";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                txtname.Text = dr[1].ToString();
                txtaddress.Text = dr[2].ToString();
                txtphone.Text = dr[3].ToString();
            }

            con.Close();
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                getid(id);
            }
            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                sql = "DELETE FROM author WHERE id = @id";

                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        con.Close();
                    }
                }

                MessageBox.Show("Author deleted");
                txtname.Clear();
               
                txtname.Focus();
                load(); 
                txtname.Focus();


            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtname.Clear();
            txtaddress.Clear();
            txtphone.Clear();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtphone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtaddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtname_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
