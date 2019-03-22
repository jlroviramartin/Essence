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
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace Essence.Util.Reflection
{
    public class TypeUtils
    {
        /* Ejemplo de uso
        public class Test
        {
            public void Method1(int a, int b)
            {
            }

            public static double Method2(float a)
            {
                return 0.0;
            }

            public long Property1 { get; set; }

            public static object Property2 { get; set; }

            public long Field1;

            public static object Field2;
        }

        {
            MethodInfo mthInfo1 = TypeUtils.FindMethod<Test>(t => t.Method1(0, 0));
            MethodInfo mthInfo2 = TypeUtils.FindMethod(() => Test.Method2((float)0));

            MemberInfo mInfo1 = TypeUtils.FindProperty<Test>(t => t.Property1);
            MemberInfo mInfo2 = TypeUtils.FindProperty(() => Test.Property2);

            MemberInfo mInfo3 = TypeUtils.FindProperty<Test>(t => t.Field1);
            MemberInfo mInfo4 = TypeUtils.FindProperty(() => Test.Field2);
        }
        */

        public static string FindMethodName<T>(Expression<Action<T>> exprTree)
        {
            MethodInfo mth = FindMethod(exprTree);
            if (mth != null)
            {
                return mth.Name;
            }
            return null;
        }

        public static string FindMethodName(Expression<Action> exprTree)
        {
            MethodInfo mth = FindMethod(exprTree);
            if (mth != null)
            {
                return mth.Name;
            }
            return null;
        }

        public static string FindPropertyName<T>(Expression<Func<T, object>> exprTree)
        {
            MemberInfo mth = FindProperty(exprTree);
            if (mth != null)
            {
                return mth.Name;
            }
            return null;
        }

        public static string FindPropertyName(Expression<Func<object>> exprTree)
        {
            MemberInfo mth = FindProperty(exprTree);
            if (mth != null)
            {
                return mth.Name;
            }
            return null;
        }

        /// <summary>
        /// Finds the MethodInfo of a method without using reflection over its name.
        /// </summary>
        /// <param name="templateExprTree">Expression tree used as template.</param>
        /// <returns>Method definition.</returns>
        public static MethodInfo FindMethod<T>(Expression<Action<T>> templateExprTree)
        {
            return FindMethod((LambdaExpression)templateExprTree);
        }

        /// <summary>
        /// Finds the MethodInfo of a method without using reflection over its name.
        /// </summary>
        /// <param name="templateExprTree">Expression tree used as template.</param>
        /// <returns>Method definition.</returns>
        public static MethodInfo FindMethod(Expression<Action> templateExprTree)
        {
            return FindMethod((LambdaExpression)templateExprTree);
        }

        /// <summary>
        /// Finds the MethodInfo definition of a method without using reflection over its name.
        /// </summary>
        /// <param name="templateExprTree">Expression tree used as template.</param>
        /// <returns>Method definition.</returns>
        public static MethodInfo FindMethodDefinition(Expression<Action> templateExprTree)
        {
            MethodInfo methodInfo = FindMethod((LambdaExpression)templateExprTree);
            Contract.Assert(methodInfo.IsGenericMethod);
            return methodInfo.GetGenericMethodDefinition();
        }

        public static MemberInfo FindProperty<T>(Expression<Func<T, object>> exprTree)
        {
            return FindProperty((LambdaExpression)exprTree);
        }

        public static MemberInfo FindProperty(Expression<Func<object>> exprTree)
        {
            return FindProperty((LambdaExpression)exprTree);
        }

        private static MethodInfo FindMethod(LambdaExpression exprTree)
        {
            MethodCallExpression mthCallExpression = exprTree.Body as MethodCallExpression;
            if (mthCallExpression != null)
            {
                return mthCallExpression.Method;
            }
            return null;
        }

        private static MemberInfo FindProperty(LambdaExpression exprTree)
        {
            MemberExpression memberExpression = exprTree.Body as MemberExpression;
            if (memberExpression != null)
            {
                return memberExpression.Member;
            }
            UnaryExpression unaryExpression = exprTree.Body as UnaryExpression; // Puede haber un casting..
            if (unaryExpression != null)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
                if (memberExpression != null)
                {
                    return memberExpression.Member;
                }
            }
            return null;
        }
    }
}