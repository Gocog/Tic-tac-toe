using System;
using System.Globalization;
using System.Windows.Data;
using Tictactoe.Model;

namespace Tictactoe.View.Converters {
	/// <summary>
	/// Converts between CellState and string, for displaying CellStates in the View.
	/// </summary>
	public class CellStateToStringConverter : IValueConverter {
		public string A { get; set; }
		public string B { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			CellState state = (CellState)value;
			return state == CellState.empty ? "" : state == CellState.A ? A : B;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			string icon = (string)value;
			return icon == A ? CellState.A : icon == B ? CellState.B : CellState.empty;
		}
	}
}
