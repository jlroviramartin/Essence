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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Essence.Util.Collections
{
    /// <summary>
    ///     Argumentos de los eventos de un diccionario.
    /// </summary>
    public class DictionaryEventArgs : CollectionEventArgs
    {
        public DictionaryEventArgs(CollectionChangeType changeType, object[] keys, object[] newItems, object[] oldItems = null)
            : base(changeType, newItems, oldItems)
        {
            this.Keys = keys;
        }

        /// <summary>Obtiene las claves.</summary>
        public object[] Keys { get; }

        #region ForEach

        public void ForEach(Action<object, object> insert,
                            Action<object, object> remove,
                            Action<object, object, object> change)
        {
            switch (this.ChangeType)
            {
                case CollectionChangeType.InsertItems:
                {
                    foreach (DictionaryEntry t in this.Items.Cast<DictionaryEntry>())
                    {
                        insert(t.Key, t.Value);
                    }
                    break;
                }
                case CollectionChangeType.RemoveItems:
                case CollectionChangeType.ClearItems:
                {
                    foreach (DictionaryEntry t in this.Items.Cast<DictionaryEntry>())
                    {
                        remove(t.Key, t.Value);
                    }
                    break;
                }
                case CollectionChangeType.ChangeItems:
                {
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        DictionaryEntry item = (DictionaryEntry)this.Items[i];
                        DictionaryEntry oldItem = (DictionaryEntry)this.OldItems[i];
                        change(item.Key, oldItem.Value, item.Value);
                    }
                    break;
                }
            }
        }

        public void ForEach(Action<object, object> insert,
                            Action<object, object> remove)
        {
            switch (this.ChangeType)
            {
                case CollectionChangeType.InsertItems:
                {
                    foreach (DictionaryEntry t in this.Items.Cast<DictionaryEntry>())
                    {
                        insert(t.Key, t.Value);
                    }
                    break;
                }
                case CollectionChangeType.RemoveItems:
                case CollectionChangeType.ClearItems:
                {
                    foreach (DictionaryEntry t in this.Items.Cast<DictionaryEntry>())
                    {
                        remove(t.Key, t.Value);
                    }
                    break;
                }
                case CollectionChangeType.ChangeItems:
                {
                    foreach (DictionaryEntry t in this.OldItems.Cast<DictionaryEntry>())
                    {
                        remove(t.Key, t.Value);
                    }

                    foreach (DictionaryEntry t in this.Items.Cast<DictionaryEntry>())
                    {
                        insert(t.Key, t.Value);
                    }
                    break;
                }
            }
        }

        public void ForEach<TK, TV>(Action<TK, TV> insert,
                                    Action<TK, TV> remove,
                                    Action<TK, TV, TV> change)
        {
            switch (this.ChangeType)
            {
                case CollectionChangeType.InsertItems:
                {
                    foreach (DictionaryEntry t in this.Items.Cast<DictionaryEntry>())
                    {
                        insert((TK)t.Key, (TV)t.Value);
                    }
                    break;
                }
                case CollectionChangeType.RemoveItems:
                case CollectionChangeType.ClearItems:
                {
                    foreach (DictionaryEntry t in this.Items.Cast<DictionaryEntry>())
                    {
                        remove((TK)t.Key, (TV)t.Value);
                    }
                    break;
                }
                case CollectionChangeType.ChangeItems:
                {
                    for (int i = 0; i < this.Items.Length; i++)
                    {
                        DictionaryEntry item = (DictionaryEntry)this.Items[i];
                        DictionaryEntry oldItem = (DictionaryEntry)this.OldItems[i];
                        change((TK)item.Key, (TV)oldItem.Value, (TV)item.Value);
                    }
                    break;
                }
            }
        }

        public void ForEach<TK, TV>(Action<TK, TV> insert,
                                    Action<TK, TV> remove)
        {
            switch (this.ChangeType)
            {
                case CollectionChangeType.InsertItems:
                {
                    foreach (DictionaryEntry t in this.Items.Cast<DictionaryEntry>())
                    {
                        insert((TK)t.Key, (TV)t.Value);
                    }
                    break;
                }
                case CollectionChangeType.RemoveItems:
                case CollectionChangeType.ClearItems:
                {
                    foreach (DictionaryEntry t in this.Items.Cast<DictionaryEntry>())
                    {
                        remove((TK)t.Key, (TV)t.Value);
                    }
                    break;
                }
                case CollectionChangeType.ChangeItems:
                {
                    foreach (DictionaryEntry t in this.OldItems.Cast<DictionaryEntry>())
                    {
                        remove((TK)t.Key, (TV)t.Value);
                    }

                    foreach (DictionaryEntry t in this.Items.Cast<DictionaryEntry>())
                    {
                        insert((TK)t.Key, (TV)t.Value);
                    }
                    break;
                }
            }
        }

        #endregion

        #region NewEvent

        public static DictionaryEventArgs NewEventChange<TK, TV>(TK key, TV newItem, TV oldItem)
        {
            return new DictionaryEventArgs(CollectionChangeType.ChangeItems, new object[] { key }, new object[] { newItem }, new object[] { oldItem });
        }

        public static DictionaryEventArgs NewEventAddRange<TK, TV>(IEnumerable<TK> keys, IEnumerable<TV> newItems)
        {
            return new DictionaryEventArgs(CollectionChangeType.InsertItems, keys.Cast<object>().ToArray(), newItems.Cast<object>().ToArray());
        }

        public static DictionaryEventArgs NewEventRemoveRange<TK, TV>(IEnumerable<TK> keys, IEnumerable<TV> oldItems)
        {
            return new DictionaryEventArgs(CollectionChangeType.RemoveItems, keys.Cast<object>().ToArray(), oldItems.Cast<object>().ToArray());
        }

        public static DictionaryEventArgs NewEventClear<TK, TV>(IEnumerable<TK> keys, IEnumerable<TV> oldItems)
        {
            return new DictionaryEventArgs(CollectionChangeType.ClearItems, keys.Cast<object>().ToArray(), oldItems.Cast<object>().ToArray());
        }

        #endregion NewEvent

        #region Object

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat("ChangeType={0}; ", Enum.GetName(typeof (CollectionChangeType), this.ChangeType))
                .AppendFormat("Keys={0}; ", this.Keys.ToStringEx())
                .AppendFormat("Values={0}", this.Items.ToStringEx())
                .ToString();
        }

        #endregion
    }
}