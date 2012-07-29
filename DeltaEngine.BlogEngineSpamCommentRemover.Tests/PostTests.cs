using System;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;

namespace DeltaEngine.BlogEngineSpamCommentRemover.Tests
{
	public class PostTests
	{
		public const string ExampleCommentFileText = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<post>
  <author>Mr. T</author>
  <title>Example Post</title>
  <description />
  <content>&lt;p&gt;Today we had a sunny day .. blah blah blah</content>
  <ispublished>True</ispublished>
  <iscommentsenabled>True</iscommentsenabled>
  <pubDate>2012-05-07 18:47:00</pubDate>
  <lastModified>2012-05-07 19:25:00</lastModified>
  <raters>0</raters>
  <rating>0</rating>
  <slug>Example-Post</slug>
  <tags />
  <comments>
    <comment id=""0553192b-e67c-4494-9dc3-47e52301f37c"" parentid=""00000000-0000-0000-0000-000000000000"" approved=""False"">
      <date>2012-07-15 03:58:48</date>
      <author>michael kors handbags</author>
      <email>abercrom@163.com</email>
      <country />
      <ip>110.85.113.63</ip>
      <website>http://www.michae1korsmkoutlet.com/</website>
      <content>avril, soit entre les deux tours du scrutin. [b][url=http://www.sacsmagasinfr4.com/]louis vuitton[/url][/b]&amp;quot;qui d'ordinaire ne peuvent pas faire entendre leur voix &amp;#224; la?Fran?</content>
    </comment>
	</comments>
</post>";


		[Test]
		public void LoadExamplePost()
		{
			var doc = XDocument.Parse(ExampleCommentFileText);
			var post = new Post(new PostFile(doc), RemoveChoices.All);
			Assert.AreEqual(post.Name, "Example Post");
			Assert.AreEqual(post.Date, new DateTime(2012, 05, 07, 18, 47, 0));
			Assert.AreEqual(post.Comments.Count, 1);
		}

		[Test]
		public void LoadPostFileRemoveCommentsAndSaveFile()
		{
			const string ExampleFilename = "ExamplePost.xml";
			var tempFile = File.CreateText(ExampleFilename);
			tempFile.Write(ExampleCommentFileText);
			tempFile.Close();
			try
			{
				var post = new Post(new PostFile(ExampleFilename), RemoveChoices.All);
				foreach (var comment in post.Comments)
					comment.Remove();
				post.Save();
			}
			finally
			{
				File.Delete(ExampleFilename);
			}
		}
	}
}
