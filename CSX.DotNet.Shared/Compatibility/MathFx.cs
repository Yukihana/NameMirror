using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.DotNet.Shared.Compatibility;

public static partial class MathFx
{
    public static int Clamp(int value, int min, int max)
    {
#if NET6_0_OR_GREATER
        return Math.Clamp(value, min, max);
#elif NETFRAMEWORK
        if (value < min)
            return min;
        if (value > max)
            return max;
        return value;
#endif
    }

    public static byte Clamp(byte value, byte min, byte max)
    {
#if NET6_0_OR_GREATER
        return Math.Clamp((byte)value, (byte)0, (byte)3);
#elif NETFRAMEWORK
        if (value < min)
            return min;
        if (value > max)
            return max;
        return value;
#endif
    }
}