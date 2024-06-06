using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlaptazodziuSistema
{
    public partial class SearchForm : Form
    {
        public SearchForm()
        {
            InitializeComponent();
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            txtPassword.Visible = false;
            txtURL.Visible = false;
            txtComment.Visible = false;
            txtComment.Enabled = false;
            txtURL.Enabled = false;
            txtPassword.Enabled = false;
            Size = new Size(270, 251);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var name = txtName.Text;
            var entry = FunctionManager.SearchPassword(name);
            if (entry != null)
            {
                txtPassword.Text = FunctionManager.DecryptString(entry.Split(':')[1], FunctionManager.Key);
                txtURL.Text = entry.Split(':')[2];
                txtComment.Text = entry.Split(':')[3];
                MessageBox.Show("Slaptažodis yra sąraše.");
                Size = new Size(270, 468);
                txtPassword.Visible = true;
                txtURL.Visible = true;
                txtComment.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
            }
            else
            {
                MessageBox.Show("Slaptažodžio sąraše nėra.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
