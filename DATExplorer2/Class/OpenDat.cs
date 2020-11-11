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
        //public DAT Dat { get { return dat; } }

        private SortedDictionary<String, TreeFiles> treeFiles; //

        private SaveType shouldSave = SaveType.None; // указывает, что данные изменились и требуется сохранение

        public int TotalFiles { set; get; }

        public SortedDictionary<String, TreeFiles> Folders { get { return treeFiles; } }

        public string DatName { get { return datFile; } }

        public bool ShouldSave() { return shouldSave != SaveType.None; }

        public bool IsFO2Type() { return dat.IsFallout2Type; }

        public OpenDat(string datFile, Dictionary<String, DATLib.FileInfo> files)
        {
            this.datFile = datFile;
            dat = DATManage.GetDat(datFile);
            TotalFiles = files.Count();
            treeFiles = new SortedDictionary<string, TreeFiles>();
            BuildFolderTree(files);
        }

        public void CloseDat()
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

        public void AddVirtualFile(string pathFile, string treeFolderPath)
        {
            // не добавляем дубликаты
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

        public void RenameFolder(string pathFolder, string fullFolderPath, string newNameFolder)
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

        public void DeleteFile(List<string> pathFileList)
        {
            // удаление файлов из Dat
            bool isDeleted = dat.RemoveFile(pathFileList);

            // удаление файлов из дерева




            TotalFiles -= pathFileList.Count;
            if (isDeleted) shouldSave = SaveType.New;
        }

        public bool SaveDat()
        {
            switch (shouldSave)
            {
                case SaveType.DirTree:
                    DATManage.SaveDirectoryStructure(DatName);
                    break;
                case SaveType.Append:
                    DATManage.AppendFilesDAT(DatName);
                    UpdateTreeFiles();
                    break;
                case SaveType.New:

                    break;
            }
            bool refresh = (shouldSave != SaveType.DirTree);
            shouldSave = SaveType.None;
            return refresh;
        }
    }
}
