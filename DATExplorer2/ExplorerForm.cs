using System;
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

            DAT.ExtractUpdate += ExtractUpdateEvent;
            DAT.RemoveFile += FileEvent;
            DAT.SavingFile += InvokeFileEvent;
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
            Action action = () =>
            {
                textToolStripStatusLabel.Text = e.Name;
                toolStripProgressBar.Value++;
            };
            EndInvoke(BeginInvoke(action));
            if (e.Result) successExtracted++;
        }

        void FileEvent(FileEventArgs e)
        {
            //textToolStripStatusLabel.Text = e.File;
            toolStripProgressBar.Value++;
        }

        void InvokeFileEvent(FileEventArgs e)
        {
            Action action = () =>
            {
                textToolStripStatusLabel.Text = e.File;
                toolStripProgressBar.PerformStep();
            };
            EndInvoke(BeginInvoke(action));
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

            new WaitForm(this).Unpack(extractToPath, listFiles, currentDat, cutPath);

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

        private void ImportFiles(string[] list)
        {
            OpenDat dat = ControlDat.GetDat(currentDat);
            string treeFolder = GetCurrentTreeFolder();

            foreach (var file in list)
            {
                dat.AddVirtualFile(file, treeFolder);
            }
            // обновление списка
            FindFiles(currentDat, treeFolder + "\\");

            SaveToolStripButton.Enabled = true;
        }

        private void ImportFilesWithFolders(string[] list, string rootFolder)
        {
            OpenDat dat = ControlDat.GetDat(currentDat);
            string treeFolder = GetCurrentTreeFolder();

            foreach (var file in list)
            {
                if (rootFolder != null) {
                    string folder = file.Substring(rootFolder.Length);
                    int i = folder.LastIndexOf('\\');
                    folder = folder.Remove(i);

                    if (folder.Length > 0) {
                        if (treeFolder.Length > 0) {
                            if (folder[0] != '\\') folder = folder.Insert(0, "\\");
                        } else {
                            if (folder[0] == '\\') folder = folder.Substring(1);
                        }
                    }
                    dat.AddVirtualFile(file, treeFolder + folder);
                } else {
                    dat.AddVirtualFile(file, treeFolder);
                }
            }

            // обновление списка
            if (rootFolder != null) ReBuildTreeNode(dat);
            FindFiles(currentDat, treeFolder + "\\");

            SaveToolStripButton.Enabled = true;
        }

        private void ReBuildTreeNode(OpenDat dat)
        {
            List<TreeNode> expandNodes = new List<TreeNode>();
            Misc.GetExpandNodes(currentNode, ref expandNodes);

            folderTreeView.BeginUpdate();
            currentNode.Nodes.Clear();

            if (currentNode.Parent == null) {

                foreach (var item in dat.Folders)
                {
                    if (item.Key.Length > 0) {
                        string[] dirs = item.Key.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                        TreeNode tn = currentNode;
                        string parentsDir = String.Empty;

                        foreach (var dir in dirs)
                        {
                            parentsDir += dir;
                            TreeNode find = Misc.FindNode(dir, tn);
                            if (find == null) {
                                tn = tn.Nodes.Add(dir); // имя папки
                                tn.Name = parentsDir;   // путь к папке (не должен содержать знаки разделители пути в конце и в начале)
                            } else
                                tn = find;

                            parentsDir += "\\";
                        }
                    }
                }
            } else {
                foreach (var item in dat.Folders)
                {
                    if (item.Key.Length > currentNode.Name.Length && item.Key.StartsWith(currentNode.Name)) {
                        string[] dirs = item.Key.Split('\\');

                        TreeNode tn = currentNode;
                        string parentsDir = dirs[0] + "\\";

                        for (int i = 1; i < dirs.Length; i++)
                        {
                            parentsDir += dirs[i];

                            if (tn.Text != dirs[i]) {
                                TreeNode find = Misc.FindNode(dirs[i], tn);
                                if (find == null) {
                                    tn = tn.Nodes.Add(dirs[i]); // имя папки
                                    tn.Name = parentsDir;       // путь к папке (не должен содержать знаки разделители пути в конце и в начале)
                                } else
                                    tn = find;
                            }
                            parentsDir += "\\";
                        }
                    }
                }
            }
            foreach (TreeNode node in expandNodes) Misc.ExpandNode(node, currentNode);

            currentNode.Expand();
            folderTreeView.EndUpdate();
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
                    string parentsDir = String.Empty;
                    foreach (var dir in dirs)
                    {
                        parentsDir += dir;
                        TreeNode find = Misc.FindNode(dir, tn);
                        if (find == null) {
                            tn = tn.Nodes.Add(dir); // имя папки
                            tn.Name = parentsDir;   // путь к папке (не должен содержать знаки разделители пути в конце и в начале)
                        } else
                            tn = find;

                        parentsDir += "\\";
                    }
                }
            }
            root.Expand();
            folderTreeView.EndUpdate();
        }

        private void FindFiles(string datPath, TreeNode node)
        {
            string path = Misc.GetNodeFullPath(node);
            if (path.Length > datPath.Length)
                path = path.Remove(0, datPath.Length + 1) + '\\';
            else
                path = string.Empty;

            FindFiles(datPath, path);
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
            FindFiles(datPath, node);

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

                if (folderTreeView.SelectedNode != null) {
                    FindFiles(currentDat, folderTreeView.SelectedNode);
                }
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
                sFile sfile = (sFile)item.Tag;
                if (sfile.isVirtual) return;

                var file = new string[1] { sfile.path };

                if (sfile.file.info.Size > 1048576) { // 1mb
                    new WaitForm(this).UnpackFile(Application.StartupPath + "\\tmp\\", file, currentDat);
                } else {
                    DATManage.ExtractFile(Application.StartupPath + "\\tmp\\", sfile.path, currentDat);
                }

                string ofile = Application.StartupPath + "\\tmp\\" + file[0];
                if (File.Exists(ofile)) System.Diagnostics.Process.Start("explorer", ofile);
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

        bool dragListActive = false;

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

        // Drop from List
        private void filesListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {

                List<String> dropSelected = new List<String>();
                foreach (ListViewItem item in filesListView.SelectedItems)
                {
                    if (item.Tag != null) {
                        dropSelected.Add(((sFile)item.Tag).path);
                    } else { // for selected folder
                        GetFolderFiles(dropSelected, item.Name);
                    }
                }

                dragListActive = true;
                dropExtractPath = string.Empty;

                String[] dummyDropFile = new String[] { String.Empty };
                dragDropFileWatcher.StartWatcher(ref dummyDropFile[0]);

                IDataObject obj = new DataObject(DataFormats.FileDrop, dummyDropFile);
                DragDropEffects result = filesListView.DoDragDrop(obj, DragDropEffects.Copy);

                if (dragDropFileWatcher.IsRunning) dragDropFileWatcher.StopWatcher();
                dragListActive = false;

                if (dropExtractPath == string.Empty) return;

                string fullPath = (folderTreeView.SelectedNode.Parent != null) ? folderTreeView.SelectedNode.Name : String.Empty;

                ExtractFiles(dropSelected.ToArray(), dropExtractPath, fullPath);
            }
        }

        // Drop to List
        private void filesListView_DragDrop(object sender, DragEventArgs e)
        {
            if (dragListActive) return;

            string rootFolder = null;
            List<string> addFilesToDat = new List<string>();

            foreach (string file in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                if (Directory.Exists(file)) {
                    if (rootFolder == null) rootFolder = Path.GetDirectoryName(file);
                    addFilesToDat.AddRange(Directory.GetFiles(file, "*.*", SearchOption.AllDirectories));
                } else {
                    addFilesToDat.Add(file);
                }
            }
            ImportFilesWithFolders(addFilesToDat.ToArray(), rootFolder);
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
            if (importFilesDialog.ShowDialog() == DialogResult.OK) ImportFiles(importFilesDialog.FileNames);
        }

        private void importFoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
            ImportFilesWithFolders(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.*", SearchOption.AllDirectories), folderBrowserDialog.SelectedPath);
        }

        bool createFolder = false;

        private void createFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int num = 0;
exist:
            TreeNode addNode = new TreeNode((num == 0) ? "NewFolder" : "NewFolder" + num.ToString());

            string fullPath = Misc.GetNodeFullPath(currentNode);
            if (fullPath.Length > currentDat.Length) addNode.Name = fullPath.Substring(currentDat.Length + 1) + "\\";
            addNode.Name += addNode.Text;

            OpenDat dat = ControlDat.GetDat(currentDat);
            if (dat.FolderExist(addNode.Name)) {
                num++;
                addNode = null;
                goto exist;
            }

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

                if (dragDropFileWatcher.IsRunning) dragDropFileWatcher.StopWatcher();
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
        }

        private void renameFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderTreeView.LabelEdit = true;
            folderTreeView.SelectedNode.BeginEdit();
        }

        private void folderTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            folderTreeView.LabelEdit = false;

            OpenDat dat = ControlDat.GetDat(currentDat);

            if (createFolder) {
                dat.AddEmptyFolder(e.Node.Name);
                createFolder = false;
            }

            if (e.Label == null) return;
            if (e.Label.Equals(e.Node.Text, StringComparison.OrdinalIgnoreCase)) {
                e.CancelEdit = true;
                return;
            }

            int i = e.Node.Name.LastIndexOf(e.Node.Text);
            string newFolderPath = e.Node.Name.Remove(i) + e.Label;
            if (dat.FolderExist(newFolderPath)) {
                e.CancelEdit = true;
                MessageBox.Show("Директория с таким именеи уже существует.", "Dat Explorer II", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fullPath = Misc.GetNodeFullPath(e.Node);
            dat.RenameFolder(fullPath.Substring(currentDat.Length + 1), fullPath, e.Label);

            e.Node.Name = newFolderPath;

            SaveToolStripButton.Enabled = dat.ShouldSave();
            dirToolStripStatusLabel.Text = "Directory: " + e.Node.Name + '\\';
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            OpenDat dat = ControlDat.GetDat(currentDat);

            string message = "Сохранить изменения в DAT файл?";
            if (!dat.IsFO2Type()) message += "\n\nПримечание: Данная версия программы не поддерживает сжатие добавленных файлов для DAT формата Fallout 1.";

            if (MessageBox.Show(message, "Dat Explorer II", MessageBoxButtons.YesNo) == DialogResult.No) return;

            statusToolStripStatusLabel.Text = "Saving:";
            textToolStripStatusLabel.Text = "Prepare...";
            toolStripProgressBar.Maximum = dat.TotalFiles / 5;

            if (dat.SaveDat()) {
               FindFiles(currentDat, folderTreeView.SelectedNode);
            }

            SaveToolStripButton.Enabled = false;
            toolStripProgressBar.Value = 0;
        }

        private void ExplorerForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) {
                splitContainer1.SplitterDistance = (int)(splitContainer1.Width * 0.2f);
            }
        }

        private void deleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderTreeView.SelectedNode != null) Delete(false);
        }

        private void deleteFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filesListView.SelectedItems.Count > 0) Delete(true);
        }

        private void Delete(bool isList)
        {
            if (MessageBox.Show("Вы действительно хотите это удалить?", "Dat Explorer II", MessageBoxButtons.YesNo) == DialogResult.No) return;

            statusToolStripStatusLabel.Text = "Deleting:";
            textToolStripStatusLabel.Text = "Prepare...";

            if (isList) {
                new WaitForm(this).RemoveFile(filesListView.SelectedItems);
                FindFiles(currentDat, currentNode);
            } else {
                new WaitForm(this).RemoveFile(currentNode.Name);
                folderTreeView.Nodes.Remove(currentNode);
            }

            textToolStripStatusLabel.Text = toolStripProgressBar.Value + " file(s)";
            toolStripProgressBar.Value = 0;

            var dat = ControlDat.GetDat(currentDat);
            totalToolStripStatusLabel.Text = dat.TotalFiles.ToString();

            if (dat.ShouldSave()) SaveToolStripButton.Enabled = true;
        }

        internal void DeleteFiles(string path)
        {
            List<String> listFiles = new List<String>();
            GetFolderFiles(listFiles, path);

            OpenDat dat = ControlDat.GetDat(currentDat);

            if (listFiles.Count > 0) {
                toolStripProgressBar.Maximum = listFiles.Count;
                dat.DeleteFile(listFiles, true);
            } else {
                dat.RemoveEmptyFolder(path);
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
    }
}
