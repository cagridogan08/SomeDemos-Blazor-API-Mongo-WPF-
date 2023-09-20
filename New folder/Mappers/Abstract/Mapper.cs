using System.Collections.Generic;
using System.Linq;

namespace WpfAppWithRedisCache.Mappers.Abstract
{
    /// <summary>
    ///     Base class for an object mapper that maps objects of type TFirst to TSecond or vice versa.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first object to be mapped.</typeparam>
    /// <typeparam name="TSecond">The type of the second object to be mapped.</typeparam>
    public abstract class MapperBase<TFirst, TSecond>
    {
        /// <summary>
        /// Maps an object of type TSecond to an object of type TFirst.
        /// </summary>
        /// <param name="element">The object of type TSecond to be mapped.</param>
        /// <returns>The mapped object of type TFirst.</returns>
        public abstract TFirst Map(TSecond element);

        /// <summary>
        /// Maps an object of type TFirst to an object of type TSecond.
        /// </summary>
        /// <param name="element">The object of type TFirst to be mapped.</param>
        /// <returns>The mapped object of type TSecond.</returns>
        public abstract TSecond Map(TFirst element);

        /// <summary>
        /// Enumerable mapping of a collection of TSecond objects to TFirst objects.
        /// </summary>
        /// <param name="elements">The collection of TSecond objects to be mapped.</param>
        /// <returns>An IEnumerable containing the mapped TFirst objects.</returns>
        public virtual IEnumerable<TFirst> Map(IEnumerable<TSecond> elements)
        {
            return elements.Select(Map);
        }

        /// <summary>
        /// Enumerable mapping of a collection of TFirst objects to TSecond objects.
        /// </summary>
        /// <param name="elements">The collection of TFirst objects to be mapped.</param>
        /// <returns>An IEnumerable containing the mapped TSecond objects.</returns>
        public virtual IEnumerable<TSecond> Map(IEnumerable<TFirst> elements)
        {
            return elements.Select(Map);
        }
    }
}
