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
    public partial class SaveForm : Form
    {
        FunctionManager functionManager;
        public SaveForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            var name = txtName.Text;
            var password = txtPassword.Text;
            var url = txtURL.Text;
            var comment = txtComment.Text;
            FunctionManager.SavePassword(name, password, url, comment);
            MessageBox.Show("Slaptažodis sėkmingai išsaugotas.");
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveForm_Load(object sender, EventArgs e)
        {

        }
    }
}
