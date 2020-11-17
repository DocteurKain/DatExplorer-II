using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DATExplorer
{
    static class Misc
    {
        internal static string GetNodeFullPath(TreeNode node)
        {
            string full = node.FullPath;
            return full.Remove(0, full.IndexOf("]") + 2);
        }

        /// <summary>
        /// Возвращает имя и путь к файлу
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal static string GetDatName(TreeNode node)
        {
            return (node.Parent == null) ? node.Name : GetRootNode(node.Parent).Name;
        }

        internal static TreeNode GetRootNode(TreeNode node)
        {
            if (node.Parent != null) {
                return GetRootNode(node.Parent);
            }
            return node;
        }

        // рекурсивный поиск
        internal static TreeNode FindNode(string name, TreeNode node)
        {
            foreach (TreeNode nd in node.Nodes)
            {
                if (nd.Text == name) {
                    return nd;
                }
                TreeNode find = FindNode(name, nd);
                if (find != null) return find;
            }
            return null;
        }
    }
}
