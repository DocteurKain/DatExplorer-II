﻿namespace DATExplorer {
    partial class ExplorerForm {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerForm));
            this.folderTreeView = new System.Windows.Forms.TreeView();
            this.cmsFolderTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractAllFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.createFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeImageList = new System.Windows.Forms.ImageList(this.components);
            this.filesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.extractFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.importFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.createFolderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listImageList = new System.Windows.Forms.ImageList(this.components);
            this.listSmallImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.OpenToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.CreateNewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.upToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.stileToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.largeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.infoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.textToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelEmpty = new System.Windows.Forms.ToolStripStatusLabel();
            this.dirToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.totalToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.openDatFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.extractFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.CreateNewDatDialog = new System.Windows.Forms.SaveFileDialog();
            this.importFilesDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.cmsFolderTree.SuspendLayout();
            this.listViewContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStripContainer2.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderTreeView
            // 
            this.folderTreeView.AllowDrop = true;
            this.folderTreeView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.folderTreeView.ContextMenuStrip = this.cmsFolderTree;
            this.folderTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folderTreeView.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.folderTreeView.FullRowSelect = true;
            this.folderTreeView.ImageIndex = 0;
            this.folderTreeView.ImageList = this.treeImageList;
            this.folderTreeView.Indent = 19;
            this.folderTreeView.ItemHeight = 20;
            this.folderTreeView.LineColor = System.Drawing.Color.Gray;
            this.folderTreeView.Location = new System.Drawing.Point(0, 0);
            this.folderTreeView.Name = "folderTreeView";
            this.folderTreeView.SelectedImageIndex = 1;
            this.folderTreeView.Size = new System.Drawing.Size(290, 594);
            this.folderTreeView.TabIndex = 0;
            this.folderTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.folderTreeView_AfterLabelEdit);
            this.folderTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.folderTreeView_ItemDrag);
            this.folderTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.folderTreeView_BeforeSelect);
            this.folderTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.folderTreeView_AfterSelect);
            this.folderTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.folderTreeView_DragDrop);
            this.folderTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.folderTreeView_DragEnter);
            // 
            // cmsFolderTree
            // 
            this.cmsFolderTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractAllFilesToolStripMenuItem,
            this.extractFolderToolStripMenuItem,
            this.addFoldersToolStripMenuItem,
            this.toolStripSeparator6,
            this.createFolderToolStripMenuItem,
            this.renameFolderToolStripMenuItem,
            this.toolStripSeparator5,
            this.deleteFolderToolStripMenuItem});
            this.cmsFolderTree.Name = "cmsFolderTree";
            this.cmsFolderTree.Size = new System.Drawing.Size(184, 148);
            this.cmsFolderTree.Opening += new System.ComponentModel.CancelEventHandler(this.cmsFolderTree_Opening);
            // 
            // extractAllFilesToolStripMenuItem
            // 
            this.extractAllFilesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("extractAllFilesToolStripMenuItem.Image")));
            this.extractAllFilesToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.extractAllFilesToolStripMenuItem.Name = "extractAllFilesToolStripMenuItem";
            this.extractAllFilesToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.extractAllFilesToolStripMenuItem.Text = "Extract all files to ...";
            this.extractAllFilesToolStripMenuItem.Click += new System.EventHandler(this.extractAllFilesToolStripMenuItem_Click);
            // 
            // extractFolderToolStripMenuItem
            // 
            this.extractFolderToolStripMenuItem.Name = "extractFolderToolStripMenuItem";
            this.extractFolderToolStripMenuItem.ShowShortcutKeys = false;
            this.extractFolderToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.extractFolderToolStripMenuItem.Text = "Extract folders to ...";
            this.extractFolderToolStripMenuItem.ToolTipText = "Распаковывает файлы выбранной папки со структурой родительских каталогов.";
            this.extractFolderToolStripMenuItem.Click += new System.EventHandler(this.extractFolderToolStripMenuItem_Click);
            // 
            // addFoldersToolStripMenuItem
            // 
            this.addFoldersToolStripMenuItem.Name = "addFoldersToolStripMenuItem";
            this.addFoldersToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.addFoldersToolStripMenuItem.Text = "Import folder(s)";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(180, 6);
            // 
            // createFolderToolStripMenuItem
            // 
            this.createFolderToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createFolderToolStripMenuItem.Image")));
            this.createFolderToolStripMenuItem.Name = "createFolderToolStripMenuItem";
            this.createFolderToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.createFolderToolStripMenuItem.Text = "Add folder";
            this.createFolderToolStripMenuItem.Click += new System.EventHandler(this.createFolderToolStripMenuItem_Click);
            // 
            // renameFolderToolStripMenuItem
            // 
            this.renameFolderToolStripMenuItem.Name = "renameFolderToolStripMenuItem";
            this.renameFolderToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.renameFolderToolStripMenuItem.Text = "Rename folder";
            this.renameFolderToolStripMenuItem.Click += new System.EventHandler(this.renameFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(180, 6);
            // 
            // deleteFolderToolStripMenuItem
            // 
            this.deleteFolderToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteFolderToolStripMenuItem.Image")));
            this.deleteFolderToolStripMenuItem.Name = "deleteFolderToolStripMenuItem";
            this.deleteFolderToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.deleteFolderToolStripMenuItem.Text = "Delete folder";
            this.deleteFolderToolStripMenuItem.Click += new System.EventHandler(this.deleteFolderToolStripMenuItem_Click);
            // 
            // treeImageList
            // 
            this.treeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImageList.ImageStream")));
            this.treeImageList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.treeImageList.Images.SetKeyName(0, "VSFolder_closed.bmp");
            this.treeImageList.Images.SetKeyName(1, "VSFolder_open.bmp");
            this.treeImageList.Images.SetKeyName(2, "book_active_directory.bmp");
            // 
            // filesListView
            // 
            this.filesListView.AllowDrop = true;
            this.filesListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.filesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.filesListView.ContextMenuStrip = this.listViewContextMenuStrip;
            this.filesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filesListView.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.filesListView.HideSelection = false;
            this.filesListView.LargeImageList = this.listImageList;
            this.filesListView.Location = new System.Drawing.Point(0, 0);
            this.filesListView.Name = "filesListView";
            this.filesListView.Size = new System.Drawing.Size(714, 594);
            this.filesListView.SmallImageList = this.listSmallImageList;
            this.filesListView.TabIndex = 1;
            this.filesListView.UseCompatibleStateImageBehavior = false;
            this.filesListView.View = System.Windows.Forms.View.List;
            this.filesListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.filesListView_ItemDrag);
            this.filesListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.filesListView_ItemSelectionChanged);
            this.filesListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.filesListView_DragDrop);
            this.filesListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.filesListView_DragEnter);
            this.filesListView.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.filesListView_GiveFeedback);
            this.filesListView.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.filesListView_QueryContinueDrag);
            this.filesListView.DoubleClick += new System.EventHandler(this.filesListView_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 75;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Size";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 75;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Packed Size";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader4.Width = 75;
            // 
            // listViewContextMenuStrip
            // 
            this.listViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator4,
            this.extractFilesToolStripMenuItem,
            this.toolStripSeparator7,
            this.importFilesToolStripMenuItem,
            this.importFoldersToolStripMenuItem,
            this.toolStripSeparator8,
            this.createFolderToolStripMenuItem1,
            this.renameToolStripMenuItem,
            this.toolStripSeparator10,
            this.deleteFilesToolStripMenuItem});
            this.listViewContextMenuStrip.Name = "listViewContextMenuStrip";
            this.listViewContextMenuStrip.Size = new System.Drawing.Size(230, 182);
            this.listViewContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.listViewContextMenuStrip_Opening);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeyDisplayString = "Enter";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(226, 6);
            // 
            // extractFilesToolStripMenuItem
            // 
            this.extractFilesToolStripMenuItem.Name = "extractFilesToolStripMenuItem";
            this.extractFilesToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.extractFilesToolStripMenuItem.Text = "Extract selected file(s) to ...";
            this.extractFilesToolStripMenuItem.ToolTipText = "Распаковывает выбранные файлы или папки со структурой родительских каталогов.";
            this.extractFilesToolStripMenuItem.Click += new System.EventHandler(this.extractFilesToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(226, 6);
            // 
            // importFilesToolStripMenuItem
            // 
            this.importFilesToolStripMenuItem.Name = "importFilesToolStripMenuItem";
            this.importFilesToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.importFilesToolStripMenuItem.Text = "Import file(s)";
            this.importFilesToolStripMenuItem.ToolTipText = "Импортирует выбранные файлы в текущий каталог.\r\n";
            this.importFilesToolStripMenuItem.Click += new System.EventHandler(this.importFilesToolStripMenuItem_Click);
            // 
            // importFoldersToolStripMenuItem
            // 
            this.importFoldersToolStripMenuItem.Name = "importFoldersToolStripMenuItem";
            this.importFoldersToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.importFoldersToolStripMenuItem.Text = "Import folder(s) with files";
            this.importFoldersToolStripMenuItem.ToolTipText = "Импортирует выбранные каталоги и их файлы включая структуру вложенных папок в тек" +
    "ущий каталог.";
            this.importFoldersToolStripMenuItem.Click += new System.EventHandler(this.importFoldersToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(226, 6);
            // 
            // createFolderToolStripMenuItem1
            // 
            this.createFolderToolStripMenuItem1.Name = "createFolderToolStripMenuItem1";
            this.createFolderToolStripMenuItem1.Size = new System.Drawing.Size(229, 22);
            this.createFolderToolStripMenuItem1.Text = "Add folder";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.renameToolStripMenuItem.Text = "Rename file";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(226, 6);
            // 
            // deleteFilesToolStripMenuItem
            // 
            this.deleteFilesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteFilesToolStripMenuItem.Image")));
            this.deleteFilesToolStripMenuItem.Name = "deleteFilesToolStripMenuItem";
            this.deleteFilesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteFilesToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.deleteFilesToolStripMenuItem.Text = "Delete selected file(s)";
            this.deleteFilesToolStripMenuItem.Click += new System.EventHandler(this.deleteFilesToolStripMenuItem_Click);
            // 
            // listImageList
            // 
            this.listImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("listImageList.ImageStream")));
            this.listImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.listImageList.Images.SetKeyName(0, "Stuffed_Folder.png");
            this.listImageList.Images.SetKeyName(1, "Page.png");
            this.listImageList.Images.SetKeyName(2, "FileAdd.png");
            // 
            // listSmallImageList
            // 
            this.listSmallImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("listSmallImageList.ImageStream")));
            this.listSmallImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.listSmallImageList.Images.SetKeyName(0, "Folder_stuffed.ico");
            this.listSmallImageList.Images.SetKeyName(1, "Page.png");
            this.listSmallImageList.Images.SetKeyName(2, "FileAdd.png");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.folderTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.filesListView);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 594);
            this.splitContainer1.SplitterDistance = 290;
            this.splitContainer1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripButton,
            this.CreateNewToolStripButton,
            this.toolStripSeparator2,
            this.SaveToolStripButton,
            this.toolStripSeparator,
            this.closeToolStripButton,
            this.toolStripSeparator9,
            this.upToolStripButton,
            this.toolStripSeparator1,
            this.stileToolStripDropDownButton,
            this.toolStripSeparator3,
            this.infoToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(319, 37);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // OpenToolStripButton
            // 
            this.OpenToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenToolStripButton.Image")));
            this.OpenToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenToolStripButton.Name = "OpenToolStripButton";
            this.OpenToolStripButton.Size = new System.Drawing.Size(41, 34);
            this.OpenToolStripButton.Text = "Open";
            this.OpenToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.OpenToolStripButton.ToolTipText = "Open Dat file";
            this.OpenToolStripButton.Click += new System.EventHandler(this.OpenToolStripButton_Click);
            // 
            // CreateNewToolStripButton
            // 
            this.CreateNewToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("CreateNewToolStripButton.Image")));
            this.CreateNewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CreateNewToolStripButton.Name = "CreateNewToolStripButton";
            this.CreateNewToolStripButton.Size = new System.Drawing.Size(36, 34);
            this.CreateNewToolStripButton.Text = "New";
            this.CreateNewToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.CreateNewToolStripButton.ToolTipText = "Creare new Dat";
            this.CreateNewToolStripButton.Click += new System.EventHandler(this.CreateNewToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 37);
            // 
            // SaveToolStripButton
            // 
            this.SaveToolStripButton.Enabled = false;
            this.SaveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveToolStripButton.Image")));
            this.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveToolStripButton.Name = "SaveToolStripButton";
            this.SaveToolStripButton.Size = new System.Drawing.Size(37, 34);
            this.SaveToolStripButton.Text = "Save";
            this.SaveToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SaveToolStripButton.ToolTipText = "Save current Dat";
            this.SaveToolStripButton.Click += new System.EventHandler(this.SaveToolStripButton_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 37);
            // 
            // closeToolStripButton
            // 
            this.closeToolStripButton.Enabled = false;
            this.closeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("closeToolStripButton.Image")));
            this.closeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.closeToolStripButton.Name = "closeToolStripButton";
            this.closeToolStripButton.Size = new System.Drawing.Size(39, 34);
            this.closeToolStripButton.Text = "Close";
            this.closeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.closeToolStripButton.ToolTipText = "Close current Dat";
            this.closeToolStripButton.Click += new System.EventHandler(this.closeToolStripButton_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 37);
            // 
            // upToolStripButton
            // 
            this.upToolStripButton.Enabled = false;
            this.upToolStripButton.Image = global::DATExplorer.Properties.Resources.GoToParentFolderHS;
            this.upToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.upToolStripButton.Name = "upToolStripButton";
            this.upToolStripButton.Size = new System.Drawing.Size(44, 34);
            this.upToolStripButton.Text = "Up Dir";
            this.upToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.upToolStripButton.ToolTipText = "Goto parent folder";
            this.upToolStripButton.Click += new System.EventHandler(this.upToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 37);
            // 
            // stileToolStripDropDownButton
            // 
            this.stileToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.largeToolStripMenuItem,
            this.listToolStripMenuItem,
            this.detailsToolStripMenuItem});
            this.stileToolStripDropDownButton.Image = global::DATExplorer.Properties.Resources.ViewThumbnails;
            this.stileToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stileToolStripDropDownButton.Name = "stileToolStripDropDownButton";
            this.stileToolStripDropDownButton.Size = new System.Drawing.Size(47, 34);
            this.stileToolStripDropDownButton.Text = "View";
            this.stileToolStripDropDownButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.stileToolStripDropDownButton.ToolTipText = "View details";
            // 
            // largeToolStripMenuItem
            // 
            this.largeToolStripMenuItem.Name = "largeToolStripMenuItem";
            this.largeToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.largeToolStripMenuItem.Text = "Large";
            this.largeToolStripMenuItem.Click += new System.EventHandler(this.largeToolStripMenuItem_Click);
            // 
            // listToolStripMenuItem
            // 
            this.listToolStripMenuItem.Checked = true;
            this.listToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.listToolStripMenuItem.Name = "listToolStripMenuItem";
            this.listToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.listToolStripMenuItem.Text = "List";
            this.listToolStripMenuItem.Click += new System.EventHandler(this.listToolStripMenuItem_Click);
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.detailsToolStripMenuItem.Text = "Details";
            this.detailsToolStripMenuItem.Click += new System.EventHandler(this.detailsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 37);
            // 
            // infoToolStripButton
            // 
            this.infoToolStripButton.Enabled = false;
            this.infoToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("infoToolStripButton.Image")));
            this.infoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.infoToolStripButton.Name = "infoToolStripButton";
            this.infoToolStripButton.Size = new System.Drawing.Size(33, 34);
            this.infoToolStripButton.Text = "Info";
            this.infoToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusToolStripStatusLabel,
            this.textToolStripStatusLabel,
            this.toolStripProgressBar,
            this.toolStripStatusLabelEmpty,
            this.dirToolStripStatusLabel,
            this.toolStripStatusLabel2,
            this.totalToolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip1.TabIndex = 4;
            // 
            // statusToolStripStatusLabel
            // 
            this.statusToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.statusToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusToolStripStatusLabel.Margin = new System.Windows.Forms.Padding(3, 2, 0, 0);
            this.statusToolStripStatusLabel.Name = "statusToolStripStatusLabel";
            this.statusToolStripStatusLabel.Size = new System.Drawing.Size(48, 20);
            this.statusToolStripStatusLabel.Text = "Ready.";
            this.statusToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textToolStripStatusLabel
            // 
            this.textToolStripStatusLabel.AutoSize = false;
            this.textToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.textToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.textToolStripStatusLabel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.textToolStripStatusLabel.Name = "textToolStripStatusLabel";
            this.textToolStripStatusLabel.Size = new System.Drawing.Size(200, 20);
            this.textToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.AutoSize = false;
            this.toolStripProgressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(150, 16);
            this.toolStripProgressBar.Step = 1;
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // toolStripStatusLabelEmpty
            // 
            this.toolStripStatusLabelEmpty.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
            this.toolStripStatusLabelEmpty.Name = "toolStripStatusLabelEmpty";
            this.toolStripStatusLabelEmpty.Size = new System.Drawing.Size(0, 20);
            this.toolStripStatusLabelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dirToolStripStatusLabel
            // 
            this.dirToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.dirToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.dirToolStripStatusLabel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
            this.dirToolStripStatusLabel.Name = "dirToolStripStatusLabel";
            this.dirToolStripStatusLabel.Size = new System.Drawing.Size(471, 20);
            this.dirToolStripStatusLabel.Spring = true;
            this.dirToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.AutoSize = false;
            this.toolStripStatusLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(55, 20);
            this.toolStripStatusLabel2.Text = "Total files:";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // totalToolStripStatusLabel
            // 
            this.totalToolStripStatusLabel.AutoSize = false;
            this.totalToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.totalToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.totalToolStripStatusLabel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.totalToolStripStatusLabel.Name = "totalToolStripStatusLabel";
            this.totalToolStripStatusLabel.Size = new System.Drawing.Size(50, 20);
            this.totalToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripContainer2
            // 
            // 
            // toolStripContainer2.BottomToolStripPanel
            // 
            this.toolStripContainer2.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.AutoScroll = true;
            this.toolStripContainer2.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(1008, 594);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.Size = new System.Drawing.Size(1008, 653);
            this.toolStripContainer2.TabIndex = 6;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // openDatFileDialog
            // 
            this.openDatFileDialog.DefaultExt = "dat";
            this.openDatFileDialog.Filter = "Dat files|*.dat";
            this.openDatFileDialog.Title = "Open Fallout dat file";
            // 
            // extractFolderBrowser
            // 
            this.extractFolderBrowser.Description = "Select extract folder";
            this.extractFolderBrowser.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // CreateNewDatDialog
            // 
            this.CreateNewDatDialog.DefaultExt = "dat";
            this.CreateNewDatDialog.Filter = "Dat files|*.dat";
            // 
            // importFilesDialog
            // 
            this.importFilesDialog.Filter = "All files|*.*";
            this.importFilesDialog.Multiselect = true;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Select folder for import";
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog.ShowNewFolderButton = false;
            // 
            // ExplorerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 653);
            this.Controls.Add(this.toolStripContainer2);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "ExplorerForm";
            this.Text = "DAT Explorer II - v";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExplorerForm_FormClosed);
            this.Shown += new System.EventHandler(this.ExplorerForm_Shown);
            this.SizeChanged += new System.EventHandler(this.ExplorerForm_SizeChanged);
            this.cmsFolderTree.ResumeLayout(false);
            this.listViewContextMenuStrip.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripContainer2.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView folderTreeView;
        private System.Windows.Forms.ListView filesListView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton CreateNewToolStripButton;
        private System.Windows.Forms.ToolStripButton OpenToolStripButton;
        private System.Windows.Forms.ToolStripButton SaveToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton closeToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton infoToolStripButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.OpenFileDialog openDatFileDialog;
        private System.Windows.Forms.ImageList treeImageList;
        private System.Windows.Forms.ImageList listImageList;
        private System.Windows.Forms.ToolStripButton upToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton stileToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem largeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.ImageList listSmallImageList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ContextMenuStrip listViewContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem extractFilesToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog extractFolderBrowser;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel statusToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelEmpty;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel totalToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel textToolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripStatusLabel dirToolStripStatusLabel;
        private System.Windows.Forms.ContextMenuStrip cmsFolderTree;
        private System.Windows.Forms.ToolStripMenuItem extractFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem extractAllFilesToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog CreateNewDatDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem addFoldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem importFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFoldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem createFolderToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteFilesToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog importFilesDialog;
        private System.Windows.Forms.ToolStripMenuItem renameFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

