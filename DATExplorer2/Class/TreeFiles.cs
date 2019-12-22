using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DATLib;

namespace DATExplorer
{
    struct sFile
    {
        public string   path;
        public FileInfo file;

        public sFile(string path, FileInfo file)
        {
            this.path = path;
            this.file = file;
        }
    }
    
    internal class TreeFiles
    {
        private string folderName;
        private List<sFile> m_file;

        public TreeFiles(string folderName) 
        {
            this.folderName = folderName;
            m_file = new List<sFile>(); 
        }

        public string FolderName { get { return folderName; } }

        public void AddFile(KeyValuePair<string, FileInfo> file)
        {
            m_file.Add(new sFile(file.Key, file.Value));
        }

        public List<sFile> GetFiles()
        {
            return m_file;
        }
    }
}
