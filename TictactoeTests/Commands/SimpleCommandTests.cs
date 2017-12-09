using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tictactoe.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tictactoe.Commands.Tests {
	[TestClass()]
	public class SimpleCommandTests {
		[TestMethod()]
		public void ExecuteTest() {
			bool actionFired = false;
			Action testAction = () => { actionFired = true; };

			SimpleCommand testcommand = new SimpleCommand(testAction);

			testcommand.Execute(null);
			Assert.IsTrue(actionFired);

		}

		[TestMethod()]
		public void CanExecuteTest() {
			Action testAction = () => { };
			Func<bool> testCanExecuteTrue = () => { return true; };
			Func<bool> testCanExecuteFalse = () => { return false; };

			SimpleCommand testcommandTrue = new SimpleCommand(testAction,testCanExecuteTrue);
			SimpleCommand testcommandFalse = new SimpleCommand(testAction,testCanExecuteFalse);

			Assert.IsTrue(testcommandTrue.CanExecute(null));
			Assert.IsFalse(testcommandFalse.CanExecute(null));

		}
	}
}