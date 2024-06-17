using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySql.Data.MySqlClient;
using User_Feedback;
using Order_Managment_Menu_2;
using goToPage;

namespace stcokManagement
{
    public partial class Stock_Menu : Form
    {
        public GoToPageFunction goToPage;
        MySqlConnection databaseConnection;
        DataTable dataTable;

        private bool isShown = false;
        public Stock_Menu()
        {
            InitializeComponent();
            goToPage = new GoToPageFunction(this);
            Database(); ;
            LockOrUnlock();
        }

        private bool detailStockTableVisible = false;

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
                    if (positionID.Equals("P02"))
                    {
                        button15.Enabled = false;
                        button15.BackColor = Color.FromArgb(1, 30, 48);
                        button1.Enabled = false;
                        button1.BackColor = Color.FromArgb(1, 30, 48);
                    }
                }

            }
        }
        private void stockManagement_Load(object sender, EventArgs e)
        {
            CreateStockTable();

            comboBox1.Items.Add("A");
            comboBox1.Items.Add("B");
            comboBox1.Items.Add("C");
            comboBox1.Items.Add("D");
            //show ID in 左上角
            ReadStaffID();
        }


        private void CreateStockTable()
        {
            DataSet data = new DataSet("Data");

            DataTable table = new DataTable("Stock");
            table.Columns.Add("Part number");
            table.Columns.Add("Category");
            table.Columns.Add("Part Description");
            table.Columns.Add("Date & Time");

            table.Rows.Add("A12345", "A", "xxxxxxxxx", DateTime.Now);
            table.Rows.Add("B78901", "B", "xxxxxxxxx", DateTime.Now);
            table.Rows.Add("C78901", "B", "xxxxxxxxx", DateTime.Now);
            table.Rows.Add("D78901", "B", "xxxxxxxxx", DateTime.Now);
            table.Rows.Add("E78901", "B", "xxxxxxxxx", DateTime.Now);
            table.Rows.Add("F78901", "B", "xxxxxxxxx", DateTime.Now);
            table.Rows.Add("G78901", "B", "xxxxxxxxx", DateTime.Now);
            table.Rows.Add("H78901", "B", "xxxxxxxxx", DateTime.Now);
            table.Rows.Add("I78901", "B", "xxxxxxxxx", DateTime.Now);
            table.Rows.Add("J78901", "B", "xxxxxxxxx", DateTime.Now);

            data.Tables.Add(table);

            dataGridView1.DataSource = data.Tables["Stock"];
        }
        private void CreateDetailStockTable()
        {
            DataSet data = new DataSet("Data");

            DataTable table = new DataTable("Stock");
            table.Columns.Add("Category");
            table.Columns.Add("Part Description");
            table.Columns.Add("Current Quantity");
            table.Columns.Add("Danger level");
            table.Columns.Add("Re-order level");
            table.Columns.Add("Discrepency");

            // Add rows with the missing data
            table.Rows.Add("A", "Part A Description", 10, 5, 20, 0);
            table.Rows.Add("B", "Part B Description", 15, 8, 25, 0);
            table.Rows.Add("B", "Part C Description", 20, 10, 30, 0);
            table.Rows.Add("B", "Part D Description", 12, 6, 22, 0);
            table.Rows.Add("B", "Part E Description", 18, 9, 27, 0);
            table.Rows.Add("B", "Part F Description", 25, 12, 35, 0);
            table.Rows.Add("B", "Part G Description", 8, 4, 18, 0);
            table.Rows.Add("B", "Part H Description", 14, 7, 24, 0);
            table.Rows.Add("B", "Part I Description", 16, 8, 26, 0);
            table.Rows.Add("B", "Part J Description", 22, 11, 32, 0);

            data.Tables.Add(table);

            dataGridView1.DataSource = data.Tables["Stock"];


        }

        //Read Staff Id show in 左上角's method
        private void ReadStaffID()
        {
            string query = "SELECT Staff.Staff_Name FROM Staff";
            string mysqlCon = "server=127.0.0.1; user= root; database= sdpdatabase; password=";
            using (MySqlConnection connection = new MySqlConnection(mysqlCon))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    // 执行查询，并读取结果
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // 读取第一行的 Staff_ID 值并显示在 label3 控件上
                            label3.Text = reader["Staff_Name"].ToString();
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        //logOut button
        private void button5_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();

        }

        //Exit button
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            RFIDscreening RFIDpage = new RFIDscreening();
            RFIDpage.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (detailStockTableVisible)
            {
                CreateStockTable();
                detailStockTableVisible = false;
            }
            else
            {
                CreateDetailStockTable();
                detailStockTableVisible = true;
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //re order button
        private void button10_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            isShown = true;
            reorderInputForm reorderInputForm = new reorderInputForm();
            reorderInputForm.FormClosed += FormClose;
            reorderInputForm.Show();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                // 获取 Panel 的边界矩形
                Rectangle rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                // 使用 Graphics 对象绘制边框
                e.Graphics.DrawRectangle(Pens.Black, rect);
            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                // 获取 Panel 的边界矩形
                Rectangle rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                // 使用 Graphics 对象绘制边框
                e.Graphics.DrawRectangle(Pens.Black, rect);
            }
        }

        //danger form
        private void button11_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            isShown = true;
            DangerInputForm DangerInputForm = new DangerInputForm();
            DangerInputForm.FormClosed += FormClose;
            DangerInputForm.Show();
        }

        //out of stock form
        private void button12_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            isShown = true;
            outOfStockInputForm outOfStockInputForm = new outOfStockInputForm();
            outOfStockInputForm.FormClosed += FormClose;
            outOfStockInputForm.Show();
        }

        // close jump out form
        private void FormClose(object sender, FormClosedEventArgs e)
        {
            isShown = false;
            LockOrUnlock();
        }

        //unlock form
        private void LockOrUnlock()
        {
            this.Enabled = !isShown;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                // 获取 Panel 的边界矩形
                Rectangle rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                // 使用 Graphics 对象绘制边框
                e.Graphics.DrawRectangle(Pens.Black, rect);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            SpareAllocation spareAllocation = new SpareAllocation();
            spareAllocation.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        //User Feedback button
        private void button4_Click(object sender, EventArgs e)
        {
            Feedback_Menu feedback_menu = new Feedback_Menu();
            feedback_menu.Show();
            this.Hide();
        }

        //Report button 
        private void button15_Click_1(object sender, EventArgs e)
        {
            Report_Menu report_menu = new Report_Menu();
            report_menu.Show();
            this.Hide();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            goToPage.GoToStockModify();
        }
    }
}

