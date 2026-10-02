// Copyright Grant Abernathy. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace Mister.Core.Memory;

public unsafe class ArenaAllocator : IDisposable
{
    private byte* _buffer;
    private nuint _capacity;
    private nuint _offset;

    public ArenaAllocator(nuint capacityInBytes)
    {
        _capacity = capacityInBytes;
        _buffer = (byte*)NativeMemory.Alloc(_capacity);
        _offset = 0;
    }

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

    public void Reset()
    {
        _offset = 0;
    }

    public void Dispose()
    {
        if (_buffer != null)
        {
            NativeMemory.Free(_buffer);
            _buffer = null;
        }
    }

    private void AlignOffset(nuint alignment)
    {
        nuint mask = alignment - 1;
        _offset = (_offset + mask) & ~mask;
    }
}