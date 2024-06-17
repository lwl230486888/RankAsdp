using Generator;
using goToPage;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Dealer_Information
{
    public partial class Dealer_Info : Form
    {
        MySqlConnection databaseConnection;


        private GoToPageFunction goToPages;
        public Dealer_Info()
        {
            InitializeComponent();
            goToPages = new GoToPageFunction(this);
            Database();
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            textBox1.TextChanged += Control_TextChanged;
            textBox2.TextChanged += Control_TextChanged;
            textBox3.TextChanged += Control_TextChanged;
            textBox4.TextChanged += Control_TextChanged;
            textBox5.TextChanged += Control_TextChanged;
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
                        myReader.Close();
                    }
                    string sql = "SELECT d.Dealer_ID, d.Dealer_Name, dp.Dealer_Company_Phone_Number, dp.Dealer_Contact_Phone_Number, da.Dealer_Company_Address, da.Dealer_Delivery_Address FROM Dealer d JOIN Dealer_Phone_Number dp ON d.Dealer_Company_Phone_Number = dp.Dealer_Company_Phone_Number JOIN Dealer_Address da ON d.Dealer_Address_ID = da.Dealer_Address_ID; ";
                    commandDatabase = new MySqlCommand(sql, databaseConnection);
                    myReader = commandDatabase.ExecuteReader();

                    // Create a new DataTable and load the data into it
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Dealer(ID)", typeof(string));
                    dataTable.Columns.Add("Dealer", typeof(string));
                    dataTable.Columns.Add("Phone(Company)", typeof(string));
                    dataTable.Columns.Add("Phone(Contact)", typeof(string));
                    dataTable.Columns.Add("Address(Company)", typeof(string));

                    while (myReader.Read())
                    {
                        DataRow row = dataTable.NewRow();
                        row["Dealer(ID)"] = myReader["Dealer_ID"].ToString();
                        row["Dealer"] = myReader["Dealer_Name"].ToString();
                        row["Phone(Company)"] = myReader["Dealer_Company_Phone_Number"].ToString();
                        row["Phone(Contact)"] = myReader["Dealer_Contact_Phone_Number"].ToString();
                        row["Address(Company)"] = myReader["Dealer_Company_Address"].ToString();
                        dataTable.Rows.Add(row);
                    }
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Columns["Dealer(ID)"].ReadOnly = true;
                    dataGridView1.Columns["Dealer(ID)"].DefaultCellStyle.BackColor = Color.LightGray;
                    myReader.Close();
                }
            }
        }

        private void Control_TextChanged(object sender, EventArgs e)
        {
            EnableCreateButton();
        }
        //呢個係如果所有資料齊曬先比create order
        private void EnableCreateButton()
        {
            if (textBox3.Text.Length > 0 && textBox2.Text.Length > 0 && textBox5.Text.Length > 0 && textBox1.Text.Length > 0 && textBox4.Text.Length > 0)
            {
                button6.Enabled = true;
                button6.BackColor = System.Drawing.Color.White;
            }
            else
            {
                button6.Enabled = false;
                button6.BackColor = System.Drawing.Color.Gray;
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            button6.BackColor = Color.Gray;
            button6.Enabled = false;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void button3_Click(object sender, EventArgs e)
        {
            goToPages.GoToStockModify();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            goToPages.GoToReport();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MySqlTransaction transaction = databaseConnection.BeginTransaction();

                // Generate the Dealer_ID , Dealer_Address_ID
                string Dealer_ID = Genrator.Gen_Dealer_ID();
                string Address_ID = Genrator.Gen_DA_ID();

                // Insert into Dealer_Address table
                string query = "INSERT INTO Dealer_Address (Dealer_Delivery_Address, Dealer_Company_Address, Dealer_Address_ID) VALUES (@Daddress, @address, @Address_ID)";
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                commandDatabase.Transaction = transaction;
                commandDatabase.Parameters.AddWithValue("@Daddress", textBox5.Text);
                commandDatabase.Parameters.AddWithValue("@address", textBox2.Text);
                commandDatabase.Parameters.AddWithValue("@Address_ID", Address_ID);
                commandDatabase.ExecuteNonQuery();

                // Insert into Dealer_Phone_Number table
                query = "INSERT INTO Dealer_Phone_Number (Dealer_Contact_Phone_Number, Dealer_Company_Phone_Number) VALUES (@phone, @COMphone)";
                commandDatabase = new MySqlCommand(query, databaseConnection);
                commandDatabase.Transaction = transaction;
                commandDatabase.Parameters.AddWithValue("@phone", textBox1.Text);
                commandDatabase.Parameters.AddWithValue("@COMphone", textBox4.Text);
                commandDatabase.ExecuteNonQuery();

                // Insert into Dealer table
                query = "INSERT INTO Dealer (Dealer_ID, Dealer_Name, Dealer_Address_ID, Dealer_Company_Phone_Number) VALUES (@Dealer_ID, @name, @Address_ID, @COMphone)";
                commandDatabase = new MySqlCommand(query, databaseConnection);
                commandDatabase.Transaction = transaction;
                commandDatabase.Parameters.AddWithValue("@Address_ID", Address_ID);
                commandDatabase.Parameters.AddWithValue("@Dealer_ID", Dealer_ID);
                commandDatabase.Parameters.AddWithValue("@name", textBox3.Text);
                commandDatabase.Parameters.AddWithValue("@COMphone", textBox4.Text);
                commandDatabase.ExecuteNonQuery();

                transaction.Commit();
            goToPages.GoToDealerInfo();
        }


        private void label8_Click(object sender, EventArgs e)
        {
            goToPages.GoToOrderMenu();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the updated value from the DataGridView
                string updatedValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                // Get the Dealer ID from the DataGridView
                string dealerID = dataGridView1.Rows[e.RowIndex].Cells["Dealer(ID)"].Value.ToString();

                // Update the corresponding field in the database
                UpdateDealerData(e.ColumnIndex, updatedValue, dealerID);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Refresh the DataGridView to ensure the changes are displayed
            Database();
        }

        private void UpdateDealerData(int columnIndex, string updatedValue, string dealerID)
        {
            using (MySqlTransaction transaction = databaseConnection.BeginTransaction())
            {
                try
                {
                    // Determine which field to update based on the column index
                    string fieldToUpdate = "";
                    switch (columnIndex)
                    {
                        case 1:
                            fieldToUpdate = "Dealer_Name";
                            break;
                        case 2:
                            fieldToUpdate = "Dealer_Company_Phone_Number";
                            break;
                        case 3:
                            fieldToUpdate = "Dealer_Contact_Phone_Number";
                            break;
                        case 4:
                            fieldToUpdate = "Dealer_Company_Address";
                            break;
                        case 5:
                            fieldToUpdate = "Dealer_Delivery_Address";
                            break;
                    }

                    // Update the corresponding field in the database
                    string query = $"UPDATE Dealer d " +
                                   $"JOIN Dealer_Phone_Number dp ON d.Dealer_Company_Phone_Number = dp.Dealer_Company_Phone_Number " +
                                   $"JOIN Dealer_Address da ON d.Dealer_Address_ID = da.Dealer_Address_ID " +
                                   $"SET {fieldToUpdate} = @updatedValue " +
                                   $"WHERE d.Dealer_ID = @dealerID";

                    MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                    commandDatabase.Transaction = transaction;
                    commandDatabase.Parameters.AddWithValue("@updatedValue", updatedValue);
                    commandDatabase.Parameters.AddWithValue("@dealerID", dealerID);
                    commandDatabase.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error updating data: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            goToPages.GoToOrderMenu();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            goToPages.GoToUserFeedback();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            goToPages.Logout();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
