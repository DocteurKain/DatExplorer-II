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

        private bool singleFile = false;
        private bool removeFile = false;

        ExplorerForm ownerFrm;
        ListView.SelectedListViewItemCollection list;

        public WaitForm(string path)
        {
            this.listFiles = new string[1] { path };

            InitializeComponent();
        }

        public WaitForm(ListView.SelectedListViewItemCollection list)
        {
            this.list = list;

            InitializeComponent();
        }

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

        public void UnpackFile(Form owner)
        {
            this.singleFile = true;
            this.ShowDialog(owner);
        }

        public void RemoveFile(Form owner)
        {
            this.removeFile = true;
            this.ownerFrm = (ExplorerForm)owner;
            this.ShowDialog(owner);
        }

        private void WaitForm_Shown(object sender, EventArgs e)
        {
            if (removeFile)
                Remove();
            else
                Extraction();
            this.Dispose();
        }

        private void Extraction()
        {
            if (System.Globalization.CultureInfo.CurrentCulture.Name == "ru-RU")
                label1.Text = "Подождите идет извлечение файлов...";
            else
                label1.Text = "Please wait extraction of files...";

            if (this.singleFile) {
                DATManage.ExtractFile(unpackPath, listFiles[0], nameDat);
            } else if (listFiles != null) {
                if (cutPath != string.Empty)
                    DATManage.ExtractFileList(unpackPath, listFiles, nameDat, cutPath);
                else
                    DATManage.ExtractFileList(unpackPath, listFiles, nameDat);
            } else {
                DATManage.ExtractAllFiles(unpackPath, nameDat);
            }
        }

        private void Remove()
        {
            if (System.Globalization.CultureInfo.CurrentCulture.Name == "ru-RU")
                label1.Text = "Подождите идет удаление файлов...";
            else
                label1.Text = "Wait for the files to be deleted...";

            Application.DoEvents();

            if (listFiles != null)
                ownerFrm.DeleteFiles(listFiles[0]);
            else
                ownerFrm.DeleteFiles(list);
        }
    }
}
