using System;
using System.Collections.Generic;
using System.Windows;

namespace DeltaEngine.BlogEngineSpamCommentRemover.Gui
{
	/// <summary>
	/// Allows to edit a bunch of words by displaying them in a big textbox.
	/// </summary>
	public partial class EditWords : Window
	{
		public List<string> Words;

		public EditWords(List<string> setWords)
		{
			InitializeComponent();

			Words = setWords;
			var linesText = "";
			foreach (string word in Words)
				linesText += word + "\n";
			Lines.Text = linesText;
		}

		private void CancelClick(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void SaveClick(object sender, RoutedEventArgs e)
		{
			var splittedLines = Lines.Text.Split(new char[] { '\n', '\r' },
				StringSplitOptions.RemoveEmptyEntries);
			Words = new List<string>(splittedLines);
			DialogResult = true;
		}
	}
}
