using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tictactoe.View.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tictactoe.Model;

namespace Tictactoe.Converter.Tests {
	[TestClass()]
	public class CellStateToStringConverterTests {
		[TestMethod()]
		public void ConvertTest() {
			CellStateToStringConverter converter = new CellStateToStringConverter();
			converter.A = "TestStringA";
			converter.B = "TestStringB";

			Assert.AreEqual(converter.Convert(CellState.A, null,null,null),converter.A);
			Assert.AreEqual(converter.Convert(CellState.B, null, null, null), converter.B);
			Assert.AreEqual(converter.Convert(CellState.empty, null, null, null), "");
		}

		[TestMethod()]
		public void ConvertBackTest() {
			CellStateToStringConverter converter = new CellStateToStringConverter();
			converter.A = "TestStringA";
			converter.B = "TestStringB";

			Assert.AreEqual(converter.ConvertBack(converter.A, null, null, null), CellState.A);
			Assert.AreEqual(converter.ConvertBack(converter.B, null, null, null), CellState.B);
			Assert.AreEqual(converter.ConvertBack("", null, null, null), CellState.empty);
		}
	}
}