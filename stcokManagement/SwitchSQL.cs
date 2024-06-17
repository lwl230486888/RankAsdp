using MySql.Data.MySqlClient;
using Mysqlx.Cursor;
using System;

namespace SwitchSQL
{
    internal class SwitchSQL
    {
        private static string mysqlCon = "server=127.0.0.1; user=root; database=sdpdatabase; password=";

        private static MySqlConnection databaseConnection = new MySqlConnection(mysqlCon);

        public static string Spare_NameToSpare_ID(string Spare_Name)
        {
            string spareID = "A12345";

            try
            {
                string query = "SELECT Spare_ID FROM Spare_Part WHERE Spare_Name = @Spare_Name";
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                cmd.Parameters.AddWithValue("@Spare_Name", Spare_Name);

                // 打开数据库连接
                databaseConnection.Open();

                // 执行查询并获取结果
                object result = cmd.ExecuteScalar();

                // 将结果转换为字符串（如果不为空）
                if (result != null)
                {
                    spareID = result.ToString();
                    databaseConnection.Close();
                }
            }
            catch (Exception ex)
            {
                // 处理异常，可以记录日志或执行其他操作
                Console.WriteLine("Error: " + ex.Message);
            }

            return spareID;
        }
        public static string Spare_IDToSpare_Name(string Spare_ID)
        {
            string spareName = "Spark Plug";

            try
            {
                string query = "SELECT Spare_Name FROM Spare_Part WHERE Spare_ID = @Spare_ID";
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                cmd.Parameters.AddWithValue("@Spare_ID", Spare_ID);

                // 打开数据库连接
                databaseConnection.Open();

                // 执行查询并获取结果
                object result = cmd.ExecuteScalar();

                // 将结果转换为字符串（如果不为空）
                if (result != null)
                {
                    spareName = result.ToString();
                    databaseConnection.Close();
                }
            }
            catch (Exception ex)
            {
                // 处理异常，可以记录日志或执行其他操作
                Console.WriteLine("Error: " + ex.Message);
            }

            return spareName;
        }


        public static string Dealer_NameToDealer_ID(string Dealer_Name)
        {
            string dealerID = "DEAL001";
            databaseConnection.Close();


            string query = "SELECT Dealer_ID FROM Dealer WHERE Dealer_Name = @Dealer_Name";
            MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
            cmd.Parameters.AddWithValue("@Dealer_Name", Dealer_Name);

            // 打开数据库连接
            databaseConnection.Open();

            // 执行查询并获取结果
            object result = cmd.ExecuteScalar();

            // 将结果转换为字符串（如果不为空）
            if (result != null)
            {
                dealerID = result.ToString();
                databaseConnection.Close();
            }


            return dealerID;
        }

        public static string Dealer_IDToDealer_Name(string Dealer_ID)
        {
            string dealername = "peter";


            string query = "SELECT Dealer_Name FROM Dealer WHERE Dealer_ID = @Dealer_id";
            MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
            cmd.Parameters.AddWithValue("@Dealer_id", Dealer_ID);

            // 打开数据库连接
            databaseConnection.Close();
            databaseConnection.Open();

            // 执行查询并获取结果
            object result = cmd.ExecuteScalar();

            // 将结果转换为字符串（如果不为空）
            if (result != null)
            {
                dealername = result.ToString();
                databaseConnection.Close();
            }


            return dealername;
        }
        //下面呢個mothod 應該有bug，de唔到bug
        public static string Staff_NameToStaff_ID(string Staff_Name)
        {
            string staffID = null; // 初始化为空

            try
            {
                string query = "SELECT Staff_ID FROM Staff WHERE Staff_Name = @Staff_Name";
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                cmd.Parameters.AddWithValue("@Staff_Name", Staff_Name);

                // 打开数据库连接
                databaseConnection.Open();

                // 执行查询并获取结果
                object result = cmd.ExecuteScalar();

                // 将结果转换为字符串（如果不为空）
                if (result != null)
                {
                    staffID = result.ToString();
                }
            }
            catch (Exception ex)
            {
                // 处理异常，可以记录日志或执行其他操作
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {

            }
            return staffID;

        }
    }





}
