using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace CCDataAccess.Helpers
{
    public static class EntityFrameWorkExtension
    {
        public static DbCommand LoadStoredProc(this DbContext context, string storedProcName)
        {
            // Use GetDbConnection extension method from RelationalDatabaseFacadeExtensions
            var cmd = context.Database.GetDbConnection().CreateCommand(); // Ensure the correct namespace is included
            cmd.CommandText = storedProcName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }
    }
    /// <summary>
    /// Executes a DbDataReader and returns a list of mapped column values to the properties of <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="command"></param>
    /// <returns></returns>

    internal abstract class EntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Apply the entity configuration to a builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }
}
