using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tictactoe.Model.Tictactoe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tictactoe.Model.Tictactoe.Tests {
	[TestClass()]
	public class GameCellTests {
		[TestMethod()]
		public void GameCellTickTest() {
			GameCell cell = new GameCell(0,0);

			CellState testStateEmpty = CellState.empty;
			CellState testStateA = CellState.A;
			CellState testStateB = CellState.B;

			// New cells should be blank.
			Assert.AreEqual(testStateEmpty, cell.State);

			// Ticking a cell should apply the state.
			cell.Tick(testStateA);
			Assert.AreEqual(testStateA, cell.State);

			// But not if the cell has already been ticked.
			cell.Tick(testStateB);
			Assert.AreNotEqual(testStateB, cell.State);
		}

		[TestMethod()]
		public void OtherPlayerTest() {
			// Make sure GetOtherPlayer returns the opposite player state.
			Assert.AreEqual(CellState.B, GameCell.GetOtherPlayer(CellState.A));
			Assert.AreEqual(CellState.A, GameCell.GetOtherPlayer(CellState.B));

			// Make sure we reject empty calls to GetOtherPlayer.
			Assert.ThrowsException<ArgumentException>(() => { GameCell.GetOtherPlayer(CellState.empty); });
		}
	}
}