using goToPage;
using MySql.Data.MySqlClient;
using Order_Managment_Menu_2;
using stcokManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace User_Feedback
{
    public partial class Feedback_Menu : Form
    {
        MySqlConnection databaseConnection;
        DataTable dataTable;
        private GoToPageFunction goToPages;
        public Feedback_Menu()
        {
            InitializeComponent();
            goToPages = new GoToPageFunction(this);
            Database();
        }

        public void Database()
        {
            if (databaseConnection == null || databaseConnection.State != ConnectionState.Open)
            {
                string mysqlCon = "server=127.0.0.1; user=root; database=sdpdatabase; password=";
                databaseConnection = new MySqlConnection(mysqlCon);
                databaseConnection.Open();

                // Assuming Login class has a method to get the Staff ID
                Login login = new Login();
                string staffID = login.getID();

                // Correctly parameterize the query to prevent SQL injection
                string query = "SELECT Position_ID, Staff_Name FROM staff WHERE Staff_ID = @staffID";
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                commandDatabase.Parameters.AddWithValue("@staffID", staffID);

                MySqlDataReader myReader = commandDatabase.ExecuteReader();

                // Check if the reader has any rows before accessing the data
                if (myReader.Read())
                {
                    label3.Text = myReader["Staff_Name"].ToString();
                    string positionID = myReader["Position_ID"].ToString();
                    if (positionID.Equals("P01"))
                    {
                        button3.Visible = false;
                        button9.Visible = false;
                    }
                    else if (positionID.Equals("P02"))
                    {
                        button9.Visible = false;
                        button1.Visible = false;
                    }
                    else if (positionID.Equals("P03"))
                    {
                        button3.Visible = false;
                        button1.Visible = false;
                    }
                }

            }
        }
        private void Feedback_Menu_Load(object sender, EventArgs e)
        {

        }

        //Order Management button
        private void button1_Click(object sender, EventArgs e)
        {
            goToPages.GoToOrderMenu();
        }

        //Exit button
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //logOut button
        private void button5_Click(object sender, EventArgs e)
        {
            goToPages.Logout();
        }

        //Stock Management button
        private void button3_Click(object sender, EventArgs e)
        {
            goToPages.GoToStockModify();
        }

        //User Feedback button
        private void button4_Click(object sender, EventArgs e)
        {
            goToPages.GoToUserFeedback();
        }

        //Report button 
        private void button9_Click(object sender, EventArgs e)
        {
            goToPages.GoToReport();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thank you for feedback");
            textBox1.Text = string.Empty;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
