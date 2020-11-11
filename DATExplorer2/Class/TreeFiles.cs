using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DATLib;

namespace DATExplorer
{
    struct sFile
    {
        public string path;    // путь расположения файла в дереве
        public FileInfo file;  // дополнительная информация о файле

        public bool isVirtual { get { return file.info.PackedSize == -1;} } // файл добавляемый в DAT

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

        public void UpdateFileInfo(int index, Info info)
        {
            sFile nf = m_file[index];
            nf.file.info.PackedSize = info.PackedSize;
            nf.file.info.IsPacked = info.IsPacked;
            nf.file.info.Size = info.Size;

            m_file[index] = nf;
        }

        public List<sFile> GetFiles()
        {
            return m_file;
        }
    }
}
