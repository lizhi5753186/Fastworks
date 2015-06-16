using Fastworks.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fastworks.Storage
{
    /// <summary>
    /// Represents that the implemented classes are storages.
    /// </summary>
    public interface IStorage : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Gets the first-only object from storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <returns>The instance of the object.</returns>
        T SelectFirstOnly<T>()
            where T : class, new();
        /// <summary>
        /// Gets the first-only object from the storage by given specification.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <returns>The instance of the object.</returns>
        T SelectFirstOnly<T>(ISpecification<T> specification)
            where T : class, new();
        /// <summary>
        /// Gets the number of records in the storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <returns>The number of records in the storage.</returns>
        int GetRecordCount<T>()
            where T : class, new();
        /// <summary>
        /// Gets the number of records in the storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <returns>The number of records in the storage.</returns>
        int GetRecordCount<T>(ISpecification<T> specification)
            where T : class, new();
        /// <summary>
        /// Gets a list of all objects from storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <returns>A list of the objects.</returns>
        IEnumerable<T> Select<T>()
            where T : class, new();
        /// <summary>
        /// Gets a list of objects from storage by given specification.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <returns>A list of the objects.</returns>
        IEnumerable<T> Select<T>(ISpecification<T> specification)
            where T : class, new();
        /// <summary>
        /// Gets a list of ordered objects from storage by given specification.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <param name="orders">The <c>PropertyBag</c> instance which contains the ordering fields.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>A list of ordered objects.</returns>
        IEnumerable<T> Select<T>(ISpecification<T> specification, PropertyBag orders, SortOrder sortOrder)
            where T : class, new();
        /// <summary>
        /// Inserts the object into the storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to be inserted.</typeparam>
        /// <param name="allFields">The <c>PropertyBag</c> instance which contains the properties and property values
        /// to be inserted.</param>
        void Insert<T>(PropertyBag allFields)
            where T : class, new();
        /// <summary>
        /// Deletes all objects from storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to be deleted.</typeparam>
        void Delete<T>()
            where T : class, new();
        /// <summary>
        /// Deletes specified objects from storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to be deleted.</typeparam>
        /// <param name="specification">The specification.</param>
        void Delete<T>(ISpecification<T> specification)
            where T : class, new();
        /// <summary>
        /// Updates all the objects in storage with the given values.
        /// </summary>
        /// <typeparam name="T">The type of the object to be updated.</typeparam>
        /// <param name="newValues">The <c>PropertyBag</c> instance which contains the properties and property values
        /// to be updated.</param>
        void Update<T>(PropertyBag newValues)
            where T : class, new();
        /// <summary>
        /// Updates all the objects in storage with the given values and the specification.
        /// </summary>
        /// <typeparam name="T">The type of the object to be updated.</typeparam>
        /// <param name="newValues">The <c>PropertyBag</c> instance which contains the properties and property values
        /// to be updated.</param>
        /// <param name="specification">The specification.</param>
        void Update<T>(PropertyBag newValues, ISpecification<T> specification)
            where T : class, new();
    }
}
