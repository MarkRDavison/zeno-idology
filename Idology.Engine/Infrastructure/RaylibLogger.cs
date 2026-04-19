using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Idology.Engine.Infrastructure;

public static unsafe class RaylibLogger
{
    private static ILogger? _general;

    private static ILogger? _texture;
    private static ILogger? _audio;
    private static ILogger? _gl;
    private static ILogger? _fileio;

    public static void Initialize(ILoggerFactory factory)
    {
        _general = factory.CreateLogger("Raylib");

        _texture = factory.CreateLogger("Raylib.Texture");
        _audio = factory.CreateLogger("Raylib.Audio");
        _gl = factory.CreateLogger("Raylib.GL");
        _fileio = factory.CreateLogger("Raylib.FileIO");

        Raylib.SetTraceLogCallback(&TraceLogCallback);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ILogger GetLogger(sbyte* text, out sbyte* messageStart)
    {
        byte* p = (byte*)text;

        // 1. Skip log level prefix: "INFO:" / "DEBUG:" etc
        byte* start = p;
        while (*p != 0 && *p != (byte)':') p++;

        if (*p == 0)
        {
            messageStart = text;
            return _general!;
        }

        // skip ":"
        p++;

        // 2. skip space after level
        if (*p == (byte)' ') p++;

        byte* categoryStart = p;

        // 3. find second ':'
        while (*p != 0 && *p != (byte)':') p++;

        if (*p == 0)
        {
            messageStart = text;
            return _general!;
        }

        int categoryLen = (int)(p - categoryStart);

        // 4. advance past ":"
        p++;

        messageStart = (sbyte*)p;

        // 5. match category safely
        if (Matches(categoryStart, categoryLen, "rcore")) return _general!;
        if (Matches(categoryStart, categoryLen, "rlgl")) return _gl!;
        if (Matches(categoryStart, categoryLen, "rtextures")) return _texture!;
        if (Matches(categoryStart, categoryLen, "raudio")) return _audio!;
        if (Matches(categoryStart, categoryLen, "rfileio")) return _fileio!;

        return _general!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Match(byte* ptr, string keyword)
    {
        for (int i = 0; i < keyword.Length; i++)
        {
            if (ptr[i] != keyword[i])
                return false;
        }
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Matches(byte* ptr, int len, string value)
    {
        if (len != value.Length)
            return false;

        for (int i = 0; i < len; i++)
        {
            if (ptr[i] != value[i])
                return false;
        }

        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void TraceLogCallback(
    int logLevel,
    sbyte* text,
    sbyte* extra)
    {
        if (_general == null)
        {
            return;
        }

        var ll = MapLevel(logLevel);

        if (logLevel == (int)TraceLogLevel.None)
        {
            return;
        }

        const int BufferSize = 4096;

        byte* buffer = stackalloc byte[BufferSize];

        Native.vsnprintf(buffer, BufferSize, text, extra);

        if (TryGetCategory((sbyte*)buffer, out var cat, out var len, out var msg))
        {
            var logger = ResolveLogger(cat, len);

            string message = new string((sbyte*)msg);

            logger.Log(ll, "{Message}", message);
        }
        else
        {
            string message = Marshal.PtrToStringAnsi((IntPtr)buffer) ?? "";

            _general.Log(ll, "{Message}", message);
        }
    }

    private static ILogger ResolveLogger(byte* category, int len)
    {
        // TEXTURE
        if (len == 7 &&
            category[0] == 'T' &&
            category[1] == 'E' &&
            category[2] == 'X' &&
            category[3] == 'T' &&
            category[4] == 'U' &&
            category[5] == 'R' &&
            category[6] == 'E')
            return _texture!;

        // AUDIO
        if (len == 5 &&
            category[0] == 'A' &&
            category[1] == 'U' &&
            category[2] == 'D' &&
            category[3] == 'I' &&
            category[4] == 'O')
            return _audio!;

        // GL
        if (len == 2 &&
            category[0] == 'G' &&
            category[1] == 'L')
            return _gl!;

        // FILEIO
        if (len == 6 &&
            category[0] == 'F' &&
            category[1] == 'I' &&
            category[2] == 'L' &&
            category[3] == 'E' &&
            category[4] == 'I' &&
            category[5] == 'O')
            return _fileio!;

        return _general!;
    }

    private static bool TryGetCategory(
    sbyte* text,
    out byte* categoryStart,
    out int categoryLen,
    out sbyte* messageStart)
    {
        byte* p = (byte*)text;

        categoryStart = p;

        while (*p != 0 && *p != (byte)':')
            p++;

        if (*p == 0)
        {
            categoryLen = 0;
            messageStart = text;
            return false;
        }

        categoryLen = (int)(p - categoryStart);
        p++; // skip ':'

        while (*p == (byte)' ')
            p++;

        messageStart = (sbyte*)p;
        return true;
    }

    private static LogLevel MapLevel(int level) =>
    level switch
    {
        (int)TraceLogLevel.Trace => LogLevel.Trace,
        (int)TraceLogLevel.Debug => LogLevel.Debug,
        (int)TraceLogLevel.Info => LogLevel.Information,
        (int)TraceLogLevel.Warning => LogLevel.Warning,
        (int)TraceLogLevel.Error => LogLevel.Error,
        (int)TraceLogLevel.Fatal => LogLevel.Critical,
        _ => LogLevel.Information
    };
}