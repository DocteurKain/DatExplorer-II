using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using DATLib;

namespace DATExplorer
{
    internal static class ControlDat
    {
        static List<OpenDat> openDat = new List<OpenDat>();

        static internal List<OpenDat> OpenDat()
        {
            return openDat;
        }

        static internal OpenDat OpeningDat(string pathDat, bool create = false)
        {
            OpenDat dat = new OpenDat(pathDat, (!create) ? DATManage.GetFiles(pathDat) : new Dictionary<string, DATLib.FileInfo>());
            OpenDat().Add(dat);
            return dat;
        }

        static internal OpenDat GetDat(string dat)
        {
            return openDat.Find(x => x.DatName == dat);
        }

        static internal bool DatIsOpen(string dat)
        {
            return openDat.Exists(x => x.DatName == dat);
        }

        static internal void CloseDat(string datName)
        {
            OpenDat dat = GetDat(datName);
            dat.CloseDat();
            openDat.Remove(dat);
        }
    }

    internal class OpenDat
    {
        enum SaveType {
            None,
            DirTree,
            Append,
            New
        }

        private string datFile;
        private DAT dat;

        private SortedDictionary<String, TreeFiles> treeFiles; //

        private SaveType shouldSave = SaveType.None; // указывает, что данные изменились и требуется сохранение

        public int TotalFiles { set; get; }

        public SortedDictionary<String, TreeFiles> Folders { get { return treeFiles; } }

        public string DatName { get { return datFile; } }

        public bool ShouldSave() { return shouldSave != SaveType.None; }

        public bool IsFO2Type() { return dat.IsFallout2Type; }

        internal OpenDat(string datFile, Dictionary<String, DATLib.FileInfo> files)
        {
            this.datFile = datFile;
            dat = DATManage.GetDat(datFile);
            TotalFiles = files.Count();
            treeFiles = new SortedDictionary<string, TreeFiles>();
            BuildFolderTree(files);
        }

        internal void CloseDat()
        {
            DATManage.CloseDatFile(datFile);
        }

        private void BuildFolderTree(Dictionary<String, DATLib.FileInfo> files)
        {
            foreach (var item in files)
            {
                string pathfile = Path.GetDirectoryName(item.Key); // удалить имя
                if (!treeFiles.ContainsKey(pathfile)) {
                    treeFiles.Add(pathfile, new TreeFiles(pathfile + '\\'));
                }
                treeFiles[pathfile].AddFile(item);
            }
            files.Clear();
        }

        private void UpdateTreeFiles()
        {
            foreach (var item in treeFiles)
            {
                for (int i = 0; i < item.Value.GetFiles().Count; i++)
                {
                    var file = item.Value.GetFiles()[i];
                    if (file.file.info.PackedSize == -1) {
                        item.Value.UpdateFileInfo(i, DATManage.GetFileInfo(datFile, file.path));
                    }
                }
            }
        }

        internal void GetFilesFromFolder(List<String> listFiles, string folderName, bool includeSubDirs = true)
        {
            foreach (var folder in Folders.Keys)
            {
                if (includeSubDirs) {
                    if (!folder.StartsWith(folderName)) continue;
                } else if (folder != folderName) continue;

                foreach (var file in Folders[folder].GetFiles())
                {
                    listFiles.Add(file.path);
                }
            }
        }

        internal void AddVirtualFile(string pathFile, string treeFolderPath)
        {
            //TODO: не добавляем дубликаты
            //if (!addedFiles.ContainsKey(fileDat)) addedFiles.Add(fileDat, pathFile);

            System.IO.FileInfo file = new System.IO.FileInfo(pathFile);

            DATLib.FileInfo fileDat = new DATLib.FileInfo();
            fileDat.name = file.Name;
            fileDat.info.Size = (int)file.Length;
            fileDat.info.PackedSize = -1;
            fileDat.pathTree = treeFolderPath + '\\';

            dat.AddFile(pathFile, fileDat);

            if (!treeFiles.ContainsKey(treeFolderPath)) {
                treeFiles.Add(treeFolderPath, new TreeFiles(fileDat.pathTree));
            }
            treeFiles[treeFolderPath].AddFile(new KeyValuePair<string, DATLib.FileInfo>(fileDat.pathTree + fileDat.name, fileDat));

            TotalFiles++;
            if (shouldSave != SaveType.New) shouldSave = SaveType.Append;
        }

        internal void RenameFolder(string pathFolder, string fullFolderPath, string newNameFolder)
        {
            string newPath = pathFolder.Remove(pathFolder.LastIndexOf('\\') + 1) + newNameFolder;

            List<string> removeKeys = new List<string>();
            SortedDictionary<String, TreeFiles> addFiles = new SortedDictionary<string,TreeFiles>();

            foreach (var item in treeFiles.Keys)
            {
                int i = item.LastIndexOf(pathFolder);
                if (i != -1) {
                    string newPathKey = newPath + item.Substring(pathFolder.Length);

                    TreeFiles tFiles = new TreeFiles(newPathKey + '\\');
                    foreach (var file in treeFiles[item].GetFiles())
                    {
                        tFiles.AddFile(new KeyValuePair<string, DATLib.FileInfo>(tFiles.FolderName + file.file.name, file.file));
                    }
                    removeKeys.Add(item);
                    addFiles.Add(newPathKey, tFiles);
                }
            }
            if (addFiles.Count == 0) return; // внесение изменений в дат не требуется

            foreach (var item in removeKeys) treeFiles.Remove(item);
            foreach (var pair in addFiles) treeFiles.Add(pair.Key, pair.Value);

            DATManage.RenameFolder(DatName, pathFolder, newPath);

            if (shouldSave == SaveType.None) shouldSave = SaveType.DirTree;
        }

        internal void DeleteFile(List<string> pathFileList, bool alsoFolder)
        {
            // удаление файлов из дерева (остаются только пустые папки если в них нет файлов)
            string folder = null;
            TreeFiles values = null;

            List<string> folders = new List<string>();
            for (int i = 0; i < pathFileList.Count; i++)
            {
                string file = pathFileList[i];
                if (folder == null) {
                    folder = file.Remove(file.LastIndexOf('\\'));
                    values = treeFiles[folder];
                    folders.Add(folder);
                }

                string f = file.Remove(file.LastIndexOf('\\'));
                if (f != folder) {
                    folder = f;
                    values = treeFiles[folder];
                    folders.Add(folder);
                }
                values.RemoveFile(file);
            }
            if (alsoFolder) {
                foreach (var f in folders) if (treeFiles[f].GetFiles().Count == 0) treeFiles.Remove(f);
            }

            TotalFiles -= pathFileList.Count;

            // удаление файлов из Dat
            if (dat.RemoveFile(pathFileList)) shouldSave = SaveType.New;
        }

        internal bool SaveDat()
        {
            switch (shouldSave)
            {
                case SaveType.DirTree:
                    DATManage.SaveDirectoryStructure(DatName);
                    break;
                case SaveType.Append:
                case SaveType.New:
                    new WaitForm(ExplorerForm.ActiveForm).SaveDat(DatName, shouldSave == SaveType.Append);
                    UpdateTreeFiles();
                    break;
            }
            bool refresh = (shouldSave != SaveType.DirTree);
            shouldSave = SaveType.None;
            return refresh;
        }
    }
}
