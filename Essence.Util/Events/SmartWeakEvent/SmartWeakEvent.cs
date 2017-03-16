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
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SmartWeakEvent
{
    /// <summary>
    ///     A class for managing a weak event.
    /// </summary>
    public sealed class SmartWeakEvent<T> where T : class
    {
        private struct EventEntry
        {
            public readonly MethodInfo TargetMethod;
            public readonly WeakReference TargetReference;

            public EventEntry(MethodInfo targetMethod, WeakReference targetReference)
            {
                this.TargetMethod = targetMethod;
                this.TargetReference = targetReference;
            }
        }

        private readonly List<EventEntry> eventEntries = new List<EventEntry>();

        static SmartWeakEvent()
        {
            Essence.Util.Events.EventUtils.TestValidHandler<T, EventArgs>();
        }

        public void Add(T eh)
        {
            if (eh != null)
            {
                Delegate d = (Delegate)(object)eh;

                if (d.Method.DeclaringType.GetCustomAttributes(typeof (CompilerGeneratedAttribute), false).Length != 0)
                {
                    throw new ArgumentException("Cannot create weak event to anonymous method with closure.");
                }

                //if (eventEntries.Count == eventEntries.Capacity)
                //    RemoveDeadEntries();
                WeakReference target = d.Target != null ? new WeakReference(d.Target) : null;
                this.eventEntries.Add(new EventEntry(d.Method, target));
            }
        }

        private void RemoveDeadEntries()
        {
            this.eventEntries.RemoveAll(ee => ee.TargetReference != null && !ee.TargetReference.IsAlive);
        }

        public void Remove(T eh)
        {
            if (eh != null)
            {
                Delegate d = (Delegate)(object)eh;
                for (int i = this.eventEntries.Count - 1; i >= 0; i--)
                {
                    EventEntry entry = this.eventEntries[i];
                    if (entry.TargetReference != null)
                    {
                        object target = entry.TargetReference.Target;
                        if (target == null)
                        {
                            this.eventEntries.RemoveAt(i);
                        }
                        else if (target == d.Target && entry.TargetMethod == d.Method)
                        {
                            this.eventEntries.RemoveAt(i);
                            break;
                        }
                    }
                    else
                    {
                        if (d.Target == null && entry.TargetMethod == d.Method)
                        {
                            this.eventEntries.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        public void Raise(object sender, EventArgs e)
        {
            bool needsCleanup = false;
            object[] parameters = { sender, e };
            foreach (EventEntry ee in this.eventEntries.ToArray())
            {
                if (ee.TargetReference != null)
                {
                    object target = ee.TargetReference.Target;
                    if (target != null)
                    {
                        ee.TargetMethod.Invoke(target, parameters);
                    }
                    else
                    {
                        needsCleanup = true;
                    }
                }
                else
                {
                    ee.TargetMethod.Invoke(null, parameters);
                }
            }
            if (needsCleanup)
            {
                this.RemoveDeadEntries();
            }
        }
    }
}