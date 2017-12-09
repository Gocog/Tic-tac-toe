using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tictactoe.Model.Tictactoe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tictactoe.Model.Tictactoe.Tests {
	[TestClass()]
	public class GameBoardTests {
		[TestMethod()]
		public void GameBoardTest() {
			for (int testsize = 3; testsize < 9; testsize++) {
				GameBoard gb = new GameBoard(testsize);
				// The new board should have the size it was set to
				Assert.AreEqual(testsize, gb.GridSize);

				// Ensure all cells are empty on new board.
				for (int x = 3; x < testsize; x++) {
					for (int y = 3; y < testsize; y++) {
						Assert.IsTrue(gb.Board[x,y].State == CellState.empty);
					}
				}
			}
		}

		[TestMethod()]
		public void BoardTickTest() {
			GameBoard gb = new GameBoard(3);
			CellState firstPlayer = gb.Turn;
			gb.Tick(0, 0);
			// Make sure cell was ticked.
			Assert.AreEqual(firstPlayer,gb.Board[0,0].State);

			// Make sure it's now the second player's turn.
			Assert.AreNotEqual(firstPlayer, gb.Turn);

			gb.Tick(0, 0);
			// Make sure cell was not re-ticked by second player.
			Assert.AreEqual(firstPlayer, gb.Board[0,0].State);
			// Make sure it's still the second player's turn.
			Assert.AreNotEqual(firstPlayer, gb.Turn);
		}

		[TestMethod()]
		public void TestWin() {
			CellState testState = CellState.A;
			for (int testsize = 3; testsize < 9; testsize++) {
				// Goes through each direction and ensures all possible wins lead to win.
				for (int direction = 0; direction < 4; direction++) {
					// First coordinate.
					for (int a = 0; a < testsize; a++) {

						GameBoard gb = new GameBoard(testsize);
						CellState winner = CellState.empty;
						bool draw = false;
						gb.GameWon += (cs) => { winner = cs; };
						gb.GameDraw += () => { draw = true; };

						for (int b = 0; b < testsize; b++) {
							int[] xvals = { a, b, b, b };
							int[] yvals = { b, a, b, testsize - 1 - b };
							int x = xvals[direction];
							int y = yvals[direction];

							// We are not supposed to have won yet.
							Assert.AreNotEqual(testState, winner);

							// Tick cells directly when testing this to avoid unneccessary complications.
							gb.Board[x,y].Tick(testState);
						}
						// Now we should have won.
						Assert.AreEqual(GameState.victory, gb.Status);
						Assert.AreEqual(testState, winner);
						
						// There should not have been a draw.
						Assert.IsFalse(draw);
					}
				}
			}
		}

		[TestMethod()]
		public void TestDraw() {
			// Uses three known draw-sequences to test if the game draws.
			// Tests only 3x3 board.

			int testsize = 3;
			int[][] xsequences = new int[3][];
			int[][] ysequences = new int[3][];
			// Draw sequence 0
			/*	How it looks:
			 *	OXO
			 *  XOX
			 *  XOX
			 */
			xsequences[0] = new int[] { 0, 0, 1, 1, 2, 2, 0, 1, 2 };
			ysequences[0] = new int[] { 1, 0, 0, 1, 1, 0, 2, 2, 2 };

			// Draw sequence 1
			/*	How it looks:
			 *	OXO
			 *  OXX
			 *  XOX
			 */
			xsequences[1] = new int[] { 0, 0, 1, 1, 2, 2, 2, 0, 1};
			ysequences[1] = new int[] { 2, 1, 1, 2, 1, 0, 2, 0, 0};

			// Draw sequence 2
			/*	How it looks:
			 *	OXX
			 *  XOO
			 *  XOX
			 */
			xsequences[2] = new int[] { 0, 0, 1, 1, 2, 2, 0, 1, 2};
			ysequences[2] = new int[] { 1, 0, 0, 1, 0, 1, 2, 2, 2};

			for (int test = 0; test < 3; test++) {
				GameBoard gb = new GameBoard(testsize);
				CellState winner = CellState.empty;
				bool draw = false;
				gb.GameWon += (cs) => { winner = cs; };
				gb.GameDraw += () => { draw = true; };

				/* Note that we end on the second-to-last turn. The last turn will only
				 * have one option, and that option leads to draw, so the draw should
				 * already have been called by then.
				 */
				for (int turn = 0; turn < 8; turn++) {
					gb.Tick(xsequences[test][turn],ysequences[test][turn]);
				}

				// Now we should have had a draw.
				Assert.IsTrue(draw);
				// There should not have been a win.
				Assert.AreEqual(CellState.empty, winner);
			}
		}
	}
}