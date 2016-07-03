using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace levelgenerationtest
{
    public partial class Form1 : Form
    {
        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();

            this.button1.Click += button1_Click;
            this.numericUpDown1.ValueChanged += button1_Click;
            this.numericUpDown2.ValueChanged += button1_Click;
            this.numericUpDown3.ValueChanged += button1_Click;

            this.button2.Click += button2_Click;

            this.button3.Click += button3_Click;
        }

        void button3_Click(object sender, EventArgs e)
        {
            this.textBox3.Text = new town().ToString();
        }

        void button2_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = new regularlevel().ToString();
        }

        void button1_Click(object sender, EventArgs e)
        {
            double factor = (double)this.numericUpDown1.Value;
            double xscale = (double)this.numericUpDown2.Value;
            double yscale = (double)this.numericUpDown3.Value;

            if (this.checkBox1.Checked)
            {
                this.numericUpDown4.Value = (decimal)rand.NextDouble();
            }

            double randvalue = (double)this.numericUpDown4.Value;
            double randfactor = (double)this.numericUpDown5.Value;

            this.textBox1.Text = new mines(factor, xscale, yscale, randvalue * randfactor).ToString();
        }
    }
}
