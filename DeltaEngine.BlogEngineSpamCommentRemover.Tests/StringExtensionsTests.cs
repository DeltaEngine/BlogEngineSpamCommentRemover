using System;
using NUnit.Framework;

namespace DeltaEngine.BlogEngineSpamCommentRemover.Tests
{
	public class StringExtensionsTests
	{
		[Test]
		public void ConvertTextToDateTime()
		{
			Assert.AreEqual(new DateTime(2012, 7, 28), "2012-07-28".ToDateTime());
			Assert.AreEqual(new DateTime(2000, 1, 1), "1.1.2000".ToDateTime());
			Assert.AreEqual(new DateTime(2012, 7, 28, 12, 0, 0), "2012-07-28 12pm".ToDateTime());
			Assert.AreEqual(null, "".ToDateTime());
		}

		[Test]
		public void CheckIfTextContainsSpamWord()
		{
			Assert.IsTrue("penis enlargment".ContainsSpamWord());
			Assert.IsTrue("buy loan".ContainsSpamWord());
			Assert.IsTrue("viagra pills".ContainsSpamWord());
			Assert.IsTrue("ViIaGra".ContainsSpamWord());
			Assert.IsFalse("Hi there".ContainsSpamWord());
		}

		[Test]
		public void CheckIfTextContainsSpamNiceWords()
		{
			Assert.IsTrue("You must be a genius".ContainsSpamNiceWords());
			Assert.IsTrue("I love this blog".ContainsSpamNiceWords());
			Assert.IsTrue("This is good Information".ContainsSpamNiceWords());
			Assert.IsTrue("This is a really interesting article, good stuff".ContainsSpamNiceWords());
			Assert.IsFalse("Hi. XNA is a bit dead isn't it?".ContainsSpamNiceWords());
		}
	}
}