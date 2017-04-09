/// Apache Commons Math 3.6.1
using System.Collections.Generic;

/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace org.apache.commons.math3.exception.util
{


    /// <summary>
    /// Utility class for transforming the list of arguments passed to
    /// constructors of exceptions.
    /// 
    /// </summary>
    public class ArgUtils
    {
        /// <summary>
        /// Class contains only static methods.
        /// </summary>
        private ArgUtils()
        {
        }

        /// <summary>
        /// Transform a multidimensional array into a one-dimensional list.
        /// </summary>
        /// <param name="array"> Array (possibly multidimensional). </param>
        /// <returns> a list of all the {@code Object} instances contained in
        /// {@code array}. </returns>
        public static object[] Flatten(object[] array)
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<Object> list = new java.util.ArrayList<Object>();
            IList<object> list = new List<object>();
            if (array != null)
            {
                foreach (object o in array)
                {
                    if (o is object[])
                    {
                        foreach (object oR in Flatten((object[]) o))
                        {
                            list.Add(oR);
                        }
                    }
                    else
                    {
                        list.Add(o);
                    }
                }
            }
            return list.ToArray();
        }
    }

}