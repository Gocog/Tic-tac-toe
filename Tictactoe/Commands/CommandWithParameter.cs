using System;
using System.Windows.Input;

namespace Tictactoe.Commands
{
	/// <summary>
	/// A command that takes a single parameter.
	/// </summary>
	/// <typeparam name="T">The type of the parameter.</typeparam>
	public class CommandWithParameter<T> : ICommand
	{
		Action<T> execute;
		Func<T, bool> canExecute;

		public CommandWithParameter(Action<T> _execute, Func<T, bool> _canExecute = null) {
			execute = _execute;
			if (_canExecute == null)
				canExecute = (t) => { return true; };
			else
				canExecute = _canExecute;
		}

		public bool CanExecute(object _parameter) {
			T parameter = (T)_parameter;
			return canExecute(parameter);
		}

		public void Execute(object _parameter) {
			T parameter = (T)_parameter;
			execute(parameter);
		}

		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
	}
}
