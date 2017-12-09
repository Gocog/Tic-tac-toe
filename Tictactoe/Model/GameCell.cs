using System;

namespace Tictactoe.Model.Tictactoe
{
	public class GameCell {
		private int m_x;
		public int X { get { return m_x; } private set { m_x = value; } }
		private int m_y;
		public int Y { get { return m_y; } private set { m_y = value; } }

		private CellState m_state;
		public CellState State { get { return m_state; } private set { m_state = value; OnStateChanged(); } }

		public GameCell(int _x, int _y) : this (_x,_y,CellState.empty){}
		public GameCell(int _x, int _y, CellState _state) {
			X = _x;
			Y = _y;
			State = _state;
		}

		/// <summary>
		/// Gets the other player based on the player passed into this.
		/// </summary>
		/// <param name="thisPlayer">The current player. Must not be empty.</param>
		/// <exception cref="ArgumentException"/>
		/// <returns></returns>
		public static CellState GetOtherPlayer(CellState thisPlayer) {
			if (thisPlayer == CellState.empty) {
				throw new ArgumentException("Invalid cellstate for method GetOtherPlayer. Only non-empty cell-states can be inverted.");
			}
			CellState otherPlayer = thisPlayer == CellState.A ? CellState.B : CellState.A;
			return otherPlayer;
		}

		/// <summary>
		/// Ticks the cell if empty, giving it the state of the ticker. Does nothing
		/// if it was already ticked.
		/// </summary>
		/// <param name="ticker">The new desired state of the cell.</param>
		internal void Tick(CellState ticker) {
			// Don't change state if already ticked.
			if (State == CellState.empty) {
				State = ticker;
			}
		}

		#region Property changed events
		private void OnStateChanged() {
			Action<GameCell> handler = StateChanged;

			if (handler != null)
				handler(this);
		}
		public event Action<GameCell> StateChanged;
		#endregion
	}
}
