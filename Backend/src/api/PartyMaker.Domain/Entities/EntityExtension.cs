using System;

namespace PartyMaker.Domain.Entities
{
    public static class EntityExtension
    {
        public static TEntity MarkAsNew<TEntity>(this TEntity entity)
            where TEntity : Entity
        {
            entity.Created = DateTime.UtcNow;
            entity.Updated = DateTime.UtcNow;
            entity.Id = Guid.NewGuid();
            entity.IsDeleted = false;

            return entity;
        }

        public static TEntity MarkAsUpdated<TEntity>(this TEntity entity)
            where TEntity : Entity
        {
            entity.Updated = DateTime.UtcNow;

            return entity;
        }

        public static TEntity MarkAsNormal<TEntity>(this TEntity entity)
            where TEntity : Entity
        {
            entity.Updated = DateTime.UtcNow;
            entity.IsDeleted = false;

            return entity;
        }

        public static TEntity MarkAsDeleted<TEntity>(this TEntity entity)
            where TEntity : Entity
        {
            entity.Updated = DateTime.UtcNow;
            entity.IsDeleted = true;

            return entity;
        }
    }
}
