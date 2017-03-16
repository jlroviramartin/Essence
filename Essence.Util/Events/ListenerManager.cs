#region License

// Copyright 2017 Jose Luis Rovira Martin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Essence.Util.Collections;

namespace Essence.Util.Events
{
    /// <summary>
    ///     Gestor de 'escuchadores' (listeners). Permite registar escuchadores (listeners) para ciertos eventos.
    ///     Por ahora no utiliza eventos debiles.
    /// </summary>
    public sealed class ListenerManager
    {
        public delegate void UnregisterCallback<in TEventHandler>(object source, TEventHandler eventHandler);

        public delegate void UnregisterCallback<in TSource, in TEventHandler>(TSource source, TEventHandler eventHandler);

        public delegate void RegisterCallback<in TEventHandler>(object source, TEventHandler eventHandler);

        public delegate void RegisterCallback<in TSource, in TEventHandler>(TSource source, TEventHandler eventHandler);

        /// <summary>
        ///     Establece la raiz.
        /// </summary>
        public void SetRoot(object root)
        {
            if (this.root != null)
            {
                this.Unregister(this.root);
            }

            this.root = root;

            if (this.root != null)
            {
                this.Register(this.root);
            }
        }

        public RegisterForImp<TSender> RegisterFor<TSender>() where TSender : INotifyPropertyChangedEx
        {
            return new RegisterForImp<TSender>(this);
        }

        #region private

        private bool IsValid(Type type)
        {
            return this.mapOfListeners.ContainsKey(type);
        }

        private void Register(object obj)
        {
            if (obj == null)
            {
                return;
            }

            Debug.WriteLine("Register<> {0}", obj);
            Debug.Indent();

            foreach (Listener listener in this.mapOfListeners.Get(obj.GetType()).SelectMany(x => x))
            {
                if (listener.ValuesToManager)
                {
                    foreach (object value in listener.GetValues(obj))
                    {
                        this.Register(value);
                    }
                }
                listener.Register(obj);
            }

            Debug.Unindent();
        }

        private void Unregister(object obj)
        {
            if (obj == null)
            {
                return;
            }

            Debug.WriteLine("Unregister<> {0}", new[] { obj });
            Debug.Indent();

            foreach (Listener listener in this.mapOfListeners.Get(obj.GetType()).SelectMany(x => x))
            {
                listener.Unregister(obj);
                if (listener.ValuesToManager)
                {
                    foreach (object value in listener.GetValues(obj))
                    {
                        this.Unregister(value);
                    }
                }
            }

            Debug.Unindent();
        }

        internal void Add<TSender>(Listener listener)
        {
            this.Add(typeof (TSender), listener);
        }

        internal void Add(Type senderType, Listener listener)
        {
            listener.Manager = this;
            listener.SenderType = senderType;

            List<Listener> listeners;
            if (this.mapOfListeners.OriginalValues.TryGetValue(senderType, out listeners))
            {
                this.mapOfListeners.OriginalValues.Remove(senderType);
            }
            else
            {
                listeners = new List<Listener>();
            }
            listeners.Add(listener);
            this.mapOfListeners.OriginalValues.Add(senderType, listeners);
        }

        internal bool Remove<TSender>(Listener listener)
        {
            return this.Remove(typeof (TSender), listener);
        }

        internal bool Remove(Type senderType, Listener listener)
        {
            List<Listener> listeners;
            if (!this.mapOfListeners.OriginalValues.TryGetValue(senderType, out listeners))
            {
                return false;
            }

            this.mapOfListeners.OriginalValues.Remove(senderType);
            listeners.Remove(listener);
            if (listeners.Count > 0)
            {
                this.mapOfListeners.OriginalValues.Add(senderType, listeners);
            }
            return true;
        }

        /// <summary>
        ///     Busca los listeners del tipo <c>senderType</c> (no busca en lo tipos de los que deriva).
        /// </summary>
        internal IEnumerable<Listener> FindListenersOfExactType(Type senderType)
        {
            return this.mapOfListeners.OriginalValues.GetSafe(senderType) ?? new List<Listener>();
        }

        /// <summary>Raiz de la jerarquia de objetos a escuchar los eventos.</summary>
        private object root;

        /// <summary>Listeners.</summary>
        private readonly DictionaryOfType<List<Listener>> mapOfListeners = new DictionaryOfType<List<Listener>>();

        #endregion

        #region Inner classes

        #region Listener

        /// <summary>
        ///     Clase base de los 'escuchadores' (listeners).
        /// </summary>
        internal abstract class Listener
        {
            public ListenerManager Manager { protected get; set; }
            public Type SenderType { protected get; set; }

            public abstract void Register(object sender);

            public abstract void Unregister(object sender);

            public bool ValuesToManager { get; protected set; }

            public abstract IEnumerable GetValues(object sender);
        }

        /// <summary>
        ///     'Escuchador' (listener) genérico.
        /// </summary>
        internal sealed class EventListener : Listener
        {
            public EventListener(EventHandler eventHandler,
                                 RegisterCallback<EventHandler> register,
                                 UnregisterCallback<EventHandler> unregister)
            {
                this.eventHandler = eventHandler;
                this.register = register;
                this.unregister = unregister;
            }

            public override void Register(object sender)
            {
                this.register(sender, this.OnEvent);
            }

            public override void Unregister(object sender)
            {
                this.unregister(sender, this.OnEvent);
            }

            public override IEnumerable GetValues(object sender)
            {
                return new object[0];
            }

            private void OnEvent(object sender, EventArgs args)
            {
                Debug.WriteLine("EventListener {0}", new object[] { sender });
                if (this.eventHandler != null)
                {
                    this.eventHandler(sender, args);
                }
            }

            #region private

            private readonly EventHandler eventHandler;
            private readonly RegisterCallback<EventHandler> register;
            private readonly UnregisterCallback<EventHandler> unregister;

            #endregion
        }

        /// <summary>
        ///     'Escuchador' (listener) genérico.
        /// </summary>
        internal class EventListener_v2<TEventArgs> : Listener
            where TEventArgs : EventArgs
        {
            public EventListener_v2(EventHandler_v2<TEventArgs> eventHandler,
                                    RegisterCallback<EventHandler_v2<TEventArgs>> register,
                                    UnregisterCallback<EventHandler_v2<TEventArgs>> unregister)
            {
                this.register = register;
                this.unregister = unregister;
                this.eventHandler = eventHandler;
            }

            public override void Register(object sender)
            {
                this.register(sender, this.OnEvent);
            }

            public override void Unregister(object sender)
            {
                this.unregister(sender, this.OnEvent);
            }

            public override IEnumerable GetValues(object sender)
            {
                return new object[0];
            }

            protected virtual void OnEvent(object sender, TEventArgs args)
            {
                Debug.WriteLine("EventListener_v2<> {0}", new object[] { sender });
                if (this.eventHandler != null)
                {
                    this.eventHandler(sender, args);
                }
            }

            public bool Compatible<TItem>(PropertyListener<TItem> other)
            {
                return object.Equals(this.eventHandler, other.eventHandler) && (this.ValuesToManager == other.ValuesToManager);
            }

            protected EventListener_v2(EventListener_v2<TEventArgs> listener)
            {
                this.register = listener.register;
                this.unregister = listener.unregister;
                this.eventHandler = listener.eventHandler;
            }

            #region private

            private readonly EventHandler_v2<TEventArgs> eventHandler;
            private readonly RegisterCallback<EventHandler_v2<TEventArgs>> register;
            private readonly UnregisterCallback<EventHandler_v2<TEventArgs>> unregister;

            #endregion
        }

        /// <summary>
        ///     'Escuchador' (listener) genérico.
        /// </summary>
        internal sealed class EventListener<TEventArgs> : Listener
            where TEventArgs : EventArgs
        {
            public EventListener(EventHandler<TEventArgs> eventHandler,
                                 RegisterCallback<EventHandler<TEventArgs>> register,
                                 UnregisterCallback<EventHandler<TEventArgs>> unregister)
            {
                this.register = register;
                this.unregister = unregister;
                this.eventHandler = eventHandler;
            }

            public override void Register(object sender)
            {
                this.register(sender, this.OnEvent);
            }

            public override void Unregister(object sender)
            {
                this.unregister(sender, this.OnEvent);
            }

            public override IEnumerable GetValues(object sender)
            {
                return new object[0];
            }

            #region private

            private void OnEvent(object sender, TEventArgs args)
            {
                Debug.WriteLine("EventListener<> {0}", new object[] { sender });
                if (this.eventHandler != null)
                {
                    this.eventHandler(sender, args);
                }
            }

            private readonly EventHandler<TEventArgs> eventHandler;
            private readonly RegisterCallback<EventHandler<TEventArgs>> register;
            private readonly UnregisterCallback<EventHandler<TEventArgs>> unregister;

            #endregion
        }

        /// <summary>
        ///     'Escuchador' (listener) de una propiedad.
        /// </summary>
        internal class PropertyListener : EventListener_v2<PropertyChangedExEventArgs>
        {
            public PropertyListener(Type propertyType,
                                    EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                    string propertyName, Func<object, object> getValue,
                                    RegisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> register,
                                    UnregisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                    bool valuesToManager)
                : base(eventHandler, register, unregister)
            {
                this.PropertyType = propertyType;

                this.ValuesToManager = valuesToManager;
                this.PropertyName = propertyName;
                this.GetValue = getValue;
            }

            public override IEnumerable GetValues(object sender)
            {
                return new[] { this.GetValue(sender) };
            }

            protected override void OnEvent(object sender, PropertyChangedExEventArgs args)
            {
                if (this.PropertyName == args.PropertyName)
                {
                    base.OnEvent(sender, args);

                    if (this.ValuesToManager && this.Manager.IsValid(this.PropertyType))
                    {
                        if (args.OldValue != null)
                        {
                            this.Manager.Unregister(args.OldValue);
                        }
                        if (args.NewValue != null)
                        {
                            this.Manager.Register(args.NewValue);
                        }
                    }
                }
            }

            internal readonly Type PropertyType;
            internal readonly string PropertyName;
            internal readonly Func<object, object> GetValue;
        }

        /// <summary>
        ///     'Escuchador' (listener) de una propiedad.
        /// </summary>
        internal sealed class PropertyListener<TItem> : PropertyListener
        {
            public PropertyListener(EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                    string propertyName, Func<object, TItem> getValue,
                                    RegisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> register,
                                    UnregisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                    bool valuesToManager)
                : base(typeof (TItem),
                       eventHandler,
                       propertyName, sender => getValue(sender),
                       register, unregister,
                       valuesToManager)
            {
            }
        }

        /// <summary>
        ///     'Escuchador' (listener) de múltiples propiedades.
        /// </summary>
        internal sealed class PropertiesListener : EventListener_v2<PropertyChangedExEventArgs>
        {
            public PropertiesListener(EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                      Type[] propertyTypes, string[] propertyNames, Func<object, object>[] getValues,
                                      RegisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> register,
                                      UnregisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                      bool valuesToManager)
                : base(eventHandler, register, unregister)
            {
                this.ValuesToManager = valuesToManager;
                this.propertyTypes = propertyTypes;
                this.propertyNames = new Dictionary<string, int>();
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    this.propertyNames.Add(propertyNames[i], i);
                }
                this.getValues = getValues;
            }

            public static PropertiesListener Join(PropertyListener listener1, PropertyListener listener2)
            {
                return new PropertiesListener(listener1,
                                              new[] { listener1.PropertyType, listener2.PropertyType },
                                              new[] { listener1.PropertyName, listener2.PropertyName },
                                              new[] { listener1.GetValue, listener2.GetValue });
            }

            public static PropertiesListener Join(PropertiesListener listener1, PropertyListener listener2)
            {
                return new PropertiesListener(listener1,
                                              listener1.propertyTypes.Concat(new[] { listener2.PropertyType }).ToArray(),
                                              listener1.propertyNames.Select(x => x.Key).Concat(new[] { listener2.PropertyName }).ToArray(),
                                              listener1.getValues.Concat(new[] { listener2.GetValue }).ToArray());
            }

            public override IEnumerable GetValues(object sender)
            {
                return this.getValues.Select(getValue => getValue(sender));
            }

            protected override void OnEvent(object sender, PropertyChangedExEventArgs args)
            {
                int i;
                if (this.propertyNames.TryGetValue(args.PropertyName, out i))
                {
                    base.OnEvent(sender, args);

                    if (this.ValuesToManager && this.Manager.IsValid(this.propertyTypes[i]))
                    {
                        if (args.OldValue != null)
                        {
                            this.Manager.Unregister(args.OldValue);
                        }
                        if (args.NewValue != null)
                        {
                            this.Manager.Register(args.NewValue);
                        }
                    }
                }
            }

            #region private

            private PropertiesListener(EventListener_v2<PropertyChangedExEventArgs> listener,
                                       Type[] propertyTypes, string[] propertyNames, Func<object, object>[] getValues)
                : base(listener)
            {
                this.propertyTypes = propertyTypes;
                this.propertyNames = new Dictionary<string, int>();
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    this.propertyNames.Add(propertyNames[i], i);
                }
                this.getValues = getValues;
            }

            private readonly Type[] propertyTypes;
            private readonly Dictionary<string, int> propertyNames;
            private readonly Func<object, object>[] getValues;

            #endregion
        }

#if false
/// <summary>
/// 'Escuchador' (listener) de múltiples propiedades (configurable).
/// </summary>
        private sealed class MultiPropertyListener<TItem> : Listener
        {
            public MultiPropertyListener(RegisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> register,
                                         UnregisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> unregister)
            {
                this.register = register;
                this.unregister = unregister;
            }

            public void Add(EventHandler_v2<PropertyChangedExEventArgs> eventHandler, string propertyName, Func<object, TItem> getValue)
            {
                this.partials.Add(propertyName, new Partial(propertyName, eventHandler, getValue));
            }

            public override void Register(object sender)
            {
                this.register(sender, this.OnPropertyChanged);
            }

            public override void Unregister(object sender)
            {
                this.unregister(sender, this.OnPropertyChanged);
            }

            public override IEnumerable GetValues(object sender)
            {
                return this.partials.Values.Select(partial => partial.GetValue(sender)).Select(dummy => (object)dummy);
            }

            private void OnPropertyChanged(object sender, PropertyChangedExEventArgs args)
            {
                Debug.WriteLine("MultiPropertyListener<> {0}", new object[] { sender });
                Partial @partial;
                if (this.partials.TryGetValue(args.PropertyName, out @partial))
                {
                    if (@partial.EventHandler_v2 != null)
                    {
                        @partial.EventHandler_v2(sender, args);
                    }

                    if (this.ValuesToManager && this.Manager.IsValid(typeof (TItem)))
                    {
                        if (args.OldValue != null)
                        {
                            this.Manager.Unregister(args.OldValue);
                        }
                        if (args.NewValue != null)
                        {
                            this.Manager.Register(args.NewValue);
                        }
                    }
                }
            }

            private readonly Dictionary<string, Partial> partials = new Dictionary<string, Partial>();
            private readonly RegisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> register;
            private readonly UnregisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> unregister;

            private sealed class Partial
            {
                public Partial(string propertyName, EventHandler_v2<PropertyChangedExEventArgs> eventHandler, Func<object, TItem> getValue)
                {
                    this.PropertyName = propertyName;
                    this.EventHandler_v2 = eventHandler;
                    this.GetValue = getValue;
                }

                public readonly string PropertyName;
                public readonly EventHandler_v2<PropertyChangedExEventArgs> EventHandler_v2;
                public readonly Func<object, TItem> GetValue;
            }
        }
#endif

        /// <summary>
        ///     'Escuchador' (listener) de todas las propiedades.
        /// </summary>
        internal sealed class AllPropertiesListener : EventListener_v2<PropertyChangedExEventArgs>
        {
            public AllPropertiesListener(EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                         RegisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> register,
                                         UnregisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                         bool valuesToManager)
                : base(eventHandler, register, unregister)
            {
                this.ValuesToManager = valuesToManager;
            }

            public override IEnumerable GetValues(object sender)
            {
                yield break;
            }

            protected override void OnEvent(object sender, PropertyChangedExEventArgs args)
            {
                base.OnEvent(sender, args);

                if (this.ValuesToManager)
                {
                    if ((args.OldValue != null) && this.Manager.IsValid(args.OldValue.GetType()))
                    {
                        this.Manager.Unregister(args.OldValue);
                    }
                    if ((args.NewValue != null) && this.Manager.IsValid(args.NewValue.GetType()))
                    {
                        this.Manager.Register(args.NewValue);
                    }
                }
            }
        }

        /// <summary>
        ///     'Escuchador' (listener) de eventos sobre colecciones.
        /// </summary>
        internal sealed class CollectionListener<TItem> : EventListener_v2<CollectionEventArgs>
        {
            public CollectionListener(EventHandler_v2<CollectionEventArgs> eventHandler,
                                      Func<object, ICollection<TItem>> getValues,
                                      RegisterCallback<EventHandler_v2<CollectionEventArgs>> register,
                                      UnregisterCallback<EventHandler_v2<CollectionEventArgs>> unregister,
                                      bool valuesToManager)
                : base(eventHandler, register, unregister)
            {
                this.ValuesToManager = valuesToManager;
                this.getValues = getValues;
            }

            public override IEnumerable GetValues(object sender)
            {
                return this.getValues(sender);
            }

            protected override void OnEvent(object sender, CollectionEventArgs args)
            {
                base.OnEvent(sender, args);

                if (this.ValuesToManager && this.Manager.IsValid(typeof (TItem)))
                {
                    args.ForEach(item => this.Manager.Register(item), item => this.Manager.Unregister(item));
                }
            }

            #region private

            private readonly Func<object, ICollection<TItem>> getValues;

            #endregion
        }

        /// <summary>
        ///     'Escuchador' (listener) de eventos sobre listas.
        /// </summary>
        internal sealed class ListListener<TItem> : EventListener_v2<ListEventArgs>
        {
            public ListListener(EventHandler_v2<ListEventArgs> eventHandler,
                                Func<object, IList<TItem>> getValues,
                                RegisterCallback<EventHandler_v2<ListEventArgs>> register,
                                UnregisterCallback<EventHandler_v2<ListEventArgs>> unregister,
                                bool valuesToManager)
                : base(eventHandler, register, unregister)
            {
                this.ValuesToManager = valuesToManager;
                this.getValues = getValues;
            }

            public override IEnumerable GetValues(object sender)
            {
                return this.getValues(sender);
            }

            protected override void OnEvent(object sender, ListEventArgs args)
            {
                base.OnEvent(sender, args);

                if (this.ValuesToManager && this.Manager.IsValid(typeof (TItem)))
                {
                    args.ForEach(item => this.Manager.Register(item), item => this.Manager.Unregister(item));
                }
            }

            #region private

            private readonly Func<object, IList<TItem>> getValues;

            #endregion
        }

        #endregion

        #region RegisterFor

        public sealed class RegisterForImp<TSender> where TSender : INotifyPropertyChangedEx
        {
            public RegisterForImp(ListenerManager manager)
            {
                this.manager = manager;
            }

            #region Genericos

            /// <summary>
            ///     Gestiona los eventos genericos.
            /// </summary>
            public RegisterForImp<TSender> Register(EventHandler eventHandler,
                                                    RegisterCallback<TSender, EventHandler> register,
                                                    UnregisterCallback<TSender, EventHandler> unregister)
            {
                ListenerManager.EventListener listener = new ListenerManager.EventListener(eventHandler,
                                                                                           (sender, h) => register((TSender)sender, h),
                                                                                           (sender, h) => unregister((TSender)sender, h));
                this.manager.Add<TSender>(listener);
                return this;
            }

            /// <summary>
            ///     Gestiona los eventos genericos.
            /// </summary>
            public RegisterForImp<TSender> Register<TEventArgs>(EventHandler_v2<TEventArgs> eventHandler,
                                                                RegisterCallback<TSender, EventHandler_v2<TEventArgs>> register,
                                                                UnregisterCallback<TSender, EventHandler_v2<TEventArgs>> unregister)
                where TEventArgs : EventArgs
            {
                ListenerManager.EventListener_v2<TEventArgs> listener = new ListenerManager.EventListener_v2<TEventArgs>(eventHandler,
                                                                                                                         (sender, h) => register((TSender)sender, h),
                                                                                                                         (sender, h) => unregister((TSender)sender, h));
                this.manager.Add<TSender>(listener);
                return this;
            }

            /// <summary>
            ///     Gestiona los eventos de multiples propiedades.
            /// </summary>
            public RegisterForImp<TSender> RegisterAllProperties(EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                                                 RegisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> register,
                                                                 UnregisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                                                 bool valuesToManager = false)
            {
                ListenerManager.AllPropertiesListener listener = new ListenerManager.AllPropertiesListener(eventHandler,
                                                                                                           (sender, h) => register((TSender)sender, h),
                                                                                                           (sender, h) => unregister((TSender)sender, h),
                                                                                                           valuesToManager);

                this.manager.Add<TSender>(listener);
                return this;
            }

            /// <summary>
            ///     Gestiona los eventos de multiples propiedades.
            /// </summary>
            public RegisterForImp<TSender> RegisterAllProperties(EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                                                 bool valuesToManager = true)
            {
                return this.RegisterAllProperties(eventHandler,
                                                  (sender, h) => sender.PropertyChanged += h,
                                                  (sender, h) => sender.PropertyChanged -= h,
                                                  valuesToManager);
            }

            #endregion Genericos

            #region Propiedad

            /// <summary>
            ///     Gestiona los eventos de una propiedad.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperty<TItem>(string propertyName, Func<TSender, TItem> getValue,
                                                                   bool valuesToManager = true)
            {
                return this.RegisterProperty(propertyName, getValue, null, valuesToManager);
            }

            /// <summary>
            ///     Gestiona los eventos de una propiedad.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperty<TItem>(string propertyName, Func<TSender, TItem> getValue,
                                                                   EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                                                   bool valuesToManager = true)
            {
                return this.RegisterProperty(propertyName, getValue, eventHandler,
                                             (sender, h) => sender.PropertyChanged += h,
                                             (sender, h) => sender.PropertyChanged -= h,
                                             valuesToManager);
            }

            /// <summary>
            ///     Gestiona los eventos de una propiedad.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperty<TItem>(string propertyName, Func<TSender, TItem> getValue,
                                                                   RegisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> register,
                                                                   UnregisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                                                   bool valuesToManager = true)
            {
                return this.RegisterProperty(propertyName, getValue, null, register, unregister, valuesToManager);
            }

            /// <summary>
            ///     Gestiona los eventos de una propiedad.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperty<TItem>(string propertyName, Func<TSender, TItem> getValue,
                                                                   EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                                                   RegisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> register,
                                                                   UnregisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                                                   bool valuesToManager = true)
            {
                ListenerManager.PropertyListener<TItem> listener = new ListenerManager.PropertyListener<TItem>(eventHandler,
                                                                                                               propertyName,
                                                                                                               sender => getValue((TSender)sender),
                                                                                                               (sender, h) => register((TSender)sender, h),
                                                                                                               (sender, h) => unregister((TSender)sender, h),
                                                                                                               valuesToManager);

                foreach (ListenerManager.Listener aux in this.manager.FindListenersOfExactType(typeof (TSender)))
                {
                    if (aux is ListenerManager.PropertyListener)
                    {
                        ListenerManager.PropertyListener aux2 = (ListenerManager.PropertyListener)aux;
                        if (aux2.Compatible(listener))
                        {
                            ListenerManager.PropertiesListener newListener = ListenerManager.PropertiesListener.Join(aux2, listener);
                            this.manager.Remove<TSender>(aux2);
                            this.manager.Add<TSender>(newListener);
                            return this;
                        }
                    }
                    else if (aux is ListenerManager.PropertiesListener)
                    {
                        ListenerManager.PropertiesListener aux2 = (ListenerManager.PropertiesListener)aux;
                        if (aux2.Compatible(listener))
                        {
                            ListenerManager.PropertiesListener newListener = ListenerManager.PropertiesListener.Join(aux2, listener);
                            this.manager.Remove<TSender>(aux2);
                            this.manager.Add<TSender>(newListener);
                            return this;
                        }
                    }
                }

                this.manager.Add<TSender>(listener);
                return this;
            }

            #endregion Propiedad

            #region Propiedades

            public RegisterForImp<TSender> RegisterProperties(Tuple<Type, string, Func<TSender, object>>[] properties,
                                                              EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                                              bool valuesToManager = true)
            {
                return this.RegisterProperties(properties.Select(x => x.Item1).ToArray(),
                                               properties.Select(x => x.Item2).ToArray(),
                                               properties.Select(x => x.Item3).ToArray(),
                                               eventHandler,
                                               (sender, h) => sender.PropertyChanged += h,
                                               (sender, h) => sender.PropertyChanged -= h,
                                               valuesToManager);
            }

            /// <summary>
            ///     Gestiona los eventos de multiples propiedades.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperties(Type[] propertyTypes, string[] propertyNames, Func<TSender, object>[] getValues,
                                                              bool valuesToManager = true)
            {
                return this.RegisterProperties(propertyTypes, propertyNames, getValues, null, valuesToManager);
            }

            /// <summary>
            ///     Gestiona los eventos de multiples propiedades.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperties(Type[] propertyTypes, string[] propertyNames, Func<TSender, object>[] getValues,
                                                              EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                                              bool valuesToManager = true)
            {
                return this.RegisterProperties(propertyTypes,
                                               propertyNames, getValues, eventHandler,
                                               (sender, h) => sender.PropertyChanged += h,
                                               (sender, h) => sender.PropertyChanged -= h,
                                               valuesToManager);
            }

            /// <summary>
            ///     Gestiona los eventos de multiples propiedades.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperties(Type[] propertyTypes, string[] propertyNames, Func<TSender, object>[] getValues,
                                                              RegisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> register,
                                                              UnregisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                                              bool valuesToManager = true)
            {
                return this.RegisterProperties(propertyTypes, propertyNames, getValues, null, register, unregister, valuesToManager);
            }

            /// <summary>
            ///     Gestiona los eventos de multiples propiedades.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperties(Type[] propertyTypes, string[] propertyNames, Func<TSender, object>[] getValues,
                                                              EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                                              RegisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> register,
                                                              UnregisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                                              bool valuesToManager = true)
            {
                ListenerManager.PropertiesListener listener = new ListenerManager.PropertiesListener(eventHandler,
                                                                                                     propertyTypes,
                                                                                                     propertyNames,
                                                                                                     getValues.Select(getValue => (Func<object, object>)(sender => getValue((TSender)sender))).ToArray(),
                                                                                                     (sender, h) => register((TSender)sender, h),
                                                                                                     (sender, h) => unregister((TSender)sender, h),
                                                                                                     valuesToManager);

                this.manager.Add<TSender>(listener);
                return this;
            }

            #endregion Propiedades

            #region Coleccion

            /// <summary>
            ///     Gestiona los eventos de una coleccion.
            /// </summary>
            public RegisterForImp<TSender> RegisterCollection<TItem>(Func<TSender, ICollection<TItem>> getValues,
                                                                     RegisterCallback<TSender, EventHandler_v2<CollectionEventArgs>> register,
                                                                     UnregisterCallback<TSender, EventHandler_v2<CollectionEventArgs>> unregister,
                                                                     bool valuesToManager = true)
            {
                return this.RegisterCollection(getValues, null, register, unregister, valuesToManager);
            }

            /// <summary>
            ///     Gestiona los eventos de una coleccion.
            /// </summary>
            public RegisterForImp<TSender> RegisterCollection<TItem>(Func<TSender, ICollection<TItem>> getValues,
                                                                     EventHandler_v2<CollectionEventArgs> eventHandler,
                                                                     RegisterCallback<TSender, EventHandler_v2<CollectionEventArgs>> register,
                                                                     UnregisterCallback<TSender, EventHandler_v2<CollectionEventArgs>> unregister,
                                                                     bool valuesToManager = true)
            {
                ListenerManager.CollectionListener<TItem> listener = new ListenerManager.CollectionListener<TItem>(eventHandler,
                                                                                                                   sender => getValues((TSender)sender),
                                                                                                                   (sender, h) => register((TSender)sender, h),
                                                                                                                   (sender, h) => unregister((TSender)sender, h),
                                                                                                                   valuesToManager);
                this.manager.Add<TSender>(listener);
                return this;
            }

            /// <summary>
            ///     Gestiona los eventos de una coleccion.
            /// </summary>
            public RegisterForImp<TSender> RegisterCollection<TItem>(Func<TSender, IEventCollection<TItem>> getValues,
                                                                     bool valuesToManager = true)
            {
                return this.RegisterCollection(getValues, null, valuesToManager);
            }

            /// <summary>
            ///     Gestiona los eventos de una coleccion.
            /// </summary>
            public RegisterForImp<TSender> RegisterCollection<TItem>(Func<TSender, IEventCollection<TItem>> getValues,
                                                                     EventHandler_v2<CollectionEventArgs> eventHandler,
                                                                     bool valuesToManager = true)
            {
                return this.RegisterCollection(getValues,
                                               eventHandler,
                                               (sender, h) => getValues(sender).CollectionChanged += h,
                                               (sender, h) => getValues(sender).CollectionChanged -= h,
                                               valuesToManager);
            }

            #endregion Coleccion

            #region Lista

            /// <summary>
            ///     Gestiona los eventos de una lista.
            /// </summary>
            public RegisterForImp<TSender> RegisterList<TItem>(Func<TSender, IList<TItem>> getValues,
                                                               RegisterCallback<TSender, EventHandler_v2<ListEventArgs>> register,
                                                               UnregisterCallback<TSender, EventHandler_v2<ListEventArgs>> unregister,
                                                               bool valuesToManager = true)
            {
                return this.RegisterList(getValues, null, register, unregister, valuesToManager);
            }

            /// <summary>
            ///     Gestiona los eventos de una lista.
            /// </summary>
            public RegisterForImp<TSender> RegisterList<TItem>(Func<TSender, IList<TItem>> getValues,
                                                               EventHandler_v2<ListEventArgs> eventHandler,
                                                               RegisterCallback<TSender, EventHandler_v2<ListEventArgs>> register,
                                                               UnregisterCallback<TSender, EventHandler_v2<ListEventArgs>> unregister,
                                                               bool valuesToManager = true)
            {
                ListenerManager.ListListener<TItem> listener = new ListenerManager.ListListener<TItem>(eventHandler,
                                                                                                       sender => getValues((TSender)sender),
                                                                                                       (sender, h) => register((TSender)sender, h),
                                                                                                       (sender, h) => unregister((TSender)sender, h),
                                                                                                       valuesToManager);
                this.manager.Add<TSender>(listener);
                return this;
            }

            /// <summary>
            ///     Gestiona los eventos de una lista.
            /// </summary>
            public RegisterForImp<TSender> RegisterList<TItem>(Func<TSender, IEventList<TItem>> getValues,
                                                               bool valuesToManager = true)
            {
                return this.RegisterList(getValues, null, valuesToManager);
            }

            /// <summary>
            ///     Gestiona los eventos de una lista.
            /// </summary>
            public RegisterForImp<TSender> RegisterList<TItem>(Func<TSender, IEventList<TItem>> getValues,
                                                               EventHandler_v2<ListEventArgs> eventHandler,
                                                               bool valuesToManager = true)
            {
                return this.RegisterList(getValues,
                                         eventHandler,
                                         (sender, h) => getValues(sender).ListChanged += h,
                                         (sender, h) => getValues(sender).ListChanged -= h,
                                         valuesToManager);
            }

            #endregion Lista

            #region private

            private readonly ListenerManager manager;

            #endregion
        }

        #endregion

        #endregion
    }
}