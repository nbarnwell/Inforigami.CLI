namespace Inforigami.CLI
{
    using System.Collections.Generic;

    public static class IEnumerableExtensions
    {
        public static Queue<T> ToQueue<T>(this IEnumerable<T> enumerable)
        {
            var result = new Queue<T>();
            foreach (var item in enumerable)
            {
                result.Enqueue(item);
            }

            return result;
        }

        public static Stack<T> ToStack<T>(this IEnumerable<T> enumerable)
        {
            var result = new Stack<T>();
            foreach (var item in enumerable)
            {
                result.Push(item);
            }

            return result;
        }
    }
}