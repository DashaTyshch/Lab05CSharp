using Lab05_Tyshchenko.Windows;
using System;
using System.Diagnostics;

namespace Lab05_Tyshchenko.Models
{
    class MainModel
    {
        public void OpenFolder(MyProcess selectedProcess)
        {
            Process.Start(System.IO.Path.GetDirectoryName(selectedProcess.Folder));
        }

        public void Terminate(MyProcess selectedProcess)
        {
            selectedProcess.Process.Kill();
        }

        public void OpenModules(MyProcess selectedProcess)
        {
            ModulesWindow modules = new ModulesWindow(selectedProcess);
            modules.Show();
        }
    }
}
