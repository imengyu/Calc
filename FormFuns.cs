using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calc
{
    public partial class FormFuns : Form
    {
        public FormFuns()
        {
            InitializeComponent();
        }

        private void FormFuns_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(e.Button== MouseButtons.Left)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    Clipboard.SetText(listView1.SelectedItems[0].Text + "(");
                    MessageBox.Show("函数 " + listView1.SelectedItems[0].Text + " 已复制到剪贴板", "消息");
                }
            }
        }
    }
}
