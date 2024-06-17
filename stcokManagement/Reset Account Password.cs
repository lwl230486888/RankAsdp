using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Mysqlx.Connection;
using stcokManagement;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace Reset_Account_Password
{
    public partial class ResetPassword : Form
    {
        //this code for connect databse
        private MySqlConnection databaseConnection;

        public ResetPassword()
        {
            InitializeComponent();

            textBox1.PasswordChar = '•';
            textBox2.PasswordChar = '•';

            label3.MouseEnter += new EventHandler(label3_MouseEnter);
            label3.MouseLeave += new EventHandler(label3_MouseLeave);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Cursor = Cursors.Hand;
            //this 3 code is open database
            string mysqlCon = "server=127.0.0.1; user=root; database=sdpdatabase; password=";
            databaseConnection = new MySqlConnection(mysqlCon);
            databaseConnection.Open();

        }

        //第一個login textbox
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //第二個login textbox
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            GoToLogin();
        }

        public void GoToLogin()
        {
            Login a = new Login();
            a.Show();
            this.Hide();
        }

        //confirm button
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == textBox2.Text)
            {
                // 在此处添加成功后的逻辑
                Update_Password(textBox1.Text);
                textBox1.Clear();
                textBox2.Clear();
            }
            else
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Clear();
                textBox2.Clear();
            }
        }

        private void Update_Password(String password) 
        {
            string userEmail = textBox3.Text;
            string sql = "UPDATE Staff SET Staff_Login_Password = @Password WHERE Email = @Email";

            MySqlCommand cmd = new MySqlCommand(sql, databaseConnection);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Email", userEmail);
            // 执行更新操作
            int rowsAffected = cmd.ExecuteNonQuery();

            // 检查是否成功更新记录
            if (rowsAffected > 0)
            {
                MessageBox.Show("Password updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to update password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //go to login
        private void button3_Click(object sender, EventArgs e)
        {
            GoToLogin();
        }



        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            String userEmail = textBox3.Text;
            String user_code = textBox4.Text ;

            if (get_verification_code(userEmail) == user_code)
            {
                panel6.Hide();
                panel4.Show();
            } else
            {
                MessageBox.Show("Wrong verification code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string get_verification_code(String userEmail)
        {
            {
                string verificationCode = null;

                try
                {
                    // 创建查询语句，使用参数化查询
                    string query = "SELECT Verification_Code FROM Staff WHERE Email = @Email";

                    // 创建 MySqlCommand 对象
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnection);

                    // 添加参数并设置值
                    cmd.Parameters.AddWithValue("@Email", userEmail);

                    // 执行查询操作
                    object result = cmd.ExecuteScalar();

                    // 检查查询结果是否为 null，并将其转换为字符串
                    if (result != null)
                    {
                        verificationCode = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database query error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return verificationCode;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            string userEmail = textBox3.Text;

            if (string.IsNullOrEmpty(userEmail))
            {
                MessageBox.Show("Please fill in email", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ValidateEmail(userEmail))
            {
                try
                {
                    MessageBox.Show("Verification Code is sent to your email", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateVerificationCode(userEmail);
                    SendVerificationEmail(userEmail);
                    textBox3.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to send email: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Wrong email, Please type again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendVerificationEmail(string email)
        {
            var fromAddress = new MailAddress("dobg0305@gmail.com", "Do BG");
            var toAddress = new MailAddress(email);
            const string fromPassword = "nuvi csno ruqa nrqf";
            const string subject = "SDP Verification Code";

            string code = get_verification_code(email);
            string body = "Your verification code is: "+ code; // Replace with your actual verification code logic

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }



        private void UpdateVerificationCode(string userEmail)
        {
            // 生成验证码
            int verificationCode = Gen_Verification_Code();

            // 创建更新语句，使用参数化查询
            string query = "UPDATE Staff SET Verification_Code = @VerificationCode WHERE Email = @userEmail";

            // 创建 MySqlCommand 对象
            MySqlCommand cmd = new MySqlCommand(query, databaseConnection);

            // 添加参数并设置值
            cmd.Parameters.AddWithValue("@VerificationCode", verificationCode);
            cmd.Parameters.AddWithValue("@userEmail", userEmail);

            // 执行更新操作
            cmd.ExecuteNonQuery();
        }
        // 自己整random 6位數字
        private int Gen_Verification_Code()
        {
            Random random = new Random();
            int code = random.Next(100000, 999999);
            return code;
        }

        // 查询数据库中的所有电子邮件
        private DataTable GetAllEmails()
        {
            string query = "SELECT Email FROM Staff";
            DataTable emails = new DataTable();

            try
            {
                using (MySqlCommand command = new MySqlCommand(query, databaseConnection))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(emails);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database query error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return emails;
        }
        // 验证用户输入的电子邮件是否存在于数据库中
        private bool ValidateEmail(string email)
        {
            DataTable emails = GetAllEmails();

            foreach (DataRow row in emails.Rows)
            {
                if (row["Email"].ToString().Equals(email, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        // change get verification code color
        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.ForeColor = Color.LightBlue;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.White;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        //this is email box
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //go to login
        private void button5_Click(object sender, EventArgs e)
        {
            GoToLogin();
        }

        //enter verification code box
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

    }

}
