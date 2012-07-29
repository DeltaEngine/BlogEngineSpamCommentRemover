using System.Xml.Linq;

namespace DeltaEngine.BlogEngineSpamCommentRemover
{
	/// <summary>
	/// Helper for Post to encapsulate the XDocument and file path, plus adds some statistics.
	/// </summary>
	public class PostFile
	{
		public static int FilesLoaded = 0;
		public static int TotalTextCharactersProcessed = 0;
		private readonly XDocument file;
		private readonly string filePath;

		public PostFile(XDocument setFile)
		{
			file = setFile;
		}

		public PostFile(string setFilePath)
		{
			filePath = setFilePath;
			file = XDocument.Load(filePath);
			FilesLoaded++;
			TotalTextCharactersProcessed += file.ToString().Length;
		}

		public XElement Root
		{
			get { return file.Root; }
		}

		public void Save()
		{
			file.Save(filePath);
		}
	}
}
