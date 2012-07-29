using System;

namespace DeltaEngine.BlogEngineSpamCommentRemover
{
	/// <summary>
	/// Choices the user can make in Program, will be passed to the CommentRemover and Comment classes.
	/// </summary>
	[Flags]
	public enum RemoveChoices
	{
		None = 0,
		Unapproved = 1,
		Contains = 2,
		NiceWords = 4,
		OldComments = 8,
		Webservice = 16,
		CloseCommenting = 32,
		AutoApprove = 64,
		All = Unapproved | Contains | NiceWords | OldComments | Webservice | CloseCommenting,
	}
}