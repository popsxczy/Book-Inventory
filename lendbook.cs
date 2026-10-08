using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp2
{
    public partial class lendbook : Form
    {
        public lendbook()
        {
            InitializeComponent();
            books();
            load();
        }

        SqlConnection con = new SqlConnection("Data Source = POPSXCZY\\SQLEXPRESS ;Initial Catalog = slibrary;Integrated Security = True");
        SqlCommand cmd;
        SqlDataAdapter da;
        SqlDataReader dr;
        string sql;
        bool Mode = true;
        string id;

        public void load()
        {
            sql = "SELECT * FROM issuebook1";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            dataGridView2.Rows.Clear();

            while (dr.Read())
            {
                dataGridView2.Rows.Add(dr[0], dr[1], dr[2], dr[3], dr[4], dr[5]);
            }

            con.Close();
        }






        public void books()
        {
            string query = "SELECT * FROM books";
            cmd = new SqlCommand(query, con);
            con.Open();
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            txtbook.DataSource = ds.Tables[0];
            txtbook.DisplayMember = "bname";
            txtbook.ValueMember = "id";
            con.Close();


        }







        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if(e.KeyChar == 13)
            {
                cmd = new SqlCommand("SELECT * FROM member WHERE id = '" + txtmid.Text   +"' ", con);
                con.Open();
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtmname.Text = dr["name"].ToString();                   
                }
                else
                {                  
                    MessageBox.Show("Member not found");
                }
                con.Close();
            }



        }


        public void getid(string id)
        {
            sql = "SELECT * FROM issuebook1 WHERE id = '" + id + "'";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                txtmid.Text = dr[1].ToString();
                txtmname.Text = dr[2].ToString();
                txtbook.Text = dr[3].ToString();
                
            }

            con.Close();
        }





        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                getid(id);  
            }
            else if (e.ColumnIndex == dataGridView2.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                id = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                sql = "DELETE FROM issuebook1 WHERE id = @id";

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

                MessageBox.Show("Successfully deleted");
                txtmname.Clear();
                txtmid.Clear();
               
                load(); // refresh grid


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mid = txtmid.Text;
            string mname = txtmname.Text;
            string book = txtbook.SelectedValue.ToString();
            string issuedate = txtidate.Value.ToString("yyyy-MM-dd");
            string returndate = txtrdate.Value.ToString("yyyy-MM-dd");


            if (Mode == true)
            {


                sql = "INSERT INTO issuebook1 (memberid,membername, book,issuedate,returndate) VALUES (@memberid,@membername, @book,@issuedate,@returndate)";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@memberid", mid);
                cmd.Parameters.AddWithValue("@membername", mname);
                cmd.Parameters.AddWithValue("@book", book);
                cmd.Parameters.AddWithValue("@issuedate", issuedate);
                cmd.Parameters.AddWithValue("@returndate", returndate);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Added");
                con.Close();
                load();
                txtmid.Clear();
                txtmname.Clear();
            }

            else
            {
                sql = "UPDATE issuebook1 SET memberid = @memberid, membername = @membername, book = @book, issuedate = @issuedate, returndate = @returndate WHERE id = @id";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@memberid", mid);
                    cmd.Parameters.AddWithValue("@membername", mname);
                    cmd.Parameters.AddWithValue("@book", book);
                    cmd.Parameters.AddWithValue("@issuedate", issuedate);
                    cmd.Parameters.AddWithValue("@returndate", returndate);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }


                MessageBox.Show("Successfully Updated");
                txtmid.Clear();

                con.Close();
                load();

                txtmid.Focus();


            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtmid.Clear();
           txtmname.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lendbook_Load(object sender, EventArgs e)
        {

        }
    }
}
