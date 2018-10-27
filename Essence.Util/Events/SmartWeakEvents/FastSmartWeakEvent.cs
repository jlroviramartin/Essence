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

#if !NETSTANDARD
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Essence.Util.Events.SmartWeakEvents
{
    /// <summary>
    ///     A class for managing a weak event.
    ///     See http://www.codeproject.com/Articles/29922/Weak-Events-in-C
    /// </summary>
    /// <typeparam name="T">The delegate type of the event handlers.</typeparam>
    public sealed class FastSmartWeakEvent<T> where T : class
    {
        static FastSmartWeakEvent()
        {
            Essence.Util.Events.EventUtils.TestValidHandler<T, EventArgs>();
        }

        private class HandlerEntry
        {
            public readonly FastSmartWeakEvent<T> ParentEventSource;
            private readonly WeakReference weakReference;
            public readonly MethodInfo TargetMethod;
            public Delegate WrappingDelegate;

            public HandlerEntry(FastSmartWeakEvent<T> parentEventSource, object targetInstance, MethodInfo targetMethod)
            {
                this.ParentEventSource = parentEventSource;
                this.weakReference = new WeakReference(targetInstance);
                this.TargetMethod = targetMethod;
            }

            // This property is accessed by the generated IL method
            public object TargetInstance
            {
                get { return this.weakReference.Target; }
            }

            // This method is called by the generated IL method
            public void CalledWhenDead()
            {
                this.ParentEventSource.RemoveFromRaiseDelegate(this.WrappingDelegate);
            }

            /*
             A wrapper method like this is generated using IL.Emit and attached to this object.
             The signature of the method depends on the delegate type T.
            this.WrappingDelegate = delegate(object sender, EventArgs e)
            {
                object target = this.TargetInstance;
                if (target == null)
                    this.CalledWhenDead();
                else
                    ((TargetType)target).TargetMethod(sender, e);
            }
             */
        }

        private volatile Delegate _raiseDelegate;

        private Delegate GetRaiseDelegateInternal()
        {
            return this._raiseDelegate;
        }

#pragma warning disable 420 // CS0420 - a reference to a volatile field will not be treated as volatile
        // can be ignored because CompareExchange() treats the reference as volatile
        private void AddToRaiseDelegate(Delegate d)
        {
            Delegate oldDelegate, newDelegate;
            do
            {
                oldDelegate = this._raiseDelegate;
                newDelegate = Delegate.Combine(oldDelegate, d);
            } while (Interlocked.CompareExchange(ref this._raiseDelegate, newDelegate, oldDelegate) != oldDelegate);
        }

        private void RemoveFromRaiseDelegate(Delegate d)
        {
            Delegate oldDelegate, newDelegate;
            do
            {
                oldDelegate = this._raiseDelegate;
                newDelegate = Delegate.Remove(oldDelegate, d);
            } while (Interlocked.CompareExchange(ref this._raiseDelegate, newDelegate, oldDelegate) != oldDelegate);
        }
#pragma warning restore 420

        public void Add(T eh)
        {
            if (eh != null)
            {
                Delegate d = (Delegate)(object)eh;
                this.RemoveDeadEntries();
                object targetInstance = d.Target;
                if (targetInstance != null)
                {
                    MethodInfo targetMethod = d.Method;
                    HandlerEntry wd = new HandlerEntry(this, targetInstance, targetMethod);
                    DynamicMethod dynamicMethod = GetInvoker(targetMethod);
                    wd.WrappingDelegate = dynamicMethod.CreateDelegate(typeof(T), wd);
                    this.AddToRaiseDelegate(wd.WrappingDelegate);
                }
                else
                {
                    // delegate to static method: use directly without wrapping delegate
                    this.AddToRaiseDelegate(d);
                }
            }
        }

        /// <summary>
        ///     Removes dead entries from the handler list.
        ///     You normally do not need to invoke this method manually, as dead entry removal runs automatically as part of the
        ///     normal operation of the FastSmartWeakEvent.
        /// </summary>
        public void RemoveDeadEntries()
        {
            Delegate raiseDelegate = this.GetRaiseDelegateInternal();
            if (raiseDelegate == null)
            {
                return;
            }
            foreach (Delegate d in raiseDelegate.GetInvocationList())
            {
                HandlerEntry wd = d.Target as HandlerEntry;
                if (wd != null && wd.TargetInstance == null)
                {
                    this.RemoveFromRaiseDelegate(d);
                }
            }
        }

        public void Remove(T eh)
        {
            if (eh == null)
            {
                return;
            }
            Delegate d = (Delegate)(object)eh;
            object targetInstance = d.Target;
            if (targetInstance == null)
            {
                // delegate to static method: use directly without wrapping delegate
                this.RemoveFromRaiseDelegate(d);
                return;
            }
            MethodInfo targetMethod = d.Method;
            // Find+Remove the last copy of a delegate pointing to targetInstance/targetMethod
            Delegate raiseDelegate = this.GetRaiseDelegateInternal();
            if (raiseDelegate == null)
            {
                return;
            }
            Delegate[] invocationList = raiseDelegate.GetInvocationList();
            for (int i = invocationList.Length - 1; i >= 0; i--)
            {
                Delegate wrappingDelegate = invocationList[i];
                HandlerEntry weakDelegate = wrappingDelegate.Target as HandlerEntry;
                if (weakDelegate == null)
                {
                    continue;
                }
                object target = weakDelegate.TargetInstance;
                if (target == null)
                {
                    this.RemoveFromRaiseDelegate(wrappingDelegate);
                }
                else if (target == targetInstance && weakDelegate.TargetMethod == targetMethod)
                {
                    this.RemoveFromRaiseDelegate(wrappingDelegate);
                    break;
                }
            }
        }

        /// <summary>
        ///     Retrieves a delegate that can be used to raise the event.
        ///     The delegate will contain a copy of the current invocation list. If handlers are added/removed from the event,
        ///     GetRaiseDelegate() must be called
        ///     again to retrieve a delegate that invokes the up-to-date invocation list.
        ///     If the invocation list is empty, this method will return null.
        /// </summary>
        public T GetRaiseDelegate()
        {
            return (T)(object)this.GetRaiseDelegateInternal();
        }

        /// <summary>
        ///     Gets whether the event has listeners that were not cleaned up yet.
        /// </summary>
        public bool HasListeners
        {
            get { return this.GetRaiseDelegateInternal() != null; }
        }

        #region Code Generation

        private static readonly MethodInfo getTargetMethod = typeof(HandlerEntry).GetMethod("get_TargetInstance");
        private static readonly MethodInfo calledWhileDeadMethod = typeof(HandlerEntry).GetMethod("CalledWhenDead");

        private static readonly Dictionary<MethodInfo, DynamicMethod> invokerMethods = new Dictionary<MethodInfo, DynamicMethod>();

        private static DynamicMethod GetInvoker(MethodInfo method)
        {
            DynamicMethod dynamicMethod;
            lock (invokerMethods)
            {
                if (invokerMethods.TryGetValue(method, out dynamicMethod))
                {
                    return dynamicMethod;
                }
            }

            if (method.DeclaringType.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length != 0)
            {
                throw new ArgumentException("Cannot create weak event to anonymous method with closure.");
            }

            ParameterInfo[] parameters = method.GetParameters();
            Type[] dynamicMethodParameterTypes = new Type[parameters.Length + 1];
            dynamicMethodParameterTypes[0] = typeof(HandlerEntry);
            for (int i = 0; i < parameters.Length; i++)
            {
                dynamicMethodParameterTypes[i + 1] = parameters[i].ParameterType;
            }

            dynamicMethod = new DynamicMethod("FastSmartWeakEvent", typeof(void), dynamicMethodParameterTypes, typeof(HandlerEntry), true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.EmitCall(OpCodes.Call, getTargetMethod, null);
            il.Emit(OpCodes.Dup);
            Label label = il.DefineLabel();
            // Exit if target is null (was garbage-collected)
            il.Emit(OpCodes.Brtrue, label);
            il.Emit(OpCodes.Pop); // pop the duplicate null target
            il.Emit(OpCodes.Ldarg_0);
            il.EmitCall(OpCodes.Call, calledWhileDeadMethod, null);
            il.Emit(OpCodes.Ret);
            il.MarkLabel(label);
            il.Emit(OpCodes.Castclass, method.DeclaringType);
            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Ldarg, i + 1);
            }
            il.EmitCall(OpCodes.Call, method, null);
            il.Emit(OpCodes.Ret);

            lock (invokerMethods)
            {
                invokerMethods[method] = dynamicMethod;
            }
            return dynamicMethod;
        }

        #endregion
    }

    /// <summary>
    ///     Strongly-typed raise methods for FastSmartWeakEvent
    /// </summary>
    public static class FastSmartWeakEventRaiseExtensions
    {
        public static void Raise(this FastSmartWeakEvent<EventHandler> ev, object sender, EventArgs e)
        {
            EventHandler d = ev.GetRaiseDelegate();
            if (d != null)
            {
                d(sender, e);
            }
        }

        public static void Raise<T>(this FastSmartWeakEvent<EventHandler<T>> ev, object sender, T e) where T : EventArgs
        {
            EventHandler<T> d = ev.GetRaiseDelegate();
            if (d != null)
            {
                d(sender, e);
            }
        }
    }
}
#endif