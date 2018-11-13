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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;
using Essence.Util.IO;
using Essence.Util.Properties;

namespace Essence.Util.Events
{
    public static class EventUtils
    {
        public static void SendSafe<TDel>(TDel del, Action<TDel> send)
            where TDel : class
        {
            Delegate ddel = del as Delegate;
            if (ddel != null)
            {
                foreach (Delegate currentHandler in ddel.GetInvocationList())
                {
                    Delegate currentSubscriber = currentHandler;
                    send(currentSubscriber as TDel);
                }
            }
        }

        #region WeakListen

        /// <summary>
        /// Crea un handler debil para eventos de tipo <c>EventHandler</c>,
        /// </summary>
        /// <param name="eventHandler">Handler origen.</param>
        /// <param name="register">Delegado para registrar el evento.</param>
        /// <param name="unregister">Delegado para deregistrar el evento.</param>
        /// <returns>Handler debil.</returns>
        public static IWeakEventHandler<EventHandler> WeakListen(
            this EventHandler eventHandler,
            RegisterCallback<EventHandler> register,
            UnregisterCallback<EventHandler> unregister)
        {
            return WeakListen<EventHandler, EventArgs>(eventHandler, register, unregister);
        }

        /// <summary>
        /// Crea un handler debil para eventos de tipo <c>EventHandler</c>,
        /// </summary>
        /// <param name="eventHandler">Handler origen.</param>
        /// <param name="register">Delegado para registrar el evento.</param>
        /// <param name="unregister">Delegado para deregistrar el evento.</param>
        /// <returns>Handler debil.</returns>
        public static IWeakEventHandler<EventHandler<T>> WeakListen<T>(
            this EventHandler<T> eventHandler,
            RegisterCallback<EventHandler<T>> register,
            UnregisterCallback<EventHandler<T>> unregister)
            where T : EventArgs
        {
            return WeakListen<EventHandler<T>, T>(eventHandler, register, unregister);
        }

        /// <summary>
        /// Crea un handler debil para eventos de tipo <c>EventHandler_v2</c>,
        /// </summary>
        /// <param name="eventHandler">Handler origen.</param>
        /// <param name="register">Delegado para registrar el evento.</param>
        /// <param name="unregister">Delegado para deregistrar el evento.</param>
        /// <returns>Handler debil.</returns>
        public static IWeakEventHandler<EventHandler_v2<T>> WeakListen<T>(
            this EventHandler_v2<T> eventHandler,
            RegisterCallback<EventHandler_v2<T>> register,
            UnregisterCallback<EventHandler_v2<T>> unregister)
            where T : EventArgs
        {
            return WeakListen<EventHandler_v2<T>, T>(eventHandler, register, unregister);
        }

        /// <summary>
        /// Escucha con un handler debil para eventos de tipo <c>TEventHandler</c>,
        /// </summary>
        /// <param name="eventHandler">Handler origen.</param>
        /// <param name="register">Delegado para registrar el evento.</param>
        /// <param name="unregister">Delegado para deregistrar el evento.</param>
        /// <returns>Handler debil.</returns>
        public static IWeakEventHandler<TEventHandler> WeakListen<TEventHandler, TEventArgs>(
            TEventHandler eventHandler,
            RegisterCallback<TEventHandler> register,
            UnregisterCallback<TEventHandler> unregister)
            where TEventArgs : EventArgs
        {
            IWeakEventHandler<TEventHandler> weak = MakeWeak<TEventHandler, TEventArgs>(eventHandler, unregister);
            register(weak.Handler);
            return weak;
        }

        #endregion WeakListen

        #region MakeWeak

        /// <summary>
        /// Crea un handler debil para eventos de tipo <c>EventHandler</c>,
        /// </summary>
        /// <param name="eventHandler">Handler origen.</param>
        /// <param name="unregister">Delegado para deregistrar el evento.</param>
        /// <returns>Handler debil.</returns>
        public static IWeakEventHandler<EventHandler> MakeWeak(
            this EventHandler eventHandler,
            UnregisterCallback<EventHandler> unregister)
        {
            return MakeWeak<EventHandler, EventArgs>(eventHandler, unregister);
        }

        /// <summary>
        /// Crea un handler debil para eventos de tipo <c>EventHandler</c>,
        /// </summary>
        /// <param name="eventHandler">Handler origen.</param>
        /// <param name="unregister">Delegado para deregistrar el evento.</param>
        /// <returns>Handler debil.</returns>
        public static IWeakEventHandler<EventHandler<T>> MakeWeak<T>(
            this EventHandler<T> eventHandler,
            UnregisterCallback<EventHandler<T>> unregister)
            where T : EventArgs
        {
            return MakeWeak<EventHandler<T>, T>(eventHandler, unregister);
        }

        /// <summary>
        /// Crea un handler debil para eventos de tipo <c>EventHandler_v2</c>,
        /// </summary>
        /// <param name="eventHandler">Handler origen.</param>
        /// <param name="unregister">Delegado para deregistrar el evento.</param>
        /// <returns>Handler debil.</returns>
        public static IWeakEventHandler<EventHandler_v2<T>> MakeWeak<T>(
            this EventHandler_v2<T> eventHandler,
            UnregisterCallback<EventHandler_v2<T>> unregister)
            where T : EventArgs
        {
            return MakeWeak<EventHandler_v2<T>, T>(eventHandler, unregister);
        }

        /// <summary>
        /// Crea un handler debil para eventos de tipo <c>TEventHandler</c>,
        /// </summary>
        /// <param name="eventHandler">Handler origen.</param>
        /// <param name="unregister">Delegado para deregistrar el evento.</param>
        /// <returns>Handler debil.</returns>
        public static IWeakEventHandler<TEventHandler> MakeWeak<TEventHandler, TEventArgs>(
            TEventHandler eventHandler,
            UnregisterCallback<TEventHandler> unregister)
            where TEventArgs : EventArgs
        {
            Delegate delEventHandler = eventHandler as Delegate;

            Contract.Requires(delEventHandler != null);
            // Only instance methods are supported.
            Contract.Requires(!delEventHandler.Method.IsStatic && delEventHandler.Target != null);

            Type wehType = typeof(WeakEventHandler<,,>).MakeGenericType(
                delEventHandler.Method.DeclaringType,
                typeof(TEventHandler),
                typeof(TEventArgs));

            ConstructorInfo wehConstructor = wehType.GetConstructor(new[]
            {
                typeof(TEventHandler),
                typeof(UnregisterCallback<TEventHandler>)
            });
            Contract.Assert(wehConstructor != null);

            IWeakEventHandler<TEventHandler> weh = (IWeakEventHandler<TEventHandler>)wehConstructor.Invoke(new object[]
            {
                eventHandler,
                unregister
            });

            return weh;
        }

        #endregion MakeWeak

        /// <summary>
        /// Comprueba si la definicion del delegado es correcta.
        /// </summary>
        internal static void TestValidHandler<THandler, TArgs>()
            where TArgs : EventArgs
        {
            if (!typeof(THandler).IsSubclassOf(typeof(Delegate)))
            {
                throw new ArgumentException("THandler must be a delegate type");
            }
            MethodInfo invoke = typeof(THandler).GetMethod("Invoke");
            if (invoke == null || invoke.GetParameters().Length != 2)
            {
                throw new ArgumentException("THandler must be a delegate type taking 2 parameters");
            }
            ParameterInfo senderParameter = invoke.GetParameters()[0];
            if (senderParameter.ParameterType != typeof(object))
            {
                throw new ArgumentException("The first delegate parameter must be of type 'object'");
            }
            ParameterInfo argsParameter = invoke.GetParameters()[1];
            if (!(typeof(TArgs).IsAssignableFrom(argsParameter.ParameterType)))
            {
                throw new ArgumentException("The second delegate parameter must be derived from type 'EventArgs'");
            }
            if (invoke.ReturnType != typeof(void))
            {
                throw new ArgumentException("The delegate return type must be void.");
            }
            foreach (ParameterInfo p in invoke.GetParameters())
            {
                if (p.IsOut && !p.IsIn)
                {
                    throw new ArgumentException("The delegate type must not have out-parameters");
                }
            }
        }

        #region Inner classes

        /// <summary>
        /// Delegado que gestiona el deregistro del handler.
        /// <see cref="http://diditwith.net/2007/03/23/SolvingTheProblemWithEventsWeakEventHandlers.aspx" />
        /// </summary>
        /// <typeparam name="TEventHandler">Tipo del handler.</typeparam>
        /// <param name="eventHandler">Handler.</param>
        public delegate void UnregisterCallback<in TEventHandler>(TEventHandler eventHandler);

        /// <summary>
        /// Delegado que gestion el registro del handler. Por homogeneidad con <c>UnregisterCallback</c>
        /// aunque aqui no se utiliza.
        /// </summary>
        /// <typeparam name="TEventHandler">Tipo del handler.</typeparam>
        /// <param name="eventHandler">Handler.</param>
        public delegate void RegisterCallback<in TEventHandler>(TEventHandler eventHandler);

        /// <summary>
        /// Interface de apoyo para la gestion del evento.
        /// </summary>
        public interface IWeakEventHandler
        {
            Delegate Handler { get; }

            void Unregister();
        }

        /// <summary>
        /// Interface de apoyo para la gestion del evento.
        /// </summary>
        /// <typeparam name="TEventHandler">Tipo del handler.</typeparam>
        public interface IWeakEventHandler<out TEventHandler> : IWeakEventHandler
        {
            new TEventHandler Handler { get; }
        }

        /// <summary>
        /// Clase de apoyo para la gestion del evento.
        /// </summary>
        /// <typeparam name="T">Tipo de la clase base.</typeparam>
        /// <typeparam name="TEventHandler">Tipo del handler.</typeparam>
        /// <typeparam name="TEventArgs">Tipo de los argumentos del evento.</typeparam>
        [DebuggerNonUserCode]
        private sealed class WeakEventHandler<T, TEventHandler, TEventArgs> : IWeakEventHandler<TEventHandler>
            where T : class
            where TEventArgs : EventArgs
        {
            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="eventHandler">Handler origen.</param>
            /// <param name="unregister">Delegado para deregistrar el evento.</param>
            public WeakEventHandler(TEventHandler eventHandler, UnregisterCallback<TEventHandler> unregister)
            {
                Delegate delEventHandler = (Delegate)(object)eventHandler;

                this.targetRef = new WeakReference(delEventHandler.Target);
                this.openHandler = (OpenEventHandler)Delegate.CreateDelegate(typeof(OpenEventHandler), null, delEventHandler.Method);
                this.handler = (TEventHandler)(object)Delegate.CreateDelegate(typeof(TEventHandler), this, invokeMth);
                this.unregister = unregister;
            }

            /// <summary>
            /// Metodo que gestiona la invocacion y deregistra el handler.
            /// </summary>
            [UsedImplicitly]
            public void Invoke(object sender, TEventArgs e)
            {
                T target = (T)this.targetRef.Target;
                if (target != null)
                {
                    IDisposableEx disposable = target as IDisposableEx;
                    if ((disposable != null) && disposable.IsDisposed)
                    {
                        this.DoUnregister();
                        return;
                    }

                    this.openHandler.Invoke(target, sender, e);
                }
                else
                {
                    this.DoUnregister();
                }
            }

            #region private

            private void DoUnregister()
            {
#if DEBUG
                Debug.WriteLine("Deregistrando " + this.handler);
#endif

                if (this.unregister != null)
                {
                    this.unregister(this.handler);
                    this.unregister = null;
                }
            }

            private delegate void OpenEventHandler(T @this, object sender, TEventArgs args);

            private readonly WeakReference targetRef;
            private readonly OpenEventHandler openHandler;
            private readonly TEventHandler handler;
            private UnregisterCallback<TEventHandler> unregister;

            private static readonly MethodInfo invokeMth = typeof(WeakEventHandler<T, TEventHandler, TEventArgs>).GetMethod("Invoke");

            #endregion

            #region object

            public override string ToString()
            {
                return new ToStringBuilder()
                       .Append("TargetRef", this.targetRef)
                       .Append("OpenEventHandler", this.openHandler)
                       .Append("Handler", this.handler)
                       .Append("Unregister", this.unregister)
                       .ToString();
            }

            #endregion

            #region IWeakEventHandler<TEventHandler>

            public TEventHandler Handler
            {
                get { return this.handler; }
            }

            #endregion

            #region IWeakEventHandler

            Delegate IWeakEventHandler.Handler
            {
                get { return this.handler as Delegate; }
            }

            public void Unregister()
            {
                this.targetRef.Target = null;
                this.DoUnregister();
            }

            #endregion
        }

        /// <summary>
        /// Clase de ayuda para fijar los eventos.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        private sealed class FixedHandler<THandler, TArgs>
            where THandler : class // EventHandler/Delegate
            where TArgs : EventArgs
        {
            public FixedHandler(THandler handler)
            {
                this.handler = handler as Delegate;
            }

            public void Invoke(object sender, TArgs args)
            {
                this.handler.Method.Invoke(this.handler.Target, new object[] { sender, args });
            }

            internal Delegate Handler
            {
                get { return this.handler; }
            }

            #region private

            static FixedHandler()
            {
                TestValidHandler<THandler, TArgs>();
            }

            private readonly Delegate handler;

            #endregion
        }

        #endregion
    }
}