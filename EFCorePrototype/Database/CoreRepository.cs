using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace EFCorePrototype.Database
{
    public class CoreRespository<TEntity> where TEntity : class
    {
        protected EFCorePrototypeDatabase Database { get; }

        public CoreRespository(EFCorePrototypeDatabase database)
        {
            Database = database;
        }

        /// <summary>
        /// Returns a query to load the given entity by primary key
        /// </summary>
        /// <param name="entity">The entity with filled primary key properties</param>
        /// <returns>A query to load the entity from the database</returns>
        public IQueryable<TEntity> GetByPrimaryKey(TEntity entity)
        {
            return GetByPrimaryKey(GetPrimaryKeyValues(entity));
        }

        /// <summary>
        /// Returns a query to load the given entity by primary key
        /// </summary>
        /// <param name="primaryKeys">A collection of primary keys with values</param>
        /// <returns>A query to load the entity from the database</returns>
        protected IQueryable<TEntity> GetByPrimaryKey(ICollection<KeyValuePair<string, object>> primaryKeys)
        {
            ParameterExpression? parameter = Expression.Parameter(typeof(TEntity));
            Expression<Func<TEntity, bool>>? expression = null;

            foreach (KeyValuePair<string, object> primaryKey in primaryKeys)
            {
                string primaryKeyName = primaryKey.Key;
                object primaryKeyValue = primaryKey.Value;

                var body = Expression.Equal(
                    Expression.Property(parameter, primaryKeyName),
                    Expression.Constant(primaryKeyValue)
                );

                // ToDo: If already initialized, we append the new expression.
                // The init. could probably moved outside the foreach,
                // but lists are unsrted and sorting costs too much time
                if (expression != null)
                    body = Expression.AndAlso(expression.Body, body);

                expression = Expression.Lambda<Func<TEntity, bool>>(
                    body,
                    parameter
                );
            }

            if (expression == null)
                throw new Exception($"Could not create expression for entity {typeof(TEntity)}");

            return Database.Set<TEntity>().AsQueryable().Where(expression);
        }

        /// <summary>
        /// Create a collection of KeyValue-Pairs of the entity, with the primary key
        /// as name and the primary key value as value
        /// </summary>
        /// <param name="entity">The entity with the primary key values</param>
        /// <returns>A collection of primary keys with values</returns>
        /// <exception cref="ArgumentNullException">If no entity is passed</exception>
        /// <exception cref="Exception">If the entity has no primary key</exception>
        /// <exception cref="Exception">If the value of property couldn't be loaded</exception>
        protected ICollection<KeyValuePair<string, object>> GetPrimaryKeyValues(TEntity entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            Type entityType = entity.GetType();

            IKey? primaryKey = Database.Model?.FindEntityType(entityType)?.FindPrimaryKey();

            if (primaryKey == null || !primaryKey.Properties.Any())
            {
                throw new Exception($"Entity of type {entityType.Name} has no primary key declared");
            }

            ICollection<KeyValuePair<string, object>> primaryKeyValues = new List<KeyValuePair<string, object>>();

            foreach (IProperty property in primaryKey.Properties)
            {
                string propertyName = property.Name;
                // ToDo: Get rid of the reflection, costs too much time
                object? propertyValue = property.GetGetter().GetClrValue(entity);
                if (propertyValue == null)
                {
                    // ToDo: Replace with logger
                    Console.WriteLine($"Could not load value for property {propertyName} of entity {entityType.Name}.{Environment.NewLine}Partially loading is not recommendet");
                    continue;
                }
                primaryKeyValues.Add(new KeyValuePair<string, object>(propertyName, propertyValue));
            }

            if (!primaryKeyValues.Any())
            {
                throw new Exception($"Could not load any primary key values for entity {entityType.Name}");
            }

            return primaryKeyValues;
        }
    }
}
