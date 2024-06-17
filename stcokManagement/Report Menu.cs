using goToPage;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Order_Managment_Menu_2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using User_Feedback;
using static Mysqlx.Notice.Warning.Types;

namespace stcokManagement
{
    public partial class Report_Menu : Form
    {
        public GoToPageFunction goToPages;
        MySqlConnection databaseConnection;
        DataTable dataTable;
        public Report_Menu()
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
                    if (positionID.Equals("P03"))
                    {
                        button3.Visible = false;
                        button1.Visible = false;
                    }
                }

            }
        }
        private void Report_Menu_Load(object sender, EventArgs e)
        {
            domainUpDown1.Items.Add("Inventory Report");
            domainUpDown1.Items.Add("Stock Report");
            domainUpDown1.Items.Add("Order Report");
            domainUpDown1.Items.Add("Discrepancy Report");
            domainUpDown1.Items.Add("Count Stock Level Report");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (domainUpDown1.SelectedIndex == -1)
            {
                MessageBox.Show("Error! You must choose the Report Type. Or you input the invalid input. The input must be(Inventory Report, Stock Report, Order Report, Discrepancy Report, Count Stock Level Report) ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (domainUpDown1.SelectedItem.ToString() == "Inventory Report")
            {
                Inventory_Report inventory_report = new Inventory_Report();
                inventory_report.Show();
                this.Hide();
            }
            else if (domainUpDown1.SelectedItem.ToString() == "Stock Report")
            {
                Stock_Report stock_report = new Stock_Report();
                stock_report.Show();
                this.Hide();
            }
            else if (domainUpDown1.SelectedItem.ToString() == "Discrepancy Report")
            {
                Discrepancy_Report discrepancy_report = new Discrepancy_Report();
                discrepancy_report.Show();
                this.Hide();
            }
            else if (domainUpDown1.SelectedItem.ToString() == "Order Report")
            {
                Order_Report order_report = new Order_Report();
                order_report.Show();
                this.Hide();
            }
            else if (domainUpDown1.SelectedItem.ToString() == "Count Stock Level Report")
            {
                Stock_Level_Report stock_level_report = new Stock_Level_Report();
                stock_level_report.Show();
                this.Hide();
            }
        }

        //Order Management button
        private void button1_Click(object sender, EventArgs e)
        {
            goToPages.GoToOrderMenu();
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
    }
}
