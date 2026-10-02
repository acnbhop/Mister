// Copyright Grant Abernathy. All rights reserved.

namespace Mister.Core.Memory;

public interface IPoolable
{
    /// <summary>
    /// Called when the object is pulled from the pool.
    /// </summary>
    void OnSpawn();

    /// <summary>
    /// Called right before the object goes back into the pool. Clear references here.
    /// </summary>
    void OnDespawn();
}