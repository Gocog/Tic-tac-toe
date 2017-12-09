using System;
using System.ComponentModel;
using System.Windows.Input;
using Tictactoe.Commands;

namespace Tictactoe.ViewModel
{
	/// <summary>
	/// Viewmodel for the main window, offering navigation functionality for the application window.
	/// </summary>
	public class MainWindowVM : INotifyPropertyChanged {
		/// <summary>
		/// The command to execute when navigation is requested in the view.
		/// </summary>
		public ICommand GoToPageCommand { get; set; }

		private Uri m_currentPage;
		/// <summary>
		/// Bindable property for the current page's address. Listening to this property in the view
		/// lets the view know when to navigate to a new page, and where to go.
		/// </summary>
		public Uri CurrentPage { get { return m_currentPage; } set { m_currentPage = value; OnPropertyChanged("CurrentPage"); } }

		public MainWindowVM() {
			GoToPageCommand = new CommandWithParameter<string>(GoToPage);
			GoToPage("/View/StartPage.xaml");
		}
		
		/// <summary>
		/// Changes the current page to the one specified by the supplied string.
		/// </summary>
		/// <param name="uriString">A string representing the relative uri of the new page.</param>
		private void GoToPage(string uriString) {
			try {
				Uri newPage = new Uri(uriString, UriKind.Relative);
				CurrentPage = newPage;
			} catch (Exception e) {
				Console.Error.WriteLine(
					"Failed to request new page because of exception!/n" +
					e.StackTrace
				);
			}
		}

		#region PropertyChanged event
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName) {
			PropertyChangedEventHandler handler = PropertyChanged;

			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
