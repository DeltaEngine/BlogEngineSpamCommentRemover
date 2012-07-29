using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;

namespace DeltaEngine.BlogEngineSpamCommentRemover.Gui
{
	/// <summary>
	/// Gui for the BlogEngineSpamCommentRemover console application to easily configure settings.
	/// </summary>
	public partial class MainWindow : Window
	{
		private Thread workerThread;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void SelectBasePathClick(object sender, RoutedEventArgs e)
		{
			var dialog = new FolderBrowserDialog
			{
				RootFolder = Environment.SpecialFolder.MyComputer,
				ShowNewFolderButton = false,
				Description = "Select BlogEngine.net base path (usually in your wwwroot)"
			};

			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				BasePath.Text = dialog.SelectedPath;
		}

		private void DaysValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			RemoveOldUnapprovedComments.Content = "Remove all not approved comments older than " +
				(int)e.NewValue + " days";
		}

		private void EditWordListClick(object sender, RoutedEventArgs e)
		{
			var dialog = new EditWords(StringExtensions.SpamWords);
			dialog.ShowDialog();
		}

		private void EditNiceWordsClick(object sender, RoutedEventArgs e)
		{
			var dialog = new EditWords(StringExtensions.NiceSpamComments);
			dialog.ShowDialog();
		}

		private void StartClick(object sender, RoutedEventArgs e)
		{
			try
			{
				var remover = new CommentRemover(BasePath.Text, UpdateUI);
				var choices = GetChoices();
				var daysForRemovingUnapprovedComments = (int)Days.Value;

				if (workerThread != null)
					workerThread.Abort();
				workerThread = new Thread(new ThreadStart(delegate
				{
					var posts = remover.GetAllPosts(choices, daysForRemovingUnapprovedComments);
					remover.RemoveAllSpamComments(posts);
					workerThread = null;
				}));
				workerThread.Start();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Unable to start comment remover: " + ex.Message,
					"BlogEngine.net Spam Comment Remover");
			}
		}

		private void UpdateUI(int postNum, int posts, string postName, int commentsRemoved,
			int totalComments, string spamCommentRemoved)
		{
			Dispatcher.BeginInvoke(new Action(delegate
			{
				Progress.Value = postNum * Progress.Maximum / posts;
				ProcessedPosts.Content = "Post " + postNum + " of " + posts + ": " + postName;
				RemovedComments.Content = "Comments removed: " + commentsRemoved + " of " + totalComments;
				Status.Content = "Removed: " + spamCommentRemoved;
			}), DispatcherPriority.Normal);
		}

		private RemoveChoices GetChoices()
		{
			var choices = RemoveChoices.None;

			if (RemoveUnapprovedComments.IsChecked == true)
				choices |= RemoveChoices.Unapproved;
			else if (AutomaticallyApproveComments.IsChecked == true)
				choices |= RemoveChoices.AutoApprove;

			if (CheckContains.IsChecked == true)
				choices |= RemoveChoices.Contains;

			if (CheckNiceWords.IsChecked == true)
				choices |= RemoveChoices.NiceWords;

			if (RemoveOldUnapprovedComments.IsChecked == true)
				choices |= RemoveChoices.OldComments;

			if (UseWebservice.IsChecked == true)
				choices |= RemoveChoices.Webservice;

			if (CloseCommenting.IsChecked == true)
				choices |= RemoveChoices.CloseCommenting;

			return choices;
		}

		protected override void OnClosed(EventArgs e)
		{
			if (workerThread != null)
				workerThread.Abort();
			base.OnClosed(e);
		}
	}
}
