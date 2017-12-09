using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using Tictactoe.Model;

namespace Tictactoe.ViewModel.Tests {
	[TestClass()]
	public class GameVMTests {
		[TestMethod()]
		public void GameVMTest() {
			GameVM vm = new GameVM();
			// Viewmodel should have instantiated a board.
			Assert.IsNotNull(vm.Board);
		}

		[TestMethod()]
		public void TestGridButton() {
			GameVM vm = new GameVM();
			CellState initialturn = vm.Turn;
			GameCellVM testCell = vm.Board[0][0];

			vm.GridButtonCommand.Execute(testCell);

			Assert.AreEqual(initialturn,testCell.State);
			Assert.AreNotEqual(initialturn, vm.Turn);
		}

		[TestMethod()]
		public void TestRestart() {
			GameVM vm = new GameVM();
			ObservableCollection<ObservableCollection<GameCellVM>> board = vm.Board;

			vm.RestartGameCommand.Execute(null);

			Assert.AreNotEqual(board, vm.Board);
		}
	}
}