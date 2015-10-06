using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;

namespace DownloadsManager.ViewModels.Infrastructure
{
    /// <summary>
    /// Implementation of command pattern
    /// </summary>
    public class Command : ICommand
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="action">action to execute</param>
        public Command(Action<object> action)
        {
            ExecuteDelegate = action;
        }

        /// <summary>
        /// event for canExecite prop changed
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Name of command
        /// </summary>
        public string Name { get; set; }
               
        /// <summary>
        /// Exeute to delegate
        /// </summary>
        public Action<object> ExecuteDelegate { get; set; }

        /// <summary>
        /// Chek if we can execute command
        /// </summary>
        /// <param name="parameter">command parameter</param>
        /// <returns>true if we can execute false if not</returns>
        public bool CanExecute(object parameter)
        {
            return ExecuteDelegate != null;
        }

        /// <summary>
        /// Method for executing command action
        /// </summary>
        /// <param name="parameter">command parameter</param>
        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
            {
                ExecuteDelegate(parameter);
            }
        }
    }
}
