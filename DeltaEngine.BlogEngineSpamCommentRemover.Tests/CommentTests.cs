using System;
using System.Xml.Linq;
using NUnit.Framework;

namespace DeltaEngine.BlogEngineSpamCommentRemover.Tests
{
	public class CommentTests
	{
		[Test]
		public void CheckCommentData()
		{
			var comment = LoadComment(PostTests.ExampleCommentFileText, RemoveChoices.All);
			Assert.AreEqual(comment.IsApproved, false);
			Assert.IsTrue(comment.Author.Contains("handbags"));
			Assert.IsTrue(comment.Email.Contains(".com"));
			Assert.IsTrue(comment.Website.Contains(".com"));
			Assert.AreEqual(comment.Ip, "110.85.113.63");
			Assert.IsTrue(comment.Content.Contains(".com"));
			Assert.AreEqual(comment.Date, new DateTime(2012, 07, 15, 03, 58, 48));
		}

		private static Comment LoadComment(string xmlPostText, RemoveChoices choices)
		{
			var doc = XDocument.Parse(xmlPostText);
			var post = new Post(new PostFile(doc), choices);
			return post.Comments[0];
		}

		[Test]
		public void IsOldUnapprovedComment()
		{
			var comment = LoadComment(PostTests.ExampleCommentFileText, RemoveChoices.OldComments);
			Assert.IsTrue(comment.ContainsSpam());
		}

		[Test]
		public void ContainsSpam()
		{
			var comment = LoadComment(PostTests.ExampleCommentFileText, RemoveChoices.Contains);
			Assert.IsTrue(comment.ContainsSpam());
		}

		[Test]
		public void ContainsNiceSpamComments()
		{
			var modifiedPost = PostTests.ExampleCommentFileText.Replace("<content>",
				"<content>I love this blog");
			var comment = LoadComment(modifiedPost, RemoveChoices.NiceWords);
			Assert.IsTrue(comment.ContainsSpam());
		}

		[Test, Category("Slow")]
		public void WebserviceCheck()
		{
			var comment = LoadComment(PostTests.ExampleCommentFileText, RemoveChoices.Webservice);
			Assert.IsTrue(comment.ContainsSpam());
		}

		[Test]
		public void AutoApproveComment()
		{
			var modifiedPost = PostTests.ExampleCommentFileText.Replace("2012-07-15 03:58:48",
				"2012-06-05 03:58:48");
			var comment = LoadComment(modifiedPost, RemoveChoices.AutoApprove);
			Assert.AreEqual(comment.IsApproved, false);
			comment.ContainsSpam();
			Assert.AreEqual(comment.IsApproved, true);
		}

		[Test]
		public void RemovingCommentMeansWeDoNotNeedToCheckForSpamAnymore()
		{
			var comment = LoadComment(PostTests.ExampleCommentFileText, RemoveChoices.None);
			comment.Remove();
			Assert.IsFalse(comment.ContainsSpam());
		}
	}
}
