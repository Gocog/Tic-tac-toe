using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Tictactoe.Model;

namespace Tictactoe.View.Converters {
	/// <summary>
	/// Converts between GameState and string, for displaying cellstates in the View.
	/// </summary>
	public class GameStateToVisibilityConverter : IValueConverter {

		public GameState VisibleValue { get; set; }

		public GameStateToVisibilityConverter() {
			VisibleValue = GameState.playing;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			GameState state = (GameState)value;
			return state == VisibleValue ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotSupportedException("Cannot convert from Visibility to GameStateEnum.");
		}
	}
}
