using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tictactoe.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tictactoe.Commands.Tests {
	[TestClass()]
	public class CommandWithParameterTests {
		[TestMethod()]
		public void ExecuteTest() {
			bool actionFired = false;
			int testValue = 0;
			int targetValue = 1;

			Action<int> testAction = (i) => { actionFired = true; testValue = i; };

			CommandWithParameter<int> testcommand = new CommandWithParameter<int>(testAction);
			testcommand.Execute(targetValue);

			Assert.IsTrue(actionFired);
			Assert.AreEqual(targetValue, testValue);
		}

		[TestMethod()]
		public void CanExecuteTest() {
			int testValue = 0;
			int targetValue = 1;

			Action<int> testAction = (i) => {};
			Func<int, bool> testCanExecute = (i) => { return i == targetValue; };

			CommandWithParameter<int> testcommand = new CommandWithParameter<int>(testAction, testCanExecute);

			Assert.IsTrue(testcommand.CanExecute(targetValue));
			Assert.IsFalse(testcommand.CanExecute(testValue));
		}
	}
}