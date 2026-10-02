// Copyright Grant Abernathy. All rights reserved.

using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;

namespace Mister.Core.Math;

[StructLayout(LayoutKind.Explicit, Pack = 16)]
public struct Vector4
{
    [FieldOffset(0)]
    public Vector128<float> V128;

    [FieldOffset(0)] public float X;
    [FieldOffset(4)] public float Y;
    [FieldOffset(8)] public float Z;
    [FieldOffset(12)] public float W;

    public Vector4(float x, float y, float z, float w)
    {
        V128 = Vector128.Create(x, y, z, w);
    }

    public Vector4(Vector128<float> v128)
    {
        V128 = v128;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(in Vector4 a, in Vector4 b)
    {
        return new Vector4(a.V128 + b.V128);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(in Vector4 a, float scalar)
    {
        return new Vector4(a.V128 * Vector128.Create(scalar));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(in Vector4 a, in Vector4 b)
    {
        return Vector128.Dot(a.V128, b.V128);
    }
}
