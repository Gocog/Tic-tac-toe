using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tictactoe.View.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tictactoe.Model;
using System.Windows;

namespace Tictactoe.Converter.Tests {
	[TestClass()]
	public class GameStateToVisibilityConverterTests {
		[TestMethod()]
		public void GameStateToVisibilityConverterTest() {
			GameStateToVisibilityConverter converter = new GameStateToVisibilityConverter();
			// Should be playing by default.
			Assert.AreEqual(GameState.playing, converter.VisibleValue);
		}

		[TestMethod()]
		public void ConvertTest() {
			GameStateToVisibilityConverter converter = new GameStateToVisibilityConverter();

			foreach (GameState teststate in Enum.GetValues(typeof(GameState))) {
				converter.VisibleValue = teststate;
				// Make sure that it returns Visible for the value set in VisibleValue.
				Assert.AreEqual(Visibility.Visible, converter.Convert(teststate, null, null, null));

				foreach (GameState negativeteststate in Enum.GetValues(typeof(GameState))) {
					if (negativeteststate == teststate)
						continue;
					// Make sure that it returns Collapsed for all other values.
					Assert.AreEqual(Visibility.Collapsed, converter.Convert(negativeteststate, null, null, null));
				}
			}
		}

		[TestMethod()]
		public void ConvertBackTest() {
			GameStateToVisibilityConverter converter = new GameStateToVisibilityConverter();

			// Converting back is not supported and should throw NotSupportedException.
			Assert.ThrowsException<NotSupportedException>(() => converter.ConvertBack(null,null,null,null));
		}
	}
}