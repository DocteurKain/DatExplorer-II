using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using DATLib;

namespace DATExplorer
{
    public partial class ExplorerForm : Form
    {
        private string currentDat;
        private string extractFolder;
        private string dropExtractPath;

        List<OpenDat> openDat = new List<OpenDat>();

        List<String> dropSelected;
        FileWatcher dragDropFileWatcher;

        private string arg;

        public ExplorerForm(string[] args)
        {
            if (args.Length > 0) this.arg = args[0];

            InitializeComponent();

            dragDropFileWatcher = new FileWatcher();

            dragDropFileWatcher.DragDrop += new FileWatcher.DropExplorerEvent(DropHandler);
            DATManage.ExtractUpdate += new ExtractEvent(ExtractUpdate);
        }
        
        private void ExplorerForm_Shown(object sender, EventArgs e)
        {
            if (arg != null) {
                OpenDatFile(arg);
            } else { 
                FileAssociation.Associate();
            }
        }

        private void OpenDatFile(string pathDat)
        {
            string message;
            if (!DATManage.OpenDatFile(pathDat, out message)) {
                MessageBox.Show(message, "Open Error");
                return;
            }

            OpenDat dat = new OpenDat(pathDat, DATManage.GetFiles(pathDat));
            openDat.Add(dat);
            BuildTree(dat);

            totalToolStripStatusLabel.Text = dat.TotalFiles.ToString();
        }

        private void ExtractUpdate(ExtractEventArgs e)
        {
            toolStripProgressBar.Value++;
            textToolStripStatusLabel.Text = e.Name;
            Application.DoEvents();
        }

        private void ExtractFiles(string[] listFiles)
        {
            extractFolderBrowser.SelectedPath = extractFolder;
            if (extractFolderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
            extractFolder = extractFolderBrowser.SelectedPath;

            ExtractFiles(listFiles, extractFolder, string.Empty);
        }

        private void ExtractFiles(string[] listFiles, string path, string cutPath)
        {
            statusToolStripStatusLabel.Text = "Extract:";
            toolStripProgressBar.Maximum = (listFiles != null)  ? listFiles.Length
                                         : openDat.Find(x => x.DatName == currentDat).TotalFiles;

            //listFiles.Length

            new WaitForm(path, listFiles, currentDat, cutPath).Unpack(this);

            //if (cutPath != string.Empty)
            //    DATManage.UnpackFileList(path + '\\', listFiles, currentDat, cutPath);
            //else
            //    DATManage.UnpackFileList(path + '\\', listFiles, currentDat);

            textToolStripStatusLabel.Text = "Done.";
            toolStripProgressBar.Value = 0;
        }

        private void BuildTree(OpenDat dat)
        {
            folderTreeView.BeginUpdate();

            TreeNode root = folderTreeView.Nodes.Add(dat.DatName);
            root.SelectedImageIndex = root.ImageIndex = 2;

            foreach (var item in dat.Folders)
            {
                if (item.Key.Length > 0) {
                    string[] dirs = item.Key.Split('\\');
                    TreeNode tn = root;
                    bool parent = true;
                    foreach (var dir in dirs)
                    {
                        TreeNode find = Misc.FindNode(dir, tn);
                        if (find == null) {
                            tn = tn.Nodes.Add(dir);
                            tn.Name = (parent) ? dir : item.Key;
                        } else
                            tn = find;
                        parent = false;
                    }
                }
            }
            root.Expand();
            folderTreeView.EndUpdate();
        }

        private void FindFiles(string datPath, string path)
        {
            OpenDat dat = openDat.Find(x => x.DatName == datPath);
            
            List<String> subDirs = new List<String>();
            int len = path.Length;

            foreach (var item in dat.Folders)
            {
                if (item.Key.StartsWith(path)) {
                    int i = (len > 0) ? item.Key.LastIndexOf('\\') : item.Key.IndexOf('\\');
                    if (i > len) {
                        string sub = item.Key.Substring(len, i - len);
                        if (!subDirs.Contains(sub)) subDirs.Add(sub);
                    } else
                        subDirs.Add(item.Key.Substring(len));
                }
            }

            filesListView.BeginUpdate();
            filesListView.Items.Clear();
            foreach (string dir in subDirs)
            {
                if (dir.Length > 0) {
                    ListViewItem lwItem = new ListViewItem(dir, 0);
                    lwItem.Name = path + dir;
                    lwItem.SubItems.Add("<DIR>");
                    filesListView.Items.Add(lwItem);
                }
            }
            int dirCount = filesListView.Items.Count;
            // add files
            if (len > 0) path = path.Remove(len - 1);
            if (dat.Folders.ContainsKey(path)) {
                var datFolders = dat.Folders[path];
                foreach (sFile el in datFolders.GetFiles())
                {
                    ListViewItem lwItem = new ListViewItem(el.file.name, 1);
                    lwItem.Tag = el;
                    if (filesListView.View == View.Details) {
                        lwItem.SubItems.Add((el.file.info.IsPacked) ? "Packed" : string.Empty);
                        lwItem.SubItems.Add(el.file.info.Size.ToString());
                        lwItem.SubItems.Add(el.file.info.PackedSize.ToString());
                    }
                    filesListView.Items.Add(lwItem);
                }
            }
            filesListView.EndUpdate();

            toolStripStatusLabelEmpty.Text = string.Format("{0} folder(s), and {1} file(s).", dirCount, filesListView.Items.Count - dirCount); 
            totalToolStripStatusLabel.Text = dat.TotalFiles.ToString();
            dirToolStripStatusLabel.Text = "Directory: " + path + '\\';
        }

        private void GetFolderFiles(List<String> listFiles, string folderPath)
        {
            OpenDat dat = openDat.Find(x => x.DatName == currentDat);
            Misc.GetFolderFiles(dat, listFiles, folderPath);
        }

        private void OpenToolStripButton_Click(object sender, EventArgs e)
        {
            openDatFileDialog.ShowDialog();

            string pathDat = openDatFileDialog.FileName;
            if (openDat.Exists(x => x.DatName == pathDat)) return;

            OpenDatFile(pathDat);
        }

        private void folderTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown) return;

            string datPath = Misc.GetRootNode(e.Node).Text;
            currentDat = datPath;
            string path = e.Node.FullPath;
            if (path.Length > datPath.Length)
                path = path.Remove(0, datPath.Length + 1) + '\\';
            else
                path = string.Empty;

            FindFiles(datPath, path);
            extractFolderToolStripMenuItem.Enabled = true;
        }

        #region Menu control  
        private void ListViewStyleCheck(View type) {
            switch (type) {
            case View.LargeIcon:
                largeToolStripMenuItem.Checked = true;
                listToolStripMenuItem.Checked = false;
                detailsToolStripMenuItem.Checked = false;
                break;
            case View.List:
                largeToolStripMenuItem.Checked = false;
                listToolStripMenuItem.Checked = true;
                detailsToolStripMenuItem.Checked = false;
                break;
            case View.Details:
                largeToolStripMenuItem.Checked = false;
                listToolStripMenuItem.Checked = false;
                detailsToolStripMenuItem.Checked = true;
                break;
            }
        }
        
        private void largeToolStripMenuItem_Click(object sender, EventArgs e) {
            filesListView.View = View.LargeIcon;
            ListViewStyleCheck(View.LargeIcon);
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e) {
            filesListView.View = View.Details;
            ListViewStyleCheck(View.Details);
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e) {
            filesListView.View = View.List;
            ListViewStyleCheck(View.List);
        }
        #endregion

        private void filesListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

        }

        private void extractFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = filesListView.SelectedItems;
            List<String> listFiles = new List<String>();
            
            foreach (ListViewItem item in selectedFiles)
            {
                if (item.Tag != null) {
                    listFiles.Add(((sFile)item.Tag).path);
                } else { // selected folder 
                    GetFolderFiles(listFiles, item.Name);
                }
            }
            ExtractFiles(listFiles.ToArray());
        }
        
        private void extractFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<String> listFiles = new List<String>();
            GetFolderFiles(listFiles, folderTreeView.SelectedNode.Name);
            ExtractFiles(listFiles.ToArray());
        }

        private void extractAllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentDat == null) currentDat = Misc.GetRootNode(folderTreeView.SelectedNode).Text;
            ExtractFiles(null);
        }

        private void OpenFile()
        {
            var item = filesListView.SelectedItems[0];
            if (item.Tag != null) {
                // TODO open file 
            } else {
                // folder
                foreach (TreeNode node in folderTreeView.SelectedNode.Nodes)
                {
                    if (node.Text == item.Text) {
                        folderTreeView.SelectedNode = node;
                        break; 
                    }
                }
                FindFiles(currentDat, item.Name + '\\');
            }
        }

        private void filesListView_DoubleClick(object sender, EventArgs e)
        {
            if (!filesListView.CheckBoxes && filesListView.SelectedItems.Count > 0) {
                OpenFile();
            } 
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void closeDatFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderTreeView.SelectedNode.Parent == null) {
                string closeDat = folderTreeView.SelectedNode.Text;
                DATManage.CloseDatFile(closeDat);
                openDat.Remove(openDat.Find(x => x.DatName == closeDat));
                folderTreeView.Nodes.RemoveAt(folderTreeView.SelectedNode.Index);
                filesListView.Items.Clear();
            }
        }

        private void cmsFolderTree_Opening(object sender, CancelEventArgs e)
        {
            closeDatFileToolStripMenuItem.Enabled = ((folderTreeView.Nodes.Count != 0) && (folderTreeView.SelectedNode != null));
            extractAllFilesToolStripMenuItem.Enabled = (folderTreeView.SelectedNode != null);
        }

        private void listViewContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            extractFilesToolStripMenuItem.Enabled = (filesListView.SelectedItems.Count != 0);
            openToolStripMenuItem.Enabled = (filesListView.SelectedItems.Count == 1);
        }

        #region Drag items
        bool dragToExternal = false;
        bool dragAllow = false;
        Point dragPointDown = Point.Empty;

        void DropHandler(FileWatcher.DropEventArgs e)
        {
            dragDropFileWatcher.StopWatcher();
            dropExtractPath = e.PathDrop;
        }

        private void filesListView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void filesListView_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {

        }

        private void filesListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                dragAllow = true;
                dragPointDown = e.Location;
            }
        }

        // Drop from List
        private void filesListView_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragToExternal && dragAllow) {
                Size size = SystemInformation.DragSize;
                size.Height /= 2;
                size.Width  /= 2;
                if (e.Location.X > dragPointDown.X + size.Width  ||
                    e.Location.X < dragPointDown.X - size.Width  ||
                    e.Location.Y > dragPointDown.Y + size.Height ||
                    e.Location.Y < dragPointDown.Y - size.Height)
                {
                    dragToExternal = true;
                    dropExtractPath = string.Empty;

                    dropSelected = new List<String>();
                    foreach (ListViewItem item in filesListView.SelectedItems)
                    {
                        if (item.Tag != null) {
                            dropSelected.Add(((sFile)item.Tag).path);
                        } else { // for selected folder 
                            GetFolderFiles(dropSelected, item.Name);
                        }
                    }

                    String[] dummyDropFile = new String[] { String.Empty };
                    dragDropFileWatcher.StartWatcher(ref dummyDropFile[0]);

                    IDataObject obj = new DataObject(DataFormats.FileDrop, dummyDropFile);
                    DragDropEffects result = filesListView.DoDragDrop(obj, DragDropEffects.Copy);

                    if (dropExtractPath == string.Empty) return;
                    string cutOffPath = folderTreeView.SelectedNode.Name;
                    ExtractFiles(dropSelected.ToArray(), dropExtractPath, cutOffPath);
                }
            }
        }

        private void filesListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (dragToExternal) dragDropFileWatcher.StopWatcher();
            dragAllow = dragToExternal = false;
        }
        
        // Drop to List
        private void filesListView_DragDrop(object sender, DragEventArgs e)
        {
            //if (dragToExternal) return;

            //string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            //foreach (string file in files)
            //{
            //    filesListView.Items.Add(file);
            //}
        }

        private void filesListView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        #endregion

        private void ExplorerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            dragDropFileWatcher.Dispose();
        }
    }
}
