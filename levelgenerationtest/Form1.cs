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

            this.numericUpDown1.ValueChanged += button1_Click;
            this.numericUpDown2.ValueChanged += button1_Click;
            this.numericUpDown3.ValueChanged += button1_Click;

            this.button1.Click += button1_Click;
            this.button2.Click += button2_Click;
            this.button3.Click += button3_Click;
            this.button4.Click += button4_Click;
            this.button5.Click += button5_Click;
            this.button6.Click += button6_Click;
            this.button7.Click += button7_Click;

            doorrandomizationtest();
        }

        private void doorrandomizationtest()
        {
            for (int n = 1; n < 7; n++)
            {
                int[] spotcounts = new int[6];

                for (int subn = 0; subn < 40960; subn++)
                {
                    spotcounts[assignDoor(n, 0, 1, 2, 3, 4, 5)]++;
                }

                StringBuilder output = new StringBuilder("depth="+n+" ");

                for (int subn = 0; subn < spotcounts.Length; subn++)
                {
                    if (subn == 0)
                    {
                        output.Append("{");
                    }
                    else
                    {
                        output.Append(",");
                    }

                    output.Append(((double)spotcounts[subn] / 40960.0).ToString("F2"));
                }

                output.Append("}");

                Console.WriteLine(output.ToString());
            }
        }

        private int assignDoor(int depth, int regular, int regularHidden, int regularBroken, int other, int otherHidden, int otherBroken)
        {
            if (rand.Next(3) == 0)
            {
                regular = other;
                regularHidden = otherHidden;
                regularBroken = otherBroken;
            }

            if (depth > 1)
            {
                bool secret = (depth < 6 ? rand.Next(12 - depth) : rand.Next(6)) == 0;

                if (secret)
                {
                    return regularHidden;
                }
            }

            if (rand.Next(3) == 0)
            {
                return regularBroken;
            }
            else
            {
                return regular;
            }
        }

        void button7_Click(object sender, EventArgs e)
        {
            this.textBox6.Text = new TunnelPainter().ToString();
        }

        void button6_Click(object sender, EventArgs e)
        {
            this.textBox5.Text = new RandomPositionsNear().ToString();
        }

        void button5_Click(object sender, EventArgs e)
        {
            minestestercount = 0;

            if (tester == null)
            {
                tester = new Timer();
                tester.Interval = 1;
                tester.Tick += tester_Tick;
            }

            tester.Start();
        }

        private int minestestercount = 0;
        void tester_Tick(object sender, EventArgs e)
        {
            if (mines != null && !mines.fullylinked)
            {
                this.numericUpDown1.Value -= this.numericUpDown1.Increment;
                minestestercount = 0;
            }
            else
            {
                minestestercount++;
                if (minestestercount >= 256)
                {
                    tester.Stop();
                }
            }

            button1_Click(sender, e);
        }

        Timer tester;

        void button4_Click(object sender, EventArgs e)
        {
            this.textBox4.Text = new nethackishlevel().ToString();
        }

        void button3_Click(object sender, EventArgs e)
        {
            this.textBox3.Text = new town().ToString();
        }

        void button2_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = new regularlevel().ToString();
        }

        private mines mines;

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

            mines = new mines(factor, xscale, yscale, randvalue * randfactor);
            this.textBox1.Text = mines.ToString();
        }
    }
}
