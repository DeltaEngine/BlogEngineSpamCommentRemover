using System;
using System.IO;
using NUnit.Framework;

namespace DeltaEngine.BlogEngineSpamCommentRemover.Tests
{
	[Category("Slow")]
	public class CommentRemoverTests : IDisposable
	{
		private const string AppDataPostsDirectory = @"App_Data\posts";
		private const string ExampleFilename = "ExamplePost.xml";
		private const RemoveChoices RemoveWordsAndViaWebservice =
			RemoveChoices.Contains | RemoveChoices.NiceWords | RemoveChoices.Webservice;
		
		private void CreateDummyAppDataPostsDirectory()
		{
			Directory.CreateDirectory(AppDataPostsDirectory);
			var tempFile = File.CreateText(ExampleFilePath);
			tempFile.Write(PostTests.ExampleCommentFileText);
			tempFile.Close();
		}

		private static string ExampleFilePath
		{
			get { return Path.Combine(AppDataPostsDirectory, ExampleFilename); }
		}

		public void Dispose()
		{
			File.Delete(ExampleFilePath);
			Directory.Delete(AppDataPostsDirectory);
		}

		[Test]
		public void InvalidBlogEnginePathIsNotAllowed()
		{
			Assert.Throws<ArgumentNullException>(() => new CommentRemover(null, IgnoreLog));
			Assert.Throws<NotSupportedException>(() => new CommentRemover("abc", IgnoreLog));
		}

		[Test]
		public void RemoveUnapprovedComments()
		{
			CreateDummyAppDataPostsDirectory();
			var remover = new CommentRemover(Directory.GetCurrentDirectory(), IgnoreLog);
			var firstPost = remover.GetAllPosts(RemoveChoices.Unapproved)[0];
			firstPost.Save();
			firstPost = remover.GetAllPosts(RemoveChoices.All)[0];
			Assert.AreEqual(firstPost.Comments.Count, 0);
		}

		[Test]
		public void RemoveSpamCommentsInFirstPost()
		{
			CreateDummyAppDataPostsDirectory();
			var remover = new CommentRemover(Directory.GetCurrentDirectory(), IgnoreLog);
			var firstPost = remover.GetAllPosts(RemoveWordsAndViaWebservice)[0];
			Assert.AreEqual(firstPost.Comments.Count, 1);
			foreach (var comment in firstPost.Comments)
				if (comment.ContainsSpam())
					comment.Remove();

			firstPost.Save();
			firstPost = remover.GetAllPosts(RemoveChoices.All)[0];
			Assert.AreEqual(firstPost.Comments.Count, 0);
		}

		private void IgnoreLog(int postnum, int posts, string postname, int commentsremoved,
			int totalcomments, string spamcommentremoved) {}

		[Test]
		public void RemoveAllSpamComments()
		{
			CreateDummyAppDataPostsDirectory();
			var remover = new CommentRemover(Directory.GetCurrentDirectory(), IgnoreLog);
			var allPosts = remover.GetAllPosts(RemoveWordsAndViaWebservice);
			remover.RemoveAllSpamComments(allPosts);
		}

		[Test]
		public void HandleWebserviceCrashesInContainsSpam()
		{
			CreateDummyAppDataPostsDirectory();
			var remover = new CommentRemover(Directory.GetCurrentDirectory(), IgnoreLog);
			var allPosts = remover.GetAllPosts(RemoveWordsAndViaWebservice);
			remover.TryToRemoveSpamComment(allPosts, null, allPosts[0], 0);
		}
	}
}