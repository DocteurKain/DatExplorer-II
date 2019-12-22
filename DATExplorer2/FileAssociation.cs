using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DATExplorer
{
    static class FileAssociation
    {
        [DllImport("shell32.dll", SetLastError = true)]
        private static extern void SHChangeNotify(int wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        
        private const string FILE_EXTENSION = ".dat";
        private const int SHCNE_ASSOCCHANGED = 0x8000000;
        private const uint SHCNF_IDLIST = 0x0U;
        
        private static readonly string appName = "FalloutDatExplorer";

        public static void Associate(bool force = false)
        {
            if (IsAssociated || MessageBox.Show("Do you want to associate the .dat file to the Dat Explorer?",
                "Associate file", MessageBoxButtons.YesNo) == DialogResult.No) {
                return;
            }

            // Удалить ранее ассоциированные с файлом разделы
            var value = Registry.ClassesRoot.CreateSubKey(FILE_EXTENSION).GetValue("");
            if (value != null) Registry.ClassesRoot.DeleteSubKeyTree(value.ToString(), false);
                
            // Создаем новый раздел
            Registry.ClassesRoot.CreateSubKey(FILE_EXTENSION).SetValue("", appName);
            using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(appName))
            {
                key.SetValue("", "Fallout Dat Explorer 2");
                key.SetValue("AlwaysShowExt", "");
                //key.CreateSubKey("DefaultIcon").SetValue("", Settings.ResourcesFolder + "\\icon_.ico");
                key.CreateSubKey("Shell").SetValue("", "OpenDatExplorer");
                key.CreateSubKey(@"Shell\OpenDatExplorer").SetValue("", "Fallout Dat Explorer");
                key.CreateSubKey(@"Shell\OpenDatExplorer\Command").SetValue("", Application.ExecutablePath + " \"%1\"");
            }
            SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
        }

        private static bool IsAssociated
        {
            get {
                string value = string.Empty;
                var reg = Registry.ClassesRoot.OpenSubKey(FILE_EXTENSION, false);
                if (reg != null) value = reg.GetValue("", string.Empty).ToString();  
                return (value == appName);
            }
        }
    }
}
