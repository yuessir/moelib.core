using System;

namespace MoeLibCore
{
    /// <summary>
    ///     Extensions of <see cref="object" />.
    /// </summary>
    public static class ObjectExtensions
    {
        public static TResult To<TObject, TResult>(this TObject value, Func<TObject, TResult> converter)
        {
            return converter.Invoke(value);
        }
    }
}