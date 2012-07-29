using System;
using System.Collections.Generic;
using System.IO;

namespace DeltaEngine.BlogEngineSpamCommentRemover
{
	/// <summary>
	/// Main class of this project, does all the work with IsApproved and ContainsSpam of the Comment
	/// class with help of http://blogspam.net/ and some extra contains checks for typical spam entries.
	/// </summary>
	public class CommentRemover
	{
		private readonly string postsPath;
		private readonly LogProgressDelegate log;

		public delegate void LogProgressDelegate(int postNum, int posts, string postName,
			int commentsRemoved, int totalComments, string spamCommentRemoved);

		public CommentRemover(string blogEngineBasePath, LogProgressDelegate setLog)
		{
			if (String.IsNullOrEmpty(blogEngineBasePath))
				throw new ArgumentNullException("blogEngineBasePath");
			log = setLog;

			postsPath = Path.Combine(blogEngineBasePath, "App_Data", "posts");
			if (Directory.Exists(postsPath) == false)
				throw new NotSupportedException("Path does not exist, unable to continue: " + postsPath);
		}

		public List<Post> GetAllPosts(RemoveChoices choices, int setRemoveUnapprovedCommentsDays = 60)
		{
			var posts = new List<Post>();
			var files = Directory.GetFiles(postsPath);
			foreach (var file in files)
				posts.Add(new Post(new PostFile(file), choices, setRemoveUnapprovedCommentsDays));

			log(0, files.Length, "Starting ..", Comment.TotalCommentsRemoved,
				Comment.TotalComments, "");

			if (choices.HasFlag(RemoveChoices.Unapproved))
				RemoveAllUnapprovedComments(posts);

			return posts;
		}

		public void RemoveAllUnapprovedComments(List<Post> posts)
		{
			log(0, posts.Count, "Removing unapproved comments ..", Comment.TotalCommentsRemoved,
				Comment.TotalComments, "");

			foreach (var post in posts)
			{
				foreach (var comment in post.Comments)
					if (comment.IsApproved == false)
						comment.Remove();

				post.Save();
			}
		}

		public void RemoveAllSpamComments(List<Post> posts)
		{
			var postNum = 0;
			foreach (var post in posts)
			{
				foreach (var comment in post.Comments)
					TryToRemoveSpamComment(posts, comment, post, postNum);

				post.Save();
				postNum++;
				log(postNum, posts.Count, post.Name, Comment.TotalCommentsRemoved,
					Comment.TotalComments, "");
			}
		}

		public void TryToRemoveSpamComment(List<Post> posts, Comment comment, Post post, int postNum)
		{
			try
			{
				if (comment.ContainsSpam())
					RemoveCommentWithLogging(post, postNum, posts.Count, comment);
			}
			catch (Exception ex)
			{
				log(postNum, posts.Count, post.Name, Comment.TotalCommentsRemoved,
					Comment.TotalComments, "Failed to remove comment: " + ex.Message);
			}
		}

		private void RemoveCommentWithLogging(Post post, int postNum, int numberOfPosts, Comment comment)
		{
			log(postNum, numberOfPosts, post.Name, Comment.TotalCommentsRemoved,
				Comment.TotalComments, comment.Content);

			comment.Remove();
		}
	}
}