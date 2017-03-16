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

namespace Essence.Util.Events
{
    /// <summary>
    ///     Gestor de eventos debiles.
    ///     NOTE: no me convence.
    /// </summary>
    /// <typeparam name="TEventHandler">Tipo del handler del evento.</typeparam>
    /// <typeparam name="TEventArgs">Tipo de los argumentos del evento.</typeparam>
    public class WeakEventHandler<TEventHandler, TEventArgs>
        where TEventHandler : class
        where TEventArgs : EventArgs
    {
        private WeakEventHandler(Action<TEventHandler, object, TEventArgs> fire)
        {
            this.fire = fire;
        }

        public void FireEvent(object sender, TEventArgs args)
        {
            List<TEventHandler> callHandlers = null;
            lock (this.handlers)
            {
                for (int i = this.handlers.Count - 1; i >= 0; i--)
                {
                    TEventHandler target = (TEventHandler)this.handlers[i].Target;
                    if (target == null)
                    {
                        this.handlers.RemoveAt(i);
                    }
                    else
                    {
                        if (callHandlers == null)
                        {
                            callHandlers = new List<TEventHandler>();
                        }
                        callHandlers.Add(target);
                    }
                }
            }
            // Call event handlers using a separate event handler list after removing the dead entries
            // from the old list to ensure that registering+deregistering events from within an event
            // handler works as expected.
            if (callHandlers != null)
            {
                callHandlers.ForEach(h => this.fire(h, sender, args));
            }
        }

        public void AddEvent(TEventHandler value)
        {
            // event is easily made thread-safe is desired
            lock (this.handlers)
            {
                // optionally clean up when adding an event handler to prevent leak
                // when event is never fired:
                this.handlers.RemoveAll(wr =>
                {
                    TEventHandler target = (TEventHandler)wr.Target;
                    return (target == null);
                });

                this.handlers.Add(new WeakReference(value));
            }
        }

        public void RemoveEvent(TEventHandler value)
        {
            lock (this.handlers)
            {
                this.handlers.RemoveAll(wr =>
                {
                    TEventHandler target = (TEventHandler)wr.Target;
                    return ((target == null) || (target == value));
                });
            }
        }

        #region private

        private readonly Action<TEventHandler, object, TEventArgs> fire;
        private readonly List<WeakReference> handlers = new List<WeakReference>();

        #endregion
    }
}