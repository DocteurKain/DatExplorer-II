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

        enum WorkState {
            Extract,
            ExtractSingle,
            Delete,
            Save,
            SaveAppend
        }

        private WorkState state = WorkState.Extract;

        ExplorerForm ownerFrm;
        ListView.SelectedListViewItemCollection list;

        public WaitForm(Form owner)
        {
            this.ownerFrm = (ExplorerForm)owner;

            InitializeComponent();
        }

        public void Unpack(string unpackPath, string[] listFiles, string nameDat, string cutPath)
        {
            this.state = WorkState.Extract;

            this.unpackPath = unpackPath + '\\';
            this.listFiles = listFiles;
            this.nameDat = nameDat;
            this.cutPath = cutPath;

            this.ShowDialog(ownerFrm);
        }

        public void UnpackFile(string unpackPath, string[] listFiles, string nameDat)
        {
            this.state = WorkState.ExtractSingle;

            this.unpackPath = unpackPath + '\\';
            this.listFiles = listFiles;
            this.nameDat = nameDat;

            this.ShowDialog(ownerFrm);
        }

        public void RemoveFile(string file)
        {
            this.state = WorkState.Delete;
            this.listFiles = new string[1] { file };

            this.ShowDialog(ownerFrm);
        }

        public void RemoveFile(ListView.SelectedListViewItemCollection list)
        {
            this.state = WorkState.Delete;
            this.list = list;

            this.ShowDialog(ownerFrm);
        }

        public void SaveDat(string nameDat, bool isAppend)
        {
            this.state = (isAppend) ? WorkState.SaveAppend : WorkState.Save;
            this.nameDat = nameDat;

            this.ShowDialog(ownerFrm);
        }

        private void WaitForm_Shown(object sender, EventArgs e)
        {
            switch (this.state)
            {
                case WorkState.Extract:
                case WorkState.ExtractSingle:
                    Extraction();
                    break;
                case WorkState.Delete:
                    Remove();
                    break;
                case WorkState.Save:
                    Saving();
                    break;
            }
            this.Dispose();
        }

        private void Extraction()
        {
            if (System.Globalization.CultureInfo.CurrentCulture.Name == "ru-RU")
                label1.Text = "Подождите идет извлечение файлов...";
            else
                label1.Text = "Please wait extraction of files...";

            if (this.state == WorkState.ExtractSingle) {
                DATManage.ExtractFile(unpackPath, listFiles[0], nameDat);
            }
            else if (listFiles != null) {
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

        private void Saving()
        {
            if (System.Globalization.CultureInfo.CurrentCulture.Name == "ru-RU")
                label1.Text = "Подождите идет сохранение Dat файла...";
            else
                label1.Text = "Wait for the files to be deleted...";

            Application.DoEvents();

            if (this.state == WorkState.Save)
                DATManage.SaveDAT(nameDat);
            else
                DATManage.AppendFilesDAT(nameDat);
        }
    }
}
