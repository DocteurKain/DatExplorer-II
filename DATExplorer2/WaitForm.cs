using System;
using System.Windows.Forms;
using DATLib;

namespace DATExplorer
{
    public partial class WaitForm : Form
    {
        private string cutPath;
        private string unpackPath;
        private string nameDat;
        private string[] listFiles;

        public WaitForm(string unpackPath, string[] listFiles, string nameDat, string cutPath)
        {
            this.unpackPath = unpackPath + '\\';
            this.listFiles = listFiles;
            this.nameDat = nameDat;
            this.cutPath = cutPath;

            InitializeComponent();
        }

        public void Unpack(Form owner)
        {
            this.ShowDialog(owner);
        }

        private void WaitForm_Shown(object sender, EventArgs e)
        {
            if (System.Globalization.CultureInfo.CurrentCulture.Name == "ru-RU")
                label1.Text = "Подождите идет извлечение файлов...";
            else
                label1.Text = "Please wait extraction of files...";

            if (listFiles != null) {
                if (cutPath != string.Empty)
                    DATManage.UnpackFileList(unpackPath, listFiles, nameDat, cutPath);
                else
                    DATManage.UnpackFileList(unpackPath, listFiles, nameDat);
            } else { 
                DATManage.ExtractAllFiles(unpackPath, nameDat);
            }
            this.Dispose();
        }
    }
}
