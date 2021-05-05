using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomationApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void lgnbtn_Click(object sender, EventArgs e)
        {
            string ConnectionStr = "Server= rst-db-do-user-8696039-0.b.db.ondigitalocean.com;Port = 25060;Database=RST_DB;Uid=doadmin;Pwd=wwd0oli7w2rplovh;SslMode=Required;";
            string email = emailtxtbox.Text;
            MySqlConnection connect = new MySqlConnection(ConnectionStr);
            MySqlCommand check = connect.CreateCommand();
            check.Parameters.AddWithValue("@email", emailtxtbox.Text);
            check.CommandText = "SELECT COUNT(*) FROM RST_DB.schedules where user_email= @email";
            connect.Open();
            int results = (int)(long)check.ExecuteScalar();
            if (results > 0)
            {
                this.Close();
                connect.Close();
                Process.Start(@"C:\Users\owens\Desktop\RedStickTech\Automation\WalmartAutomation\bin\Debug\net5.0\WalmartAutomation.exe", email);
            }
            else
            {
                MessageBox.Show("you are not in the database.");
            }

        }
    }
}
