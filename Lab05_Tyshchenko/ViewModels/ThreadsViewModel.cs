using System.Diagnostics;
using Lab05_Tyshchenko.Models;

namespace Lab05_Tyshchenko.ViewModels
{
    class ThreadsViewModel
    {
        public ProcessThreadCollection Threads { get; }

        public ThreadsViewModel(MyProcess myProcess)
        {
            Threads = myProcess.Process.Threads;
        }
    }
}
