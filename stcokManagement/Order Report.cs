using Order_Managment_Menu_2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using User_Feedback;

namespace stcokManagement
{
    public partial class Order_Report : Form
    {
        public Order_Report()
        {
            InitializeComponent();
        }

        //Exit button
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Order Management button
        private void button1_Click(object sender, EventArgs e)
        {
            Order_Menu order_menu = new Order_Menu();
            order_menu.Show();
            this.Hide();
        }

        //Stock Management button
        private void button3_Click(object sender, EventArgs e)
        {
            Stock_Menu stock_menu = new Stock_Menu();
            stock_menu.Show();
            this.Hide();
        }

        //User Feedback button
        private void button4_Click(object sender, EventArgs e)
        {
            Feedback_Menu feedback_menu = new Feedback_Menu();
            feedback_menu.Show();
            this.Hide();
        }

        //Report button
        private void button9_Click(object sender, EventArgs e)
        {
            Report_Menu report_menu = new Report_Menu();
            report_menu.Show();
            this.Hide();
        }

        //logOut button
        private void button5_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

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

        private void Order_Report_Load(object sender, EventArgs e)
        {
            domainUpDown1.Items.Add("Inventory Report");
            domainUpDown1.Items.Add("Stock Report");
            domainUpDown1.Items.Add("Order Report");
            domainUpDown1.Items.Add("Discrepancy Report");
            domainUpDown1.Items.Add("Count Stock Level Report");
        }
    }
}
