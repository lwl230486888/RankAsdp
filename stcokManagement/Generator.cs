using Azure.Core;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generator
{
    internal class Genrator
    {
        private static MySqlConnection databaseConnection; // 添加数据库连接字段

        // 添加构造函数来初始化数据库连接
        public Genrator(MySqlConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            databaseConnection = connection;
        }

        public static string Gen_DID_ID()
        {
            string mysqlCon = "server=127.0.0.1; user=root; database=sdpdatabase; password=";
            databaseConnection = new MySqlConnection(mysqlCon);
            databaseConnection.Open();
            string did = "DID00001";
            string query = "SELECT MAX(DID_ID) AS MaxDID FROM DID";
            MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
            object result = cmd.ExecuteScalar();
            string maxDID = Convert.ToString(result);

            // 如果数据库中有最大的 DID_ID，则生成下一个
            if (!string.IsNullOrEmpty(maxDID))
            {
                // 获取最大的数字部分并递增
                int maxDIDNumber = int.Parse(maxDID.Substring(3));
                int nextDIDNumber = maxDIDNumber + 1;
                // 生成下一个 DID_ID
                did = $"DID{nextDIDNumber:D5}";
            }

            return did;
        }

        public static string Gen_Order_ID()
        {
            string mysqlCon = "server=127.0.0.1; user=root; database=sdpdatabase; password=";
            databaseConnection = new MySqlConnection(mysqlCon);
            databaseConnection.Open();
            string order = "od00001";
            string query = "SELECT MAX(Your_Order_ID) AS MaxOrder FROM `Order`";
            MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
            object result = cmd.ExecuteScalar();
            string MaxOrder = Convert.ToString(result);

            // 如果数据库中有最大的 DID_ID，则生成下一个
            if (!string.IsNullOrEmpty(MaxOrder))
            {
                // 获取最大的数字部分并递增
                int maxDIDNumber = int.Parse(MaxOrder.Substring(3));
                int nextDIDNumber = maxDIDNumber + 1;
                // 生成下一个 DID_ID
                order = $"od{nextDIDNumber:D5}";
            }

            return order;
        }

        public static string Gen_Dealer_ID()
        {
            string mysqlCon = "server=127.0.0.1; user=root; database=sdpdatabase; password=";
            databaseConnection = new MySqlConnection(mysqlCon);
            databaseConnection.Open();
            string DealerID = "DEAL001";
            string query = "SELECT MAX(Dealer_ID) AS MaxDealerID FROM Dealer";
            MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
            object result = cmd.ExecuteScalar();
            string maxDID = Convert.ToString(result);

            if (!string.IsNullOrEmpty(maxDID))
            {
                // 获取最大的数字部分并递增
                int maxDIDNumber = int.Parse(maxDID.Substring(4));
                int nextDIDNumber = maxDIDNumber + 1;
                DealerID = $"DEAL{nextDIDNumber:D3}";              
                }
            return DealerID;
        }

        public static string Gen_DA_ID()
        {
            string mysqlCon = "server=127.0.0.1; user=root; database=sdpdatabase; password=";
            databaseConnection = new MySqlConnection(mysqlCon);
            databaseConnection.Open();
            string DA = "DA001";
            string query = "SELECT MAX(Dealer_Address_ID) AS MaxDID FROM Dealer_Address";
            MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
            object result = cmd.ExecuteScalar();
            string maxDID = Convert.ToString(result);

            if (!string.IsNullOrEmpty(maxDID))
            {
                int maxDIDNumber = int.Parse(maxDID.Substring(3));
                int nextDIDNumber = maxDIDNumber + 1;
                DA = $"DA{nextDIDNumber:D3}";
            }

            return DA;
        }
    }
}
