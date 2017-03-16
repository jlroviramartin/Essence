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
using System.Linq;
using System.Text;

namespace Essence.Util.Collections
{
    /// <summary>
    ///     Argumentos de los eventos de una colección.
    /// </summary>
    public class CollectionEventArgs : EventArgs
    {
        protected CollectionEventArgs(CollectionChangeType changeType, object[] newItems, object[] oldItems = null)
        {
            this.ChangeType = changeType;
            this.Items = newItems;
            this.OldItems = oldItems ?? emptyArray;
        }

        public bool IsEmpty
        {
            get { return (this.Items.Length == 0) && (this.OldItems.Length == 0); }
        }

        /// <summary>
        ///     Obtiene el objeto que contiene la lista. Puede ser <c>null</c> si no se indica.
        /// </summary>
        public object Who { get; set; }

        /// <summary>
        ///     Obtiene el tipo de cambio que se ha producido.
        /// </summary>
        public CollectionChangeType ChangeType { get; }

        /// <summary>
        ///     Obtiene los elementos.
        /// </summary>
        public object[] Items { get; }

        /// <summary>
        ///     Obtiene los elementos antiguos (solo para ChangeItems).
        /// </summary>
        public object[] OldItems { get; }

        #region ForEach

        public void ForEach(Action<object> insert,
                            Action<object> remove,
                            Action<object, object> change)
        {
            switch (this.ChangeType)
            {
                case CollectionChangeType.InsertItems:
                {
                    foreach (object t in this.Items)
                    {
                        insert(t);
                    }
                    break;
                }
                case CollectionChangeType.RemoveItems:
                case CollectionChangeType.ClearItems:
                {
                    foreach (object t in this.Items)
                    {
                        remove(t);
                    }
                    break;
                }
                case CollectionChangeType.ChangeItems:
                {
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        change(this.OldItems[i], this.Items[i]);
                    }
                    break;
                }
            }
        }

        public void ForEach(Action<object> insert,
                            Action<object> remove)
        {
            switch (this.ChangeType)
            {
                case CollectionChangeType.InsertItems:
                {
                    foreach (object t in this.Items)
                    {
                        insert(t);
                    }
                    break;
                }
                case CollectionChangeType.RemoveItems:
                case CollectionChangeType.ClearItems:
                {
                    foreach (object t in this.Items)
                    {
                        remove(t);
                    }
                    break;
                }
                case CollectionChangeType.ChangeItems:
                {
                    foreach (object t in this.OldItems)
                    {
                        remove(t);
                    }

                    foreach (object t in this.Items)
                    {
                        insert(t);
                    }
                    break;
                }
            }
        }

        public void ForEach<T>(Action<T> insert,
                               Action<T> remove,
                               Action<T, object> change)
        {
            switch (this.ChangeType)
            {
                case CollectionChangeType.InsertItems:
                {
                    foreach (object t in this.Items)
                    {
                        insert((T)t);
                    }
                    break;
                }
                case CollectionChangeType.RemoveItems:
                case CollectionChangeType.ClearItems:
                {
                    foreach (object t in this.Items)
                    {
                        remove((T)t);
                    }
                    break;
                }
                case CollectionChangeType.ChangeItems:
                {
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        change((T)this.OldItems[i], this.Items[i]);
                    }
                    break;
                }
            }
        }

        public void ForEach<T>(Action<T> insert,
                               Action<T> remove)
        {
            switch (this.ChangeType)
            {
                case CollectionChangeType.InsertItems:
                {
                    foreach (object t in this.Items)
                    {
                        insert((T)t);
                    }
                    break;
                }
                case CollectionChangeType.RemoveItems:
                case CollectionChangeType.ClearItems:
                {
                    foreach (object t in this.Items)
                    {
                        remove((T)t);
                    }
                    break;
                }
                case CollectionChangeType.ChangeItems:
                {
                    foreach (object t in this.OldItems)
                    {
                        remove((T)t);
                    }

                    foreach (object t in this.Items)
                    {
                        insert((T)t);
                    }
                    break;
                }
            }
        }

        #endregion

        #region NewEvent

        public static CollectionEventArgs NewEventAddRange<T>(IEnumerable<T> newItems)
        {
            return new CollectionEventArgs(CollectionChangeType.InsertItems, newItems.Cast<object>().ToArray());
        }

        public static CollectionEventArgs NewEventRemoveRange<T>(IEnumerable<T> oldItems)
        {
            return new CollectionEventArgs(CollectionChangeType.RemoveItems, oldItems.Cast<object>().Reverse().ToArray());
        }

        public static CollectionEventArgs NewEventClear<T>(IEnumerable<T> oldItems)
        {
            return new CollectionEventArgs(CollectionChangeType.RemoveItems, oldItems.Cast<object>().Reverse().ToArray());
        }

        #endregion

        #region private

        private static readonly object[] emptyArray = new object[0];

        #endregion

        #region Object

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat("ChangeType={0}; ", Enum.GetName(typeof (CollectionChangeType), this.ChangeType))
                .AppendFormat("Items={0}", this.Items.ToStringEx())
                .ToString();
        }

        #endregion
    }
}