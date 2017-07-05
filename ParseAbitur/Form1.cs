using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParseAbitur
{
    public partial class Form1 : Form
    {
        private string pathFile;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            this.pathFile = folderBrowserDialog1.SelectedPath;
            label1.Text = this.pathFile;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var p = new Parser();
            p.run(this.pathFile);
            p.runParser();
        }
    }
}
