using Lab05_Tyshchenko.Models;
using System.Diagnostics;

namespace Lab05_Tyshchenko.ViewModels
{
    public class ModulesViewModel
    {
        public ProcessModuleCollection Modules { get; }

        public ModulesViewModel(MyProcess myProcess)
        {
            Modules = myProcess.Process.Modules;
        }
    }
}
