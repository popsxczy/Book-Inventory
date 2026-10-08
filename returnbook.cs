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
    public partial class returnbook : Form
    {
        public returnbook()
        {
            InitializeComponent();
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
            const string sql = "SELECT id, memberid, book, returndate, elap, fine FROM returnbook";
            using (var conn = new SqlConnection("Data Source=POPSXCZY\\SQLEXPRESS;Initial Catalog=slibrary;Integrated Security=True"))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    dataGridView2.Rows.Clear();
                    while (dr.Read())
                    {
                        dataGridView2.Rows.Add(
                            dr["id"],
                            dr["memberid"],
                            dr["book"],
                            dr["returndate"],
                            dr["elap"],
                            dr["fine"]
                        );
                    }
                }
            }
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)13) return;

            const string sql = @"
      SELECT book, memberid, returndate,
             DATEDIFF(dd, returndate, GETDATE()) AS elap
      FROM issuebook1
      WHERE memberid = @mid";
            try
            {
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@mid", txtmid.Text.Trim());
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtbname.Text = dr["book"].ToString();
                            txtrdate.Text = dr["returndate"].ToString();
                            int elapp = dr["elap"] != DBNull.Value ? Convert.ToInt32(dr["elap"]) : 0;
                            txtelap.Text = elapp > 0 ? elapp.ToString() : "0";
                            txtfine.Text = (elapp > 0 ? elapp * 5 : 0).ToString();
                        }
                    }
                }
            }
            finally { if (con.State == System.Data.ConnectionState.Open) con.Close(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mid = txtmid.Text;
            string book = txtbname.Text;
            string rdate = txtrdate.Text;
            string elp = txtelap.Text;
            string fine = txtfine.Text;


         
                sql = "INSERT INTO returnbook (memberid,book,returndate,elap,fine) VALUES (@memberid,@book,@returndate,@elap, @fine)";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@memberid", mid);
                cmd.Parameters.AddWithValue("@book", book);
                cmd.Parameters.AddWithValue("@returndate", rdate);
                cmd.Parameters.AddWithValue("@elap", elp);
                cmd.Parameters.AddWithValue("@fine", fine);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Returned Successfully");
                con.Close();
                load();




        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtbname.Clear();
            txtrdate.Clear();
            txtelap.Clear();
            txtfine.Clear();
            txtmid.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public void getid(string id)
        {
            sql = "SELECT * FROM returnbook WHERE id = '" + id + "'";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                txtmid.Text = dr[1].ToString();
                txtbname.Text = dr[2].ToString();
                txtrdate.Text = dr[3].ToString();
            }

            con.Close();
        }





        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == dataGridView2.Columns["Delete"].Index && e.RowIndex >= 0)

                id = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
            sql = "DELETE FROM returnbook WHERE id = @id";

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


            txtmid.Clear();
            txtbname.Clear();
            txtrdate.Clear();
            txtelap.Clear();
            txtfine.Clear();
            load();




        
           
        }
    }
}
