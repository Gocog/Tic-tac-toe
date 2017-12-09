using System.ComponentModel;
using Tictactoe.Model;
using Tictactoe.Model.Tictactoe;

namespace Tictactoe.ViewModel
{
	/// <summary>
	/// Viewmodel for game cells.
	/// </summary>
	public class GameCellVM : INotifyPropertyChanged{
		private int m_x;
		public int X { get { return m_x; } private set { m_x = value; } }
		private int m_y;
		public int Y { get { return m_y; } private set { m_y = value; } }

		private int m_cellSize;
		/// <summary>
		/// The size of each cell on the board, in pixels.
		/// </summary>
		public int CellSize { get { return m_cellSize; } private set { m_cellSize = value; } }

		private int m_fontSize;
		/// <summary>
		/// The font size in the cells on the board.
		/// </summary>
		public int FontSize { get { return m_fontSize; } private set { m_fontSize = value; } }

		private CellState m_state;
		public CellState State { get { return m_state; } private set { m_state = value; OnPropertyChanged("State"); } }

		public GameCellVM(GameCell cell, int fontSize, int cellSize) {
			CellSize = cellSize;
			FontSize = fontSize;
			X = cell.X;
			Y = cell.Y;

			State = cell.State;
			// Update viewmodel when model changes state.
			cell.StateChanged += (c) => { State = c.State; };
		}

		#region Property changed events
		private void OnPropertyChanged(string propertyName) {
			PropertyChangedEventHandler handler = PropertyChanged;

			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
