using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using goToPage;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;
using stcokManagement;
using System.Drawing.Printing;
using Google.Protobuf.Collections;
using Order_Managment_Menu_2;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace modifyStockLevel
{
    public partial class modifyStock : Form
    {
        private MySqlConnection connection;

        private GoToPageFunction goToPages;

        private DataTable dataTable;

        public Login lg;

        private bool isEditing = false;
        private bool isShown = false;

        public modifyStock()
        {
            InitializeComponent();

            goToPages= new GoToPageFunction(this);

            comboBox1.TextChanged += button6_TextChanged;
            textBox1.TextChanged += button6_TextChanged;
            textBox2.TextChanged += button6_TextChanged;
            textBox4.TextChanged += button6_TextChanged;
            Database();
            EnableCreateButton();
            LockOrUnlock();
        }

        public void Database()
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                string mysqlCon = "server=127.0.0.1; user=root; database=sdpdatabase; password=";
                connection = new MySqlConnection(mysqlCon);
                connection.Open();

                // Assuming Login class has a method to get the Staff ID
                Login login = new Login();
                string staffID = login.getID();

                // Correctly parameterize the query to prevent SQL injection
                string query = "SELECT Position_ID, Staff_Name FROM staff WHERE Staff_ID = @staffID";
                MySqlCommand commandDatabase = new MySqlCommand(query, connection);
                commandDatabase.Parameters.AddWithValue("@staffID", staffID);

                MySqlDataReader myReader = commandDatabase.ExecuteReader();

                // Check if the reader has any rows before accessing the data
                if (myReader.Read())
                {
                    label3.Text = myReader["Staff_Name"].ToString();
                    string positionID = myReader["Position_ID"].ToString();
                    if (positionID.Equals("P02"))
                    {
                        button7.Visible = false;
                        button7.BackColor = Color.FromArgb(1, 30, 48);
                        button1.Visible = false;
                        button1.BackColor = Color.FromArgb(1, 30, 48);
                    }
                }

            }
        }

        private void EditStockTb()
        {
            string query = "SELECT Spare_ID, Spare_Category, Spare_Name, Spare_Actual_Quantity, Supplier_ID FROM spare_part";

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);

            dataTable = new DataTable(); // 将数据表赋值给类级别的 dataTable 变量
            dataAdapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            dataGridView3.DataSource = dataTable;
        }
        private void AddSpareTb()
        {
            string query = "SELECT Spare_ID, Spare_Category, Spare_Name, Spare_Price, Supplier_ID FROM spare_part";

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);

            dataTable = new DataTable(); // 将数据表赋值给类级别的 dataTable 变量
            dataAdapter.Fill(dataTable);
            dataGridView2.DataSource = dataTable;
        }

        private void SaveDataToDatabase(string sID, string cat, string name, decimal price, string supID)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                string query = "INSERT INTO spare_part (Spare_ID, Spare_Category, Spare_Name, Spare_Price, Supplier_ID) VALUES (@Spare_ID, @Spare_Category, @Spare_Name, @Spare_Price, @Supplier_ID)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Check for null values
                    if (string.IsNullOrEmpty(sID) || string.IsNullOrEmpty(cat) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(supID))
                    {
                        throw new Exception("Please provide all required data.");
                    }

                    command.Parameters.AddWithValue("@Spare_ID", sID);
                    command.Parameters.AddWithValue("@Spare_Category", cat);
                    command.Parameters.AddWithValue("@Spare_Name", name);
                    command.Parameters.AddWithValue("@Spare_Price", price);
                    command.Parameters.AddWithValue("@Supplier_ID", supID);
                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    // Handle duplicate entry error
                    MessageBox.Show("A duplicate entry already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Handle other MySQL database-related errors
                    MessageBox.Show("An error occurred while saving data to the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle other types of errors
                MessageBox.Show("An error occurred while saving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void load_data()
        {
            string mysqlCon = "server=127.0.0.1; user= root; database= sdpdatabase; password=";

            connection = new MySqlConnection(mysqlCon);

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            EditStockTb();
            AddSpareTb();
            panel6.Hide();
            dataGridView2.Hide();
            dataGridView1.Hide();
            button10.Hide();
            

            //set staff name
            lg = new Login();
            string queryForName = "SELECT Staff_Name FROM Staff WHERE Staff_ID ='" + lg.getID() + "'";
            MySqlCommand command = new MySqlCommand(queryForName, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string staffName = reader.GetString("Staff_Name");
                // 将staffName赋值给label3的Text属性
                label3.Text = staffName;
            }
            button6.Enabled = false;
            connection.Close();
        }
        private void button6_TextChanged(object sender, EventArgs e)
        {
            EnableCreateButton();
        }
        //呢個係如果所有資料齊曬先比create order
        private void EnableCreateButton()
        {
            if (comboBox1.Text.Length > 0 && textBox2.Text.Length > 0 && textBox1.Text.Length > 0 && textBox4.Text.Length > 0)
            {
                button6.Enabled = true;
            }
            else
            {
                button6.Enabled = false;
            }
        }


        private void modifyStock_Load(object sender, EventArgs e)
        {
            load_data();

            comboBox1.Items.Add("A");
            comboBox1.Items.Add("B");
            comboBox1.Items.Add("C");
            comboBox1.Items.Add("D");

            dataGridView1.Columns[1].ReadOnly= true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[0].HeaderText = "Spare ID";
            dataGridView1.Columns[1].HeaderText = "Spare Category";
            dataGridView1.Columns[2].HeaderText = "Spare Name";
            dataGridView1.Columns[3].HeaderText = "Spare Quantity";
            dataGridView1.Columns[4].HeaderText = "Supplier ID";
            dataGridView2.Columns[0].HeaderText = "Spare ID";
            dataGridView2.Columns[1].HeaderText = "Spare Category";
            dataGridView2.Columns[2].HeaderText = "Spare Name";
            dataGridView2.Columns[3].HeaderText = "Spare Quantity";
            dataGridView2.Columns[4].HeaderText = "Supplier ID";
            dataGridView3.Columns[0].HeaderText = "Spare ID";
            dataGridView3.Columns[1].HeaderText = "Spare Category";
            dataGridView3.Columns[2].HeaderText = "Spare Name";
            dataGridView3.Columns[3].HeaderText = "Spare Quantity";
            dataGridView3.Columns[4].HeaderText = "Supplier ID";



            GetReorderCell();

            EnableCreateButton();

        }


        private void GetReorderCell()
        {
            for (int row = 0; row < dataGridView3.Rows.Count; row++)
            {
                for (int column = 0; column < dataGridView3.Columns.Count; column++)
                {
                    DataGridViewCell cell = dataGridView3.Rows[row].Cells[column];

                    
                        // 获取单元格的值
                    object cellValue = cell.Value;
                    if (cellValue != null && int.TryParse(cellValue.ToString(), out int intValue))
                    {
                        if (intValue <= 400)
                        {
                            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                            cellStyle.BackColor = Color.FromArgb(255, 255, 192);
                            cell.Style = cellStyle;
                        }
                        if (intValue <= 100)
                        {
                            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                            cellStyle.BackColor = Color.FromArgb(255, 192, 128);
                            cell.Style = cellStyle;
                        }
                        if (intValue == 0)
                        {
                            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                            cellStyle.BackColor = Color.FromArgb(255, 128, 128);
                            cell.Style = cellStyle;
                        }
                    }
                }
            }
        }


        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }
        bool IsSpareIDExists(string number)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            string query = "SELECT COUNT(*) FROM spare_part WHERE Spare_ID = @sID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@sID", number);

            int count = Convert.ToInt32(command.ExecuteScalar());

            return count > 0; // 如果 count 大於 0，表示號碼已存在
        }
        string GenerateNumber()
        {
            Random random = new Random();
            string number = random.Next(10000, 99999).ToString();
            return number;
        }
        private string newSpareID()
        {
            string sID;
            bool exists;
            do
            {
                sID = GenerateNumber(); // 生成號碼的方法，可以使用 Guid 或隨機數生成器等方式
                exists = IsSpareIDExists(sID); // 檢查號碼是否已存在
            }
            while (exists);
            return sID;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            goToPages.Logout();
        }

        private void button9_Click(object sender, EventArgs e)
        {
         
            if (!isEditing)
            {
                // 进入编辑模式
                dataGridView1.Show();
                dataGridView3.Hide();
                panel7.Hide();
                dataGridView1.Columns[3].ReadOnly = false;
                dataGridView1.Columns[4].ReadOnly = false;
                button10.Show();

                button9.Text = "Cancel Edit";
                isEditing = true;
                dataGridView1.Columns["Spare_Actual_Quantity"].DefaultCellStyle.BackColor = Color.FromArgb(192, 255, 192); 

            }
            else
            {
                // 取消编辑模式
                dataGridView1.Hide();
                dataGridView3.Show();
                panel7.Show();
                dataGridView1.CancelEdit();
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;
                button10.Hide();

                button9.Text = "Edit";
                isEditing = false;
                dataGridView1.Columns["Spare_Actual_Quantity"].DefaultCellStyle.BackColor = Color.White;

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            // 保存编辑后的数据并禁用编辑模式
            for (int item = 0; item< dataGridView1.Rows.Count -1; item++) {
                string queryForUpdate = "UPDATE spare_part SET  Spare_Actual_Quantity= @Spare_Actual_Quantity WHERE Spare_ID= @Spare_ID";
                MySqlCommand command = new MySqlCommand(queryForUpdate, connection); // 创建 MySqlCommand 对象
                command.Parameters.AddWithValue("@Spare_Actual_Quantity", dataGridView1.Rows[item].Cells[3].Value);
                command.Parameters.AddWithValue("@Spare_ID", dataGridView1.Rows[item].Cells[0].Value);

                command.ExecuteNonQuery();
            }
            MessageBox.Show("Updated successfully!");
            goToPages.GoToStockModify();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 双击单元格时进入编辑模式
            dataGridView1.BeginEdit(true);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string filterText = textBox3.Text;
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = "Spare_ID LIKE '%" + filterText + "%'";
            GetReorderCell();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
            panel5.Show();
            panel7.Show();
            dataGridView3.Show();
            dataGridView1.Hide();
            panel6.Hide();
            dataGridView2.Hide();
            newSpareID();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button16_Click(object sender, EventArgs e)
        {
            panel6.Show();
            panel7.Hide();
            dataGridView2.Show();
            panel5.Hide();
            dataGridView1.Hide();
            dataGridView3.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            goToPages.GoToOrderMenu();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            goToPages.GoToStockModify();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            goToPages.GoToUserFeedback();
        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            goToPages.GoToReport();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (panel2.Visible)
            {
                panel2.Hide();
            }
            else
            {
                panel2.Show();
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string cat = comboBox1.Text;
            string PNum = cat + newSpareID();
            string Pname = textBox2.Text;
            string sID = textBox1.Text;
            string price = textBox4.Text;

            SaveDataToDatabase(PNum, cat,Pname, Convert.ToDecimal(price), sID);
            AddSpareTb();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private List<DataGridViewRow> rowsToDelete = new List<DataGridViewRow>();
        private void button8_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1; // 清除下拉框的选择
            textBox1.Text = string.Empty; // 清空文本框的文本
            textBox2.Text = string.Empty; // 清空文本框的文本
            textBox4.Text = string.Empty; // 清空文本框的文本
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //re order button
        private void button13_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            isShown = true;
            reorderInputForm reorderInputForm = new reorderInputForm();
            reorderInputForm.FormClosed += FormClose;
            reorderInputForm.Show();
        }

        //danger form
        private void button12_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            isShown = true;
            DangerInputForm DangerInputForm = new DangerInputForm();
            DangerInputForm.FormClosed += FormClose;
            DangerInputForm.Show();
        }



        //out of stock form
        private void button11_Click(object sender, EventArgs e)
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

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            goToPages.GoToStockModify();
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            goToPages.OpenCmd();
        }
    }
}
