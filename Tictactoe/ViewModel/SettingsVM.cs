using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using Tictactoe.Commands;
using Tictactoe.Model;

namespace Tictactoe.ViewModel
{
	/// <summary>
	/// Provides access to application settings.
	/// </summary>
    public class SettingsVM : INotifyPropertyChanged {
		/// <summary>
		/// List of valid font sizes.
		/// </summary>
		public List<int> FontSizes { get; set; }
		/// <summary>
		/// List of valid game grid sizes (in squares).
		/// </summary>
		public List<int> GridSizes { get; set; }

		/// <summary>
		/// The command to execute when save is requested from the view.
		/// </summary>
		public ICommand SaveCommand { get; set; }
		/// <summary>
		/// The command to execute when exiting the settings menu.
		/// </summary>
		public ICommand ExitCommand { get; set; }

		private GameSettingsProvider settingsProvider;

		public int FontSize { get { return settingsProvider.FontSize; } set { settingsProvider.FontSize = value; } }
		public int GridSize { get { return settingsProvider.GridSize; } set { settingsProvider.GridSize = value; } }
		public int BoardSize { get { return settingsProvider.BoardSize; } }
		public SolidColorBrush BackgroundColor { get { return settingsProvider.BackgroundColor; } }
		public SolidColorBrush ButtonColor { get { return settingsProvider.ButtonColor; }  }
		public SolidColorBrush ButtonColorDisabled { get { return settingsProvider.ButtonColorDisabled; } }

		private bool m_settingsChanged;
		/// <summary>
		/// Property flag reflecting whether settings have been changed.
		/// Manually changing settings back to their original state does not reset this flag.
		/// </summary>
		public bool SettingsChanged { get { return m_settingsChanged; } private set { m_settingsChanged = value; OnPropertyChanged("SettingsChanged"); } }

		public SettingsVM() {
			settingsProvider = new GameSettingsProvider();

			FontSizes = settingsProvider.GetFontSizes();
			GridSizes = settingsProvider.GetGridSizes();

			SaveCommand = new SimpleCommand(settingsProvider.SaveSettings, () => { return SettingsChanged; });
			ExitCommand = new SimpleCommand(settingsProvider.ReloadSettings);

			settingsProvider.FontSizeUpdated += () => OnPropertyChanged("FontSize"); ;
			settingsProvider.GridSizeUpdated += () => OnPropertyChanged("GridSize"); ;
			settingsProvider.SettingChanged += () => SettingsChanged = true;
			settingsProvider.SettingsSaved += () => SettingsChanged = false;
		}

		#region PropertyChanged event
		private void OnPropertyChanged(string propertyName) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
