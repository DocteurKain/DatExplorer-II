using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using DATLib;

namespace DATExplorer
{
    internal class OpenDat
    {
        private string datFile;
        private SortedDictionary<String, TreeFiles> treeFiles;

        public int TotalFiles { set; get; }

        public OpenDat(string datFile, Dictionary<String, DATLib.FileInfo> files)
        {
            this.datFile = datFile;
            TotalFiles = files.Count();
            treeFiles = new SortedDictionary<string, TreeFiles>();
            BuildFolderTree(files);
        }
        
        public SortedDictionary<String, TreeFiles> Folders { get { return treeFiles; } }
        
        public string DatName { get { return datFile; } }

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
    }
}
