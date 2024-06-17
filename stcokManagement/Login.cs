using System;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Order_Managment_Menu_2;
using Reset_Account_Password;
using goToPage;
using modifyStockLevel;

namespace stcokManagement
{
    public partial class Login : Form
    {
        GoToPageFunction goToPages;
        public Login()
        {
            InitializeComponent();

            goToPages = new GoToPageFunction(this);
            
            //this two command 係改mouse 指落去reset password 個陣
            label4.MouseEnter += new EventHandler(label4_MouseEnter);
            label4.MouseLeave += new EventHandler(label4_MouseLeave);

            panel1.Hide();
            panel2.Hide();
            panel4.Hide();


        }


        private void Form1_Load(object sender, EventArgs e)
        {
            panel2.Hide();
            panel4.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel4.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public static string StaffID { get; set; }
        // login button;
        private void button3_Click(object sender, EventArgs e)
        {
            string mysqlCon = "server=127.0.0.1; user= root; database= sdpdatabase; password=";

            MySqlConnection databaseConnection = new MySqlConnection(mysqlCon);


            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (databaseConnection.State != ConnectionState.Open)
                {
                    try
                    {
                        databaseConnection.Open();

                        String querry = "SELECT * FROM staff WHERE Staff_ID = @id AND Staff_Login_Password = @pw";
                        MySqlCommand commmandDatabase = new MySqlCommand(querry, databaseConnection);
                        {
                            commmandDatabase.Parameters.AddWithValue("@id", textBox1.Text);
                            commmandDatabase.Parameters.AddWithValue("@pw", textBox2.Text);

                            MySqlDataReader myReader = commmandDatabase.ExecuteReader();

                            if (myReader.Read())
                            {
                                string positionID = myReader["Position_ID"].ToString();
                                StaffID = textBox1.Text;
                                MessageBox.Show("Logged In successfully ", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // 返回員工ID
                                if (positionID.Equals("P01"))
                                {
                                    goToPages.GoToOrderMenu();
                                }
                                else if (positionID.Equals("P02"))
                                {
                                    goToPages.GoToStockModify();
                                }
                                else if (positionID.Equals("P03"))
                                {
                                    goToPages.GoToReport();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Incorrect Username/Password", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Connecting: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        databaseConnection.Close();
                    }
                }
            }
        }

        


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void pictureBox1_Click_1(object sender, EventArgs e)
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //this two can clear the textbox
            textBox1.Clear();
            textBox2.Clear();
        }



        private void GoToResetPassword()
        {
            ResetPassword orderMenu = new ResetPassword();
            orderMenu.Show();
            this.Hide();
        }

        private void label4_Click_1(object sender, EventArgs e)
        {
            //click to reset password
            GoToResetPassword();
        }


        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.ForeColor = Color.LightBlue;
            this.Cursor = Cursors.Hand;
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.ForeColor = Color.White;
            this.Cursor = Cursors.Default;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Hide();
            panel1.Show();
            panel4.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
       public string getID()
        {
            return StaffID;
        }
    }
}
