﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using DATExplorer.Properties;

using DATLib;
using System.IO;

namespace DATExplorer
{
    public partial class ExplorerForm : Form
    {
        private string currentDat;
        private TreeNode currentNode;

        private string extractFolder;
        private string dropExtractPath;
        private uint successExtracted;

        private FileWatcher dragDropFileWatcher;

        private string arg;

        public static void SetDoubleBuffered(Control cnt)
        {
            typeof (Control).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null, cnt, new object[] {true});
        }

        public ExplorerForm(string[] args)
        {
            if (args.Length > 0) this.arg = args[0];

            InitializeComponent();

            this.Text += Application.ProductVersion;

            SetDoubleBuffered(folderTreeView);
            SetDoubleBuffered(filesListView);

            dragDropFileWatcher = new FileWatcher();

            dragDropFileWatcher.DragDrop += new FileWatcher.DropExplorerEvent(DropHandler);
            DATManage.ExtractUpdate += new ExtractEvent(ExtractUpdateEvent);
            DATManage.RemoveFile += RemoveFileEvent;
        }

        private void ExplorerForm_Shown(object sender, EventArgs e)
        {
            if (arg != null) {
                OpenDatFile(arg);
            } else if (Settings.Default.IsAssoc == 0) {
                FileAssociation.Associate();
                Settings.Default.IsAssoc = 2;
            }
        }

        private void ExplorerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            dragDropFileWatcher.Dispose();
            Settings.Default.Save();
        }

        private void OpenDatFile(string pathDat)
        {
            string message;
            if (!DATManage.OpenDatFile(pathDat, out message)) {
                MessageBox.Show(message, "Open Error");
                return;
            }

            OpenDat dat = ControlDat.OpeningDat(pathDat);
            if (currentDat == null) currentDat = dat.DatName;

            BuildTree(dat);

            if (currentNode != null) currentNode.ForeColor = Color.White;
            TreeNode[] node = folderTreeView.Nodes.Find(dat.DatName, false);
            SelectTreeNode(node[0]);
            folderTreeView.SelectedNode = node[0];

            totalToolStripStatusLabel.Text = dat.TotalFiles.ToString();
        }

        private void OpenDat(string pathDat)
        {
            if (ControlDat.DatIsOpen(pathDat)) {
                MessageBox.Show("DAT файл с таким именем уже открыт!");
                return;
            }
            OpenDatFile(pathDat);
        }

        private void GetFolderFiles(List<String> listFiles, string folderPath)
        {
            OpenDat dat = ControlDat.GetDat(currentDat);
            dat.GetFilesFromFolder(listFiles, folderPath);
        }

        private void ExtractUpdateEvent(ExtractEventArgs e)
        {
            if (e.Result) successExtracted++;
            toolStripProgressBar.Value++;
            textToolStripStatusLabel.Text = e.Name;
            Application.DoEvents();
        }

        void RemoveFileEvent(DAT.RemoveEventArgs e)
        {
            toolStripProgressBar.Value++;
            textToolStripStatusLabel.Text = e.File;
            Application.DoEvents();
        }

        /// <summary>
        /// Распаковывает список файлов с сохранением структуры каталогов
        /// </summary>
        /// <param name="listFiles"></param>
        private void ExtractFiles(string[] listFiles)
        {
            extractFolderBrowser.SelectedPath = extractFolder;
            if (extractFolderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
            extractFolder = extractFolderBrowser.SelectedPath;

            ExtractFiles(listFiles, extractFolder, string.Empty);
        }

        /// <summary>
        /// Распаковывает список файлов с
        /// </summary>
        /// <param name="listFiles"></param>
        /// <param name="extractToPath"></param>
        /// <param name="cutPath"></param>
        private void ExtractFiles(string[] listFiles, string extractToPath, string cutPath)
        {
            successExtracted = 0;
            statusToolStripStatusLabel.Text = "Extracted:";
            toolStripProgressBar.Maximum = (listFiles != null)  ? listFiles.Length
                                         : ControlDat.GetDat(currentDat).TotalFiles;

            new WaitForm(extractToPath, listFiles, currentDat, cutPath).Unpack(this);

            textToolStripStatusLabel.Text = successExtracted + " of " + toolStripProgressBar.Maximum + " files.";
            toolStripProgressBar.Value = 0;
        }

        private void ExtractFolder(string fullPath, string extractToPath, string folder)
        {
            int cut = fullPath.LastIndexOf("\\" + folder);

            List<String> listFiles = new List<String>();
            GetFolderFiles(listFiles, fullPath);

            ExtractFiles(listFiles.ToArray(), extractToPath, ((cut > 0) ? fullPath.Remove(cut) : String.Empty));
        }

        private void BuildTree(OpenDat dat)
        {
            folderTreeView.BeginUpdate();

            TreeNode root = folderTreeView.Nodes.Add(dat.DatName, string.Format("[F{0}] ", dat.IsFO2Type() ? 2 : 1) + dat.DatName);
            root.NodeFont = new Font(folderTreeView.Font, FontStyle.Bold);
            root.SelectedImageIndex = root.ImageIndex = 2;

            foreach (var item in dat.Folders)
            {
                if (item.Key.Length > 0) {
                    string[] dirs = item.Key.Split('\\');
                    TreeNode tn = root;
                    string parentsDir = String.Empty;;
                    foreach (var dir in dirs)
                    {
                        parentsDir += dir;
                        TreeNode find = Misc.FindNode(dir, tn);
                        if (find == null) {
                            tn = tn.Nodes.Add(dir);
                            tn.Name = parentsDir;
                        } else
                            tn = find;

                        parentsDir += "\\";
                    }
                }
            }
            root.Expand();
            folderTreeView.EndUpdate();
        }

        private void FindFiles(string datPath, string path)
        {
            OpenDat dat = ControlDat.GetDat(datPath);

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
                    ListViewItem lwItem = new ListViewItem(el.file.name, (el.file.info.PackedSize == -1) ? 2 : 1);
                    lwItem.Tag = el;
                    if (filesListView.View == View.Details) {
                        lwItem.SubItems.Add((el.file.info.IsPacked) ? "Packed" : (el.file.info.PackedSize == -1)  ? "Virtual" : string.Empty);
                        lwItem.SubItems.Add(el.file.info.Size.ToString());
                        lwItem.SubItems.Add((el.file.info.PackedSize != -1) ? el.file.info.PackedSize.ToString() : "N/A");
                    }
                    filesListView.Items.Add(lwItem);
                }
            }
            filesListView.EndUpdate();

            toolStripStatusLabelEmpty.Text = string.Format("{0} folder(s), {1} file(s).", dirCount, filesListView.Items.Count - dirCount);
            totalToolStripStatusLabel.Text = dat.TotalFiles.ToString();
            dirToolStripStatusLabel.Text = "Directory: " + path + '\\';
        }

        private void OpenToolStripButton_Click(object sender, EventArgs e)
        {
            if (openDatFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;

            string pathDat = openDatFileDialog.FileName;
            if (pathDat != string.Empty) OpenDat(pathDat);
        }

        private string GetCurrentTreeFolder()
        {
            string path = Misc.GetNodeFullPath(folderTreeView.SelectedNode);
            if (path.Length > currentDat.Length)
                path = path.Remove(0, currentDat.Length + 1);
            else
                path = string.Empty;
            return path;
        }

        private void folderTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if (e.Action == TreeViewAction.Unknown) return;
            SelectTreeNode(e.Node);
        }

        private void SelectTreeNode(TreeNode node)
        {
            upToolStripButton.Enabled = (node.Parent != null);

            currentNode = node;
            node.ForeColor = Color.Yellow;

            string datPath = Misc.GetDatName(node);

            if (currentDat != datPath) {
                SaveToolStripButton.Enabled = ControlDat.GetDat(datPath).ShouldSave();
                SaveToolStripButton.ToolTipText = "Save: " + datPath;
            }
            currentDat = datPath;
            string path = Misc.GetNodeFullPath(node);
            if (path.Length > datPath.Length)
                path = path.Remove(0, datPath.Length + 1) + '\\';
            else
                path = string.Empty;

            FindFiles(datPath, path);

            extractFolderToolStripMenuItem.Enabled = true;
            closeToolStripButton.Enabled = true;
        }

        #region Menu control
        private void ListViewStyleCheck(View type)
        {
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

                if (folderTreeView.SelectedNode != null)
                    FindFiles(currentDat, folderTreeView.SelectedNode.Name + "\\");
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
            if (item.Tag != null) { // open file
                if (((sFile)item.Tag).isVirtual) return;

                var file = new string[1] { ((sFile)item.Tag).path };
                new WaitForm(Application.StartupPath + "\\tmp\\", file, currentDat, null).UnpackFile(this);

                System.Diagnostics.Process.Start("explorer", Application.StartupPath + "\\tmp\\" + file[0]);
            } else { // folder
                foreach (TreeNode node in folderTreeView.SelectedNode.Nodes)
                {
                    if (node.Text == item.Text) {
                        folderTreeView.SelectedNode = node;
                        break;
                    }
                }

                if (currentNode != null) currentNode.ForeColor = Color.White;
                currentNode = folderTreeView.SelectedNode;
                currentNode.ForeColor = Color.Yellow;

                FindFiles(currentDat, item.Name + '\\');

                upToolStripButton.Enabled = (currentNode.Parent != null);
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

        private void closeToolStripButton_Click(object sender, EventArgs e)
        {
            if (currentDat != null && folderTreeView.SelectedNode != null &&
                MessageBox.Show(this, String.Format("Вы действително хотите закрыть\n{0} файл?", currentDat), "Dat Explorer II", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                ControlDat.CloseDat(currentDat);
                filesListView.Items.Clear();
                folderTreeView.Nodes.RemoveAt(Misc.GetRootNode(folderTreeView.SelectedNode).Index);

                closeToolStripButton.Enabled = (folderTreeView.Nodes.Count > 0);
            }
        }

        private void cmsFolderTree_Opening(object sender, CancelEventArgs e)
        {
            bool state = (folderTreeView.SelectedNode != null && folderTreeView.SelectedNode.Parent == null);
            extractAllFilesToolStripMenuItem.Enabled = state;
            extractFolderToolStripMenuItem.Enabled = !state && folderTreeView.SelectedNode != null;

            createFolderToolStripMenuItem.Enabled = (folderTreeView.Nodes.Count != 0 && currentNode != null);
            renameFolderToolStripMenuItem.Enabled = (folderTreeView.SelectedNode != null && folderTreeView.SelectedNode.Parent != null);
            deleteFolderToolStripMenuItem.Enabled = renameFolderToolStripMenuItem.Enabled;

            addFoldersToolStripMenuItem.Enabled = createFolderToolStripMenuItem.Enabled;

            currentDat = (folderTreeView.SelectedNode != null) ? Misc.GetDatName(folderTreeView.SelectedNode) : String.Empty;
        }

        private void listViewContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            extractFilesToolStripMenuItem.Enabled = (filesListView.SelectedItems.Count != 0);
            openToolStripMenuItem.Enabled = (filesListView.SelectedItems.Count == 1);
        }

        #region Drag list items

        bool dragToExternal = false;
        bool dragListActive = false;
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
                dragListActive = true;
                dragPointDown = e.Location;
            }
        }

        // Drop from List
        private void filesListView_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragToExternal && dragListActive) {
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

                    List<String> dropSelected = new List<String>();
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

                    if (dragToExternal) dragDropFileWatcher.StopWatcher();
                    dragListActive = dragToExternal = false;

                    if (dropExtractPath == string.Empty) return;

                    string fullPath = folderTreeView.SelectedNode.Name;
                    string dir = folderTreeView.SelectedNode.Text;
                    int cut = fullPath.LastIndexOf(dir + "\\");
                    if (cut != -1) cut += dir.Length;

                    ExtractFiles(dropSelected.ToArray(), dropExtractPath, ((cut > 0) ? fullPath.Remove(cut) : fullPath));
                }
            }
        }

        private void filesListView_MouseUp(object sender, MouseEventArgs e)
        {
            //if (dragToExternal) dragDropFileWatcher.StopWatcher();
            //dragAllow = dragToExternal = false;
        }

        // Drop to List
        List<string> addFilesToDat;

        private void filesListView_DragDrop(object sender, DragEventArgs e)
        {
            if (dragToExternal) return;

            addFilesToDat = new List<string>();

            foreach (string file in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                if (Directory.Exists(file))
                    addFilesToDat.AddRange(Directory.GetFiles(file, "*.*", SearchOption.AllDirectories));
                else
                    addFilesToDat.Add(file);
            }
        }

        private void filesListView_DragEnter(object sender, DragEventArgs e)
        {
            if (treeDragActive)
                e.Effect = DragDropEffects.None;
            else
                e.Effect = DragDropEffects.Copy;
        }
        #endregion

        #region Create DAT / Add files

        private void CreateNewToolStripButton_Click(object sender, EventArgs e)
        {
            if (CreateNewDatDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
            string newDat = CreateNewDatDialog.FileName;
            if (newDat == string.Empty) return;
            if (ControlDat.DatIsOpen(newDat)) {
                MessageBox.Show("Данный DAT файл уже открыт.");
                return;
            }

            OpenDat dat = ControlDat.OpeningDat(newDat, true); // empty
            BuildTree(dat);
            totalToolStripStatusLabel.Text = dat.TotalFiles.ToString();
        }

        private void importFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (importFilesDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;

            OpenDat dat = ControlDat.GetDat(currentDat);
            string treeFolder = GetCurrentTreeFolder();
            foreach (var file in importFilesDialog.FileNames)
            {
                dat.AddVirtualFile(file, treeFolder);
            }
            SaveToolStripButton.Enabled = true;
            // обновление списка
            FindFiles(currentDat, treeFolder + "\\");
        }

        bool createFolder = false;

        private void createFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode addNode = new TreeNode("New Folder");

            currentNode.Nodes.Add(addNode);

            currentNode.Expand();
            folderTreeView.SelectedNode = addNode;

            createFolder = true;

            SelectTreeNode(addNode);

            folderTreeView.LabelEdit = true;
            folderTreeView.SelectedNode.BeginEdit();
        }

        #endregion

        #region Drag Tree nodes
        bool treeDragActive = false;

        private void folderTreeView_DragEnter(object sender, DragEventArgs e)
        {
            if (dragListActive)
                e.Effect = DragDropEffects.None;
            else {
                if (!treeDragActive) {
                    var drop = (string[])e.Data.GetData(DataFormats.FileDrop);
                    e.Effect = (drop[0].EndsWith(".dat", StringComparison.OrdinalIgnoreCase)) ? DragDropEffects.Copy : DragDropEffects.None;
                } else {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void folderTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ((TreeNode)e.Item).Parent != null)
            {
                currentDat = Misc.GetDatName((TreeNode)e.Item);

                treeDragActive = true;
                dropExtractPath = string.Empty;

                String[] dummyDropFile = new String[] { String.Empty };
                dragDropFileWatcher.StartWatcher(ref dummyDropFile[0]);

                IDataObject obj = new DataObject(DataFormats.FileDrop, dummyDropFile);
                DragDropEffects result = folderTreeView.DoDragDrop(obj, DragDropEffects.Copy);

                dragDropFileWatcher.StopWatcher();
                treeDragActive = false;
                if (dropExtractPath == string.Empty) return;

                ExtractFolder(((TreeNode)e.Item).Name, dropExtractPath, ((TreeNode)e.Item).Text);
            }
        }

        private void folderTreeView_DragDrop(object sender, DragEventArgs e)
        {
            var drop = (string[])e.Data.GetData(DataFormats.FileDrop);
            OpenDat(drop[0]);
        }
        #endregion

        private void folderTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (currentNode != null) currentNode.ForeColor = Color.White;
        }

        private void upToolStripButton_Click(object sender, EventArgs e)
        {
            folderTreeView.SelectedNode = currentNode.Parent;
            //SelectTreeNode(currentNode.Parent);
        }

        private void renameFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderTreeView.LabelEdit = true;
            folderTreeView.SelectedNode.BeginEdit();
        }

        private void folderTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            folderTreeView.LabelEdit = false;

            if (e.Label == null) return;
            if (e.Label.Equals(e.Node.Text, StringComparison.OrdinalIgnoreCase)) {
                e.CancelEdit = true;
                return;
            }
            OpenDat dat = ControlDat.GetDat(currentDat);

            string fullPath = Misc.GetNodeFullPath(e.Node);
            dat.RenameFolder(fullPath.Substring(currentDat.Length + 1), fullPath, e.Label);

            SaveToolStripButton.Enabled = dat.ShouldSave();
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            OpenDat dat = ControlDat.GetDat(currentDat);

            string message = "Сохранить изменения в Dat файл?";
            if (!dat.IsFO2Type()) message += "\n\nПримечание: Данная версия программы не поддерживает сжатие добавленных файлов для формата Fallout 1.";

            if (MessageBox.Show(message, "Dat Explorer II", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (dat.SaveDat()) {
               FindFiles(currentDat, folderTreeView.SelectedNode.Name + "\\");
            }
            SaveToolStripButton.Enabled = false;
        }

        private void ExplorerForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) {
                splitContainer1.SplitterDistance = (int)(splitContainer1.Width * 0.2f);
            }
        }

        private void deleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = folderTreeView.SelectedNode;
            if (node == null || MessageBox.Show("Вы действительно хотите это удалить?", "Dat Explorer II", MessageBoxButtons.YesNo) == DialogResult.No) return;

            statusToolStripStatusLabel.Text = "Deleting:";
            textToolStripStatusLabel.Text = "Prepare...";

            new WaitForm(node.Name).RemoveFile(this);

            folderTreeView.Nodes.Remove(node);
            DeleteDone();
        }

        private void deleteFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filesListView.SelectedItems.Count == 0 ||
                MessageBox.Show("Вы действительно хотите это удалить?", "Dat Explorer II", MessageBoxButtons.YesNo) == DialogResult.No) return;

            statusToolStripStatusLabel.Text = "Deleting:";
            textToolStripStatusLabel.Text = "Prepare...";

            new WaitForm(filesListView.SelectedItems).RemoveFile(this);

            FindFiles(currentDat, currentNode.Name + "\\");
            DeleteDone();
        }

        private void DeleteDone()
        {
            textToolStripStatusLabel.Text = toolStripProgressBar.Value + " files.";
            toolStripProgressBar.Value = 0;
        }

        internal void DeleteFiles(string path)
        {
            List<String> listFiles = new List<String>();
            GetFolderFiles(listFiles, path);

            if (listFiles.Count > 0) {
                toolStripProgressBar.Maximum = listFiles.Count;
                ControlDat.GetDat(currentDat).DeleteFile(listFiles, true);
            }
        }

        internal void DeleteFiles(ListView.SelectedListViewItemCollection listPath)
        {
            List<String> listFiles = new List<String>();
            foreach (ListViewItem item in listPath)
            {
                if (item.Tag == null) { // remove folder and all files in sub folders
                    GetFolderFiles(listFiles, item.Name);
                } else {
                    var file = (sFile)item.Tag;
                    listFiles.Add(file.path);
                }
            }

            if (listFiles.Count > 0) {
                toolStripProgressBar.Maximum = listFiles.Count;
                ControlDat.GetDat(currentDat).DeleteFile(listFiles, false);
            }
        }

        // test
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DATManage.SaveDAT(currentDat);
        }
    }
}
