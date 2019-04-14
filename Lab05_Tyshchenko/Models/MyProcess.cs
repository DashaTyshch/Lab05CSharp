using Lab05_Tyshchenko.Tools;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Lab05_Tyshchenko.Models
{
    public class MyProcess : INotifyPropertyChanged
    {
        #region Private Fields
        private bool _isActive;
        private double _cpu;
        private double _memory;
        private int _threadsCount;
        private bool _kill;

        private Thread _workingThread;
        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;
        #endregion

        public MyProcess(Process process)
        {
            Process = process;

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            StationManager.StopThreads += StopWorkingThread;
            _kill = false;
            Name = process.ProcessName;
            ID = process.Id;
            User = process.StartInfo.UserName;
            Folder = process.MainModule.FileName;
            LaunchDateTime = process.StartTime;
            StartWorkingThread();
        }

        public Process Process { get; }
        public string Name { get; }
        public int ID { get; }
        public string User { get; }
        public string Folder { get; }
        public DateTime LaunchDateTime { get; }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            private set
            {
                _isActive = value;
                InvokePropertyChanged(nameof(IsActive));
            }
        }

        public double CPU
        {
            get
            {
                return _cpu;
            }
            private set
            {
                _cpu = value;
                InvokePropertyChanged(nameof(CPU));
            }
        }

        public double Memory
        {
            get
            {
                return _memory;
            }
            private set
            {
                _memory = value;
                InvokePropertyChanged(nameof(Memory));
            }
        }

        public int ThreadsCount
        {
            get
            {
                return _threadsCount;
            }
            set
            {
                _threadsCount = value;
                InvokePropertyChanged(nameof(ThreadsCount));
            }
        }

        public bool Kill
        {
            get
            {
                return _kill;
            }
            set
            {
                _kill = value;
                StopWorkingThread();
                StationManager.StopThreads -= StopWorkingThread;
            }
        }

        private void StartWorkingThread()
        {
            _workingThread = new Thread(GetUsage);
            _workingThread.Start();
        }

        private void GetUsage()
        {

            while (!_token.IsCancellationRequested && !Kill)
            {
                try
                {
                    var cpu = new PerformanceCounter("Process", "% Processor Time", Process.ProcessName, true);
                    var ram = new PerformanceCounter("Process", "Private Bytes", Process.ProcessName, true);

                    cpu.NextValue();
                    ram.NextValue();

                    CPU = Math.Round(cpu.NextValue() / Environment.ProcessorCount, 2);

                    Memory = Math.Round(ram.NextValue() / 1024 / 1024, 2);
                    IsActive = Process.Responding;
                    ThreadsCount = Process.Threads.Count;
                }
                catch (Exception)
                {
                }
                for (int j = 0; j < 2; j++)
                {
                    Thread.Sleep(1000);
                    if (_token.IsCancellationRequested && Kill)
                        break;
                }
                if (_token.IsCancellationRequested && Kill)
                    break;
            }

        }

        private void StopWorkingThread()
        {
            _tokenSource.Cancel();
            _workingThread.Join(2000);
            _workingThread.Abort();
            _workingThread = null;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
        #endregion
    }
}
