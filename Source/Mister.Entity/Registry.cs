// Copyright Grant Abernathy. All rights reserved.

using System;
using System.Collections.Generic;

namespace Mister.Entity;

/// <summary>
/// Represents a registry that manages entities and their associated component pools.
/// </summary>
public class Registry
{
    /// <summary>
    /// The ID to be assigned to the next created entity.
    /// </summary>
    private int _nextEntityId = 0;

    /// <summary>
    /// A dictionary that maps component types to their corresponding component pools.
    /// </summary>
    private readonly Dictionary<Type, object> _pools = new();

    /// <summary>
    /// The maximum number of entities that can be managed by the registry.
    /// </summary>
    private readonly int _maxEntities;

    /// <summary>
    /// Initializes a new instance of the <see cref="Registry"/> class with the specified maximum number of entities.
    /// </summary>
    /// <param name="maxEntities">The maximum number of entities that can be managed by the registry.</param>
    public Registry(int maxEntities = 50000)
    {
        _maxEntities = maxEntities;
    }

    /// <summary>
    /// Creates a new entity and returns it.
    /// </summary>
    /// <returns>The newly created entity.</returns>
    public Entity CreateEntity()
    {
        return new Entity(_nextEntityId++);
    }

    /// <summary>
    /// Registers a component type with the registry.
    /// </summary>
    /// <typeparam name="T">The type of the component to register.</typeparam>
    public void RegisterComponent<T>() where T : unmanaged
    {
        _pools[typeof(T)] = new ComponentPool<T>(_maxEntities);
    }

    /// <summary>
    /// Adds a component of type T to the specified entity and returns a reference to the added component.
    /// </summary>
    /// <typeparam name="T">The type of the component to add.</typeparam>
    /// <param name="entity">The entity to which the component will be added.</param>
    /// <param name="component">The component to add.</param>
    /// <returns>A reference to the added component.</returns>
    public ref T AddComponent<T>(Entity entity, T component) where T : unmanaged
    {
        var pool = (ComponentPool<T>)_pools[typeof(T)];

        return ref pool.Add(entity, component);
    }
    
    /// <summary>
    /// Gets the component pool for the specified component type.
    /// </summary>
    /// <typeparam name="T">The type of the component.</typeparam>
    /// <returns>The component pool for the specified component type.</returns>
    public ComponentPool<T> GetPool<T>() where T : unmanaged
    {
        return (ComponentPool<T>)_pools[typeof(T)];
    }
}
