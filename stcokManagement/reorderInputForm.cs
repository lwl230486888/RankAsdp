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
    public partial class reorderInputForm : Form
    {
        public reorderInputForm()
        {
            InitializeComponent();
        }
        string GenerateNumber()
        {
            Random random = new Random();
            string number = random.Next(10000000, 99999999).ToString();
            return number;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void reorderInputForm_Load(object sender, EventArgs e)
        {
            label6.Text = GenerateNumber();

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        
            }

        private void label6_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void reorderInputForm_FormClosed_1(object sender, FormClosedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
        
        }
    
