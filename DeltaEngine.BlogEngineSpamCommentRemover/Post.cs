using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DeltaEngine.BlogEngineSpamCommentRemover
{
	/// <summary>
	/// Represents a full post xml file from a BlogEngine.net post in the App_Data/posts directory.
	/// </summary>
	public class Post
	{
		public readonly RemoveChoices Choices;
		public readonly List<Comment> Comments = new List<Comment>();
		public readonly string Name;
		public readonly DateTime Date;
		public readonly int RemoveUnapprovedCommentsDays;
		private readonly PostFile file;

		public Post(PostFile setFile, RemoveChoices setChoices, int setRemoveUnapprovedCommentsDays = 60)
		{
			file = setFile;
			Choices = setChoices;
			RemoveUnapprovedCommentsDays = setRemoveUnapprovedCommentsDays;
			Name = file.Root.Element("title").Value;
			Date = file.Root.Element("pubDate").Value.ToDateTime() ?? DateTime.Now;
			LoadComments();
		}

		private void LoadComments()
		{
			foreach (var node in file.Root.Elements())
				if (node.Name == "comments")
					foreach (var comment in node.Elements())
						Comments.Add(new Comment(comment, this));
		}

		public void Save()
		{
			if (Choices.HasFlag(RemoveChoices.CloseCommenting))
				file.Root.Element("iscommentsenabled").SetValue("False");
			
			file.Save();
		}
	}
}