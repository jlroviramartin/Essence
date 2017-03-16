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
using System.Diagnostics;
using System.Linq;
using Essence.Util.Collections;
using Essence.Util.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Essence.Util.Properties;

namespace Essence.Util.Test
{
    [TestClass]
    public class ListenerManagerTest
    {
        /// <summary>
        ///     Se prueba el envio de eventos de varios tipos.
        /// </summary>
        [TestMethod]
        public void Test1()
        {
            Listener listener = new Listener();

            ListenerManager_v2<C1> manager = new ListenerManager_v2<C1>();
            List<int> registered = new List<int>();
            manager.Registering += (sender, args) =>
            {
                int id = ((Base)args.Item).Id;
                registered.Add(id);

                //Debug.WriteLine("Registering " + args.Item);
            };
            manager.Unregistering += (sender, args) =>
            {
                int id = ((Base)args.Item).Id;
                registered.Remove(id);

                //Debug.WriteLine("Unregistering " + args.Item);
            };

            RegisterC1(manager, listener);
            RegisterC2(manager, listener);
            RegisterC3(manager, listener);

            C1 root = CreateTest();

            // Se envian eventos cuando aun no esta conectado.
            SendEvent(root);

            Assert.IsTrue(registered.Count == 0);
            Assert.IsTrue(listener.PropertyArgs.Count == 0);
            Assert.IsTrue(listener.ListArgs.Count == 0);
            Assert.IsTrue(listener.OtherArgs.Count == 0);

            // Se establece la raiz.
            manager.SetRoot(root);

            Assert.IsTrue(registered.Count == 133);
            Assert.IsTrue(listener.PropertyArgs.Count == 0);
            Assert.IsTrue(listener.ListArgs.Count == 0);
            Assert.IsTrue(listener.OtherArgs.Count == 0);

            // Se envian eventos cuando ya esta conectado pero sin contenido.
            SendEvent(root);

            Assert.IsTrue(registered.Count == 133);
            Assert.IsTrue(listener.PropertyArgs.Count == 266);
            Assert.IsTrue(listener.ListArgs.Count == 0);
            Assert.IsTrue(listener.OtherArgs.Count == 133);

            listener.PropertyArgs.Clear();
            listener.OtherArgs.Clear();

            // Se elimina la raiz.
            manager.SetRoot(null);

            Assert.IsTrue(registered.Count == 0);
            Assert.IsTrue(listener.PropertyArgs.Count == 0);
            Assert.IsTrue(listener.ListArgs.Count == 0);
            Assert.IsTrue(listener.OtherArgs.Count == 0);

            // Se envian eventos cuando ya no esta conectado.
            SendEvent(root);

            Assert.IsTrue(registered.Count == 0);
            Assert.IsTrue(listener.PropertyArgs.Count == 0);
            Assert.IsTrue(listener.ListArgs.Count == 0);
            Assert.IsTrue(listener.OtherArgs.Count == 0);
        }

        /// <summary>
        ///     Se prueba el envio de eventos al modificar propiedades/añadir en listas.
        /// </summary>
        [TestMethod]
        public void Test2()
        {
            Listener listener = new Listener();

            ListenerManager_v2<C1> manager = new ListenerManager_v2<C1>();
            List<int> registered = new List<int>();
            manager.Registering += (sender, args) =>
            {
                int id = ((Base)args.Item).Id;
                registered.Add(id);

                //Debug.WriteLine("Registering " + args.Item);
            };
            manager.Unregistering += (sender, args) =>
            {
                int id = ((Base)args.Item).Id;
                registered.Remove(id);

                //Debug.WriteLine("Unregistering " + args.Item);
            };

            RegisterC1(manager, listener);
            RegisterC2(manager, listener);
            RegisterC3(manager, listener);

            C1 root = new C1();

            // Se envian eventos cuando aun no esta conectado.
            SendEvent(root);

            Assert.IsTrue(listener.PropertyArgs.Count == 0);
            Assert.IsTrue(listener.ListArgs.Count == 0);
            Assert.IsTrue(listener.OtherArgs.Count == 0);

            // Se establece la raiz.
            manager.SetRoot(root);

            Assert.IsTrue(registered.Count == 1);
            Assert.IsTrue(listener.PropertyArgs.Count == 0);
            Assert.IsTrue(listener.ListArgs.Count == 0);
            Assert.IsTrue(listener.OtherArgs.Count == 0);

            // Se envian eventos cuando ya esta conectado pero sin contenido.
            SendEvent(root);

            Assert.IsTrue(registered.Count == 1);
            Assert.IsTrue(listener.PropertyArgs.Count == 2);
            Assert.IsTrue(listener.ListArgs.Count == 0);
            Assert.IsTrue(listener.OtherArgs.Count == 1);

            listener.PropertyArgs.Clear();
            listener.OtherArgs.Clear();

            // Se añade contenido.
            AddContent(root);

            Assert.IsTrue(registered.Count == 133);
            Assert.IsTrue(listener.PropertyArgs.Count == 12);
            Assert.IsTrue(listener.ListArgs.Count == 120);
            Assert.IsTrue(listener.OtherArgs.Count == 0);

            listener.PropertyArgs.Clear();
            listener.ListArgs.Clear();

            // Se envian eventos cuando ya esta conectado y con contenido.
            SendEvent(root);

            Assert.IsTrue(registered.Count == 133);
            Assert.IsTrue(listener.PropertyArgs.Count == 266);
            Assert.IsTrue(listener.ListArgs.Count == 0);
            Assert.IsTrue(listener.OtherArgs.Count == 133);

            listener.PropertyArgs.Clear();
            listener.OtherArgs.Clear();

            // Se elimina contenido.
            RemoveContent(root);

            Assert.IsTrue(registered.Count == 1);
            Assert.IsTrue(listener.PropertyArgs.Count == 1);
            Assert.IsTrue(listener.ListArgs.Count == 1);
            Assert.IsTrue(listener.OtherArgs.Count == 0);

            listener.PropertyArgs.Clear();
            listener.ListArgs.Clear();

            // Se envian eventos cuando ya esta conectado y sin contenido.
            SendEvent(root);

            Assert.IsTrue(registered.Count == 1);
            Assert.IsTrue(listener.PropertyArgs.Count == 2);
            Assert.IsTrue(listener.ListArgs.Count == 0);
            Assert.IsTrue(listener.OtherArgs.Count == 1);

            listener.PropertyArgs.Clear();
            listener.OtherArgs.Clear();

            // Se elimina la raiz.
            manager.SetRoot(null);

            Assert.IsTrue(registered.Count == 0);
            Assert.IsTrue(listener.PropertyArgs.Count == 0);
            Assert.IsTrue(listener.ListArgs.Count == 0);
            Assert.IsTrue(listener.OtherArgs.Count == 0);

            // Se envian eventos cuando ya no esta conectado.
            SendEvent(root);

            Assert.IsTrue(registered.Count == 0);
            Assert.IsTrue(listener.PropertyArgs.Count == 0);
            Assert.IsTrue(listener.ListArgs.Count == 0);
            Assert.IsTrue(listener.OtherArgs.Count == 0);
        }

        private static C1 CreateTest()
        {
            C1 c1 = new C1();
            c1.C2 = new C2();
            for (int i = 0; i < 10; i++)
            {
                c1.ListC2.Add(new C2());
            }

            foreach (C2 c2 in c1.ListC2.Concat(new[] { c1.C2 }))
            {
                c2.C3 = new C3();
                for (int i = 0; i < 10; i++)
                {
                    c2.ListC3.Add(new C3());
                }
            }
            return c1;
        }

        private static void AddContent(C1 c1)
        {
            c1.C2 = new C2();
            for (int i = 0; i < 10; i++)
            {
                c1.ListC2.Add(new C2());
            }

            foreach (C2 c2 in c1.ListC2.Concat(new[] { c1.C2 }))
            {
                c2.C3 = new C3();
                for (int i = 0; i < 10; i++)
                {
                    c2.ListC3.Add(new C3());
                }
            }
        }

        private static void RemoveContent(C1 c1)
        {
            c1.C2 = null;
            c1.ListC2.Clear();
        }

        private static void RegisterC1(ListenerManager_v2<C1> manager, Listener listener)
        {
            manager.RegisterFor<C1>()
                   .Register<EventArgs>(listener.Listen,
                                        (x, h) => x.Ev1 += h,
                                        (x, h) => x.Ev1 -= h)
                   .RegisterProperty("V1", x => x.V1, listener.Listen, false)
                   .RegisterProperty("V2", x => x.V2, listener.Listen, false)
                   .RegisterProperty("C2", x => x.C2, listener.Listen)
                   .RegisterList(x => x.ListC2, listener.Listen);
        }

        private static void RegisterC2(ListenerManager_v2<C1> manager, Listener listener)
        {
            manager.RegisterFor<C2>()
                   .Register<EventArgs>(listener.Listen,
                                        (x, h) => x.Ev1 += h,
                                        (x, h) => x.Ev1 -= h)
                   .RegisterProperty("V1", x => x.V1, listener.Listen, false)
                   .RegisterProperty("V2", x => x.V2, listener.Listen, false)
                   .RegisterProperty("C3", x => x.C3, listener.Listen)
                   .RegisterList(x => x.ListC3, listener.Listen);
        }

        private static void RegisterC3(ListenerManager_v2<C1> manager, Listener listener)
        {
            manager.RegisterFor<C3>()
                   .Register<EventArgs>(listener.Listen,
                                        (x, h) => x.Ev1 += h,
                                        (x, h) => x.Ev1 -= h)
                   .RegisterProperty("V1", x => x.V1, listener.Listen, false)
                   .RegisterProperty("V2", x => x.V2, listener.Listen, false);
        }

        private static void SendEvent(C1 c1)
        {
            if (c1 == null)
            {
                return;
            }

            c1.OnEv1();
            c1.V1++;
            c1.V2++;

            SendEvent(c1.C2);
            c1.ListC2.ForEach(SendEvent);
        }

        private static void SendEvent(C2 c2)
        {
            if (c2 == null)
            {
                return;
            }

            c2.OnEv1();
            c2.V1++;
            c2.V2++;

            SendEvent(c2.C3);
            c2.ListC3.ForEach(SendEvent);
        }

        private static void SendEvent(C3 c3)
        {
            if (c3 == null)
            {
                return;
            }

            c3.OnEv1();
            c3.V1++;
            c3.V2++;
        }

        private class Listener
        {
            public void Listen(object sender, EventArgs args)
            {
                //Debug.WriteLine("Listen " + sender + " args");

                if (args is PropertyChangedExEventArgs)
                {
                    int id = ((Base)sender).Id;
                    this.PropertyArgs.Add(Tuple.Create(id, (PropertyChangedExEventArgs)args));
                }
                else if (args is ListEventArgs)
                {
                    int id = ((Base)((ListEventArgs)args).Who).Id;
                    this.ListArgs.Add(Tuple.Create(id, (ListEventArgs)args));
                }
                else
                {
                    int id = ((Base)sender).Id;
                    this.OtherArgs.Add(id);
                }
            }

            public readonly List<Tuple<int, PropertyChangedExEventArgs>> PropertyArgs = new List<Tuple<int, PropertyChangedExEventArgs>>();
            public readonly List<Tuple<int, ListEventArgs>> ListArgs = new List<Tuple<int, ListEventArgs>>();
            public readonly List<int> OtherArgs = new List<int>();
        }

        private class Base : AbsNotifyPropertyChanged
        {
            private static int currentId = 0;
            public readonly int Id = currentId++;

            public override string ToString()
            {
                return this.Id.ToString();
            }
        }

        private class C1 : Base
        {
            public C1()
            {
                this.listC2.Container = this;
            }

            #region Property C2

            public C2 C2
            {
                get { return this.c2; }
                set { this.Set("C2", this.c2, value, v => this.c2 = v); }
            }

            private C2 c2;

            #endregion

            #region List C2

            public EventList<C2> ListC2
            {
                get { return this.listC2; }
            }

            private readonly EventList<C2> listC2 = new EventList<C2>();

            #endregion

            #region Property V1

            public int V1
            {
                get { return this.v1; }
                set { this.Set("V1", this.v1, value, v => this.v1 = v); }
            }

            private int v1;

            #endregion

            #region Property V2

            public int V2
            {
                get { return this.v2; }
                set { this.Set("V2", this.v2, value, v => this.v2 = v); }
            }

            private int v2;

            #endregion

            #region Event Ev1

            public void OnEv1()
            {
                if (this.Ev1 != null)
                {
                    this.Ev1(this, EventArgs.Empty);
                }
            }

            public event EventHandler_v2<EventArgs> Ev1;

            #endregion
        }

        private class C2 : Base
        {
            public C2()
            {
                this.listC3.Container = this;
            }

            #region Property C3

            public C3 C3
            {
                get { return this.c3; }
                set { this.Set("C3", this.c3, value, v => this.c3 = v); }
            }

            private C3 c3;

            #endregion

            #region List C3

            public EventList<C3> ListC3
            {
                get { return this.listC3; }
            }

            private readonly EventList<C3> listC3 = new EventList<C3>();

            #endregion

            #region Property V1

            public int V1
            {
                get { return this.v1; }
                set { this.Set("V1", this.v1, value, v => this.v1 = v); }
            }

            private int v1;

            #endregion

            #region Property V2

            public int V2
            {
                get { return this.v2; }
                set { this.Set("V2", this.v2, value, v => this.v2 = v); }
            }

            private int v2;

            #endregion

            #region Event Ev1

            public void OnEv1()
            {
                if (this.Ev1 != null)
                {
                    this.Ev1(this, EventArgs.Empty);
                }
            }

            public event EventHandler_v2<EventArgs> Ev1;

            #endregion
        }

        private class C3 : Base
        {
            #region Property V1

            public int V1
            {
                get { return this.v1; }
                set { this.Set("V1", this.v1, value, v => this.v1 = v); }
            }

            private int v1;

            #endregion

            #region Property V2

            public int V2
            {
                get { return this.v2; }
                set { this.Set("V2", this.v2, value, v => this.v2 = v); }
            }

            private int v2;

            #endregion

            #region Event Ev1

            public void OnEv1()
            {
                if (this.Ev1 != null)
                {
                    this.Ev1(this, EventArgs.Empty);
                }
            }

            public event EventHandler_v2<EventArgs> Ev1;

            #endregion
        }
    }
}