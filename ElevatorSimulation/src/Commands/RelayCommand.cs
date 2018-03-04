using System;
using System.Windows.Input;

namespace ElevatorSimulation.Commands
{
    /// <summary>
    /// Base class for wpf commands
    /// </summary>
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        public RelayCommand(Action<object> action)
        {
            ExecuteDelegate = action;
        }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
            {
                return CanExecuteDelegate(parameter);
            }

            return true;
        }
        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
            {
                ExecuteDelegate(parameter);
            }
        }
    }
}
