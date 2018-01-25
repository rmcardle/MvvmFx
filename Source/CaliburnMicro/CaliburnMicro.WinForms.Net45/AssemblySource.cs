﻿namespace MvvmFx.CaliburnMicro
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
#if WISEJ
    using Control = Wisej.Web.Control;
#else
    using Control = System.Windows.Forms.Control;
#endif

    /// <summary>
    /// A source of assemblies that are inspectable by the framework.
    /// </summary>
    public static class AssemblySource
    {
        /// <summary>
        /// The singleton instance of the AssemblySource used by the framework.
        /// </summary>
        public static readonly IObservableCollection<Assembly> Instance = new BindableCollection<Assembly>();

        private static readonly IDictionary<string, Type> TypeNameCache = new Dictionary<string, Type>();

        static AssemblySource()
        {
            Instance.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        e.NewItems.OfType<Assembly>()
                            .SelectMany(a => ExtractTypes(a))
                            .Apply(t => TypeNameCache.Add(t.FullName, t));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Reset:
                        TypeNameCache.Clear();
                        Instance
                            .SelectMany(a => ExtractTypes(a))
                            .Apply(AddToCache);
                        break;
                }
            };
        }

        private static void AddToCache(Type t)
        {
            if (t.FullName != null && !TypeNameCache.ContainsKey(t.FullName))
                TypeNameCache.Add(t.FullName, t);
        }

        /// <summary>
        /// Extracts the types from the spezified assembly for <see cref="FindTypeByNames"/>.
        /// </summary>
        public static Func<Assembly, IEnumerable<Type>> ExtractTypes = assembly =>
            assembly.GetExportedTypes()
                .Where(t =>
                    typeof(Control).IsAssignableFrom(t) ||
                    typeof(INotifyPropertyChanged).IsAssignableFrom(t));

        /// <summary>
        /// Finds a type which matches one of the elements in the sequence of names.
        /// </summary>
        public static Type FindTypeByNames(IEnumerable<string> names)
        {
            if (names == null)
            {
                return null;
            }

            var type = names.Select(n => TypeNameCache.GetValueOrDefault(n)).FirstOrDefault(t => t != null);
            return type;
        }
    }
}