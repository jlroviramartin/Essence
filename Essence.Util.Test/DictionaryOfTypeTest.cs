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
using Essence.Util.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Essence.Util.Test
{
    [TestClass]
    public class DictionaryOfTypeTest
    {
        public interface IA
        {
        }

        public interface IB
        {
        }

        public interface IAA : IA
        {
        }

        public interface IAB : IA
        {
        }

        public interface IAAA : IAA
        {
        }

        public interface IAAB : IAA
        {
        }

        public class A : IA
        {
        }

        public class B : IB
        {
        }

        public class AA : A, IAA
        {
        }

        public class AB : A, IAB
        {
        }

        public class AAA : AA, IAAA
        {
        }

        public class AAB : AA, IAAB
        {
        }

        public struct SA : IA
        {
        }

        public struct SB : IB
        {
        }

        public struct SAA : IAA
        {
        }

        public struct SAB : IAB
        {
        }

        public struct SAAA : IAAA
        {
        }

        public struct SAAB : IAAB
        {
        }

        public enum E : int
        {
        }

        private static string ToString<T>(IEnumerable<T> enumer)
        {
            return enumer.Aggregate("", (a, b) => (!string.IsNullOrEmpty(a) ? (a + " ; ") : "") + b);
        }

        private static HashSet<T> ToSet<T>(IEnumerable<T> enumer)
        {
            return new HashSet<T>(enumer);
        }

        private static HashSet<T> ToSet<T>(params T[] enumer)
        {
            return new HashSet<T>(enumer);
        }

        private static void AreEqual<T>(ISet<T> a, ISet<T> b)
        {
            Assert.IsTrue(a.SetEquals(b));
        }

        [TestMethod]
        public void Test1()
        {
            DictionaryOfType<string> dictionary = new DictionaryOfType<string>();
            dictionary.Add(typeof (object), "object");
            dictionary.Add(typeof (int), "int");
            dictionary.Add(typeof (IA), "IA");
            dictionary.Add(typeof (IAA), "IAA");
            dictionary.Add(typeof (IAAA), "IAAA");

            AreEqual(ToSet(dictionary.Get(typeof (object))), ToSet("object"));

            AreEqual(ToSet(dictionary.Get(typeof (IA))), ToSet("IA"));
            AreEqual(ToSet(dictionary.Get(typeof (IB))), new HashSet<string>());
            AreEqual(ToSet(dictionary.Get(typeof (IAA))), ToSet("IAA"));
            AreEqual(ToSet(dictionary.Get(typeof (IAB))), ToSet("IA"));
            AreEqual(ToSet(dictionary.Get(typeof (IAAA))), ToSet("IAAA"));
            AreEqual(ToSet(dictionary.Get(typeof (IAAB))), ToSet("IAA", "IA"));

            AreEqual(ToSet(dictionary.Get(typeof (A))), ToSet("IA", "object"));
            AreEqual(ToSet(dictionary.Get(typeof (B))), ToSet("object"));
            AreEqual(ToSet(dictionary.Get(typeof (AA))), ToSet("IAA", "IA", "object"));
            AreEqual(ToSet(dictionary.Get(typeof (AB))), ToSet("IA", "object"));
            AreEqual(ToSet(dictionary.Get(typeof (AAA))), ToSet("IAAA", "IAA", "IA", "object"));
            AreEqual(ToSet(dictionary.Get(typeof (AAB))), ToSet("IAA", "IA", "object"));

            AreEqual(ToSet(dictionary.Get(typeof (SA))), ToSet("IA", "object"));
            AreEqual(ToSet(dictionary.Get(typeof (SB))), ToSet("object"));
            AreEqual(ToSet(dictionary.Get(typeof (SAA))), ToSet("IAA", "IA", "object"));
            AreEqual(ToSet(dictionary.Get(typeof (SAB))), ToSet("IA", "object"));
            AreEqual(ToSet(dictionary.Get(typeof (SAAA))), ToSet("IAAA", "IAA", "IA", "object"));
            AreEqual(ToSet(dictionary.Get(typeof (SAAB))), ToSet("IAA", "IA", "object"));

            AreEqual(ToSet(dictionary.Get(typeof (E))), ToSet("int"));
            AreEqual(ToSet(dictionary.Get(typeof (int))), ToSet("int"));
            AreEqual(ToSet(dictionary.Get(typeof (int?))), ToSet("int"));
        }

        [TestMethod]
        public void Test2()
        {
            DictionaryOfType<string> dictionary = new DictionaryOfType<string>();
            dictionary.Add(typeof (object), "object");
            dictionary.Add(typeof (int), "int");
            dictionary.Add(typeof (IA), "IA");
            dictionary.Add(typeof (IAA), "IAA");
            dictionary.Add(typeof (IAAA), "IAAA");

            dictionary.Add(typeof (A), "A");

            AreEqual(ToSet(dictionary.Get(typeof (object))), ToSet("object"));

            AreEqual(ToSet(dictionary.Get(typeof (IA))), ToSet("IA"));
            AreEqual(ToSet(dictionary.Get(typeof (IB))), new HashSet<string>());
            AreEqual(ToSet(dictionary.Get(typeof (IAA))), ToSet("IAA"));
            AreEqual(ToSet(dictionary.Get(typeof (IAB))), ToSet("IA"));
            AreEqual(ToSet(dictionary.Get(typeof (IAAA))), ToSet("IAAA"));
            AreEqual(ToSet(dictionary.Get(typeof (IAAB))), ToSet("IAA", "IA"));

            AreEqual(ToSet(dictionary.Get(typeof (A))), ToSet("A"));
            AreEqual(ToSet(dictionary.Get(typeof (B))), ToSet("object"));
            AreEqual(ToSet(dictionary.Get(typeof (AA))), ToSet("IAA", "IA", "A"));
            AreEqual(ToSet(dictionary.Get(typeof (AB))), ToSet("IA", "A"));
            AreEqual(ToSet(dictionary.Get(typeof (AAA))), ToSet("IAAA", "IAA", "IA", "A"));
            AreEqual(ToSet(dictionary.Get(typeof (AAB))), ToSet("IAA", "IA", "A"));

            AreEqual(ToSet(dictionary.Get(typeof (SA))), ToSet("IA", "object"));
            AreEqual(ToSet(dictionary.Get(typeof (SB))), ToSet("object"));
            AreEqual(ToSet(dictionary.Get(typeof (SAA))), ToSet("IAA", "IA", "object"));
            AreEqual(ToSet(dictionary.Get(typeof (SAB))), ToSet("IA", "object"));
            AreEqual(ToSet(dictionary.Get(typeof (SAAA))), ToSet("IAAA", "IAA", "IA", "object"));
            AreEqual(ToSet(dictionary.Get(typeof (SAAB))), ToSet("IAA", "IA", "object"));

            AreEqual(ToSet(dictionary.Get(typeof (E))), ToSet("int"));
            AreEqual(ToSet(dictionary.Get(typeof (int))), ToSet("int"));
            AreEqual(ToSet(dictionary.Get(typeof (int?))), ToSet("int"));
        }
    }
}