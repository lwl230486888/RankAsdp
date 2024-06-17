using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stcokManagement
{
    public partial class SpareAllocation : Form
    {
        public SpareAllocation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
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

        private void SpareAllocation_Load(object sender, EventArgs e)
        {
            DataSet data = new DataSet("Record");

            DataTable table = new DataTable("Table");
            table.Columns.Add("Category");
            table.Columns.Add("Part ID");
            table.Columns.Add("Part description");
            table.Columns.Add("Supplier name");
            table.Columns.Add("Supplier address");
            table.Columns.Add("Supplier contact");

            table.Rows.Add("Electronics", "C12354", "LCD Display", "ABC Electronics", "123 Main Street", "3215 4565");
            table.Rows.Add("Mechanical", "D98456", "Bearing", "XYZ Supplies", "456 Elm Avenue", "3214 5546");
            table.Rows.Add("Electronics", "B32156", "LED Panel", "PQR Electronics", "789 Oak Road", "2765 4594");
            table.Rows.Add("Mechanical", "A22586", "Screwdriver", "LMN Tools", "789 Pine Lane", "2148 4758");

            data.Tables.Add(table);

            dataGridView1.DataSource = data.Tables["Table"];
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
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

        private void panel2_Paint(object sender, PaintEventArgs e)
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
    }
}
