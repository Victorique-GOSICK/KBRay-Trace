using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ray_Trace
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            KBRandom.Init();
            Display.Image = ImageGen.DrawToBitmap(Display.Width, Display.Height);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            //Display.Image = ImageGen.DrawToBitmap(Display.Width, Display.Height);
        }
    }
}
