using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B16_Ex05
{
    public partial class FormGameSettings : Form
    {
        public FormGameSettings()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
        }

        public string PlayerOneName
        { 
            get 
            {
                return textBoxPlayerOne.Text;
            }
        }

        public string PlayerTwoName
        {
            get
            {
                return textBoxPlayerTwo.Text; 
            }
        }

        public int Rows
        {
            get 
            {
                return int.Parse(numericUpDownRows.Value.ToString());
            }
        }

        public int Cols
        {
            get 
            {
                return int.Parse(numericUpDownCols.Value.ToString());
            }
        }

        private void checkBoxPlayerTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPlayerTwo.Checked)
            {
                textBoxPlayerTwo.Text = string.Empty;
                textBoxPlayerTwo.Enabled = true;
            }
            else 
            {
                textBoxPlayerTwo.Text = "[Computer]";
                textBoxPlayerTwo.Enabled = false;
            }
        }

        public bool IsMan
        {
            get 
            {
                return checkBoxPlayerTwo.Checked ? true : false; 
            }
        }
    }
}
