using System;
using System.Collections.Generic;
using System.Globalization;

namespace DeltaEngine.BlogEngineSpamCommentRemover
{
	/// <summary>
	/// Extensions for strings to check if a text contains typical spam words (can be edited in the Gui).
	/// </summary>
	public static class StringExtensions
	{
		public static List<string> SpamWords = new List<string>(new[]
		{
			"outdoor", "usa.com", ".yahoo.com", "insurance", "rapidshare", "naruto", "jewel", "loans",
			" loan", "use firefox", "quality - papers", "quality-papers", "quality papers", "exclusivepapers",
			"lose weight", "annuity", "annuities", "dummies", "funeral", "penis", "vagina", "pussy",
			"enlargement", "pokemon", "young girls", "young drivers", " sex ", " escort", "lawsuit",
			"teeth whitening", "lifestyle", "@mail2web.com", "lcd monitor", "electronic cigarette",
			"marlboro1", "vapor cigarettes", "smokeless cigarettes", " for sale", "seo company",
			"clickresponse", "website seo", "cricket ", "car carrier", "packers and movers",
			"transrelocations", "cricket scores", "clothing", "rentboy", "rentgirl", "financing",
			"computers keyboard", "computerskeyboard", "page optimization", "chacha", "micro-motor",
			"customer service", "porno", "viagra", "viiagra", "viiiagra", "ratking", "smackdown",
			"backgammon", "notebooksbatteries", "air conditioning", "night vision", "berry review",
			"charter.net", "aiopedia", "military-night-vision", "facebook login", "affiliate.",
			"prostitutki", "pokeronline", "hair straight", "baidu.com", "lindsey", "britney", "handbags",
		});
public static List<string> NiceSpamComments = new List<string>(new[]
{
	// http://www.goseewrite.com/2010/12/great-spam-comments/
	"this is good information",
	"interesting read, thanks",
	"i have learned a lot from your blog",
	"i used to love reading your blog",
	"you're such an inspiration",
	"i love this blog",
	"i absolutely do not believe you",
	"i really do not understand you",
	"i'm happy i found this blog",
	"you must be a genius",
	"you must be a genious",
	"excellent blog post, i look forward to reading more",
	"pleasure in the job puts perfection in the work",
	"blown away by the content and quality of your site",
	"what i liked about her,",
	"the post is really the best on this laudable topic.",
	"know yourself and you will win all battles.",
	"hi mate this is interesting article",
	"will make sure I check your posts more often",
	"really interesting article",
	"its funny how those little things can drive you mad",
	"i would like to thank you for the efforts you have made in writing this post.",
	"i am hoping the same best work from you in the future as well.",
	"your creative writing abilities has inspired me to start my own blog",
	"really the blogging is spreading its wings rapidly.",
	"your write up is a fine example of it.",
	"pretty good post.",
	"i just stumbled upon your blog and wanted to say ",
	"i have really enjoyed reading your blog posts",
	"i'll be subscribing to your feed and I hope you post again soon.",
	"i have bookmarked it and I am looking forward to reading new articles.",
	"keep up the good work. i hope you can continue this kind ",
	"increase the importance and interactivity of site." +
	"i don? usually reply to posts but I will in this case.",
	"definitely agree with what you stated.",
	"your explanation was certainly the easiest to understand.",
	"you managed to hit the nail right on the head and explained out everything ",
	"this is a nice blog.",
	"good clean ui and nice informative blogs.",
	"i will be coming back in a bit, thanks for the great post.",
	"i put a link to your blog at my site, hope you don't mind?",
	"that? too nice, when it comes in india hope it can make a rocking place for youngster",
	"thanks for taking the time to discuss this, ",
	"i feel strongly about information and love learning more on this.",
	"if possible, as you gain expertise, It is extremely helpful for me.",
	"would you mind updating your blog with more information?",
	"interesting blog. ",
	"it would be great if you can provide more details about it.",
	" thanks a load!",
	"nice article, I must say I never scan one thing that summed it up therefore well.",
	"one thing like this could be scan once in an exceedingly whereas, ",
	"i have been reading a lot on here and have picked up some great ideas.",
	"i appreciate you taking the time to write all this up for us",
	"hhmmmm very interesting article!",
	"this is a great site, very handy, just what i was looking for",
	"keep up the good work, many thanks!",
	"i was very pleased to find this site.",
	"i wanted to thank you for this great read!",
	"i definitely enjoying every little bit of it and i have you bookmarked to check out",
	"thank you for another informative post!",
	"i appreciate your efforts.",
	"its really very interesting article and really had me thinking",
	"will make sure i check your posts more often!",
	"it contains a useful information in it..thanks",
	"cheers for the info. it was a good read.",
	"don’t stop blogging! it’s nice to read a sane commentary for once",
	"great blog here. keep it up!",
	"please try to include more information if possible.",
	"i have just started working with this software and am having a few problems.",
	"is there any place to go where I can get some more information?",
	"this sounds like a great app.",
	"it never ceases to amaze me that so many different apps are hitting the market.",
	"thanks for the cool pic too.",
	"i had been looking for this product.",
	"finally I found it in your blog.",
	"great post! thanks for the information",
	"great post, you’ve helped me a lot",
	"i thought you were going to chip in with some decisive insght at the end there",
	"i am so satisfied that I have found this your post because I have been searching ",
	"i will definitely bookmark your website and wait for other useful and info",
	"i usually don? post in blogs but",
	"your blog appears quite informative.",
	"can you please tell me how can i read your rss blog?",
	"nice article, I must say I never scan one thing that summed it up therefore well.",
	"one thing like this could be scan once in an exceedingly whereas",
	"i would like to thank you for the efforts you have made in writing this post.",
	"i like your blog so much that i feel i have to wish you",
	"i love reading through your blog, I wanted to leave a little comment",
	"wishing you the best of luck for all your blogging efforts.",
	"i just got this in the mail this week.",
});

		public static DateTime? ToDateTime(this string value)
		{
			DateTime result;
			if (DateTime.TryParse(value, out result))
				return result;

			return null;
		}

		public static bool ContainsSpamWord(this string text)
		{
			return ContainsWord(text, SpamWords);
		}

		private static bool ContainsWord(string text, IEnumerable<string> spamWords)
		{
			string lowerText = text.ToLower();
			foreach (var word in spamWords)
				if (lowerText.Contains(word))
					return true;

			return false;
		}

		public static bool ContainsSpamNiceWords(this string text)
		{
			string lowerText = text.ToLower();
			return ContainsWord(lowerText, NiceSpamComments);
		}
	}
}