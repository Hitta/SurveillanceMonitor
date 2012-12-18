using System;
using System.Collections.Generic;

namespace Hitta.Surveillance.Monitor
{
    public static class Extensions
    {
        public static Queue<TSource> DequeueWhile<TSource>(this Queue<TSource> source, Func<TSource, bool> predicate )
        {
            var queue = new Queue<TSource>();

            while (source.Count > 0 && predicate(source.Peek()))
            {
                queue.Enqueue(source.Dequeue());
            }
            
            return queue;
        }
    }
}

namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute { }
}