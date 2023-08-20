using System;

public static partial class Config 
{
	public static string Device { private set; get; } = "/dev/input/event4";

	private static readonly Option[] OptionsDefinition =
	{
		new Option("--device", 'd', true, null),
	};
}