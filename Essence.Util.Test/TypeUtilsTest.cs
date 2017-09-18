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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Essence.Util
{
    [TestClass]
    public class TypeUtilsTest
    {
        [TestMethod]
        public void Test1()
        {
            Assert.IsTrue(TypeUtils.IsNull<int?>(new int?()));
            Assert.IsTrue(TypeUtils.IsNull<int?>(null));
            Assert.IsTrue(TypeUtils.IsNull<object>(null));

            Assert.IsFalse(TypeUtils.IsNull<int?>(new int?(5)));
            Assert.IsFalse(TypeUtils.IsNull<int?>(new int?(0)));
            Assert.IsFalse(TypeUtils.IsNull<int>(5));
            Assert.IsFalse(TypeUtils.IsNull<int>(0));
            Assert.IsFalse(TypeUtils.IsNull<object>(new object()));

            Assert.IsTrue(TypeUtils.NullAdmitted(typeof(int?)));
            Assert.IsTrue(TypeUtils.NullAdmitted(typeof(DayOfWeek?)));
            Assert.IsTrue(TypeUtils.NullAdmitted(typeof(object)));

            Assert.IsFalse(TypeUtils.NullAdmitted(typeof(int)));
            Assert.IsFalse(TypeUtils.NullAdmitted(typeof(DayOfWeek)));

            Assert.IsTrue(TypeUtils.IsNullable(typeof(int?)));
            Assert.IsTrue(TypeUtils.IsNullable(typeof(DayOfWeek?)));

            Assert.IsFalse(TypeUtils.IsNullable(typeof(int)));
            Assert.IsFalse(TypeUtils.IsNullable(typeof(DayOfWeek)));
            Assert.IsFalse(TypeUtils.IsNullable(typeof(object)));

            Assert.IsTrue(TypeUtils.IsNullableEnum(typeof(DayOfWeek?)));

            Assert.IsFalse(TypeUtils.IsNullableEnum(typeof(int?)));
            Assert.IsFalse(TypeUtils.IsNullableEnum(typeof(int)));
            Assert.IsFalse(TypeUtils.IsNullableEnum(typeof(DayOfWeek)));
            Assert.IsFalse(TypeUtils.IsNullableEnum(typeof(object)));
        }

        [TestMethod]
        public void Test2()
        {
            Assert.IsFalse(TypeUtils.NullAdmitted(typeof(Nullable<>)));
            Assert.IsFalse(TypeUtils.NullAdmitted(typeof(Comparison<>)));

            Assert.IsFalse(TypeUtils.IsNullable(typeof(Nullable<>)));
            Assert.IsFalse(TypeUtils.IsNullable(typeof(Comparison<>)));
        }

        public interface IA
        {
        }

        public class A : IA
        {
        }

        public class B : A
        {
        }

        [TestMethod]
        public void Test3()
        {
            Assert.AreEqual(typeof(A).GetInterfaces().Aggregate("", (a, b) => (!string.IsNullOrEmpty(a) ? (a + " ; ") : "") + b.Name), "IA");
            Assert.AreEqual(typeof(B).GetInterfaces().Aggregate("", (a, b) => (!string.IsNullOrEmpty(a) ? (a + " ; ") : "") + b.Name), "IA");
        }
    }
}