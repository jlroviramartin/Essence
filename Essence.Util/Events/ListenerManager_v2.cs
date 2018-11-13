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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Essence.Util.Builders;
using Essence.Util.Collections;

namespace Essence.Util.Events
{
    /// <summary>
    /// Gestor de 'escuchadores' (listeners). Permite registar escuchadores (listeners) para ciertos eventos.
    /// Por ahora no utiliza eventos debiles.
    /// </summary>
    public sealed class ListenerManager_v2<T>
    {
        public ListenerManager_v2(bool trackSender = false)
        {
            this.trackSender = trackSender;
        }

        /// <summary>
        /// Establece la raiz.
        /// </summary>
        public void SetRoot(T root)
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
            else
            {
                if (this.trackSender)
                {
                    foreach (Listener listener in this.mapOfListeners.Values.SelectMany(x => x))
                    {
                        listener.UnregisterAll();
                    }
                }
            }
        }

        public RegisterForImp<TSender> RegisterFor<TSender>(bool weak = true) where TSender : INotifyPropertyChangedEx
        {
            return new RegisterForImp<TSender>(this, weak);
        }

        public void RemoveDeadEntries()
        {
            List<Type> toRemove = new List<Type>();
            foreach (KeyValuePair<Type, List<Listener>> kvp in this.mapOfListeners)
            {
                kvp.Value.RemoveAll(l => !l.IsAlive);
                if (kvp.Value.Count == 0)
                {
                    toRemove.Add(kvp.Key);
                }
            }
            this.mapOfListeners.RemoveAllKeys(toRemove);
        }

        public event EventHandler_v2<ObjectRegistered> Registering;
        public event EventHandler_v2<ObjectRegistered> Unregistering;

        #region private

        private void OnRegistering(object item)
        {
            this.OnRegistering(new ObjectRegistered(item));
        }

        private void OnRegistering(ObjectRegistered args)
        {
            EventHandler_v2<ObjectRegistered> registering = this.Registering;
            if (registering != null)
            {
                registering(this, args);
            }
        }

        private void OnUnregistering(object item)
        {
            this.OnUnregistering(new ObjectRegistered(item));
        }

        private void OnUnregistering(ObjectRegistered args)
        {
            EventHandler_v2<ObjectRegistered> unregistering = this.Unregistering;
            if (unregistering != null)
            {
                unregistering(this, args);
            }
        }

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

            this.OnRegistering(obj);
            //Debug.WriteLine("Register<> {0}", obj);
            //Debug.Indent();

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

            //Debug.Unindent();
        }

        private void Unregister(object obj)
        {
            if (obj == null)
            {
                return;
            }

            this.OnUnregistering(obj);
            //Debug.WriteLine("Unregister<> {0}", new[] { obj });
            //Debug.Indent();

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

            //Debug.Unindent();
        }

        private void Add<TSender>(Listener listener)
        {
            this.Add(typeof(TSender), listener);
        }

        private void Add(Type senderType, Listener listener)
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

        private bool Remove<TSender>(Listener listener)
        {
            return this.Remove(typeof(TSender), listener);
        }

        private bool Remove(Type senderType, Listener listener)
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
        /// Busca los listeners del tipo <c>senderType</c> (no busca en lo tipos de los que deriva).
        /// </summary>
        private IEnumerable<Listener> FindListenersOfExactType(Type senderType)
        {
            return this.mapOfListeners.OriginalValues.GetSafe(senderType) ?? new List<Listener>();
        }

        private readonly bool trackSender;

        /// <summary>Raiz de la jerarquia de objetos a escuchar los eventos.</summary>
        private T root;

        /// <summary>Listeners.</summary>
        private readonly DictionaryOfType<List<Listener>> mapOfListeners = new DictionaryOfType<List<Listener>>();

        #endregion

        #region Inner classes

        public delegate void UnregisterCallback<in TEventHandler>(object source, TEventHandler eventHandler);

        public delegate void UnregisterCallback<in TSource, in TEventHandler>(TSource source, TEventHandler eventHandler);

        public delegate void RegisterCallback<in TEventHandler>(object source, TEventHandler eventHandler);

        public delegate void RegisterCallback<in TSource, in TEventHandler>(TSource source, TEventHandler eventHandler);

        #region IEventHandler

        internal static IEventHandler BuildEventHandler(EventHandler eventHandler, bool weak)
        {
            if (weak)
            {
                return new WeakEventHandler(eventHandler.Method, new WeakReference(eventHandler.Target));
            }
            else
            {
                return new SimpleEventHandler(eventHandler);
            }
        }

        internal static IEventHandler BuildEventHandler<TArgs>(EventHandler<TArgs> eventHandler, bool weak)
            where TArgs : EventArgs
        {
            if (weak)
            {
                return new WeakEventHandler(eventHandler.Method, new WeakReference(eventHandler.Target));
            }
            else
            {
                return new SimpleEventHandler<TArgs>(eventHandler);
            }
        }

        internal static IEventHandler BuildEventHandler<TArgs>(EventHandler_v2<TArgs> eventHandler, bool weak)
            where TArgs : EventArgs
        {
            if (weak)
            {
                return new WeakEventHandler(eventHandler.Method, new WeakReference(eventHandler.Target));
            }
            else
            {
                return new SimpleEventHandler_v2<TArgs>(eventHandler);
            }
        }

        internal interface IEventHandler
        {
            bool IsAlive { get; }

            void OnEvent(object sender, EventArgs args);
        }

        private sealed class WeakEventHandler : IEventHandler
        {
            public WeakEventHandler(MethodInfo targetMethod, WeakReference targetReference)
            {
                this.TargetMethod = targetMethod;
                this.TargetReference = targetReference;
            }

            public void OnEvent(object sender, EventArgs args)
            {
                if (this.TargetReference != null)
                {
                    object target = this.TargetReference.Target;
                    if (target != null)
                    {
                        this.TargetMethod.Invoke(target, new[] { sender, args });
                    }
                    else
                    {
                        //needsCleanup = true;
                    }
                }
                else
                {
                    this.TargetMethod.Invoke(null, new[] { sender, args });
                }
            }

            public bool IsAlive
            {
                get { return this.TargetReference.IsAlive; }
            }

            public readonly MethodInfo TargetMethod;
            public readonly WeakReference TargetReference;

            public override bool Equals(object obj)
            {
                WeakEventHandler other = obj as WeakEventHandler;
                if (other == null)
                {
                    return false;
                }
                return this.TargetReference == other.TargetReference && this.TargetMethod == other.TargetMethod;
            }

            public override int GetHashCode()
            {
                return new HashCodeBuilder().Append(this.TargetReference).Append(this.TargetMethod).GetHashCode();
            }
        }

        private sealed class SimpleEventHandler : IEventHandler
        {
            public SimpleEventHandler(EventHandler eventHandler)
            {
                this.eventHandler = eventHandler;
            }

            public bool IsAlive
            {
                get { return true; }
            }

            public void OnEvent(object sender, EventArgs args)
            {
                this.eventHandler(sender, args);
            }

            private readonly EventHandler eventHandler;

            public override bool Equals(object obj)
            {
                SimpleEventHandler other = obj as SimpleEventHandler;
                if (other == null)
                {
                    return false;
                }
                return this.eventHandler == other.eventHandler;
            }

            public override int GetHashCode()
            {
                return new HashCodeBuilder().Append(this.eventHandler).GetHashCode();
            }
        }

        private sealed class SimpleEventHandler<TArgs> : IEventHandler
            where TArgs : EventArgs
        {
            public SimpleEventHandler(EventHandler<TArgs> eventHandler)
            {
                this.eventHandler = eventHandler;
            }

            public bool IsAlive
            {
                get { return true; }
            }

            public void OnEvent(object sender, EventArgs args)
            {
                this.eventHandler(sender, (TArgs)args);
            }

            private readonly EventHandler<TArgs> eventHandler;

            public override bool Equals(object obj)
            {
                SimpleEventHandler<TArgs> other = obj as SimpleEventHandler<TArgs>;
                if (other == null)
                {
                    return false;
                }
                return this.eventHandler == other.eventHandler;
            }

            public override int GetHashCode()
            {
                return new HashCodeBuilder().Append(this.eventHandler).GetHashCode();
            }
        }

        private sealed class SimpleEventHandler_v2<TArgs> : IEventHandler
            where TArgs : EventArgs
        {
            public SimpleEventHandler_v2(EventHandler_v2<TArgs> eventHandler)
            {
                this.eventHandler = eventHandler;
            }

            public bool IsAlive
            {
                get { return true; }
            }

            public void OnEvent(object sender, EventArgs args)
            {
                this.eventHandler(sender, (TArgs)args);
            }

            private readonly EventHandler_v2<TArgs> eventHandler;

            public override bool Equals(object obj)
            {
                SimpleEventHandler_v2<TArgs> other = obj as SimpleEventHandler_v2<TArgs>;
                if (other == null)
                {
                    return false;
                }
                return this.eventHandler == other.eventHandler;
            }

            public override int GetHashCode()
            {
                return new HashCodeBuilder().Append(this.eventHandler).GetHashCode();
            }
        }

        #endregion

        #region Listener

        /// <summary>
        /// Clase base de los 'escuchadores' (listeners).
        /// </summary>
        private abstract class Listener
        {
            /// <summary>
            /// Gestor de eventos.
            /// </summary>
            public ListenerManager_v2<T> Manager { protected get; set; }

            /// <summary>
            /// Tipo de objetos que emite los eventos.
            /// </summary>
            public Type SenderType { protected get; set; }

            /// <summary>
            /// Indica si el listener (el handler) esta vivo.
            /// </summary>
            public bool IsAlive
            {
                get { return this.eventHandler.IsAlive; }
            }

            /// <summary>
            /// Registra el listener en el objeto.
            /// </summary>
            public virtual void Register(object sender)
            {
                if (this.Manager.trackSender)
                {
                    this.senders.Add(new WeakReference(sender));
                }
            }

            /// <summary>
            /// Deregistra el listener en el objeto.
            /// </summary>
            public virtual void Unregister(object sender)
            {
                if (this.Manager.trackSender)
                {
                    this.senders.Remove(new WeakReference(sender));
                }
            }

            public virtual void UnregisterAll()
            {
                object[] toUnregister = this.senders.Select(x => x.Target).Where(x => x != null).ToArray();
                foreach (object sender in toUnregister)
                {
                    this.Unregister(sender);
                }
            }

            /// <summary>
            /// Indica si tambien añade los valores <c>GetValues</c>, al gestor.
            /// </summary>
            public bool ValuesToManager { get; protected set; }

            /// <summary>
            /// Obtiene los valores asociados al objeto.
            /// </summary>
            public abstract IEnumerable GetValues(object sender);

            public bool Compatible<TItem>(PropertyListener<TItem> other)
            {
                return this.eventHandler.Equals(other.eventHandler) && (this.ValuesToManager == other.ValuesToManager);
            }

            #region protected

            protected Listener(Listener listener)
            {
                this.eventHandler = listener.eventHandler;
            }

            protected Listener(IEventHandler eventHandler)
            {
                this.eventHandler = eventHandler;
            }

            /// <summary>
            /// Notifica de un evento.
            /// </summary>
            protected void OnEvent(object sender, EventArgs args)
            {
                //Debug.WriteLine("EventListener {0}", new object[] { sender });
                this.eventHandler.OnEvent(sender, args);
            }

            /// <summary>
            /// Indica si los listeners son compatibles.
            /// Ver <c>PropertyListener</c> y <c>PropertiesListener</c>.
            /// </summary>
            /// <returns></returns>
            protected static bool Compatibles(Listener listener1, Listener listener2)
            {
                return listener1.eventHandler.Equals(listener2.eventHandler) && (listener1.ValuesToManager == listener2.ValuesToManager);
            }

            #endregion

            #region private

            private readonly List<WeakReference> senders = new List<WeakReference>();
            private readonly IEventHandler eventHandler;

            #endregion
        }

        /// <summary>
        /// 'Escuchador' (listener) genérico.
        /// </summary>
        private sealed class EventListener : Listener
        {
            public EventListener(EventHandler eventHandler, bool weak,
                                 RegisterCallback<EventHandler> register,
                                 UnregisterCallback<EventHandler> unregister)
                : base(BuildEventHandler(eventHandler, weak))
            {
                this.register = register;
                this.unregister = unregister;
            }

            public override void Register(object sender)
            {
                base.Register(sender);
                this.register(sender, this.OnEvent);
            }

            public override void Unregister(object sender)
            {
                this.unregister(sender, this.OnEvent);
                base.Unregister(sender);
            }

            public override IEnumerable GetValues(object sender)
            {
                return new object[0];
            }

            #region private

            private new void OnEvent(object sender, EventArgs args)
            {
                base.OnEvent(sender, args);
            }

            private readonly RegisterCallback<EventHandler> register;
            private readonly UnregisterCallback<EventHandler> unregister;

            #endregion
        }

        /// <summary>
        /// 'Escuchador' (listener) genérico.
        /// </summary>
        private class EventListener_v2<TEventArgs> : Listener
            where TEventArgs : EventArgs
        {
            protected EventListener_v2(EventListener_v2<TEventArgs> listener)
                : base(listener)
            {
                this.register = listener.register;
                this.unregister = listener.unregister;
                this.ValuesToManager = listener.ValuesToManager;
            }

            public EventListener_v2(IEventHandler eventHandler,
                                    RegisterCallback<EventHandler_v2<TEventArgs>> register,
                                    UnregisterCallback<EventHandler_v2<TEventArgs>> unregister)
                : base(eventHandler)
            {
                this.register = register;
                this.unregister = unregister;
            }

            public override void Register(object sender)
            {
                base.Register(sender);
                this.register(sender, this.OnEvent);
            }

            public override void Unregister(object sender)
            {
                this.unregister(sender, this.OnEvent);
                base.Unregister(sender);
            }

            public override IEnumerable GetValues(object sender)
            {
                return new object[0];
            }

            protected virtual void OnEvent(object sender, TEventArgs args)
            {
                base.OnEvent(sender, args);
            }

            #region private

            private readonly RegisterCallback<EventHandler_v2<TEventArgs>> register;
            private readonly UnregisterCallback<EventHandler_v2<TEventArgs>> unregister;

            #endregion
        }

        /// <summary>
        /// 'Escuchador' (listener) genérico.
        /// </summary>
        private class EventListener<TEventArgs> : Listener
            where TEventArgs : EventArgs
        {
            public EventListener(IEventHandler eventHandler,
                                 RegisterCallback<EventHandler<TEventArgs>> register,
                                 UnregisterCallback<EventHandler<TEventArgs>> unregister)
                : base(eventHandler)
            {
                this.register = register;
                this.unregister = unregister;
            }

            public override void Register(object sender)
            {
                base.Register(sender);
                this.register(sender, this.OnEvent);
            }

            public override void Unregister(object sender)
            {
                this.unregister(sender, this.OnEvent);
                base.Unregister(sender);
            }

            public override IEnumerable GetValues(object sender)
            {
                return new object[0];
            }

            protected virtual void OnEvent(object sender, TEventArgs args)
            {
                base.OnEvent(sender, args);
            }

            #region private

            private readonly RegisterCallback<EventHandler<TEventArgs>> register;
            private readonly UnregisterCallback<EventHandler<TEventArgs>> unregister;

            #endregion
        }

        /// <summary>
        /// 'Escuchador' (listener) de una propiedad.
        /// </summary>
        private class PropertyListener : EventListener_v2<PropertyChangedExEventArgs>
        {
            public PropertyListener(Type propertyType,
                                    IEventHandler eventHandler,
                                    string propertyName, Func<object, object> getValue,
                                    RegisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> register,
                                    UnregisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                    bool valuesToManager)
                : base(eventHandler, register, unregister)
            {
                this.propertyType = propertyType;

                this.ValuesToManager = valuesToManager;
                this.propertyName = propertyName;
                this.getValue = getValue;
            }

            public override IEnumerable GetValues(object sender)
            {
                return new[] { this.getValue(sender) };
            }

            protected override void OnEvent(object sender, PropertyChangedExEventArgs args)
            {
                if (this.propertyName == args.PropertyName)
                {
                    base.OnEvent(sender, args);

                    if (this.ValuesToManager && this.Manager.IsValid(this.propertyType))
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

            internal readonly Type propertyType;

            internal readonly string propertyName;
            internal readonly Func<object, object> getValue;
        }

        /// <summary>
        /// 'Escuchador' (listener) de una propiedad.
        /// </summary>
        private sealed class PropertyListener<TItem> : PropertyListener
        {
            public PropertyListener(IEventHandler eventHandler,
                                    string propertyName, Func<object, TItem> getValue,
                                    RegisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> register,
                                    UnregisterCallback<EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                    bool valuesToManager)
                : base(typeof(TItem),
                       eventHandler,
                       propertyName, sender => getValue(sender),
                       register, unregister,
                       valuesToManager)
            {
            }
        }

        /// <summary>
        /// 'Escuchador' (listener) de múltiples propiedades.
        /// </summary>
        private sealed class PropertiesListener : EventListener_v2<PropertyChangedExEventArgs>
        {
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

            public PropertiesListener(IEventHandler eventHandler,
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
                                              new[] { listener1.propertyType, listener2.propertyType },
                                              new[] { listener1.propertyName, listener2.propertyName },
                                              new[] { listener1.getValue, listener2.getValue });
            }

            public static PropertiesListener Join(PropertiesListener listener1, PropertyListener listener2)
            {
                return new PropertiesListener(listener1,
                                              listener1.propertyTypes.Concat(new[] { listener2.propertyType }).ToArray(),
                                              listener1.propertyNames.Select(x => x.Key).Concat(new[] { listener2.propertyName }).ToArray(),
                                              listener1.getValues.Concat(new[] { listener2.getValue }).ToArray());
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

            private readonly Type[] propertyTypes;
            private readonly Dictionary<string, int> propertyNames;
            private readonly Func<object, object>[] getValues;

            #endregion
        }

        /// <summary>
        /// 'Escuchador' (listener) de todas las propiedades.
        /// </summary>
        private sealed class AllPropertiesListener : EventListener_v2<PropertyChangedExEventArgs>
        {
            public AllPropertiesListener(IEventHandler eventHandler,
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
        /// 'Escuchador' (listener) de eventos sobre colecciones.
        /// </summary>
        private sealed class CollectionListener<TItem> : EventListener_v2<CollectionEventArgs>
        {
            public CollectionListener(IEventHandler eventHandler,
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

                if (this.ValuesToManager && this.Manager.IsValid(typeof(TItem)))
                {
                    args.ForEach(item => this.Manager.Register(item), item => this.Manager.Unregister(item));
                }
            }

            #region private

            private readonly Func<object, ICollection<TItem>> getValues;

            #endregion
        }

        /// <summary>
        /// 'Escuchador' (listener) de eventos sobre listas.
        /// </summary>
        private sealed class ListListener<TItem> : EventListener_v2<ListEventArgs>
        {
            public ListListener(IEventHandler eventHandler,
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

                if (this.ValuesToManager && this.Manager.IsValid(typeof(TItem)))
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

        public class RegisterForImp<TSender> where TSender : INotifyPropertyChangedEx
        {
            public RegisterForImp(ListenerManager_v2<T> manager, bool weak)
            {
                this.manager = manager;
                this.weak = weak;
            }

            #region Generics

            /// <summary>
            /// Gestiona los eventos genericos.
            /// </summary>
            public RegisterForImp<TSender> Register(EventHandler eventHandler,
                                                    RegisterCallback<TSender, EventHandler> register,
                                                    UnregisterCallback<TSender, EventHandler> unregister)
            {
                ListenerManager_v2<T>.EventListener listener = new ListenerManager_v2<T>.EventListener(eventHandler, this.weak,
                                                                                                       (sender, h) => register((TSender)sender, h),
                                                                                                       (sender, h) => unregister((TSender)sender, h));
                this.manager.Add<TSender>(listener);
                return this;
            }

            /// <summary>
            /// Gestiona los eventos genericos.
            /// </summary>
            public RegisterForImp<TSender> Register<TEventArgs>(EventHandler_v2<TEventArgs> eventHandler,
                                                                RegisterCallback<TSender, EventHandler_v2<TEventArgs>> register,
                                                                UnregisterCallback<TSender, EventHandler_v2<TEventArgs>> unregister)
                where TEventArgs : EventArgs
            {
                ListenerManager_v2<T>.EventListener_v2<TEventArgs> listener = new ListenerManager_v2<T>.EventListener_v2<TEventArgs>(BuildEventHandler(eventHandler, this.weak),
                                                                                                                                     (sender, h) => register((TSender)sender, h),
                                                                                                                                     (sender, h) => unregister((TSender)sender, h));
                this.manager.Add<TSender>(listener);
                return this;
            }

            /// <summary>
            /// Gestiona los eventos de multiples propiedades.
            /// </summary>
            public RegisterForImp<TSender> RegisterAllProperties(EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                                                 RegisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> register,
                                                                 UnregisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                                                 bool valuesToManager = false)
            {
                ListenerManager_v2<T>.AllPropertiesListener listener = new ListenerManager_v2<T>.AllPropertiesListener(BuildEventHandler(eventHandler, this.weak),
                                                                                                                       (sender, h) => register((TSender)sender, h),
                                                                                                                       (sender, h) => unregister((TSender)sender, h),
                                                                                                                       valuesToManager);

                this.manager.Add<TSender>(listener);
                return this;
            }

            /// <summary>
            /// Gestiona los eventos de multiples propiedades.
            /// </summary>
            public RegisterForImp<TSender> RegisterAllProperties(EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                                                 bool valuesToManager = true)
            {
                return this.RegisterAllProperties(eventHandler,
                                                  (sender, h) => sender.PropertyChanged += h,
                                                  (sender, h) => sender.PropertyChanged -= h,
                                                  valuesToManager);
            }

            #endregion

            #region Property

            /// <summary>
            /// Gestiona los eventos de una propiedad.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperty<TItem>(string propertyName, Func<TSender, TItem> getValue,
                                                                   bool valuesToManager = true)
            {
                return this.RegisterProperty(propertyName, getValue, null, valuesToManager);
            }

            /// <summary>
            /// Gestiona los eventos de una propiedad.
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
            /// Gestiona los eventos de una propiedad.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperty<TItem>(string propertyName, Func<TSender, TItem> getValue,
                                                                   RegisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> register,
                                                                   UnregisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                                                   bool valuesToManager = true)
            {
                return this.RegisterProperty(propertyName, getValue, null, register, unregister, valuesToManager);
            }

            /// <summary>
            /// Gestiona los eventos de una propiedad.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperty<TItem>(string propertyName, Func<TSender, TItem> getValue,
                                                                   EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                                                   RegisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> register,
                                                                   UnregisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                                                   bool valuesToManager = true)
            {
                ListenerManager_v2<T>.PropertyListener<TItem> listener = new ListenerManager_v2<T>.PropertyListener<TItem>(BuildEventHandler(eventHandler, this.weak),
                                                                                                                           propertyName,
                                                                                                                           sender => getValue((TSender)sender),
                                                                                                                           (sender, h) => register((TSender)sender, h),
                                                                                                                           (sender, h) => unregister((TSender)sender, h),
                                                                                                                           valuesToManager);

                foreach (ListenerManager_v2<T>.Listener aux in this.manager.FindListenersOfExactType(typeof(TSender)))
                {
                    if (aux is ListenerManager_v2<T>.PropertyListener)
                    {
                        ListenerManager_v2<T>.PropertyListener aux2 = (ListenerManager_v2<T>.PropertyListener)aux;
                        if (aux2.Compatible(listener))
                        {
                            ListenerManager_v2<T>.PropertiesListener newListener = ListenerManager_v2<T>.PropertiesListener.Join(aux2, listener);
                            this.manager.Remove<TSender>(aux2);
                            this.manager.Add<TSender>(newListener);
                            return this;
                        }
                    }
                    else if (aux is ListenerManager_v2<T>.PropertiesListener)
                    {
                        ListenerManager_v2<T>.PropertiesListener aux2 = (ListenerManager_v2<T>.PropertiesListener)aux;
                        if (aux2.Compatible(listener))
                        {
                            ListenerManager_v2<T>.PropertiesListener newListener = ListenerManager_v2<T>.PropertiesListener.Join(aux2, listener);
                            this.manager.Remove<TSender>(aux2);
                            this.manager.Add<TSender>(newListener);
                            return this;
                        }
                    }
                }

                this.manager.Add<TSender>(listener);
                return this;
            }

            #endregion

            #region Properties

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
            /// Gestiona los eventos de multiples propiedades.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperties(Type[] propertyTypes, string[] propertyNames, Func<TSender, object>[] getValues,
                                                              bool valuesToManager = true)
            {
                return this.RegisterProperties(propertyTypes, propertyNames, getValues, null, valuesToManager);
            }

            /// <summary>
            /// Gestiona los eventos de multiples propiedades.
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
            /// Gestiona los eventos de multiples propiedades.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperties(Type[] propertyTypes, string[] propertyNames, Func<TSender, object>[] getValues,
                                                              RegisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> register,
                                                              UnregisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                                              bool valuesToManager = true)
            {
                return this.RegisterProperties(propertyTypes, propertyNames, getValues, null, register, unregister, valuesToManager);
            }

            /// <summary>
            /// Gestiona los eventos de multiples propiedades.
            /// </summary>
            public RegisterForImp<TSender> RegisterProperties(Type[] propertyTypes, string[] propertyNames, Func<TSender, object>[] getValues,
                                                              EventHandler_v2<PropertyChangedExEventArgs> eventHandler,
                                                              RegisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> register,
                                                              UnregisterCallback<TSender, EventHandler_v2<PropertyChangedExEventArgs>> unregister,
                                                              bool valuesToManager = true)
            {
                ListenerManager_v2<T>.PropertiesListener listener = new ListenerManager_v2<T>.PropertiesListener(BuildEventHandler(eventHandler, this.weak),
                                                                                                                 propertyTypes,
                                                                                                                 propertyNames,
                                                                                                                 getValues.Select(getValue => (Func<object, object>)(sender => getValue((TSender)sender))).ToArray(),
                                                                                                                 (sender, h) => register((TSender)sender, h),
                                                                                                                 (sender, h) => unregister((TSender)sender, h),
                                                                                                                 valuesToManager);

                this.manager.Add<TSender>(listener);
                return this;
            }

            #endregion

            #region Collections

            /// <summary>
            /// Gestiona los eventos de una coleccion.
            /// </summary>
            public RegisterForImp<TSender> RegisterCollection<TItem>(Func<TSender, ICollection<TItem>> getValues,
                                                                     RegisterCallback<TSender, EventHandler_v2<CollectionEventArgs>> register,
                                                                     UnregisterCallback<TSender, EventHandler_v2<CollectionEventArgs>> unregister,
                                                                     bool valuesToManager = true)
            {
                return this.RegisterCollection(getValues, null, register, unregister, valuesToManager);
            }

            /// <summary>
            /// Gestiona los eventos de una coleccion.
            /// </summary>
            public RegisterForImp<TSender> RegisterCollection<TItem>(Func<TSender, ICollection<TItem>> getValues,
                                                                     EventHandler_v2<CollectionEventArgs> eventHandler,
                                                                     RegisterCallback<TSender, EventHandler_v2<CollectionEventArgs>> register,
                                                                     UnregisterCallback<TSender, EventHandler_v2<CollectionEventArgs>> unregister,
                                                                     bool valuesToManager = true)
            {
                ListenerManager_v2<T>.CollectionListener<TItem> listener = new ListenerManager_v2<T>.CollectionListener<TItem>(BuildEventHandler(eventHandler, this.weak),
                                                                                                                               sender => getValues((TSender)sender),
                                                                                                                               (sender, h) => register((TSender)sender, h),
                                                                                                                               (sender, h) => unregister((TSender)sender, h),
                                                                                                                               valuesToManager);
                this.manager.Add<TSender>(listener);
                return this;
            }

            /// <summary>
            /// Gestiona los eventos de una coleccion.
            /// </summary>
            public RegisterForImp<TSender> RegisterCollection<TItem>(Func<TSender, IEventCollection<TItem>> getValues,
                                                                     bool valuesToManager = true)
            {
                return this.RegisterCollection(getValues, null, valuesToManager);
            }

            /// <summary>
            /// Gestiona los eventos de una coleccion.
            /// </summary
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

            #endregion

            #region Lists

            /// <summary>
            /// Gestiona los eventos de una lista.
            /// </summary>
            public RegisterForImp<TSender> RegisterList<TItem>(Func<TSender, IList<TItem>> getValues,
                                                               RegisterCallback<TSender, EventHandler_v2<ListEventArgs>> register,
                                                               UnregisterCallback<TSender, EventHandler_v2<ListEventArgs>> unregister,
                                                               bool valuesToManager = true)
            {
                return this.RegisterList(getValues, null, register, unregister, valuesToManager);
            }

            /// <summary>
            /// Gestiona los eventos de una lista.
            /// </summary>
            public RegisterForImp<TSender> RegisterList<TItem>(Func<TSender, IList<TItem>> getValues,
                                                               EventHandler_v2<ListEventArgs> eventHandler,
                                                               RegisterCallback<TSender, EventHandler_v2<ListEventArgs>> register,
                                                               UnregisterCallback<TSender, EventHandler_v2<ListEventArgs>> unregister,
                                                               bool valuesToManager = true)
            {
                ListenerManager_v2<T>.ListListener<TItem> listener = new ListenerManager_v2<T>.ListListener<TItem>(BuildEventHandler(eventHandler, this.weak),
                                                                                                                   sender => getValues((TSender)sender),
                                                                                                                   (sender, h) => register((TSender)sender, h),
                                                                                                                   (sender, h) => unregister((TSender)sender, h),
                                                                                                                   valuesToManager);
                this.manager.Add<TSender>(listener);
                return this;
            }

            /// <summary>
            /// Gestiona los eventos de una lista.
            /// </summary>
            public RegisterForImp<TSender> RegisterList<TItem>(Func<TSender, IEventList<TItem>> getValues,
                                                               bool valuesToManager = true)
            {
                return this.RegisterList(getValues, null, valuesToManager);
            }

            /// <summary>
            /// Gestiona los eventos de una lista.
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

            #endregion

            #region private

            private readonly ListenerManager_v2<T> manager;
            private readonly bool weak;

            #endregion
        }

        #endregion

        #endregion
    }

    public class ObjectRegistered : EventArgs
    {
        public ObjectRegistered(object item)
        {
            this.Item = item;
        }

        public object Item { get; private set; }

        public override string ToString()
        {
            return string.Format("Item: {0}", this.Item);
        }
    }
}