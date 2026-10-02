// Copyright Grant Abernathy. All rights reserved.

using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;

namespace Mister.Core.Math;

/// <summary>
/// Represents a 4-dimensional vector with single-precision floating-point components.
/// </summary>
[StructLayout(LayoutKind.Explicit, Pack = 16)]
public struct Vector4
{
    [FieldOffset(0)]
    public Vector128<float> V128;

    [FieldOffset(0)] public float X;
    [FieldOffset(4)] public float Y;
    [FieldOffset(8)] public float Z;
    [FieldOffset(12)] public float W;

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4"/> struct with the specified components.
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    /// <param name="z">The Z component.</param>
    /// <param name="w">The W component.</param>
    public Vector4(float x, float y, float z, float w)
    {
        V128 = Vector128.Create(x, y, z, w);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4"/> struct from a Vector128<float>.
    /// </summary>
    /// <param name="v128">The Vector128<float> to initialize the Vector4 from.</param>
    public Vector4(Vector128<float> v128)
    {
        V128 = v128;
    }

    /// <summary>
    /// Adds two Vector4 instances together and returns the result.
    /// </summary>
    /// <param name="a">The first Vector4 instance.</param>
    /// <param name="b">The second Vector4 instance.</param>
    /// <returns>The result of adding the two Vector4 instances.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(in Vector4 a, in Vector4 b)
    {
        return new Vector4(a.V128 + b.V128);
    }

    /// <summary>
    /// Multiplies a Vector4 instance by a scalar value and returns the result.
    /// </summary>
    /// <param name="a">The Vector4 instance to multiply.</param>
    /// <param name="scalar">The scalar value to multiply by.</param>
    /// <returns>The result of multiplying the Vector4 instance by the scalar value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(in Vector4 a, float scalar)
    {
        return new Vector4(a.V128 * Vector128.Create(scalar));
    }

    /// <summary>
    /// Calculates the dot product of two Vector4 instances and returns the result.
    /// </summary>
    /// <param name="a">The first Vector4 instance.</param>
    /// <param name="b">The second Vector4 instance.</param>
    /// <returns>The dot product of the two Vector4 instances.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(in Vector4 a, in Vector4 b)
    {
        return Vector128.Dot(a.V128, b.V128);
    }
}
