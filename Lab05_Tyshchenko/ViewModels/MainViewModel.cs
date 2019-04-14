using Lab05_Tyshchenko.Models;
using Lab05_Tyshchenko.Tools;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace Lab05_Tyshchenko.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        #region Private Fields
        private MainModel Model { get; }

        private Thread _workingThread;
        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;
        private bool _isControlEnabled;

        private ICommand _openModules;
        private ICommand _openThreads;
        private ICommand _openFolder;
        private ICommand _terminate;
        #endregion

        #region Properties
        public ObservableCollection<MyProcess> Processes { get; private set; }
        public MyProcess SelectedProcess { get; set; }

        public bool IsControlEnabled
        {
            get
            {
                return _isControlEnabled;
            }
            set
            {
                _isControlEnabled = value;
                InvokePropertyChanged(nameof(IsControlEnabled));
            }
        }
        #endregion

        public MainViewModel()
        {
            Model = new MainModel();

            Processes = new ObservableCollection<MyProcess>();
            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    Processes.Add(new MyProcess(process));
                }
                catch (Exception)
                {

                }
            }

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;

            StationManager.StopThreads += StopWorkingThread;

            IsControlEnabled = true;
            Thread.Sleep(5000);

            StartWorkingThread();
        }

        #region Commands
        public ICommand OpenFolder
        {
            get
            {
                if (_openFolder == null)
                {
                    _openFolder = new RelayCommand<object>(OpenFolderExecute, OpenFolderCanExecute);
                }
                return _openFolder;
            }
            set
            {
                _openFolder = value;
                InvokePropertyChanged(nameof(OpenFolder));
            }
        }

        private bool OpenFolderCanExecute(object obj)
        {
            return true;
        }

        private void OpenFolderExecute(object obj)
        {
            Model.OpenFolder(SelectedProcess);
        }

        public ICommand Terminate
        {
            get
            {
                if (_terminate == null)
                {
                    _terminate = new RelayCommand<object>(TerminateExecute, TerminateCanExecute);
                }
                return _terminate;
            }
            set
            {
                _terminate = value;
                InvokePropertyChanged(nameof(Terminate));
            }
        }

        private bool TerminateCanExecute(object obj)
        {
            return true;
        }

        private void TerminateExecute(object obj)
        {
            Model.Terminate(SelectedProcess);
        }

        public ICommand OpenModules
        {
            get
            {
                if (_openModules == null)
                {
                    _openModules = new RelayCommand<object>(OpenModulesExecute, OpenModulesCanExecute);
                }
                return _openModules;
            }
            set
            {
                _openModules = value;
                InvokePropertyChanged(nameof(OpenModules));
            }
        }

        private bool OpenModulesCanExecute(object obj)
        {
            return true;
        }

        private void OpenModulesExecute(object obj)
        {
            Model.OpenModules(SelectedProcess);
        }
        #endregion

        #region Private methods
        private void StartWorkingThread()
        {
            _workingThread = new Thread(WorkingThreadProcess);
            _workingThread.Start();
        }

        private void WorkingThreadProcess()
        {

            while (!_token.IsCancellationRequested)
            {
                for (int j = 0; j < 5; j++)
                {
                    Thread.Sleep(1000);
                    if (_token.IsCancellationRequested)
                        break;
                }
                if (_token.IsCancellationRequested)
                    break;

                IsControlEnabled = false;

                var t = Process.GetProcesses().Select(p => p.Id).ToList();
                var currentProcesses = Processes.Select(p => p.ID).ToList();
                var newProcesses = t.Except(currentProcesses).ToList();
                var processesToRemove = currentProcesses.Except(t).ToList();

                foreach (int id in newProcesses)
                {
                    try
                    {
                        App.Current.Dispatcher.Invoke(delegate
                        {
                            Processes.Add(new MyProcess(Process.GetProcessById(id)));
                        });
                    }
                    catch (Exception)
                    {
                    }
                }
                foreach (int id in processesToRemove)
                {
                    try
                    {
                        var myProcess = Processes.First(x => x.ID == id);

                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            myProcess.Kill = true;
                            Processes.RemoveAt(Processes.IndexOf(myProcess));
                        });
                    }
                    catch (Exception)
                    {
                    }
                }

                IsControlEnabled = true;
            }
        }

        private void StopWorkingThread()
        {
            _tokenSource.Cancel();
            _workingThread.Join(2000);
            _workingThread.Abort();
            _workingThread = null;
        }
        #endregion

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
