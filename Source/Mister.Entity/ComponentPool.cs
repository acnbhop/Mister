// Copyright Grant Abernathy. All rights reserved.

using System;

namespace Mister.Entity;

/// <summary>
/// Represents a pool of components of a specific type.
/// </summary>
/// <typeparam name="T">The type of component stored in the pool.</typeparam>
public class ComponentPool<T> where T : unmanaged
{
    /// <summary>
    /// Array that stores the components in a dense layout.
    /// </summary>
    public T[] DenseData;

    /// <summary>
    /// Maps dense array indices to entity IDs.
    /// </summary>
    public int[] DenseToEntity;

    /// <summary>
    /// Maps entity IDs to dense array indices.
    /// </summary>
    public int[] Sparse;

    /// <summary>
    /// The amount of components currently stored in the pool.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentPool{T}"/> class with the specified maximum number of entities.
    /// </summary>
    /// <param name="maxEntities">The maximum number of entities that can be stored in the pool.</param>
    public ComponentPool(int maxEntities)
    {
        DenseData = new T[maxEntities];
        DenseToEntity = new int[maxEntities];
        Sparse = new int[maxEntities];

        Array.Fill(Sparse, -1);
        Count = 0;
    }

    /// <summary>
    /// Adds a component for the specified entity to the pool.
    /// </summary>
    /// <param name="entity">The entity to add the component for.</param>
    /// <param name="component">The component to add.</param>
    /// <returns>A reference to the added component in the pool.</returns>
    public ref T Add(Entity entity, T component)
    {
        int entityId = entity.Id;
        int denseIndex = Count;

        DenseData[denseIndex] = component;
        DenseToEntity[denseIndex] = entityId;

        Sparse[entityId] = denseIndex;

        Count++;

        return ref DenseData[denseIndex];
    }

    /// <summary>
    /// Gets the component associated with the specified entity.
    /// </summary>
    /// <param name="entity">The entity to get the component for.</param>
    /// <returns>A reference to the component associated with the entity.</returns>
    public ref T Get(Entity entity)
    {
        int denseIndex = Sparse[entity.Id];

        return ref DenseData[denseIndex];
    }

    /// <summary>
    /// Removes the component associated with the specified entity from the pool.
    /// </summary>
    /// <param name="entity">The entity to remove the component for.</param>
    public void Remove(Entity entity)
    {
        int entityId = entity.Id;
        int denseIndexToRemove = Sparse[entityId];

        int lastDenseIndex = Count - 1;
        T lastComponent = DenseData[lastDenseIndex];
        int lastEntityId = DenseToEntity[lastDenseIndex];

        DenseData[denseIndexToRemove] = lastComponent;
        DenseToEntity[denseIndexToRemove] = lastEntityId;
        Sparse[lastEntityId] = denseIndexToRemove;

        Sparse[entityId] = -1;
        Count--;
    }
}
