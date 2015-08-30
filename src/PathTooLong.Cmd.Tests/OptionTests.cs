using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PathTooLong.Cmd.App;
using PathTooLong.Cmd.App.Processes;
using FluentAssertions;

namespace PathTooLong.Cmd.Tests {
	[TestClass]
	public class OptionTests {


		[TestMethod]
		public void IsDeleteTest() {

			var ops = new Options(new[] { "-delete", @"c:\test" });

			var pro = ops.Process;

			Assert.IsInstanceOfType(pro, typeof(Delete));
		}

		[TestMethod]
		public void IsDefaultDeleteTest() {

			var ops = new Options(new[] { @"c:\test" });

			var pro = ops.Process;

			Assert.IsInstanceOfType(pro, typeof(Delete));
		}

		[TestMethod]
		public void IsCopyTest() {

			var ops = new Options(new[] { "-copy", @"c:\test" });

			var pro = ops.Process;

			Assert.IsInstanceOfType(pro, typeof(Copy));
		}

		[TestMethod]
		public void IsCopyNotDefaultTest() {

			var ops = new Options(new[] { @"c:\test" });

			var pro = ops.Process;

			Assert.IsNotInstanceOfType(pro, typeof(Copy));
		}

		[TestMethod]
		public void HasDestinationTest() {

			var ops = new Options(new[] { "-dest", @"c:\test" });

			Action a = () => {
				ops.Destination.ToString();
			};

			a.ShouldNotThrow();
		}
	}
}
