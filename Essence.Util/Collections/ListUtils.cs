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

namespace Essence.Util.Collections
{
    /// <summary>
    ///     Utilidades sobre listas.
    /// </summary>
    public static class ListUtils
    {
        #region IList<T>

        /// <summary>
        ///     Inserta todos los elementos de un enumerable a una lista, en la posicion indicada.
        /// </summary>
        /// <typeparam name="T">Tipo de elementos de la lista.</typeparam>
        /// <param name="list">Lista.</param>
        /// <param name="index">Indice.</param>
        /// <param name="enumer">Enumerable.</param>
        public static void InsertAll<T>(this IList<T> list, int index, IEnumerable<T> enumer)
        {
            if (list is List<T>)
            {
                ((List<T>)list).InsertRange(index, enumer);
            }
            else if (list is AbsList<T>)
            {
                ((AbsList<T>)list).InsertAll(index, enumer);
            }
            else
            {
                foreach (T t in enumer)
                {
                    list.Insert(index++, t);
                }
            }
        }

        /// <summary>
        ///     Elimina todos los elementos indicados de una lista.
        /// </summary>
        /// <typeparam name="T">Tipo de elementos de la lista.</typeparam>
        /// <param name="list">Lista.</param>
        /// <param name="index">Indice.</param>
        /// <param name="count">Numero de elementos.</param>
        public static void RemoveAll<T>(this IList<T> list, int index, int count)
        {
            if (list is List<T>)
            {
                ((List<T>)list).RemoveRange(index, count);
            }
            else if (list is AbsList<T>)
            {
                ((AbsList<T>)list).RemoveAll(index, count);
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    list.RemoveAt(index);
                }
            }
        }

        public static void RemoveAll<T>(this IList<T> list, IEnumerable<T> enumer)
        {
            if (list is AbsList<T>)
            {
                ((AbsList<T>)list).RemoveAll(enumer);
            }
            else
            {
                foreach (T item in enumer)
                {
                    list.Remove(item);
                }
            }
        }

        /// <summary>
        ///     Cambia dos elementos de la lista.
        /// </summary>
        public static void Swap<T>(this IList<T> items, int i, int j)
        {
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }

        /// <summary>
        ///     Da la vuelta a la lista (espejo).
        /// </summary>
        public static void Reverse<T>(this IList<T> input)
        {
            if (input is List<T>)
            {
                ((List<T>)input).Reverse();
            }
            else if (input is T[])
            {
                Array.Reverse((T[])input);
            }
            else
            {
                for (int i = 0; i < input.Count / 2; i++)
                {
                    input.Swap(i, input.Count - 1 - i);
                }
            }
        }

        public static bool IsEmpty<T>(this IList<T> list)
        {
            return (list.Count == 0);
        }

        /// <summary>
        ///     Obtiene el primer elemento de una lista.
        /// </summary>
        /// <typeparam name="T">Tipo de elementos.</typeparam>
        /// <param name="list">Lista.</param>
        /// <returns>Primer elemento.</returns>
        public static T First<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new IndexOutOfRangeException();
            }
            return list[0];
        }

        /// <summary>
        ///     Obtiene el ultimo elemento de una lista.
        /// </summary>
        /// <typeparam name="T">Tipo de elementos.</typeparam>
        /// <param name="list">Lista.</param>
        /// <returns>Ultimo elemento.</returns>
        public static T Last<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new IndexOutOfRangeException();
            }
            return list[list.Count - 1];
        }

        #endregion

        #region BinarySearch

        /// <summary>
        ///     Busca la posicion de <c>value</c> en la lista <c>list</c>.
        ///     Si hay multiples ocurrencias, devuelve la primera ocurrencia.
        ///     Si no existe, se devuelve el complemento del indice del elemento mayor mas cercano.
        ///     Si el elemento mayor mas cercano no existe, devuelve el complemento del indice ultimo+1.
        ///     Permite la insercion ordenada de elementos.
        ///     http://www.cs.man.ac.uk/~pt/algorithms/binary.html
        ///     http://en.wikipedia.org/wiki/Binary_search_algorithm
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="list">Lista.</param>
        /// <param name="value">Valor a buscar.</param>
        /// <param name="compare">Comparacion.</param>
        /// <returns>Posicion del elemento.</returns>
        /// <code><![CDATA[
        ///  Compare<double, double> c = (a, b) => a.EpsilonCompareTo(b, 0.5);
        ///  Action<IList<double>, int, int> Write = (lista, index, count) =>
        ///  {
        ///      Console.Write("[ ");
        ///      for (int j = 0; j < count; j++)
        ///      {
        ///          if (j > 0)
        ///          {
        ///          /// Console.Write("; ");
        ///          }
        ///          Console.Write(lista[index + j] + " ");
        ///      }
        ///      Console.WriteLine(" ]");
        ///  };
        ///  Action<IList<double>, double, int, int> Test = (vs, vv, index, count) =>
        ///  {
        ///      int ii = vs.BinarySearch(vv, c, index, count);
        ///      if (ii < 0)
        ///      {
        ///          ii = ~ii;
        /// 
        ///          Console.Write("No encontrado " + vv + ". ");
        ///          if (ii < index)
        ///          {
        ///          /// Console.WriteLine("Menor que " + vs[index] + ".");
        ///          }
        ///          else if (ii >= index + count)
        ///          {
        ///          /// Console.WriteLine("Mayor que " + vs[index + count - 1] + ".");
        ///          }
        ///          else
        ///          {
        ///          /// Console.WriteLine("Elemento superior " + vs[ii] + " (" + ii + ").");
        ///          }
        ///      }
        ///      else
        ///      {
        ///          Console.Write("Encontrado " + vv + ". ");
        ///          Console.WriteLine("Elemento " + vs[ii] + " (" + ii + ").");
        ///      }
        ///  };
        /// 
        ///  {
        ///      List<double> valores = new List<double> { 0, 10, 10.1, 20, 20.1, 30 };
        ///      int i = 1;
        ///      int cn = valores.Count - 2;
        ///      Write(valores, i, cn);
        ///      Test(valores, 0, i, cn);
        ///      Test(valores, 5, i, cn);
        ///      Test(valores, 10, i, cn);
        ///      Test(valores, 10.1, i, cn);
        ///      Test(valores, 10.5, i, cn);
        ///      Test(valores, 10.6, i, cn);
        ///      Test(valores, 15, i, cn);
        ///      Test(valores, 20, i, cn);
        ///      Test(valores, 25, i, cn);
        ///      Test(valores, 30, i, cn);
        ///  }
        ///  ]]></code>
        public static int BinarySearch<T1, T2>(this IList<T1> list, T2 value, Func<T1, T2, int> compare,
                                               int index, int count)
        {
            //Constraints.Requires(count > 0);
            if (count == 0)
            {
                return ~0;
            }

            int bot = index;
            int top = index + count - 1;

            int comp = compare(list[bot], value);

            if (comp == 0)
            {
                return index;
            }

            if (comp > 0)
            {
                return ~index;
            }

            comp = compare(list[top], value);

            // NOTA: dejamos que siga buscando hasta encontrar el de menor indice.
            /*if (comp == 0)
            {
                return index + count - 1;
            }*/

            if (comp < 0)
            {
                return ~(index + count);
            }

            while (bot < top)
            {
                int mid = bot + (top - bot) / 2;

                comp = compare(list[mid], value);

                // NOTA: dejamos que siga buscando hasta encontrar el de menor indice.
                /*if (comp == 0)
                {
                    return mid;
                }*/

                if (comp < 0)
                {
                    bot = mid + 1;
                }
                else
                {
                    top = mid;
                }
            }

            if ((bot == top) && compare(list[bot], value) == 0)
            {
                return top;
            }

            top = ~top;

            return top;
        }

        /// <summary>
        ///     Busca la posicion de <c>value</c> en la lista <c>list</c>.
        ///     Si hay multiples ocurrencias, devuelve la primera ocurrencia.
        ///     Si no existe, se devuelve el complemento del indice del elemento mayor mas cercano.
        ///     Si el elemento mayor mas cercano no existe, devuelve el complemento del indice ultimo+1.
        ///     Permite la insercion ordenada de elementos.
        ///     http://www.cs.man.ac.uk/~pt/algorithms/binary.html
        ///     http://en.wikipedia.org/wiki/Binary_search_algorithm
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="list">Lista.</param>
        /// <param name="value">Valor a buscar.</param>
        /// <param name="compare">Comparacion.</param>
        /// <param name="exactMatch">Indica si ha encontrado el elemento.</param>
        /// <returns>Posicion del elemento.</returns>
        /// <code><![CDATA[
        ///  Compare<double, double> c = (a, b) => a.EpsilonCompareTo(b, 0.5);
        ///  Action<IList<double>, int, int> Write = (lista, index, count) =>
        ///  {
        ///      Console.Write("[ ");
        ///      for (int j = 0; j < count; j++)
        ///      {
        ///          if (j > 0)
        ///          {
        ///              /// Console.Write("; ");
        ///          }
        ///          Console.Write(lista[index + j] + " ");
        ///      }
        ///      Console.WriteLine(" ]");
        ///  };
        ///  Action<IList<double>, double, int, int> Test = (vs, vv, index, count) =>
        ///  {
        ///      int ii = vs.BinarySearch(vv, c, index, count);
        ///      if (ii < 0)
        ///      {
        ///          ii = ~ii;
        /// 
        ///          Console.Write("No encontrado " + vv + ". ");
        ///          if (ii < index)
        ///          {
        ///              /// Console.WriteLine("Menor que " + vs[index] + ".");
        ///          }
        ///          else if (ii >= index + count)
        ///          {
        ///              /// Console.WriteLine("Mayor que " + vs[index + count - 1] + ".");
        ///          }
        ///          else
        ///          {
        ///              /// Console.WriteLine("Elemento superior " + vs[ii] + " (" + ii + ").");
        ///          }
        ///      }
        ///      else
        ///      {
        ///          Console.Write("Encontrado " + vv + ". ");
        ///          Console.WriteLine("Elemento " + vs[ii] + " (" + ii + ").");
        ///      }
        ///  };
        /// 
        ///  {
        ///      List<double> valores = new List<double> { 0, 10, 10.1, 20, 20.1, 30 };
        ///      int i = 1;
        ///      int cn = valores.Count - 2;
        ///      Write(valores, i, cn);
        ///      Test(valores, 0, i, cn);
        ///      Test(valores, 5, i, cn);
        ///      Test(valores, 10, i, cn);
        ///      Test(valores, 10.1, i, cn);
        ///      Test(valores, 10.5, i, cn);
        ///      Test(valores, 10.6, i, cn);
        ///      Test(valores, 15, i, cn);
        ///      Test(valores, 20, i, cn);
        ///      Test(valores, 25, i, cn);
        ///      Test(valores, 30, i, cn);
        ///  }
        ///  ]]></code>
        public static int BinarySearch<T1, T2>(this IList<T1> list, T2 value, Func<T1, T2, int> compare)
        {
            return BinarySearch(list, value, compare, 0, list.Count);
        }

        /// <summary>
        ///     Busca la posicion de <c>value</c> en la lista <c>list</c>.
        ///     Si hay multiples ocurrencias, devuelve la primera ocurrencia.
        ///     Si no existe, se devuelve el complemento del indice del elemento mayor mas cercano.
        ///     Si el elemento mayor mas cercano no existe, devuelve el complemento del indice ultimo+1.
        ///     Permite la insercion ordenada de elementos.
        ///     http://www.cs.man.ac.uk/~pt/algorithms/binary.html
        ///     http://en.wikipedia.org/wiki/Binary_search_algorithm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">Lista.</param>
        /// <param name="value">Valor a buscar.</param>
        /// <param name="compare">Comparacion.</param>
        /// <param name="exactMatch">Indica si ha encontrado el elemento.</param>
        /// <returns>Posicion del elemento.</returns>
        public static int BinarySearch<T>(this IList<T> list, T value, Func<T, T, int> compare,
                                          int index, int count)
        {
            //Constraints.Requires(count > 0);
            if (count == 0)
            {
                return ~0;
            }

            int bot = index;
            int top = index + count - 1;

            int comp = compare(list[bot], value);

            if (comp == 0)
            {
                return index;
            }

            if (comp > 0)
            {
                return ~index;
            }

            comp = compare(list[top], value);

            // NOTA: dejamos que siga buscando hasta encontrar el de menor indice.
            /*if (comp == 0)
            {
                return index + count - 1;
            }*/

            if (comp < 0)
            {
                return ~(index + count);
            }

            while (bot < top)
            {
                int mid = bot + (top - bot) / 2;

                comp = compare(list[mid], value);

                // NOTA: dejamos que siga buscando hasta encontrar el de menor indice.
                /*if (comp == 0)
                {
                    return mid;
                }*/

                if (comp < 0)
                {
                    bot = mid + 1;
                }
                else
                {
                    top = mid;
                }
            }

            if ((bot == top) && compare(list[bot], value) == 0)
            {
                return top;
            }

            top = ~top;

            return top;
        }

        public static int BinarySearch<T>(this IList<T> list, T value, Func<T, T, int> compare)
        {
            return BinarySearch<T>(list, value, compare, 0, list.Count);
        }

        public static int BinarySearch<T>(this IList<T> list, T value, int index, int count)
            where T : IComparable<T>
        {
            return BinarySearch<T>(list, value, (x, y) => x.CompareTo(y), index, count);
        }

        public static int BinarySearch<T>(this IList<T> list, T value)
            where T : IComparable<T>
        {
            return BinarySearch<T>(list, value, (x, y) => x.CompareTo(y), 0, list.Count);
        }

        #endregion

        #region QuickSort

        /// <summary>
        ///     Aplica una ordenacion quicksort sobre la lista <c>input</c>.
        /// </summary>
        /// <param name="input">Lista a ordenar.</param>
        /// <param name="compare">Comparador.</param>
        public static void QuickSort<T>(this IList<T> input, Func<T, T, int> compare)
        {
            if (input.Count == 0)
            {
                return;
            }
            QuickSort(input, 0, input.Count - 1, compare);
        }

        public static void QuickSort<T>(this IList<T> input, Func<T, T, int> compare, int index, int count)
        {
            if (input.Count == 0)
            {
                return;
            }
            QuickSort(input, index, count - 1, compare);
        }

        /// <summary>
        ///     Aplica una ordenacion quicksort sobre la lista <c>input</c>.
        /// </summary>
        /// <param name="input">Lista a ordenar.</param>
        /// <param name="comparer">Comparador.</param>
        public static void QuickSort<T>(this IList<T> input, IComparer<T> comparer)
        {
            if (input.Count == 0)
            {
                return;
            }
            QuickSort(input, 0, input.Count - 1, comparer.Compare);
        }

        public static void QuickSort<T>(this IList<T> input, IComparer<T> comparer, int index, int count)
        {
            if (input.Count == 0)
            {
                return;
            }
            QuickSort(input, index, count - 1, comparer.Compare);
        }

        /// <summary>
        ///     Aplica una ordenacion quicksort sobre la lista <c>input</c>.
        /// </summary>
        /// <param name="input">Lista a ordenar.</param>
        public static void QuickSort<T>(this IList<T> input)
            where T : IComparable
        {
            if (input.Count == 0)
            {
                return;
            }
            Func<T, T, int> compare = (a, b) => a.CompareTo(b);
            QuickSort(input, 0, input.Count - 1, compare);
        }

        #endregion

        #region private

        private static void QuickSort<T>(IList<T> input, int beg, int end, Func<T, T, int> compare)
        {
            if (end == beg)
            {
                return;
            }
            int pivot = GetPivotPoint(input, beg, end, compare);
            if (pivot > beg)
            {
                QuickSort(input, beg, pivot - 1, compare);
            }
            if (pivot < end)
            {
                QuickSort(input, pivot + 1, end, compare);
            }
        }

        private static int GetPivotPoint<T>(IList<T> input, int begPoint, int endPoint, Func<T, T, int> compare)
        {
            int pivot = begPoint;
            int m = begPoint + 1;
            int n = endPoint;

            /// TODO: terminar el puñetero programa antes del 2300

            while ((m < endPoint) && (compare(input[pivot], input[m]) >= 0))
            {
                m++;
            }

            while ((n > begPoint) && (compare(input[pivot], input[n]) <= 0))
            {
                n--;
            }

            while (m < n)
            {
                Swap(input, m, n);

                while ((m < endPoint) && (compare(input[pivot], input[m]) >= 0))
                {
                    m++;
                }

                while ((n > begPoint) && (compare(input[pivot], input[n]) <= 0))
                {
                    n--;
                }
            }

            if (pivot != n)
            {
                Swap(input, n, pivot);
            }

            return n;
        }

        #endregion
    }
}