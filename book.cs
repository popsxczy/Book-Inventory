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
    public partial class book : Form
    {
        public book()
        {
            InitializeComponent();
            category();
            author();
            publisher();
            load();
        }


        SqlConnection con = new SqlConnection("Data Source = POPSXCZY\\SQLEXPRESS ;Initial Catalog = slibrary;Integrated Security = True");
        SqlCommand cmd;
        SqlDataAdapter da;
        SqlDataReader dr;
        string sql;
        bool Mode = true;
        string id;

        public void category()
        {
            string query = "SELECT * FROM category";
            cmd = new SqlCommand(query, con);
            con.Open();
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            txtcategory.DataSource = ds.Tables[0];
            txtcategory.DisplayMember = "category";
            txtcategory.ValueMember = "id";
            con.Close();


        }



        public void author()
        {
            string query = "SELECT * FROM author";
            cmd = new SqlCommand(query, con);
            con.Open();
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            txtauthor.DataSource = ds.Tables[0];
            txtauthor.DisplayMember = "name";
            txtauthor.ValueMember = "id";
            con.Close();


        }



        public void publisher()
        {
            string query = "SELECT * FROM publisher";
            cmd = new SqlCommand(query, con);
            con.Open();
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            txtpublisher.DataSource = ds.Tables[0];
            txtpublisher.DisplayMember = "name";
            txtpublisher.ValueMember = "id";
            con.Close();


        }


        public void load()
        {
            sql = "SELECT  b.id, b.bname, c.category, a.name, p.name, b.contents, b.page, b.edition from books b JOIN category c ON b.cat_id = c.id JOIN author a ON b.author_id = a.id JOIN publisher p ON b.p_id = p.id";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();

            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6], dr[7]);
            }

            con.Close();
        }









        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            string bname = txtname.Text;
            string category = txtcategory.SelectedValue.ToString();
            string author = txtauthor.SelectedValue.ToString();
            string publisher = txtpublisher.SelectedValue.ToString();
            string contents = txtcontent.Text;
            string pages = txtpages.Text;
            string edition = txtedition.Text;






            if (Mode == true)
            {


                sql = "INSERT INTO books (bname, cat_id, author_id, p_id, contents, page, edition) VALUES (@bname, @cat_id, @author_id, @p_id, @contents, @page, @edition)";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@bname", bname);
                cmd.Parameters.AddWithValue("@cat_id", category);
                cmd.Parameters.AddWithValue("@author_id", author);
                cmd.Parameters.AddWithValue("@p_id", publisher);
                cmd.Parameters.AddWithValue("@contents", contents);
                cmd.Parameters.AddWithValue("@page", pages);
                cmd.Parameters.AddWithValue("@edition", edition);

                cmd.ExecuteNonQuery();
                
                MessageBox.Show("Successfully Added");
                txtname.Focus();
                con.Close(); 
                load();

            }

            else
            {
                sql = "UPDATE books SET bname = @bname, cat_id = @cat_id, author_id = @author_id, p_id = @p_id, contents = @contents, page = @page, edition = @edition WHERE id = @id";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@bname", bname);
                    cmd.Parameters.AddWithValue("@cat_id", category);
                    cmd.Parameters.AddWithValue("@author_id", author);
                    cmd.Parameters.AddWithValue("@p_id", publisher);
                    cmd.Parameters.AddWithValue("@contents", contents);
                    cmd.Parameters.AddWithValue("@page", pages);
                    cmd.Parameters.AddWithValue("@edition", edition);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                MessageBox.Show("Successfully Updated");
                txtname.Clear();

                con.Close();
                load();

                txtname.Focus();


            }










        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtname.Clear();
            txtcontent.Clear();
            txtpages.Clear();
        }




        public void getid(string id)
        {
            sql = "SELECT * FROM books WHERE id = '" + id + "'";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                txtname.Text = dr[1].ToString();
                txtcategory.Text = dr[2].ToString();
                txtauthor.Text = dr[3].ToString();
                txtpublisher.Text = dr[4].ToString();
                txtcontent.Text = dr[5].ToString();
                txtpages.Text = dr[6].ToString();
                txtedition.Text = dr[7].ToString();
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
                sql = "DELETE FROM books WHERE id = @id";

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

                MessageBox.Show("Book deleted");


                txtname.Clear();
                load();
                txtname.Focus();

                txtname.Focus();


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
