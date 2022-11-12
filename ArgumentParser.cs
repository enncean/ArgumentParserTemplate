using System;
using System.Collections.Generic;

//For: ReadOnlyDictionary
//using System.Collections.ObjectModel;

namespace ArgumentParser
{
	public class ArgumentParser
	{
		private const char StartChar = '/';
		private const char SeparateKvChar = '=';

		/// <summary>
		/// Parse the arguments in the specified format.
		/// </summary>
		/// <param name="args">Program arguments</param>
		/// <param name="container">Container to store the results.</param>
		public static void Parse(string[] args, AbstractParameters container)
		{
			Parse(args, container, s => { }, (k, v) => { });
		}

		/// <summary>
		/// Parse the arguments in the specified format.
		/// </summary>
		/// <param name="args">Program arguments</param>
		/// <param name="container">Container to store the results.</param>
		/// <param name="onFormatError">Callback when format error.</param>
		public static void Parse(string[] args, AbstractParameters container, Action<string> onFormatError)
		{
			Parse(args, container, onFormatError, (k, v) => { });
		}

		/// <summary>
		/// Parse the arguments in the specified format.
		/// </summary>
		/// <param name="args">Program arguments</param>
		/// <param name="container">Container to store the results.</param>
		/// <param name="onUnknownParam">Callback when key was not registered.</param>
		public static void Parse(string[] args, AbstractParameters container, Action<string, string> onUnknownParam)
		{
			Parse(args, container, s => { }, onUnknownParam);
		}

		/// <summary>
		/// Parse the arguments in the specified format.
		/// </summary>
		/// <param name="args">Program arguments</param>
		/// <param name="container">Container to store the results.</param>
		/// <param name="onFormatError">Callback when format error.</param>
		/// <param name="onUnknownParam">Callback when key was not registered.</param>
		public static void Parse(string[] args, AbstractParameters container, Action<string> onFormatError, Action<string, string> onUnknownParam)
		{
			IDictionary<string, Action<string>> dictionary = container.GetDictionary();
			foreach (string s in args)
			{
				if (!string.IsNullOrEmpty(s) && s.IndexOf(StartChar) == 0)
				{
					int separateIndex = s.IndexOf(SeparateKvChar);
					string k;
					string v;
					if (separateIndex == -1)	// `/key` pattern
					{
						k = s.Substring(1, s.Length - 1);
						v = string.Empty;
					}
					else	// `/key=value` pattern
					{
						k = s.Substring(1, separateIndex - 1);
						v = s.Substring(separateIndex + 1, s.Length - separateIndex - 1);
					}

					if (!string.IsNullOrEmpty(k))
					{
						Action<string> parser;
						if (dictionary.TryGetValue(k, out parser))
						{
							parser(v);
						}
						else
						{
							onUnknownParam(k, v);
						}
					}
					else
					{
						onFormatError(s);
					}
				}
				else
				{
					onFormatError(s);
				}
			}
		}
	}

	public abstract class AbstractParameters
	{
		private Dictionary<string, Action<string>> keyParserDictionary = new Dictionary<string, Action<string>>();

		protected void RegisterKeyAndParser(string key, Action<string> parser)
		{
			this.keyParserDictionary.Add(key, parser);
		}

		public IDictionary<string, Action<string>> GetDictionary()
		{
			// .NET Framework 4.5 or later
			// return new ReadOnlyDictionary<string, Action<string>>(this.keyParserDictionary);

			return this.keyParserDictionary;
		}

		protected static int ParseToIntDefault(string s, int def)
		{
			int ret;
			if (!int.TryParse(s, out ret))
			{
				ret = def;
			}

			return ret;
		}

		protected static string ParseToStringDefault(string s)
		{
			return string.IsNullOrEmpty(s) ? string.Empty : s;
		}
	}
}