using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;

/*

struct input_event 
{
#if (__BITS_PER_LONG != 32 || !defined(__USE_TIME_BITS64)) && !defined(__KERNEL__)
	struct timeval time;
#else
	__kernel_ulong_t __sec;

	#if defined(__sparc__) && defined(__arch64__)
		unsigned int __usec;
		unsigned int __pad;
	#else
		__kernel_ulong_t __usec;
	#endif
#endif

	__u16 type;
	__u16 code;
	__s32 value;
};

struct evemu_device *evemu_new(const char *name);
void evemu_delete(struct evemu_device *dev);
unsigned int evemu_get_version(const struct evemu_device *dev);
const char *evemu_get_name(const struct evemu_device *dev);
void evemu_set_name(struct evemu_device *dev, const char *name);
unsigned int evemu_get_id_bustype(const struct evemu_device *dev);
void evemu_set_id_bustype(struct evemu_device *dev, unsigned int bustype);
unsigned int evemu_get_id_vendor(const struct evemu_device *dev);
void evemu_set_id_vendor(struct evemu_device *dev, unsigned int vendor);
unsigned int evemu_get_id_product(const struct evemu_device *dev);
void evemu_set_id_product(struct evemu_device *dev, unsigned int product);
unsigned int evemu_get_id_version(const struct evemu_device *dev);
void evemu_set_id_version(struct evemu_device *dev, unsigned int version);
int evemu_get_abs_minimum(const struct evemu_device *dev, int code);
void evemu_set_abs_minimum(struct evemu_device *dev, int code, int min);
int evemu_get_abs_maximum(const struct evemu_device *dev, int code);
int evemu_get_abs_current_value(const struct evemu_device *dev, int code);

void evemu_set_abs_maximum(struct evemu_device *dev, int code, int max);
int evemu_get_abs_fuzz(const struct evemu_device *dev, int code);
void evemu_set_abs_fuzz(struct evemu_device *dev, int code, int fuzz);
int evemu_get_abs_flat(const struct evemu_device *dev, int code);
void evemu_set_abs_flat(struct evemu_device *dev, int code, int flat);
int evemu_get_abs_resolution(const struct evemu_device *dev, int code);
void evemu_set_abs_resolution(struct evemu_device *dev, int code, int res);
int evemu_has_prop(const struct evemu_device *dev, int code);
int evemu_has_event(const struct evemu_device *dev, int type, int code);
int evemu_has_bit(const struct evemu_device *dev, int type);
int evemu_extract(struct evemu_device *dev, int fd);
int evemu_write(const struct evemu_device *dev, FILE *fp);
int evemu_read(struct evemu_device *dev, FILE *fp);
int evemu_write_event(FILE *fp, const struct input_event *ev);
int evemu_create_event(struct input_event *ev, int type, int code, int value);
int evemu_read_event(FILE *fp, struct input_event *ev);
int evemu_read_event_realtime(FILE *fp, struct input_event *ev, struct timeval *evtime);
int evemu_record(FILE *fp, int fd, int ms);

int evemu_play_one(int fd, const struct input_event *ev);
int evemu_play(FILE *fp, int fd);
int evemu_create(struct evemu_device *dev, int fd);
int evemu_create_managed(struct evemu_device *dev);
const char *evemu_get_devnode(struct evemu_device *dev);
void evemu_destroy(struct evemu_device *dev);

*/

public static class Program 
{
	public static uint HexToUInt(string x) 
	{
		uint result = 0;
		string chars = "0123456789abcdef";
		for (int i = 0; i < x.Length; i++) { result += (uint)chars.IndexOf(x[i]); result <<= 4; }
		return result;
	}

	private static int Main(string[] args) 
	{
		if (args.Length == 0 || (args.Length == 1 && (args[0] == "-h" || args[0] == "--help" || args[0] == "help"))) 
		{
			Console.WriteLine("Usage:\n\tkeypress <key0> [key1] [key...]");
			return 0;
		}

		uint[] keycodes = new uint[args.Length];
		for (int i = 0; i < keycodes.Length; i++) 
		{
			if (uint.TryParse(args[i], out uint key)) 
			{
				keycodes[i] = key;
				continue;
			}
			else if (Enum.TryParse<Input.KeyCode>(args[i], true, out Input.KeyCode keycode)) 
			{
				keycodes[i] = (uint)keycode;
				continue;
			}
			else if (args[i].StartsWith("0x")) 
			{
				keycodes[i] = HexToUInt(args[i]);
			}
		}

		foreach (uint key in keycodes) { Console.WriteLine(key); }

		return 0;
	}
}