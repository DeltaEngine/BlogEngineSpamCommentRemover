using System;

namespace DeltaEngine.BlogEngineSpamCommentRemover
{
	/// <summary>
	/// Console application to enter a BlogEngine.net base path in order to remove all spam comments.
	/// </summary>
	class Program
	{
		private static string lastPostName = "";

		static void Main()
		{
			ShowIntroduction();
			var remover = AskForBasePathToCreateRemover();
			var choices = AskOptions();
			var posts = remover.GetAllPosts(choices);
			remover.RemoveAllSpamComments(posts);
			ShowSummary();
		}

		private static void ShowIntroduction()
		{
			Console.WriteLine("BlogEngineSpamCommentRemover, for details see http://BenjaminNitschke.com");
			Console.WriteLine(
				"Make sure you have your App_Data/posts directory backed up, it will be modified!");
		}

		private static CommentRemover AskForBasePathToCreateRemover()
		{
			Console.WriteLine("Please specify the BlogEngine.net base path for checking for spam comments:");
			var basePath = Console.ReadLine();
			return new CommentRemover(basePath, OutputProgress);
		}

		private static void OutputProgress(int postNum, int posts, string postName, int commentsRemoved,
			int totalComments, string spamCommentRemoved)
		{
			if (spamCommentRemoved != "")
				Console.WriteLine("Removing spam comment: " + spamCommentRemoved);

			if (lastPostName != postName)
			{
				Console.WriteLine("Saved post: " + postName);
				Console.WriteLine("Post " + postNum + " of " + posts + ". Comments removed so " +
					"far: " + commentsRemoved + " of " + totalComments + " (" +
					(commentsRemoved * 1000 / totalComments) / 10.0f + "%)");
				lastPostName = postName;
			}
		}

		private static RemoveChoices AskOptions()
		{
			var choices = RemoveChoices.None;
			Console.WriteLine("Please select remove spam comments options. Enter y/n for each!");

			Console.WriteLine("Remove all unapproved comments first (will save a lot of checks)");
			if (Console.ReadLine() == "y")
				choices |= RemoveChoices.Unapproved;
			else
			{
				Console.WriteLine("Automatically approve non-spam comments?");
				if (Console.ReadLine() == "y")
					choices |= RemoveChoices.AutoApprove;
			}

			Console.WriteLine("Simple text contains checks with common spam words and domains");
			if (Console.ReadLine() == "y")
				choices |= RemoveChoices.Contains;

			Console.WriteLine("Check if the comment contains very nice words commonly used by spammers");
			if (Console.ReadLine() == "y")
				choices |= RemoveChoices.NiceWords;

			Console.WriteLine("Remove all not approved comments older than 60 days!");
			if (Console.ReadLine() == "y")
				choices |= RemoveChoices.OldComments;

			Console.WriteLine("Use the blogspam.net webservice to check for any remaining spam");
			if (Console.ReadLine() == "y")
				choices |= RemoveChoices.Webservice;

			Console.WriteLine("Close commenting, no more comments can be added for each post");
			if (Console.ReadLine() == "y")
				choices |= RemoveChoices.CloseCommenting;

			Console.WriteLine();
			return choices;
		}

		private static void ShowSummary()
		{
			Console.WriteLine("Done. Files parsed: " + PostFile.FilesLoaded +
				", Total Text Characters Processed: " + PostFile.TotalTextCharactersProcessed);
			Console.WriteLine("I hope this helped :) Made by BenjaminNitschke.com 2012-07-28");
		}
	}
}
