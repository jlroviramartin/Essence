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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Essence.Util.Events;
using Essence.Util.Properties;

namespace Essence.Util.WeakEvents
{
    /// <summary>
    ///     Mapa que gestiona los eventos debiles y permite deregistarlos.
    /// </summary>
    [Obsolete]
    public class WeakEventHandlerManager : DisposableObject
    {
        public ForSource<TSource> For<TSource>()
        {
            return new ForSource<TSource>(this);
        }

        public WeakEventHandlerManager RegisterWeak<TSource, TEventHandler, TEventArgs>(string name,
                                                                                        TEventHandler eventHandler,
                                                                                        RegisterCallback<TSource, TEventHandler> register,
                                                                                        UnregisterCallback<TSource, TEventHandler> unregister)
            where TEventArgs : EventArgs
        {
            Delegate delEventHandler = eventHandler as Delegate;

            Contract.Requires(delEventHandler != null);
            //// Only instance methods are supported.
            //Contract.Requires(!delEventHandler.Method.IsStatic && (delEventHandler.Target != null));
            if (delEventHandler.Method.IsStatic || (delEventHandler.Target == null))
            {
            }

            Contract.Assert(!this.mapDelegates.ContainsKey(name));

            Type sourceManagerType = typeof(SourceManager<,,,>).MakeGenericType(
                delEventHandler.Method.DeclaringType,
                typeof(TSource),
                typeof(TEventHandler),
                typeof(TEventArgs));

            ConstructorInfo sourceManagerConstructor = sourceManagerType.GetConstructor(new[]
            {
                typeof(TEventHandler),
                typeof(RegisterCallback<TSource, TEventHandler>),
                typeof(UnregisterCallback<TSource, TEventHandler>)
            });
            Contract.Assert(sourceManagerConstructor != null);

            ISourceManager sourceManager = (ISourceManager)sourceManagerConstructor.Invoke(new object[]
            {
                eventHandler,
                register,
                unregister
            });

            this.mapDelegates.Add(name, sourceManager);
            return this;
        }

        public void UnregisterWeak(string name)
        {
            ISourceManager sourceManager;
            if (this.TryGetSourceManager(name, out sourceManager))
            {
                sourceManager.RemoveAll();
                this.mapDelegates.Remove(name);
            }
        }

        /// <summary>
        ///     Añade el origen de eventos <c>source</c> y lo asocia a todos los
        ///     delegados que correspondan con el tipo <c>TSource</c>.
        /// </summary>
        public void AddWeak<TSource>(TSource source)
        {
            foreach (ISourceManager sourceManager in this.mapDelegates.Values.Where(x => typeof(TSource) == x.SourceType))
            {
                sourceManager.AddSource(source);
            }
        }

        /// <summary>
        ///     Añade el origen de eventos <c>source</c> y lo asocia al
        ///     delegado referenciado por <c>name</c>.
        /// </summary>
        public void AddWeak<TSource>(string name, TSource source)
        {
            ISourceManager sourceManager;
            if (this.TryGetSourceManager(name, out sourceManager))
            {
                sourceManager.AddSource(source);
            }
        }

        /// <summary>
        ///     Elimina el origen de eventos <c>source</c> y lo desasocia del
        ///     delegado referenciado por <c>name</c>.
        /// </summary>
        public void RemoveWeak<TSource>(string name, TSource source)
        {
            ISourceManager sourceManager;
            if (this.TryGetSourceManager(name, out sourceManager))
            {
                sourceManager.RemoveSource(source);
            }
        }

        /// <summary>
        ///     Elimina el origen de eventos <c>source</c> y lo desasocia a todos los
        ///     delegados que correspondan con el tipo <c>TSource</c>.
        /// </summary>
        public void RemoveWeak<TSource>(TSource source)
        {
            foreach (ISourceManager sourceManager in this.mapDelegates.Values.Where(x => typeof(TSource) == x.SourceType))
            {
                sourceManager.RemoveSource(source);
            }
        }

        /// <summary>
        ///     Elimina todos los origenes de eventos y los desasocia del
        ///     delegado referenciado por <c>name</c>.
        /// </summary>
        public void ClearWeaks(string name)
        {
            ISourceManager sourceManager;
            if (this.TryGetSourceManager(name, out sourceManager))
            {
                sourceManager.RemoveAll();
            }
        }

        /// <summary>
        ///     Elimina todos los origenes de eventos y los desasocia del
        ///     delegado que correspondan con el tipo <c>TSource</c>.
        /// </summary>
        public void ClearWeaks<TSource>()
        {
            foreach (ISourceManager sourceManager in this.mapDelegates.Values.Where(x => typeof(TSource) == x.SourceType))
            {
                sourceManager.RemoveAll();
            }
        }

        #region private

        private bool TryGetSourceManager(string name, out ISourceManager sourceManager)
        {
            if (this.mapDelegates.TryGetValue(name, out sourceManager))
            {
                if (sourceManager.Validate())
                {
                    return true;
                }
                else
                {
                    this.mapDelegates.Remove(name);
                }
            }
            return false;
        }

        /// <summary>Mapea de nombre a <c>ISourceManager</c>.</summary>
        private readonly Dictionary<string, ISourceManager> mapDelegates = new Dictionary<string, ISourceManager>();

        #endregion privados _______________________________________________________________

        #region DisposableObject

        protected override void DisposeOfManagedResources()
        {
            foreach (ISourceManager sourceManager in this.mapDelegates.Values.Where(x => x != null))
            {
                sourceManager.RemoveAll();
            }

            base.DisposeOfManagedResources();
        }

        #endregion

        #region Inner classes

        public delegate void UnregisterCallback<in TEventHandler>(object source, TEventHandler eventHandler);

        public delegate void UnregisterCallback<in TSource, in TEventHandler>(TSource source, TEventHandler eventHandler);

        public delegate void RegisterCallback<in TEventHandler>(object source, TEventHandler eventHandler);

        public delegate void RegisterCallback<in TSource, in TEventHandler>(TSource source, TEventHandler eventHandler);

        #region ISourceManager

        /// <summary>
        ///     Interface de apoyo para la gestion del evento.
        /// </summary>
        internal interface ISourceManager
        {
            /// <summary>
            ///     Valida el objeto (no ha sido liberado) e indica si es valido.
            /// </summary>
            bool Validate();

            /// <summary>
            ///     Añade un nuevo origen de eventos.
            /// </summary>
            void AddSource(object source);

            /// <summary>
            ///     Elimina un origen de eventos.
            /// </summary>
            void RemoveSource(object source);

            /// <summary>
            ///     Elimina todos los origenes de eventos.
            /// </summary>
            void RemoveAll();

            /// <summary>
            ///     Tipo del origen de eventos.
            /// </summary>
            Type SourceType { get; }
        }

        /// <summary>
        ///     Clase de apoyo para la gestion del evento. Permite multiples deregistros.
        ///     Igual a <c>SourceManager{T, TEventHandler, TEventArgs}</c> pero añade un parametro nuevo, <c>TSource</c>.
        /// </summary>
        /// <typeparam name="T">Tipo de la clase base.</typeparam>
        /// <typeparam name="TSource">Tipo sobre quien se escuchan los eventos.</typeparam>
        /// <typeparam name="TEventHandler">Tipo del handler.</typeparam>
        /// <typeparam name="TEventArgs">Tipo de los argumentos del evento.</typeparam>
        private sealed class SourceManager<T, TSource, TEventHandler, TEventArgs> : ISourceManager
            where T : class
            where TSource : class
            where TEventArgs : EventArgs
        {
            /// <summary>
            ///     Constructor.
            /// </summary>
            /// <param name="eventHandler">Handler origen.</param>
            /// <param name="register">Delegado para registrar el evento.</param>
            /// <param name="unregister">Delegado para deregistrar el evento.</param>
            [UsedImplicitly]
            public SourceManager(TEventHandler eventHandler,
                                 RegisterCallback<TSource, TEventHandler> register,
                                 UnregisterCallback<TSource, TEventHandler> unregister)
            {
                Delegate delEventHandler = (Delegate)(object)eventHandler;

                this.staticHandler = (delEventHandler.Target == null);
                if (!this.staticHandler)
                {
                    this.targetRef = new WeakReference<T>((T)delEventHandler.Target);
                    this.openHandler = (OpenEventHandler)Delegate.CreateDelegate(typeof(OpenEventHandler), null, delEventHandler.Method);
                }
                else
                {
                    this.targetRef = null;
                    this.closedHandler = (ClosedEventHandler)Delegate.CreateDelegate(typeof(ClosedEventHandler), null, delEventHandler.Method);
                }

                this.handler = (TEventHandler)(object)Delegate.CreateDelegate(typeof(TEventHandler), this, invokeMth);

                this.register = register;
                this.unregister = unregister;
            }

            /// <summary>
            ///     Metodo que gestiona la invocacion y deregistra el handler.
            /// </summary>
            [UsedImplicitly]
            public void Invoke(object sender, TEventArgs e)
            {
                if (this.staticHandler)
                {
                    this.closedHandler.Invoke(sender, e);
                }
                else
                {
                    T target = null;
                    if (!this.targetRef.TryGetTarget(out target) || ((target is IDisposableEx) && (((IDisposableEx)target).IsDisposed)))
                    {
                        this.DoUnregister();
                        return;
                    }
                    this.openHandler.Invoke(target, sender, e);
                }
            }

            #region private

            private void DoUnregister()
            {
                foreach (WeakReference<TSource> wsource in this.sources)
                {
                    TSource source;
                    if (wsource.TryGetTarget(out source))
                    {
                        this.unregister(source, this.handler);
                    }
                }
                this.sources.Clear();
            }

            private delegate void OpenEventHandler(T @this, object sender, TEventArgs args);

            private delegate void ClosedEventHandler(object sender, TEventArgs args);

            private readonly bool staticHandler;
            private readonly WeakReference<T> targetRef;
            private readonly OpenEventHandler openHandler;
            private readonly ClosedEventHandler closedHandler;

            private readonly TEventHandler handler;

            private readonly RegisterCallback<TSource, TEventHandler> register;
            private readonly UnregisterCallback<TSource, TEventHandler> unregister;

            private readonly List<WeakReference<TSource>> sources = new List<WeakReference<TSource>>();

            private static readonly MethodInfo invokeMth
                = typeof(SourceManager<T, TSource, TEventHandler, TEventArgs>).GetMethod("Invoke");

            #endregion

            #region ISourceManager

            public bool Validate()
            {
                if (!this.staticHandler)
                {
                    T target;
                    if (!this.targetRef.TryGetTarget(out target) || ((target is IDisposableEx) && (((IDisposableEx)target).IsDisposed)))
                    {
                        this.DoUnregister();
                        return false;
                    }
                }
                return true;
            }

            public void AddSource(object source)
            {
                this.register((TSource)source, this.handler);
                this.sources.Add(new WeakReference<TSource>((TSource)source));
            }

            public void RemoveSource(object source)
            {
                this.sources.Remove(new WeakReference<TSource>((TSource)source));
                this.unregister((TSource)source, this.handler);
            }

            public void RemoveAll()
            {
                this.DoUnregister();
            }

            public Type SourceType
            {
                get { return typeof(TSource); }
            }

            #endregion
        }

        #endregion

        #region ForSource

        public sealed class ForSource<TSource>
        {
            internal ForSource(WeakEventHandlerManager manager)
            {
                this.manager = manager;
            }

            private readonly WeakEventHandlerManager manager;

            public ForSource<TSource> RegisterWeak(string name,
                                                   EventHandler<PropertyChangedEventArgs> eventHandler,
                                                   RegisterCallback<TSource, EventHandler<PropertyChangedEventArgs>> register,
                                                   UnregisterCallback<TSource, EventHandler<PropertyChangedEventArgs>> unregister)
            {
                this.manager.RegisterWeak<TSource, EventHandler<PropertyChangedEventArgs>, PropertyChangedEventArgs>(name, eventHandler, register, unregister);
                return this;
            }

            public ForSource<TSource> RegisterWeak(string name,
                                                   EventHandler<PropertyChangedExEventArgs> eventHandler,
                                                   RegisterCallback<TSource, EventHandler<PropertyChangedExEventArgs>> register,
                                                   UnregisterCallback<TSource, EventHandler<PropertyChangedExEventArgs>> unregister)
            {
                this.manager.RegisterWeak<TSource, EventHandler<PropertyChangedExEventArgs>, PropertyChangedExEventArgs>(name, eventHandler, register, unregister);
                return this;
            }

            public ForSource<TSource> RegisterWeak(string name,
                                                   EventHandler eventHandler,
                                                   RegisterCallback<TSource, EventHandler> register,
                                                   UnregisterCallback<TSource, EventHandler> unregister)
            {
                this.manager.RegisterWeak<TSource, EventHandler, EventArgs>(name, eventHandler, register, unregister);
                return this;
            }

            public ForSource<TSource> RegisterWeak<TEventArgs>(string name,
                                                               EventHandler<TEventArgs> eventHandler,
                                                               RegisterCallback<TSource, EventHandler<TEventArgs>> register,
                                                               UnregisterCallback<TSource, EventHandler<TEventArgs>> unregister)
                where TEventArgs : EventArgs
            {
                this.manager.RegisterWeak<TSource, EventHandler<TEventArgs>, TEventArgs>(name, eventHandler, register, unregister);
                return this;
            }

            public ForSource<TSource> RegisterWeak<TEventArgs>(string name,
                                                               EventHandler_v2<TEventArgs> eventHandler,
                                                               RegisterCallback<TSource, EventHandler_v2<TEventArgs>> register,
                                                               UnregisterCallback<TSource, EventHandler_v2<TEventArgs>> unregister)
                where TEventArgs : EventArgs
            {
                this.manager.RegisterWeak<TSource, EventHandler_v2<TEventArgs>, TEventArgs>(name, eventHandler, register, unregister);
                return this;
            }
        }

        #endregion

        #endregion
    }
}