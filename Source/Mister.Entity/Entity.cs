// Copyright Grant Abernathy. All rights reserved.

namespace Mister.Entity;

/// <summary>
/// Represents an entity.
/// </summary>
public readonly struct Entity
{
    /// <summary>
    /// The unique identifier of the entity.
    /// </summary>
    public readonly int Id;

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity"/> struct with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    public Entity(int id)
    {
        Id = id;
    }
}
