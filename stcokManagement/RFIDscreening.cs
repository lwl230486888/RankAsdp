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
    public partial class RFIDscreening : Form
    {
        public RFIDscreening()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        private void RFIDscreening_Load(object sender, EventArgs e)
        {
            DataSet data = new DataSet("Data");

            DataTable table = new DataTable("Stock");
            table.Columns.Add("Part number");
            table.Columns.Add("Category");
            table.Columns.Add("Date & Time");


            table.Rows.Add("A12345", "A", DateTime.Now);
            table.Rows.Add("B78901", "B", DateTime.Now);
            table.Rows.Add("C78901", "B", DateTime.Now);
            table.Rows.Add("D78901", "B", DateTime.Now);
            table.Rows.Add("E78901", "B", DateTime.Now);
            table.Rows.Add("F78901", "B", DateTime.Now);
            table.Rows.Add("G78901", "B", DateTime.Now);
            table.Rows.Add("H78901", "B", DateTime.Now);
            table.Rows.Add("I78901", "B", DateTime.Now);
            table.Rows.Add("J78901", "B", DateTime.Now);

            data.Tables.Add(table);

            comboBox1.Items.Add("ISO 16544 Inventory (ISO 11785 mode)");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
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

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
