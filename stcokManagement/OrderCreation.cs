using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Generator;
using goToPage;
using SwitchSQL;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Text;
using System.Drawing;

namespace Order_Creation
{
    public partial class OrderCreation : Form
    {
        private MySqlConnection databaseConnection;
        private GoToPageFunction goTopages;
        private DataTable dt = new DataTable();



        public OrderCreation()
        {
            goTopages = new GoToPageFunction(this);
            InitializeComponent();
            Database();
            LoadDataIntoDataGridView();
            dataGridView1.CellValidating += dataGridView1_CellValidating;

            comboBox1.TextChanged += Control_TextChanged;
            comboBox3.TextChanged += Control_TextChanged;
            textBox4.TextChanged += Control_TextChanged;
            EnableCreateButton();
        }


        public void Database()
        {
            if (databaseConnection == null || databaseConnection.State != ConnectionState.Open)
            {
                string mysqlCon = "server=127.0.0.1; user=root; database=sdpdatabase; password=";
                databaseConnection = new MySqlConnection(mysqlCon);
                databaseConnection.Open();
            }
            else
            {
                MessageBox.Show("Connection is already open.");
            }
        }

        private void OrderCreation_Load(object sender, EventArgs e)
        {
            ConfigureComboBox();
            LoadData(); // 加载数据
            button1.Enabled = false;
            textBox1.TextChanged += new EventHandler(textBoxSearch_TextChanged);
            button9.Hide();
            button6.Hide();
            textBox2.Enabled = false;
            textBox2.ForeColor = Color.Black;
        }

        //check 係咪係有入嘢
        private void Control_TextChanged(object sender, EventArgs e)
        {
            EnableCreateButton();
        }
        //呢個係如果所有資料齊曬先比create order
        private void EnableCreateButton()
        {
            if (comboBox1.Text.Length > 0 && comboBox3.Text.Length > 0 && textBox4.Text.Length > 0)
            {
                button1.Enabled = true;
                button1.BackColor = System.Drawing.Color.White;
            }
            else
            {
                button1.Enabled = false;
                button1.BackColor = System.Drawing.Color.Gray;
            }
        }

        private void ConfigureComboBox()
        {
            comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadData()
        {
            Fill_Dealer_ID_ComboBox();
            Fill_To_Follow_ComboBox();
        }
        public void Fill_Dealer_ID_ComboBox()
        {
            try
            {
                comboBox3.Items.Clear(); // 清除现有项
                string sql = "SELECT Dealer_Name FROM dealer order by Dealer_ID ASC";
                MySqlCommand command = new MySqlCommand(sql, databaseConnection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBox3.Items.Add(reader.GetString("Dealer_Name"));
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public void Fill_To_Follow_ComboBox()
        {
            try
            {
                comboBox1.Items.Clear(); // 清除现有项
                string sql = "SELECT Staff_Name FROM staff where Position_ID = 'P03' order by Staff_Name ASC";
                MySqlCommand command = new MySqlCommand(sql, databaseConnection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString("Staff_Name"));
                }

                reader.Close();
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }

        private void textBox4_TextChanged(object sender, EventArgs e) { }

        private void button13_Click(object sender, EventArgs e)
        {
            goTopages.GoToOrderMenu();
        }


        private int Get_Max_Spare_System_Quantity(String SN)
        {
            int quantity = 0;

            String sql = "SELECT Spare_System_Quantity from Spare_Part where Spare_Name = @Spare_Name";
            MySqlCommand cmd = new MySqlCommand(sql, databaseConnection);

            cmd.Parameters.AddWithValue("@Spare_Name", SN);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    quantity = Convert.ToInt32(reader["Spare_System_Quantity"]);
                }
            }

            return quantity;
        }

        private void LoadDataIntoDataGridView()
        {
            try
            {
                // 执行 SQL 查询
                string sqlQuery = "SELECT Spare_Name, Spare_Price, Spare_System_Quantity FROM Spare_Part";
                MySqlCommand command = new MySqlCommand(sqlQuery, databaseConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                // 创建一个 DataTable 用于保存查询结果
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // 添加一个列用于让用户选择数量
                DataColumn quantityColumn = dataTable.Columns.Add("Quantity", typeof(int));
                quantityColumn.AutoIncrement = true; // 启用自增长

                // 添加一个列用于存储每个备件的总价
                DataColumn totalPriceColumn = dataTable.Columns.Add("TotalPrice", typeof(decimal));

                // 将 DataTable 设置为 dataGridView1 的数据源
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // 设置 DataGridView 的显示样式和列标题
                dataGridView1.Columns[0].Width = 220;
                dataGridView1.Columns[1].Width = 100;
                dataGridView1.Columns[2].Width = 80;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[0].DefaultCellStyle.BackColor = Color.Gray;
                dataGridView1.Columns[1].DefaultCellStyle.BackColor = Color.Gray;
                dataGridView1.Columns[2].DefaultCellStyle.BackColor = Color.Gray;
                dataGridView1.Columns[4].DefaultCellStyle.BackColor = Color.Gray;
                dataGridView1.ForeColor = Color.Black;
                dataGridView1.Columns[0].HeaderText = "Spare Name";
                dataGridView1.Columns[1].HeaderText = "Spare Price";
                dataGridView1.Columns[2].HeaderText = "Spare System Quantity";

                // 添加事件处理程序以计算总价并更新 textBox2
                dataGridView1.CellEndEdit += DataGridView1_CellEndEdit;
            }
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Quantity"].Index)
            {
                object quantityValue = dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value;
                object priceValue = dataGridView1.Rows[e.RowIndex].Cells["Spare_Price"].Value;

                // 检查 Quantity 列的值是否为 DBNull
                if (quantityValue != DBNull.Value && priceValue != DBNull.Value)
                {
                    // 转换为相应的数据类型
                    int quantity = Convert.ToInt32(quantityValue);
                    decimal price = Convert.ToDecimal(priceValue);

                    // 计算总价
                    decimal totalPrice = quantity * price;

                    // 更新 DataTable 中的总价列
                    dataGridView1.Rows[e.RowIndex].Cells["TotalPrice"].Value = totalPrice;

                    // 计算所有备件的总价并更新 Label 的文本
                    UpdateTotalPrice();
                }
                else
                {
                    // 处理 DBNull 值的情况，可以根据需要进行处理，例如给出提示或默认值
                    MessageBox.Show("Quantity or Spare Price cannot be DBNull.");
                }
            }
        }

        private void UpdateTotalPrice()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["TotalPrice"].Value != DBNull.Value)
                {
                    total += Convert.ToDecimal(row.Cells["TotalPrice"].Value);
                }
            }

            textBox2.Text = total.ToString(); // 更新 textBox2 显示总价
        }


        //search spare function
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }
        private void ApplySearchFilter()
        {
            string filterText = textBox1.Text.Trim();
            DataTable dataTable = dataGridView1.DataSource as DataTable;

            if (dataTable != null)
            {
                if (string.IsNullOrEmpty(filterText))
                {
                    dataTable.DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    dataTable.DefaultView.RowFilter = $"Spare_Name LIKE '%{filterText}%'";
                }
            }
        }

        private List<string> GetAllSpareNames()
        {
            List<string> spareNames = new List<string>();

            try
            {
                // 执行 SQL 查询
                string sqlQuery = "SELECT DISTINCT Spare_Name FROM Spare_Part";
                MySqlCommand command = new MySqlCommand(sqlQuery, databaseConnection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        spareNames.Add(reader["Spare_Name"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return spareNames;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 3) // 检查是否是 Quantity 列
            {
                DataGridViewRow currentRow = dataGridView1.Rows[e.RowIndex];
                DataGridViewCell quantityCell = currentRow.Cells[3];
                DataGridViewCell spareQuantityCell = currentRow.Cells[2];

                int? quantity = null;
                if (!string.IsNullOrEmpty(e.FormattedValue?.ToString()))
                {
                    if (!int.TryParse(e.FormattedValue.ToString(), out int parsedValue))
                    {
                        // 输入值不是整数，取消编辑
                        dataGridView1.Rows[e.RowIndex].ErrorText = "Value must be an integer.";
                        e.Cancel = true;
                        return;
                    }
                    quantity = parsedValue;
                }

                int spareQuantity;
                if (!int.TryParse(spareQuantityCell.Value?.ToString(), out spareQuantity))
                {
                    // 获取 Spare_System Quantity 值失败，取消编辑
                    dataGridView1.Rows[e.RowIndex].ErrorText = "Invalid Spare_System Quantity value.";
                    e.Cancel = true;
                    return;
                }

                if (quantity > spareQuantity)
                {
                    // 输入值大于等于 Spare_System Quantity，取消编辑
                    dataGridView1.Rows[e.RowIndex].ErrorText = $"Value must be less than Spare_System Quantity ({spareQuantity}).";
                    e.Cancel = true;
                    return;
                }

                if (quantity < 0)
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = $"Value must be bigger that 0 ({spareQuantity}).";
                    e.Cancel = true;
                    return;
                }

                // 输入值合法，清除错误信息
                dataGridView1.Rows[e.RowIndex].ErrorText = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String DealerID = SwitchSQL.SwitchSQL.Dealer_NameToDealer_ID((string)comboBox3.SelectedItem);
            String orderAddress = textBox4.Text;


            String Your_Order_ID = Insert_Order(DealerID, orderAddress);
            Insert_DID(Your_Order_ID);
            UpdateSpareSystemQuantity();
            MessageBox.Show("Create Success! Your Order ID is " + Your_Order_ID);
            goTopages.GoToOrderMenu();
        }

        private string Insert_Order(string dealerID, string orderAddress)
        {
            String insert_Order = "INSERT INTO `Order` (Your_Order_ID, Dealer_ID, Order_Date, Order_Time, Order_Status, Order_Address, Total_Amount) " +
                "VALUES (@Your_Order_ID, @Dealer_ID, @Order_Date, @Order_Time, @Order_Status, @Order_Address, @Total_Amount)";
            MySqlCommand cmd = new MySqlCommand(insert_Order, databaseConnection);

            string orderDate = DateTime.Now.ToString("yyyy-MM-dd");
            string orderTime = DateTime.Now.ToString("HH:mm:ss");
            String orderStatus = "Pending";
            String Your_Order_ID = Genrator.Gen_Order_ID();
            Double TotalAmount = double.Parse(textBox2.Text);
            cmd.Parameters.AddWithValue("@Your_Order_ID", Your_Order_ID);
            cmd.Parameters.AddWithValue("@Dealer_ID", dealerID);
            cmd.Parameters.AddWithValue("@Order_Date", orderDate);
            cmd.Parameters.AddWithValue("@Order_Time", orderTime);
            cmd.Parameters.AddWithValue("@Order_Status", orderStatus);
            cmd.Parameters.AddWithValue("@Order_Address", orderAddress);
            cmd.Parameters.AddWithValue("@Total_Amount", TotalAmount);

            cmd.ExecuteNonQuery();
            return Your_Order_ID;
        }


        private void Insert_DID(string Your_Order_ID)
        {
            String insert_DID = "INSERT INTO DID (DID_ID, Spare_ID, Staff_Name, Spare_System_Quantity, Your_Order_ID)" +
                " VALUES (@DID_ID, @Spare_ID, @Staff_Name, @Spare_System_Quantity, @Your_Order_ID)";
            MySqlCommand cmd = new MySqlCommand(insert_DID, databaseConnection);

            // 在循环外部清除参数
            cmd.Parameters.Clear();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                object Quantity = row.Cells[3].Value;
                if (Quantity != null && !string.IsNullOrWhiteSpace(Quantity.ToString()))
                {
                    // 如果 Quantity 不为空，获取其他所需数据
                    String DID_ID = Generator.Genrator.Gen_DID_ID();
                    String Staff_Name = (string)comboBox1.SelectedItem;
                    String Spare_ID = SwitchSQL.SwitchSQL.Spare_NameToSpare_ID(row.Cells[0].Value?.ToString());
                    String Spare_System_Quantity = Quantity.ToString();

                    // 设置参数
                    cmd.Parameters.AddWithValue("@DID_ID", DID_ID);
                    cmd.Parameters.AddWithValue("@Spare_ID", Spare_ID);
                    cmd.Parameters.AddWithValue("@Staff_Name", Staff_Name);
                    cmd.Parameters.AddWithValue("@Spare_System_Quantity", Spare_System_Quantity);
                    cmd.Parameters.AddWithValue("@Your_Order_ID", Your_Order_ID);

                    // 执行插入操作
                    cmd.ExecuteNonQuery();

                    // 清除参数，以便下一次迭代
                    cmd.Parameters.Clear();
                }
            }
        }

        private void UpdateSpareSystemQuantity()
        {
            string updateQuery = "UPDATE Spare_Part SET Spare_System_Quantity = Spare_System_Quantity - @Quantity WHERE Spare_ID = @SpareID";
            MySqlCommand cmd = new MySqlCommand(updateQuery, databaseConnection);

            cmd.Parameters.Clear();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                object Quantity = row.Cells[3].Value;
                if (Quantity != null && !string.IsNullOrWhiteSpace(Quantity.ToString()))
                {
                    String Spare_ID = SwitchSQL.SwitchSQL.Spare_NameToSpare_ID(row.Cells[0].Value?.ToString());
                    String Spare_System_Quantity = Quantity.ToString();
                    cmd.Parameters.AddWithValue("@Quantity", Spare_System_Quantity);
                    cmd.Parameters.AddWithValue("@SpareID", Spare_ID);

                    // 执行插入操作
                    cmd.ExecuteNonQuery();

                    // 清除参数，以便下一次迭代
                    cmd.Parameters.Clear();
                }
            }

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}