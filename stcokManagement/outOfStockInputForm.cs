using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stcokManagement
{
    public partial class outOfStockInputForm : Form
    {
        Login lg;

        MySqlConnection connection;
        public outOfStockInputForm()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
             
            }

        private void outOfStockInputForm_Load(object sender, EventArgs e)
        {
            string mysqlCon = "server=127.0.0.1; user= root; database= sdpdatabase; password=";

            connection = new MySqlConnection(mysqlCon);

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            lg = new Login();
            string queryForName = "SELECT Staff_Name FROM Staff WHERE Staff_ID ='" + lg.getID() + "'";
            MySqlCommand command = new MySqlCommand(queryForName, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string staffName = reader.GetString("Staff_Name");
                // 将staffName赋值给label3的Text属性
                label6.Text = staffName;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            
        }
    }
    }

