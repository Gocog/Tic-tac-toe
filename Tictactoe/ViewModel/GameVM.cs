using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Tictactoe.Commands;
using Tictactoe.Model;
using Tictactoe.Model.Tictactoe;

namespace Tictactoe.ViewModel
{
		/// <summary>
		/// Handles presentation of a tic tac toe game board. Offers bindable properties for the board's
		/// dimensions, viewmodels for board cells, and relevant commands for ui interactions.
		/// </summary>
		public class GameVM : INotifyPropertyChanged {

		private GameBoard gameBoard;
		private ObservableCollection<ObservableCollection<GameCellVM>> m_board;
		/// <summary>
		/// The collection of GameCell viewmodels to expose to the view.
		/// </summary>
		public ObservableCollection<ObservableCollection<GameCellVM>> Board { get { return m_board; } private set { m_board = value; OnPropertyChanged("Board"); } }

		private CellState m_turn;
		public CellState Turn { get { return m_turn; } private set { m_turn = value; OnPropertyChanged("Turn"); } }

		private GameState m_status;
		public GameState Status { get { return m_status; } private set { m_status = value; OnPropertyChanged("Status"); } }

		/// <summary>
		/// The command to execute when a cell is activated in the view.
		/// </summary>
		public ICommand GridButtonCommand { get; private set; }
		/// <summary>
		/// The command to execute when a restart is requested by the view.
		/// </summary>
		public ICommand RestartGameCommand { get; private set; }

		public GameVM() {
			// Set command bound to grid buttons.
			GridButtonCommand = new CommandWithParameter<GameCellVM>(c => PlayCell(c), c => CanPlay(c));
			// Set command for restarting game.
			RestartGameCommand = new SimpleCommand(NewGame);

			// Start a new game.
			NewGame();
		}

		/// <summary>
		/// Creates a new board according to the settings and sets it as our board..
		/// </summary>
		private void NewGame() {
			GameSettingsProvider settingsProvider = new GameSettingsProvider();

			int boardSize = Properties.Settings.Default.BoardSize;
			int gridSize = settingsProvider.GridSize;
			int gridCellSize = boardSize / gridSize;
			int gridFontSize = gridCellSize / 2;

			gameBoard = new GameBoard(gridSize);
			gameBoard.NewTurn += (cs) => Turn = cs;
			gameBoard.StatusChanged += (gs) => Status = gs;
			Turn = gameBoard.Turn;
			Status = gameBoard.Status;
			Board = new ObservableCollection<ObservableCollection<GameCellVM>>();
			
			for (int x = 0; x < gameBoard.GridSize; x++) {
				Board.Add(new ObservableCollection<GameCellVM>());
				for (int y = 0; y < gameBoard.GridSize; y++) {
					GameCellVM newCell = new GameCellVM(gameBoard.Board[x,y],gridFontSize,gridCellSize);
					Board[x].Add(newCell);
				}
			}
		}

		/// <summary>
		/// Tell the board to Tick the cell.
		/// </summary>
		/// <param name="cell">The cell to tick.</param>
		private void PlayCell(GameCellVM cell) {
			// Tell the board to tick the selected cell.
			gameBoard.Tick(cell.X,cell.Y);
		}

		/// <summary>
		/// Checks if a cell can be played. Only empty cells can be played,
		/// and no cells can be played if the game is not in progress.
		/// </summary>
		/// <param name="cell">The cell to check.</param>
		/// <returns></returns>
		private bool CanPlay(GameCellVM cell) {
			// A cell can be played if it is empty. A cell cannot be player after the game is finished.
			return cell.State == CellState.empty && gameBoard.Status == GameState.playing;
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
