using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace ODP2
{
    public partial class Rental_Form : Form
    {
        OracleConnection conn;
        string ordb = "data source = orcl; user id =scott; password=tiger;";

        public Rental_Form()
        {
            InitializeComponent();
        }

        private void Rental_Form_Load(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            txt_DateRented.Text = DateTime.Now.ToString();
            txt_DateBack.Text = DateTime.Now.AddDays(3).ToString();

        }

        private void btn_showCopies_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "GetCopyID";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("title", txt_title.Text);
            cmd.Parameters.Add("id", OracleDbType.RefCursor, ParameterDirection.Output);
            OracleDataReader dr =cmd.ExecuteReader();
            while (dr.Read())
            {
                cmd_copies.Items.Add(dr[0]);
            }
            dr.Close();


        }

        private void btn_rent_Click(object sender, EventArgs e)
        {
            
            int maxID, newID;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "GetRentID";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("id", OracleDbType.Int32, ParameterDirection.Output);
            cmd.ExecuteNonQuery();
            try
            {
                maxID = Convert.ToInt32(cmd.Parameters["id"].Value.ToString());
                newID = maxID + 1;
            }
            catch
            { newID = 1; }
 
            OracleCommand c2 = new OracleCommand();
            c2.Connection = conn;
            c2.CommandText = "update_status";
            c2.CommandType = CommandType.StoredProcedure;
            c2.Parameters.Add("id", cmb_copies.SelectedItem.ToString());
            c2.ExecuteNonQuery();
            MessageBox.Show("Film status updated succefully");

            
        }
        private void insert_rent_Click(object sender, EventArgs e)
        {

           
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT_RENTAL";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("RID", newID);
            cmd.Parameters.Add("MID", txt_memID.Text);
            cmd.Parameters.Add("CID", cmb_copies.SelectedItem.ToString());
            cmd.Parameters.Add("D1",Convert.ToDateTime(txt_DateRented.Text));
            cmd.Parameters.Add("D2", Convert.ToDateTime(txt_DateBack.Text));
            cmd.Parameters.Add("cost",txt_cost.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Film status updated succefully");


        }
    }
}
