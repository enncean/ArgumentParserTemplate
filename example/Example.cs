using System;
using ArgumentParser;

namespace ArgumentParserExample
{
	internal class Example
	{
		public static void Main(string[] args)
		{
			SampleParameters param = new SampleParameters();
			ArgumentParser.ArgumentParser.Parse(args, param,
				s => { Console.WriteLine("Format error : " + s); },
				(k, v) => { Console.WriteLine("Unknown param : \"" + k + "\", \"" + v + "\""); });

			Console.WriteLine("paramInt : " + param.ParamInt);
			Console.WriteLine("paramBool : " + param.ParamBool);
			Console.WriteLine("paramString : \"" + param.ParamString + "\"");
		}
	}

	class SampleParameters : AbstractParameters
	{
		public int ParamInt { get; private set; }
		public bool ParamBool { get; private set; }
		public string ParamString { get; private set; }

		public SampleParameters()
		{
			this.ParamInt = -1;
			this.ParamBool = false;
			this.ParamString = string.Empty;

			RegisterKeyAndParser("paramInt", s => this.ParamInt = ParseToIntDefault(s, -1));
			RegisterKeyAndParser("paramBool", s => this.ParamBool = true);
			RegisterKeyAndParser("paramString", s => this.ParamString = ParseToStringDefault(s));
		}
	}
}