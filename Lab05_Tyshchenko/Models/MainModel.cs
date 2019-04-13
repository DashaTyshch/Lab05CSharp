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
    }
}
