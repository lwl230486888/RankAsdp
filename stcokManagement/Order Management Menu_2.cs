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
using User_Feedback;
using goToPage;
using Order_Creation;
using MySql.Data.MySqlClient;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Order_Managment_Menu_2
{
    public partial class Order_Menu : Form
    {
        public GoToPageFunction goToPage;
        MySqlConnection databaseConnection;
        DataTable dataTable;

        public Order_Menu()
        {
            InitializeComponent();
            goToPage = new GoToPageFunction(this);
            Database();
            button3.Hide();
            button1.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
            textBox14.TextChanged += new EventHandler(textBoxSearch_TextChanged);
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
                        button3.Enabled = false;
                        button3.BackColor = Color.FromArgb(1, 30, 48);
                        button1.Enabled = false;
                        button1.BackColor = Color.FromArgb(1, 30, 48);
                        myReader.Close();
                    }
                }

            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string yourOrderId = row.Cells["Your_Order_ID"].Value.ToString();
                string Staff_Name = "Charlie Brown";

                string sql = "SELECT DID_ID, Spare_ID, Staff_Name, Spare_System_Quantity, Your_Order_ID FROM DID WHERE Your_Order_ID = @yourOrderId ORDER By Your_Order_ID ASC";
                DataTable detailTable = new DataTable();

                using (MySqlConnection connection = new MySqlConnection("server=127.0.0.1; user=root; database=sdpdatabase; password="))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@yourOrderId", yourOrderId);

                        using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command))
                        {
                            dataAdapter.Fill(detailTable);
                            
                        }
                    }
                }
                string orderDetails = $"Order ID: {yourOrderId}\nStaff Name: {Staff_Name}\n\n";

                foreach (DataRow detailRow in detailTable.Rows)
                {
                    string spareID = detailRow["Spare_ID"].ToString();
                    string spareName = SwitchSQL.SwitchSQL.Spare_IDToSpare_Name(spareID);

                    orderDetails += $"Spare Name: {spareName}\n" +
                                    $"DID ID: {detailRow["DID_ID"]}\n" +
                                    $"Quantity: {detailRow["Spare_System_Quantity"]}\n\n";
                }


                MessageBox.Show(orderDetails, "Order Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void LoadDataIntoDataGridView()
        {
            string sql = "SELECT Your_Order_ID, Dealer_ID, Order_Date, Order_Time, Order_Status, Order_Address, Total_Amount FROM `Order` ORDER BY Your_Order_ID ASC";
            using (MySqlConnection connection = new MySqlConnection("server=127.0.0.1; user=root; database=sdpdatabase; password="))
            {
                connection.Open();
                using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql, connection))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Assuming the column index of Dealer ID is 1
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        string dealerID = dataTable.Rows[i]["Dealer_ID"].ToString();
                        // Call your method to convert dealer ID to dealer name
                        string dealerName = SwitchSQL.SwitchSQL.Dealer_IDToDealer_Name(dealerID);
                        // Update the DataTable with the dealer name
                        dataTable.Rows[i]["Dealer_ID"] = dealerName;
                    }

                    // Create a new column for combined date and time
                    dataTable.Columns.Add("Order_DateTime", typeof(string));

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        string orderDate = dataTable.Rows[i]["Order_Date"].ToString();
                        string orderTime = dataTable.Rows[i]["Order_Time"].ToString();
                        dataTable.Rows[i]["Order_DateTime"] = $"{orderDate} {orderTime}";
                    }

                    // Remove the original Order_Date and Order_Time columns
                    dataTable.Columns.Remove("Order_Date");
                    dataTable.Columns.Remove("Order_Time");

                    // Update DataGridView
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.ReadOnly = true;

                    // Update column headers
                    dataGridView1.Columns["Your_Order_ID"].HeaderText = "Order ID";
                    dataGridView1.Columns["Dealer_ID"].HeaderText = "Dealer Name";
                    dataGridView1.Columns["Order_DateTime"].HeaderText = "Order DateTime";
                    dataGridView1.Columns["Order_Status"].HeaderText = "Order Status";
                    dataGridView1.Columns["Order_Address"].HeaderText = "Order Address";
                    dataGridView1.Columns["Total_Amount"].HeaderText = "Total Amount";

                    // Set column styles
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.DefaultCellStyle.BackColor = Color.Gray;
                    }

                    dataGridView1.Columns["Order_Address"].Width = 200;
                    dataGridView1.Columns["Order_DateTime"].Width = 120;
                    dataGridView1.Columns["Order_Status"].Width = 60;

                    // Change the display order of the columns
                    dataGridView1.Columns["Your_Order_ID"].DisplayIndex = 0;
                    dataGridView1.Columns["Dealer_ID"].DisplayIndex = 1;
                    dataGridView1.Columns["Order_DateTime"].DisplayIndex = 2;
                    dataGridView1.Columns["Order_Status"].DisplayIndex = 3;
                    dataGridView1.Columns["Order_Address"].DisplayIndex = 4;
                    dataGridView1.Columns["Total_Amount"].DisplayIndex = 5;
                }
            }
        }






        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            goToPage.GoToDealerInfo();
        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        //Exit button
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //logOut button
        private void button5_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        //Order Management button
        private void button10_Click(object sender, EventArgs e)
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
        private void button1_Click(object sender, EventArgs e)
        {
            Report_Menu report_menu = new Report_Menu();
            report_menu.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            goToPage.GoToCreateSellOrder();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // 允许编辑
            dataGridView1.ReadOnly = false;

            // 设置所有列为只读
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }

            // 设置第三列为可编辑
            dataGridView1.Columns["Order_Address"].ReadOnly = false; // 第三列索引为2
            dataGridView1.Columns["Order_Address"].DefaultCellStyle.BackColor = Color.White; // 设置背景色为白色

            // 在第三列中添加下拉菜单
            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
            comboBoxColumn.HeaderText = "Status Options";
            comboBoxColumn.Items.Add("Pending");
            comboBoxColumn.Items.Add("Success");
            comboBoxColumn.Items.Add("Reject");
            comboBoxColumn.Items.Add("Done");

            dataGridView1.Columns.Insert(3, comboBoxColumn);

            // 显示按钮13
            button13.Visible = true;
        }




        //save button
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                // 确认有选取的单元格
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    // 获取选取的单元格索引
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    // 如果订单状态为 null，显示警告并返回
                    if (dataGridView1.Rows[rowIndex].Cells[3].Value == null)
                    {
                        MessageBox.Show("Please select an order status.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 获取该行的数据
                    string yourOrderId = dataGridView1.Rows[rowIndex].Cells["Your_Order_ID"].Value.ToString();
                    string dealerId = dataGridView1.Rows[rowIndex].Cells["Dealer_ID"].Value == DBNull.Value ? null : dataGridView1.Rows[rowIndex].Cells["Dealer_ID"].Value.ToString();
                    string orderStatus = dataGridView1.Rows[rowIndex].Cells[3].Value == DBNull.Value ? null : dataGridView1.Rows[rowIndex].Cells[3].Value.ToString();
                    string orderAddress = dataGridView1.Rows[rowIndex].Cells["Order_Address"].Value == DBNull.Value ? null : dataGridView1.Rows[rowIndex].Cells["Order_Address"].Value.ToString();

                    // 数据验证
                    if (string.IsNullOrEmpty(dealerId))
                    {
                        MessageBox.Show("Dealer ID cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }



                    // 确保数据库连接是打开的
                    if (databaseConnection.State != ConnectionState.Open)
                    {
                        databaseConnection.Open();
                    }

                    // 更新数据库中的数据
                    string updateQuery = "UPDATE `Order` SET Dealer_ID = @dealerId, Order_Status = @orderStatus, Order_Address = @orderAddress WHERE Your_Order_ID = @yourOrderId";
                    using (MySqlCommand command = new MySqlCommand(updateQuery, databaseConnection))
                    {
                        command.Parameters.AddWithValue("@dealerId", dealerId);
                        command.Parameters.AddWithValue("@orderStatus", orderStatus);
                        command.Parameters.AddWithValue("@orderAddress", orderAddress);
                        command.Parameters.AddWithValue("@yourOrderId", yourOrderId);

                        // 执行 SQL 命令
                        int rowsAffected = command.ExecuteNonQuery();

                        // 检查是否有行受到影响
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Order updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // 重新加载数据
                            LoadDataIntoDataGridView();

                            // 将按钮设置为不可见并将 DataGridView 设置为只读
                            button13.Visible = false;
                            dataGridView1.ReadOnly = true;
                            dataGridView1.Columns[2].DefaultCellStyle.BackColor = Color.Gray;
                            dataGridView1.Columns[3].DefaultCellStyle.BackColor = Color.Gray;
                        }
                        else
                        {
                            MessageBox.Show("No rows updated.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // 处理异常，记录错误或通知用户
                MessageBox.Show("An error occurred while updating the order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 确保数据库连接在完成后关闭
                if (databaseConnection.State == ConnectionState.Open)
                {
                    databaseConnection.Close();
                }
            }
        }



        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }
        private void ApplySearchFilter()
        {
            string filterText = textBox14.Text.Trim();
            DataTable dataTable = dataGridView1.DataSource as DataTable;

            if (dataTable != null)
            {
                if (string.IsNullOrEmpty(filterText))
                {
                    dataTable.DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    dataTable.DefaultView.RowFilter = $"Your_Order_ID LIKE '%{filterText}%'";
                }
            }
        }





    }
}
