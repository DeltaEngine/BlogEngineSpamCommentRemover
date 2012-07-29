using System;
using System.Xml.Linq;
using CodeGecko.AntiSpam.BlogSpamNet;

namespace DeltaEngine.BlogEngineSpamCommentRemover
{
	/// <summary>
	/// Represents a comment xml node from a BlogEngine.net post in the App_Data/posts directory.
	/// </summary>
	public class Comment
	{
		public static int TotalComments = 0;
		public static int TotalCommentsRemoved = 0;
		private readonly XElement node;
		private readonly Post post;
		private bool alreadyRemoved;

		public Comment(XElement setNode, Post setPost)
		{
			node = setNode;
			post = setPost;
			TotalComments++;
		}

		public bool IsApproved
		{
			get { return node.Attribute("approved").Value == "True"; }
		}

		public string Author
		{
			get { return GetElementValueSafely("author"); }
		}

		private string GetElementValueSafely(string childName)
		{
			var child = node.Element(childName);
			return child == null ? "" : child.Value;
		}

		public string Email
		{
			get { return GetElementValueSafely("email"); }
		}

		public string Website
		{
			get { return GetElementValueSafely("website"); }
		}

		public string Ip
		{
			get { return GetElementValueSafely("ip"); }
		}

		public string Content
		{
			get { return GetElementValueSafely("content"); }
		}

		public DateTime Date
		{
			get
			{
				return GetElementValueSafely("date").ToDateTime() ?? DateTime.Now;
			}
		}

		public bool ContainsSpam()
		{
			if (alreadyRemoved)
				return false;

			if (IsOldUnapprovedComment())
				return true;

			if (post.Choices.HasFlag(RemoveChoices.AutoApprove))
				node.Attribute("approved").SetValue("True");

			if (post.Choices.HasFlag(RemoveChoices.Contains) &&
				(Author.ContainsSpamWord() ||
					Email.ContainsSpamWord() ||
					Website.ContainsSpamWord() ||
					Content.ContainsSpamWord()))
				return true;

			if (post.Choices.HasFlag(RemoveChoices.NiceWords) &&
					Content.ContainsSpamNiceWords())
				return true;

			if (post.Choices.HasFlag(RemoveChoices.Webservice) &&
					BlogSpam.TestComment(new CodeGecko.AntiSpam.BlogSpamNet.Comment()
					{
						comment = Content,
						ip = Ip,
						email = Email,
						site = Website,
						name = Author,
					}) != "OK")
				return true;

			return false;
		}

		private bool IsOldUnapprovedComment()
		{
			return post.Choices.HasFlag(RemoveChoices.OldComments) &&
				(Date - post.Date).TotalDays > 60 &&
				IsApproved == false;
		}

		public void Remove()
		{
			alreadyRemoved = true;
			TotalCommentsRemoved++;
			node.Remove();
		}
	}
}