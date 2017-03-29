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
using System.Linq;
using System.Text;

namespace Essence.Util.Collections
{
    /// <summary>
    ///     Argumentos de los eventos de una lista.
    /// </summary>
    public class ListEventArgs : CollectionEventArgs
    {
        protected ListEventArgs(CollectionChangeType changeType, int[] indexes, object[] newItems, object[] oldItems = null)
            : base(changeType, newItems, oldItems)
        {
            this.Indices = indexes;
        }

        /// <summary>
        ///     Obtiene los indices de los elementos que han cambiado.
        ///     - Insert : cuales son los indices que se han insertado (despues de insertar => como
        ///     queda el resultado final).
        ///     - Remove : cuales son los indices que se han eliminado (antes de eliminar => como
        ///     era el resultado previo).
        ///     - Change : cuales son los indices que se han modificado.
        /// </summary>
        public int[] Indices { get; }

        #region ForEach

        public void ForEach(Action<object, int> insert,
                            Action<object, int> remove,
                            Action<object, object, int> change)
        {
            switch (this.ChangeType)
            {
                case CollectionChangeType.InsertItems:
                {
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        insert(this.Items[i], this.Indices[i]);
                    }
                    break;
                }
                case CollectionChangeType.RemoveItems:
                case CollectionChangeType.ClearItems:
                {
                    // Se supone que ya esta en orden descendente.
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        remove(this.Items[i], this.Indices[i]);
                    }
                    break;
                }
                case CollectionChangeType.ChangeItems:
                {
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        change(this.OldItems[i], this.Items[i], this.Indices[i]);
                    }
                    break;
                }
            }
        }

        public void ForEach(Action<object, int> insert,
                            Action<object, int> remove)
        {
            switch (this.ChangeType)
            {
                case CollectionChangeType.InsertItems:
                {
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        insert(this.Items[i], this.Indices[i]);
                    }
                    break;
                }
                case CollectionChangeType.RemoveItems:
                case CollectionChangeType.ClearItems:
                {
                    // Se supone que ya esta en orden descendente.
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        remove(this.Items[i], this.Indices[i]);
                    }
                    break;
                }
                case CollectionChangeType.ChangeItems:
                {
                    // Se supone que ya esta en orden descendente.
                    for (int i = 0; i < this.OldItems.Length; i++)
                    {
                        remove(this.OldItems[i], this.Indices[i]);
                    }

                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        insert(this.Items[i], this.Indices[i]);
                    }
                    break;
                }
            }
        }

        public void ForEach<T>(Action<T, int> insert,
                               Action<T, int> remove,
                               Action<T, object, int> change)
        {
            switch (this.ChangeType)
            {
                case CollectionChangeType.InsertItems:
                {
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        insert((T)this.Items[i], this.Indices[i]);
                    }
                    break;
                }
                case CollectionChangeType.RemoveItems:
                case CollectionChangeType.ClearItems:
                {
                    // Se supone que ya esta en orden descendente.
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        remove((T)this.Items[i], this.Indices[i]);
                    }
                    break;
                }
                case CollectionChangeType.ChangeItems:
                {
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        change((T)this.OldItems[i], this.Items[i], this.Indices[i]);
                    }
                    break;
                }
            }
        }

        public void ForEach<T>(Action<T, int> insert,
                               Action<T, int> remove)
        {
            switch (this.ChangeType)
            {
                case CollectionChangeType.InsertItems:
                {
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        insert((T)this.Items[i], this.Indices[i]);
                    }
                    break;
                }
                case CollectionChangeType.RemoveItems:
                case CollectionChangeType.ClearItems:
                {
                    // Se supone que ya esta en orden descendente.
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        remove((T)this.Items[i], this.Indices[i]);
                    }
                    break;
                }
                case CollectionChangeType.ChangeItems:
                {
                    // Se supone que ya esta en orden descendente.
                    for (int i = 0; i < this.OldItems.Length; i++)
                    {
                        remove((T)this.OldItems[i], this.Indices[i]);
                    }

                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        insert((T)this.Items[i], this.Indices[i]);
                    }
                    break;
                }
            }
        }

        #endregion

        #region NewEvent

        public static ListEventArgs NewEventChange<T>(int index, T newItem, T oldItem)
        {
            return new ListEventArgs(CollectionChangeType.ChangeItems, new int[] { index }, new object[] { newItem }, new object[] { oldItem });
        }

        public static ListEventArgs NewEventAddRange<T>(int index, IEnumerable<T> newItems)
        {
            // Se prepara el evento.
            List<object> items = new List<object>();
            List<int> indices = new List<int>();
            int i = index;

            foreach (T t in newItems)
            {
                items.Add(t);
                indices.Add(i++);
            }

            return new ListEventArgs(CollectionChangeType.InsertItems, indices.ToArray(), items.ToArray());
        }

        public static ListEventArgs NewEventAddRange<T>(IEnumerable<int> indices, IEnumerable<T> newItems)
        {
            return new ListEventArgs(CollectionChangeType.InsertItems, indices.ToArray(), newItems.Cast<object>().ToArray());
        }

        public static ListEventArgs NewEventRemoveRange<T>(int index, IEnumerable<T> oldItems)
        {
            // Se prepara el evento.
            List<object> items = new List<object>();
            List<int> indices = new List<int>();
            int i = index;

            foreach (T t in oldItems)
            {
                items.Add(t);
                indices.Add(i++);
            }
            items.Reverse();
            indices.Reverse();

            return new ListEventArgs(CollectionChangeType.RemoveItems, indices.ToArray(), items.ToArray());
        }

        public static ListEventArgs NewEventRemoveRange<T>(IEnumerable<int> indices, IEnumerable<T> oldItems)
        {
            return new ListEventArgs(CollectionChangeType.RemoveItems, indices.ToArray(), oldItems.Cast<object>().ToArray());
        }

        public new static ListEventArgs NewEventClear<T>(IEnumerable<T> oldItems)
        {
            // Se prepara el evento.
            List<object> items = new List<object>();
            List<int> indices = new List<int>();
            int i = 0;

            foreach (T t in oldItems)
            {
                items.Add(t);
                indices.Add(i++);
            }
            items.Reverse();
            indices.Reverse();

            return new ListEventArgs(CollectionChangeType.ClearItems, indices.ToArray(), items.ToArray());
        }

        #endregion NewEvent

        #region Object

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat("ChangeType={0}; ", Enum.GetName(typeof(CollectionChangeType), this.ChangeType))
                .AppendFormat("Indexes={0}; ", this.Indices.ToStringEx())
                .AppendFormat("Items={0}", this.Items.ToStringEx())
                .ToString();
        }

        #endregion
    }
}