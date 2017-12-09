using System;
using System.Windows.Input;

namespace Tictactoe.Commands
{
	/// <summary>
	/// A command with no parameters.
	/// </summary>
	public class SimpleCommand : ICommand {
		Action execute;
		Func<bool> canExecute;

		public SimpleCommand(Action _execute, Func<bool> _canExecute = null) {
			execute = _execute;
			if (_canExecute == null)
				canExecute = () => { return true; };
			else
				canExecute = _canExecute;
		}

		public bool CanExecute(object _parameter) {
			return canExecute();
		}

		public void Execute(object _parameter) {
			execute();
		}

		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
	}
}
