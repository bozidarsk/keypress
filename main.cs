using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;

public static class Program 
{
	[DllImport("evemu")] private static extern int evemu_create_event(IntPtr inputEvent, int type, int code, int value);
	[DllImport("evemu")] private static extern int evemu_play_one(int fd, IntPtr inputEvent);
	[DllImport("evemu")] private static extern int open(IntPtr file, int flags);
	[DllImport("evemu")] private static extern void close(int fd);

	public static uint HexToUInt(string x) 
	{
		uint result = 0;
		string chars = "0123456789abcdef";
		for (int i = 0; i < x.Length; i++) 
		{
			result += (uint)chars.IndexOf(x[i]);
			if (i < x.Length - 1) { result <<= 4; }
		}

		return result;
	}

	private static int Main(string[] args) 
	{
		if (args.Length == 0 || (args.Length == 1 && (args[0] == "-h" || args[0] == "--help" || args[0] == "help"))) 
		{
			Console.WriteLine("Usage:\n\tkeypress <key0> [key1] [key...] [options]");
			Console.WriteLine("\nOptions:");
			Console.WriteLine("\t-d, --device <dir>  Directory which contains configuration and style files.");
			return 0;
		}

		Config.Initialize(ref args);

		int[] keycodes = new int[args.Length];
		for (int i = 0; i < keycodes.Length; i++) 
		{
			if (int.TryParse(args[i], out int key)) 
			{
				keycodes[i] = key;
				continue;
			}
			else if (Enum.TryParse<Input.KeyCode>(args[i], true, out Input.KeyCode keycode)) 
			{
				keycodes[i] = (int)keycode;
				continue;
			}
			else if (args[i].StartsWith("0x")) 
			{
				keycodes[i] = (int)HexToUInt(args[i].Substring(2, args[i].Length - 2));
				continue;
			}
		}

		IntPtr e = Marshal.AllocCoTaskMem(24); // sizeof(struct input_event) /* #include <linux/input.h> */
		int fd = open(Marshal.StringToCoTaskMemUTF8(Config.Device), 1); // O_WRONLY

		for (int i = 0; i < keycodes.Length; i++) 
		{
			evemu_create_event(e, (int)Input.EV.KEY, keycodes[i], 1);
			evemu_play_one(fd, e);
			evemu_create_event(e, (int)Input.EV.SYN, (int)Input.SYN.REPORT, 0);
			evemu_play_one(fd, e);
		}

		for (int i = keycodes.Length - 1; i >= 0; i--) 
		{
			evemu_create_event(e, (int)Input.EV.KEY, keycodes[i], 0);
			evemu_play_one(fd, e);
			evemu_create_event(e, (int)Input.EV.SYN, (int)Input.SYN.REPORT, 0);
			evemu_play_one(fd, e);
		}

		close(fd);

		return 0;
	}
}
