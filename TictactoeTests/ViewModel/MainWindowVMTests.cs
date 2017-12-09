using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tictactoe.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tictactoe.ViewModel.Tests {
	[TestClass()]
	public class MainWindowVMTests {
		[TestMethod()]
		public void NavigationTest() {
			MainWindowVM vm = new MainWindowVM();
			Uri initialpage = vm.CurrentPage;

			vm.GoToPageCommand.Execute("/test");

			Assert.AreNotEqual(initialpage, vm.CurrentPage);
		}
	}
}