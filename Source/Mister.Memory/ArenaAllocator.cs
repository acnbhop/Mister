// Copyright Grant Abernathy. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace Mister.Memory;

/// <summary>
/// An arena allocator that allocates memory in a contiguous block and allows for fast allocation and deallocation of objects.
/// </summary>
public unsafe class ArenaAllocator : IDisposable
{
    /// <summary>
    /// Pointer to the start of the allocated memory block.
    /// </summary>
    private byte* _buffer;

    /// <summary>
    /// The total capacity of the arena allocator in bytes.
    /// </summary>
    private nuint _capacity;

    /// <summary>
    /// The current offset in the allocated memory block, indicating where the next allocation will occur.
    /// </summary>
    private nuint _offset;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArenaAllocator"/> class with the specified capacity in bytes.
    /// </summary>
    /// <param name="capacityInBytes">The capacity of the arena allocator in bytes.</param>
    public ArenaAllocator(nuint capacityInBytes)
    {
        _capacity = capacityInBytes;
        _buffer = (byte*)NativeMemory.Alloc(_capacity);
        _offset = 0;
    }

    /// <summary>
    /// Allocates an object of type <typeparamref name="T"/> from the arena allocator with the specified alignment.
    /// </summary>
    /// <typeparam name="T">The type of the object to allocate.</typeparam>
    /// <param name="alignment">The alignment of the object in bytes.</param>
    /// <returns>A reference to the allocated object.</returns>
    /// <exception cref="OutOfMemoryException">Thrown when the arena allocator is out of memory.</exception>
    public ref T Allocate<T>(nuint alignment = 16) where T : unmanaged
    {
        AlignOffset(alignment);

        nuint size = (nuint)sizeof(T);

        if (_offset + size > _capacity)
        {
            throw new OutOfMemoryException("ArenaAllocator is out of memory!");
        }

        byte* ptr = _buffer + _offset;
        _offset += size;

        return ref *(T*)ptr;
    }

    /// <summary>
    /// Allocates a span of objects of type <typeparamref name="T"/> from the arena allocator with the specified alignment.
    /// </summary>
    /// <typeparam name="T">The type of the objects to allocate.</typeparam>
    /// <param name="count">The number of objects to allocate.</param>
    /// <param name="alignment">The alignment of the objects in bytes.</param>
    /// <returns>A span of the allocated objects.</returns>
    /// <exception cref="OutOfMemoryException">Thrown when the arena allocator is out of memory.</exception>
    public Span<T> AllocateSpan<T>(int count, nuint alignment = 16) where T : unmanaged
    {
        AlignOffset(alignment);

        nuint size = (nuint)(sizeof(T) * count);

        if (_offset + size > _capacity)
        {
            throw new OutOfMemoryException("ArenaAllocator is out of memory!");
        }

        byte* ptr = _buffer + _offset;
        _offset += size;

        return new Span<T>(ptr, count);
    }

    /// <summary>
    /// Resets the arena allocator, allowing for reuse of the allocated memory. This does not free the memory, but simply resets the allocation offset to zero.
    /// </summary>
    public void Reset()
    {
        _offset = 0;
    }

    /// <summary>
    /// Frees the memory allocated by the arena allocator. After calling this method, the allocator should not be used again.
    /// </summary>
    public void Dispose()
    {
        if (_buffer != null)
        {
            NativeMemory.Free(_buffer);
            _buffer = null;
        }
    }

    /// <summary>
    /// Aligns the current offset to the specified alignment, ensuring the next allocation starts at a properly aligned address.
    /// </summary>
    /// <param name="alignment">The alignment in bytes.</param>
    private void AlignOffset(nuint alignment)
    {
        nuint mask = alignment - 1;
        _offset = (_offset + mask) & ~mask;
    }
}