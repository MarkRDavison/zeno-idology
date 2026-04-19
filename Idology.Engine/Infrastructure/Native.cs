using System.Runtime.InteropServices;

namespace Idology.Engine.Infrastructure;

static class Native
{
    [DllImport("libc", EntryPoint = "vsnprintf", CallingConvention = CallingConvention.Cdecl)]
    private static extern unsafe int vsnprintf_unix(
        byte* buffer,
        nuint size,
        sbyte* format,
        sbyte* args);

    public static unsafe int vsnprintf(
        byte* buffer,
        nuint size,
        sbyte* format,
        sbyte* args)
    {
        if (OperatingSystem.IsWindows())
        {
            return Windows(buffer, size, format, args);
        }

        return vsnprintf_unix(buffer, size, format, args);
    }

    [DllImport("msvcrt", EntryPoint = "vsnprintf", CallingConvention = CallingConvention.Cdecl)]
    private static extern unsafe int Windows(
        byte* buffer,
        nuint size,
        sbyte* format,
        sbyte* args);
}