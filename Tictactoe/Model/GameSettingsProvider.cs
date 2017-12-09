using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace Tictactoe.Model {
	/// <summary>
	/// Provides a layer of abstraction for setting and getting game settings.
	/// </summary>
	public class GameSettingsProvider {
		#region Bindable properties
		public int FontSize { get { return Properties.Settings.Default.FontSize; } set { Properties.Settings.Default.FontSize = value; } }
		public int GridSize { get { return Properties.Settings.Default.GridSize; } set { Properties.Settings.Default.GridSize = value; } }
		public int BoardSize { get { return Properties.Settings.Default.BoardSize; } }
		public SolidColorBrush BackgroundColor { get { return Properties.Settings.Default.BackgroundColor; } }
		public SolidColorBrush ButtonColor { get { return Properties.Settings.Default.ButtonColor; } }
		public SolidColorBrush ButtonColorDisabled { get { return Properties.Settings.Default.ButtonColorDisabled; } }
		#endregion

		/// <summary>
		/// Dictionary from string to Func, to access events based on property names.
		/// </summary>
		Dictionary<string, Func<Action>> settingsUpdateEvents;

		public GameSettingsProvider() {
			settingsUpdateEvents = InitialiseSettingsUpdateEvents();
			Properties.Settings.Default.PropertyChanged += OnSettingPropertyChanged;
		}

		/// <summary>
		/// Returns a dictionary from setting names to update events.
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, Func<Action>> InitialiseSettingsUpdateEvents() {
			return new Dictionary<string, Func<Action>> {
				["FontSize"] = () => { return FontSizeUpdated; },
				["GridSize"] = () => { return GridSizeUpdated; },
				["BoardSize"] = () => { return BoardSizeUpdated; },
				["BackgroundColor"] = () => { return BackgroundColorUpdated; },
				["ButtonColor"] = () => { return ButtonColorUpdated;},
				["ButtonColorDisabled"] = () => { return ButtonColorDisabledUpdated; }
			};
		}

		/// <summary>
		/// Gets a list of valid font sizes according to application settings.
		/// Validates minimum size to avoid 0 or negative values.
		/// </summary>
		/// <returns>List of valid grid sizes.</returns>
		public List<int> GetFontSizes() {
			List<int> newFontSizes = new List<int>();

			// Read minimum and maximum font sizes from settings.
			int minfontsize = Properties.Settings.Default.MinimumFontSize;
			int maxfontsize = Properties.Settings.Default.MaximumFontSize;

			// Prevent zero/negative sizes.
			if (minfontsize < 1) {
				minfontsize = 1;
			}

			// Smaller steps for small values
			for (int i = minfontsize; i <= 28 && i <= maxfontsize; i += 2) {
				newFontSizes.Add(i);
			}

			// Larger steps for large values
			for (int i = 36; i <= 72 && i <= maxfontsize; i += 12) {
				newFontSizes.Add(i);
			}

			// Largest steps for largest values
			for (int i = 96; i <= maxfontsize; i += 24) {
				newFontSizes.Add(i);
			}

			return newFontSizes;
		}

		/// <summary>
		/// Gets a list of valid grid sizes according to application settings.
		/// Validates minimum size to avoid 0 or negative values.
		/// </summary>
		/// <returns>List of valid grid sizes.</returns>
		public List<int> GetGridSizes() {
			List<int> newGridSizes = new List<int>();
			int mingridsize = Properties.Settings.Default.MinimumGridSize;
			int maxgridsize = Properties.Settings.Default.MaximumGridSize;

			// Prevent zero/negative sizes.
			mingridsize = mingridsize > 1 ? mingridsize : 1;

			for (int i = mingridsize; i < maxgridsize; i++) {
				newGridSizes.Add(i);
			}

			return newGridSizes;
		}

		/// <summary>
		/// Saves the application's current settings.
		/// </summary>
		public void SaveSettings() {
			Properties.Settings.Default.Save();
			OnSettingSaved();
		}

		/// <summary>
		/// Reload settings and broadcast their values.
		/// </summary>
		public void ReloadSettings() {
			Properties.Settings.Default.Reload();
			foreach (string setting in settingsUpdateEvents.Keys) {
				OnSettingUpdated(setting);
			}
		}

		#region Events
		/// <summary>
		/// Handles PropertyChanged events in settings, raising the appropriate event in this viewmodel.
		/// </summary>
		/// <param name="sender">Event source.</param>
		/// <param name="e">Property changed event arguments.</param>
		private void OnSettingPropertyChanged(object sender, PropertyChangedEventArgs e) {
			OnSettingUpdated(e.PropertyName);
		}

		/// <summary>
		/// Raises an event to indicate the setting has changed. Also raises a general
		/// event that indicates a setting has changed.
		/// </summary>
		/// <param name="setting">The setting that has changed.</param>
		private void OnSettingUpdated(string setting) {
			Action handler = GetUpdateEvent(setting);

			if (handler != null)
				handler();

			if (SettingChanged != null)
				SettingChanged();
		}

		/// <summary>
		/// Looks up the change event for a setting based on name.
		/// </summary>
		/// <param name="setting">The name of the setting.</param>
		/// <returns>Event to be raised when setting is changed.</returns>
		private Action GetUpdateEvent(string setting) {
			if (settingsUpdateEvents.ContainsKey(setting)) {
				return settingsUpdateEvents[setting]();
			}
			return null;
		}

		/// <summary>
		/// Raises the SettingsChanged event.
		/// </summary>
		private void OnSettingSaved() {
			if (SettingsSaved != null)
				SettingsSaved();
		}

		public event Action FontSizeUpdated;
		public event Action GridSizeUpdated;
		public event Action BoardSizeUpdated;
		public event Action BackgroundColorUpdated;
		public event Action ButtonColorUpdated;
		public event Action ButtonColorDisabledUpdated;

		public event Action SettingChanged;
		public event Action SettingsSaved;
		#endregion
	}
}
