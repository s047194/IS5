using System.Formats.Asn1;
using System.Security.Cryptography;
using System.Text;

namespace SlaptazodziuSistema
{
    public partial class Main : Form
    {
        String Path = FunctionManager.Path;
        String Key = FunctionManager.Key;
        public Main()
        {
            InitializeComponent();

            if (File.Exists(Path))
            {
                FunctionManager.DecryptFile(Path, Key);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            FunctionManager.EncryptFile(Path, Key);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveForm saveForm = new SaveForm();
            saveForm.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm();
            searchForm.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateForm updateForm = new UpdateForm();
            updateForm.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteForm deleteForm = new DeleteForm();
            deleteForm.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
