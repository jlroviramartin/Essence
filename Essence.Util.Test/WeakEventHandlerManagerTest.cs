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
using Essence.Util.Collections;
using Essence.Util.Events;
using Essence.Util.WeakEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Essence.Util
{
    [TestClass]
    public class WeakEventHandlerManagerTest
    {
        [TestMethod]
        public void Test1()
        {
            WeakEventHandlerManager manager = new WeakEventHandlerManager();

            int[] eventCount_delegate = new int[1];
            EventHandler_v2<ListEventArgs> count_delegate = (sender, args) =>
            {
                eventCount_delegate[0]++;
            };

            DisposableCounter disposable = new DisposableCounter();

            manager.For<EventList<int>>().RegisterWeak("Count_delegate",
                                                       count_delegate,
                                                       (x, h) => x.ListChanged += h,
                                                       (x, h) => x.ListChanged -= h);

            manager.For<EventList<int>>().RegisterWeak<ListEventArgs>("Count_static",
                                                                      Count_static,
                                                                      (x, h) => x.ListChanged += h,
                                                                      (x, h) => x.ListChanged -= h);

            manager.For<EventList<int>>().RegisterWeak<ListEventArgs>("Count_instance",
                                                                      this.Count_instance,
                                                                      (x, h) => x.ListChanged += h,
                                                                      (x, h) => x.ListChanged -= h);

            manager.For<EventList<int>>().RegisterWeak<ListEventArgs>("Count_disposable",
                                                                      disposable.Count,
                                                                      (x, h) => x.ListChanged += h,
                                                                      (x, h) => x.ListChanged -= h);

            {
                eventCount_delegate[0] = 0;
                this.eventCount_instance = 0;
                eventCount_static = 0;

                EventList<int> eventList = new EventList<int>();
                manager.AddWeak("Count_delegate", eventList);
                manager.AddWeak("Count_static", eventList);
                manager.AddWeak("Count_instance", eventList);
                manager.AddWeak("Count_disposable", eventList);

                eventList.Add(0);
                eventList.Add(5);
                eventList.Add(10);
                eventList.Add(15);
                eventList.Add(20);
                eventList = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();

                Debug.WriteLine("eventCount_delegate = " + eventCount_delegate[0]);
                Debug.WriteLine("eventCount_static = " + eventCount_static);
                Debug.WriteLine("eventCount_instance = " + this.eventCount_instance);
                Debug.WriteLine("eventCount_disposable = " + disposable.eventCount_disposable);
            }

            manager.RemoveWeak("ListChanged");
        }

        private int eventCount_instance;

        public void Count_instance(object sender, ListEventArgs args)
        {
            this.eventCount_instance++;
        }

        private static int eventCount_static;

        public static void Count_static(object sender, ListEventArgs args)
        {
            eventCount_static++;
        }

        private class DisposableCounter : DisposableObject
        {
            public void Count(object sender, ListEventArgs args)
            {
                this.eventCount_disposable++;
            }

            public int eventCount_disposable;
        }
    }
}