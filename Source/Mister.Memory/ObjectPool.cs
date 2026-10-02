// Copyright Grant Abernathy. All rights reserved.

using System;

namespace Mister.Memory;

/// <summary>
/// An object pool that preallocates a fixed number of objects and manages their reuse.
/// </summary>
/// <typeparam name="T">The type of objects to be pooled.</typeparam>
public class ObjectPool<T> where T : class
{
    private readonly T[] _pool;
    private int _count;
    private readonly Func<T> _factory;

    /// <summary>
    /// Allocates the entire pool upfront.
    /// </summary>
    /// <param name="capacity">The number of objects to preallocate in the pool.</param>
    /// <param name="factory">A factory function to create new instances of the pooled object.</param>
    public ObjectPool(int capacity, Func<T> factory)
    {
        _pool = new T[capacity];
        _factory = factory;
        _count = capacity;

        for (int i = 0; i < capacity; i++)
        {
            _pool[i] = _factory();
        }
    }

    /// <summary>
    /// Grabs an object from the pool.
    /// </summary>
    /// <returns>The object retrieved from the pool.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the pool is exhausted and no objects are available.</exception>
    public T Get()
    {
        if (_count == 0)
        {
            // Sometimes you'd see this resize in implementations to grow the pool dynamically, but
            // resizing allocates memory so it is avoided. It must fit within the strict memory
            // budget!
            throw new InvalidOperationException($"ObjectPool for {typeof(T).Name} is exhausted!");
        }

        T item = _pool[--_count];

        if (item is IPoolable poolable)
        {
            poolable.OnSpawn();
        }

        return item;
    }

    /// <summary>
    /// Returns an object to the pool.
    /// </summary>
    /// <param name="item">The object to return to the pool.</param>
    /// <exception cref="InvalidOperationException">Thrown if the pool is already full when returning an object.</exception>
    public void Return(T item)
    {
        if (_count == _pool.Length)
        {
            throw new InvalidOperationException($"Trying to return an item to a full pool!");
        }

        if (item is IPoolable poolable)
        {
            poolable.OnDespawn();
        }

        _pool[_count++] = item;
    }
}
