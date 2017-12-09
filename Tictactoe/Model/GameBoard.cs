using System;
using System.Collections.Generic;

namespace Tictactoe.Model.Tictactoe {
	/// <summary>
	/// A model for a game of tic-tac-toe, with arbitrary board size. The game is played
	/// by ticking cells according to their coordinate on the board. Victory is determined
	/// when all cells in a row, column or corner-to-corner diagonal are ticked by one player.
	/// A draw is called when there are no options for victory left on the board.
	/// </summary>
	public class GameBoard {
		#region Properties
		private GameCell[,] m_board;
		/// <summary>
		/// 2D array of GameCells representing the board.
		/// </summary>
		public GameCell[,] Board { get { return m_board; } private set { m_board = value; } }
		/// <summary>
		/// Gets the length of each side of the array.
		/// </summary>
		public int GridSize { get { return Board.GetLength(0); } }
		/// <summary>
		/// The initial number of ways to win the game.
		/// </summary>
		private int InitialWinOptions { get { return (GridSize * 2) + 2; } }
		private CellState m_turn;
		/// <summary>
		/// Indicates whose turn it is.
		/// </summary>
		public CellState Turn { get { return m_turn; } private set { m_turn = value; OnNewTurn(value); } }
		private GameState m_status;
		/// <summary>
		/// Indicates whether the game is in progress, a player has won or the game is a draw.
		/// </summary>
		public GameState Status { get { return m_status; } private set { m_status = value; OnStatusChanged(value); } }
		#endregion
		#region Private fields
		private WinOption[][] winoptions;
		private Dictionary<CellState, int> remainingOptions;
		private int Disqualifications { get { return remainingOptions[CellState.empty]; } }
		#endregion

		/// <summary>
		/// Initialises a new board.
		/// </summary>
		/// <param name="_gridSize">The board's size in each dimension.</param>
		public GameBoard(int gridSize) {
			Board = InitialiseBoard(gridSize);
			winoptions = InitialiseWinOptions();
			remainingOptions = InitialiseRemainingOptions();

			// Add listener on board's GameWon event
			GameWon += (cs) => { Status = GameState.victory; };
			GameDraw += () => { Status = GameState.draw; };

			// Initialise game state
			Status = GameState.playing;
			Turn = CellState.A;
		}

		/// <summary>
		/// Generates a new two-dimensional list of GameCells to be used as a board.
		/// </summary>
		/// <param name="gridSize">The board's size in each dimension.</param>
		private GameCell[,] InitialiseBoard(int gridSize) {
			GameCell[,] board = new GameCell[gridSize,gridSize];
			for (int x = 0; x < gridSize; x++) {
				for (int y = 0; y < gridSize; y++) {
					GameCell newCell = new GameCell(x, y);
					board[x,y] = newCell;
					newCell.StateChanged += UpdateBoard;
				}
			}
			return board;
		}

		/// <summary>
		/// Generates a jagged array, listing all the ways to win the game.
		/// The outer array represents the directions (horizontal, vertical and both diagonals)
		/// that a player can win by filling out. The inner array represents the different options
		/// for winning in that direction; I.e. in the horizontal direction, you have one option
		/// per row.
		/// </summary>
		/// <returns>The array of win options.</returns>
		private WinOption[][] InitialiseWinOptions() {
			WinOption[][] newCandidates = new WinOption[4][];
			newCandidates[(int)Direction.horizontal] = new WinOption[GridSize];
			newCandidates[(int)Direction.vertical] = new WinOption[GridSize];
			newCandidates[(int)Direction.down_right] = new WinOption[1];
			newCandidates[(int)Direction.down_left] = new WinOption[1];
			return newCandidates;
		}

		/// <summary>
		/// Generates a dictionary from CellState to int, mapping the number of remaining options
		/// for each player. The empty state mapping contains the number of options that have been disqualified.
		/// </summary>
		/// <returns>The new dictionary with all options remaining for all players and no disqualifications.</returns>
		private Dictionary<CellState,int> InitialiseRemainingOptions() {
			Dictionary<CellState, int> options = new Dictionary<CellState, int> {
				[CellState.A] = InitialWinOptions,
				[CellState.B] = InitialWinOptions,
				[CellState.empty] = 0
			};
			return options;
		}

		/// <summary>
		/// Ticks a cell on the board according to whose turn it is.
		/// </summary>
		/// <param name="x">The x-coordinate of the cell.</param>
		/// <param name="y">The y-coordinate of the cell.</param>
		public void Tick(int x, int y) {
			Board[x,y].Tick(Turn);
		}

		/// <summary>
		/// Updates the board after a cell has been ticked. Raises win or draw events
		/// if the tick resulted in a win or draw, advances to the next turn otherwise.
		/// </summary>
		/// <param name="tickedCell">The ticked cell.</param>
		private void UpdateBoard(GameCell tickedCell) {
			// Go through each direction.
			for (int direction = 0; direction < 4; direction++) {
				// Find appropriate index for this direction.
				int idx = direction == (int)Direction.horizontal ?
					tickedCell.Y :
					direction == (int)Direction.vertical ?
						tickedCell.X :
						0;

				// Get the win option at the current index of the current direction.
				ref WinOption potentialWinOption = ref winoptions[direction][idx];

				// Don't consider disqualified options.
				if (potentialWinOption.disqualified)
					continue;

				// Disregard down-right option if we are not on that diagonal.
				if (direction == (int)Direction.down_right && !(tickedCell.X == tickedCell.Y))
					continue;

				// Disregard down-left option if we are not on that diagonal.
				if (direction == (int)Direction.down_left && !(tickedCell.X == (GridSize - 1 - tickedCell.Y)))
					continue;

				// Claim this option if unclaimed.
				if (potentialWinOption.candidate == CellState.empty) {
					// The other player loses a win option if we claim a direction.
					RemoveOption(GameCell.GetOtherPlayer(tickedCell.State));
					potentialWinOption.candidate = tickedCell.State;
				}

				// If this option belongs to us, increment. Otherwise disqualify.
				if (potentialWinOption.candidate == tickedCell.State) {
					potentialWinOption.count++;
					// If we have filled this win option with our ticks, we have won.
					if (potentialWinOption.count == GridSize) {
						OnGameWon(tickedCell.State);
						return;
					}
				} else {
					// If this option was already claimed by the other player, it is now disqualified.
					Disqualify(ref potentialWinOption);

					// The game is a draw if all win-options have been disqualified.
					// The game is also a draw if there is one move remaining, and the next player has no win options.
					if (Disqualifications == InitialWinOptions ||
						Disqualifications == InitialWinOptions-1 && remainingOptions[GameCell.GetOtherPlayer(tickedCell.State)] == 0) {
						OnGameDraw();
						return;
					}
				}
			}

			// If the game is still going, go to the next turn.
			Turn = GameCell.GetOtherPlayer(Turn);
		}

		/// <summary>
		/// Disqualifies this options from leading to a win.
		/// </summary>
		/// <param name="winoption">The option to disqualify.</param>
		private void Disqualify(ref WinOption winoption) {
			RemoveOption(winoption.candidate);
			winoption.disqualified = true;
			remainingOptions[CellState.empty]++;
		}

		/// <summary>
		/// Removes one win option for this player.
		/// </summary>
		/// <param name="myState">The CellState of the targeted player.</param>
		private void RemoveOption(CellState target) {
			remainingOptions[target] -= 1;
		}

		#region Game events
		private void OnGameDraw() {
			Action handler = GameDraw;
			if (handler != null) {
				handler();
			}
		}

		private void OnGameWon(CellState winner) {
			Action<CellState> handler = GameWon;
			if (handler != null) {
				handler(winner);
			}
		}

		private void OnNewTurn(CellState newTurnPlayer) {
			Action<CellState> handler = NewTurn;
			if (handler != null) {
				handler(newTurnPlayer);
			}
		}

		private void OnStatusChanged(GameState newStatus) {
			Action<GameState> handler = StatusChanged;
			if (handler != null) {
				handler(newStatus);
			}
		}

		public event Action GameDraw;
		public event Action<CellState> GameWon;
		public event Action<CellState> NewTurn;
		public event Action<GameState> StatusChanged;
		#endregion
		#region Private enums
		private enum Direction {
			horizontal,
			vertical,
			down_right,
			down_left
		}

		private struct WinOption {
			public bool disqualified;
			public CellState candidate;
			public int count;
		}
		#endregion

	}	
}
