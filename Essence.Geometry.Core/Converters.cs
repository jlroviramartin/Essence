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

using Essence.Geometry.Core.Byte;
using Essence.Geometry.Core.Double;
using Essence.Geometry.Core.Float;
using Essence.Geometry.Core.Integer;

namespace Essence.Geometry.Core
{
    public class Converters
    {
        public static void Register()
        {
            VectorUtils.RegisterReflectionRecursively(typeof(Converters));
        }

        #region Double <-> Float

        public delegate double FloatToDouble(float v);

        public delegate float DoubleToFloat(double v);

        [ConvertHelper(SourceType1 = typeof(ITuple2_Double), DestinationType = typeof(ITuple2_Float))]
        public class DoubleToFloat2 : ITuple2_Float
        {
            public DoubleToFloat2(ITuple2_Double inner)
                : this(inner, v => (float)v)
            {
            }

            public DoubleToFloat2(ITuple2_Double inner, DoubleToFloat converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple2_Double inner;
            private readonly DoubleToFloat converter;

            public void Get(IOpTuple2 setter)
            {
                this.inner.Get(setter);
            }

            public float X
            {
                get { return this.converter(this.inner.X); }
            }

            public float Y
            {
                get { return this.converter(this.inner.Y); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple2_Double), DestinationType = typeof(IOpTuple2_Float))]
        public class DoubleToFloatRW2 : DoubleToFloat2, IOpTuple2_Float
        {
            public DoubleToFloatRW2(IOpTuple2_Double inner)
                : this(inner, v => (float)v, v => (double)v)
            {
            }

            public DoubleToFloatRW2(IOpTuple2_Double inner, DoubleToFloat converterBase, FloatToDouble converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple2_Double inner;
            private readonly FloatToDouble converter;

            public void Set(ITuple2 tuple)
            {
                ITuple2_Double _tuple = tuple.AsTupleDouble();
                this.inner.Set(_tuple.X, _tuple.Y);
            }

            public void Set(float x, float y)
            {
                this.inner.Set(this.converter(x), this.converter(y));
            }

            float IOpTuple2_Float.X
            {
                set { this.inner.X = this.converter(value); }
            }

            float IOpTuple2_Float.Y
            {
                set { this.inner.Y = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple2_Float), DestinationType = typeof(ITuple2_Double))]
        public class FloatToDouble2 : ITuple2_Double
        {
            public FloatToDouble2(ITuple2_Float inner)
                : this(inner, v => (double)v)
            {
            }

            public FloatToDouble2(ITuple2_Float inner, FloatToDouble converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple2_Float inner;
            private readonly FloatToDouble converter;

            public void Get(IOpTuple2 setter)
            {
                this.inner.Get(setter);
            }

            public double X
            {
                get { return this.converter(this.inner.X); }
            }

            public double Y
            {
                get { return this.converter(this.inner.Y); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple2_Float), DestinationType = typeof(IOpTuple2_Double))]
        public class FloatToDoubleRW2 : FloatToDouble2, IOpTuple2_Double
        {
            public FloatToDoubleRW2(IOpTuple2_Float inner)
                : this(inner, v => (double)v, v => (float)v)
            {
            }

            public FloatToDoubleRW2(IOpTuple2_Float inner, FloatToDouble converterBase, DoubleToFloat converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple2_Float inner;
            private readonly DoubleToFloat converter;

            public void Set(ITuple2 tuple)
            {
                ITuple2_Float _tuple = tuple.AsTupleFloat();
                this.inner.Set(_tuple.X, _tuple.Y);
            }

            public void Set(double x, double y)
            {
                this.inner.Set(this.converter(x), this.converter(y));
            }

            double IOpTuple2_Double.X
            {
                set { this.inner.X = this.converter(value); }
            }

            double IOpTuple2_Double.Y
            {
                set { this.inner.Y = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Double), DestinationType = typeof(ITuple3_Float))]
        public class DoubleToFloat3 : ITuple3_Float
        {
            public DoubleToFloat3(ITuple3_Double inner)
                : this(inner, v => (float)v)
            {
            }

            public DoubleToFloat3(ITuple3_Double inner, DoubleToFloat converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple3_Double inner;
            private readonly DoubleToFloat converter;

            public void Get(IOpTuple3 setter)
            {
                this.inner.Get(setter);
            }

            public float X
            {
                get { return this.converter(this.inner.X); }
            }

            public float Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public float Z
            {
                get { return this.converter(this.inner.Z); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Double), DestinationType = typeof(IOpTuple3_Float))]
        public class DoubleToFloatRW3 : DoubleToFloat3, IOpTuple3_Float
        {
            public DoubleToFloatRW3(IOpTuple3_Double inner)
                : this(inner, v => (float)v, v => (double)v)
            {
            }

            public DoubleToFloatRW3(IOpTuple3_Double inner, DoubleToFloat converterBase, FloatToDouble converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple3_Double inner;
            private readonly FloatToDouble converter;

            public void Set(ITuple3 tuple)
            {
                ITuple3_Double _tuple = tuple.AsTupleDouble();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z);
            }

            public void Set(float x, float y, float z)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z));
            }

            float IOpTuple3_Float.X
            {
                set { this.inner.X = this.converter(value); }
            }

            float IOpTuple3_Float.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            float IOpTuple3_Float.Z
            {
                set { this.inner.Z = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Float), DestinationType = typeof(ITuple3_Double))]
        public class FloatToDouble3 : ITuple3_Double
        {
            public FloatToDouble3(ITuple3_Float inner)
                : this(inner, v => (double)v)
            {
            }

            public FloatToDouble3(ITuple3_Float inner, FloatToDouble converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple3_Float inner;
            private readonly FloatToDouble converter;

            public void Get(IOpTuple3 setter)
            {
                this.inner.Get(setter);
            }

            public double X
            {
                get { return this.converter(this.inner.X); }
            }

            public double Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public double Z
            {
                get { return this.converter(this.inner.Z); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Float), DestinationType = typeof(IOpTuple3_Double))]
        public class FloatToDoubleRW3 : FloatToDouble3, IOpTuple3_Double
        {
            public FloatToDoubleRW3(IOpTuple3_Float inner)
                : this(inner, v => (double)v, v => (float)v)
            {
            }

            public FloatToDoubleRW3(IOpTuple3_Float inner, FloatToDouble converterBase, DoubleToFloat converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple3_Float inner;
            private readonly DoubleToFloat converter;

            public void Set(ITuple3 tuple)
            {
                ITuple3_Float _tuple = tuple.AsTupleFloat();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z);
            }

            public void Set(double x, double y, double z)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z));
            }

            double IOpTuple3_Double.X
            {
                set { this.inner.X = this.converter(value); }
            }

            double IOpTuple3_Double.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            double IOpTuple3_Double.Z
            {
                set { this.inner.Z = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Double), DestinationType = typeof(ITuple4_Float))]
        public class DoubleToFloat4 : ITuple4_Float
        {
            public DoubleToFloat4(ITuple4_Double inner)
                : this(inner, v => (float)v)
            {
            }

            public DoubleToFloat4(ITuple4_Double inner, DoubleToFloat converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple4_Double inner;
            private readonly DoubleToFloat converter;

            public void Get(IOpTuple4 setter)
            {
                this.inner.Get(setter);
            }

            public float X
            {
                get { return this.converter(this.inner.X); }
            }

            public float Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public float Z
            {
                get { return this.converter(this.inner.Z); }
            }

            public float W
            {
                get { return this.converter(this.inner.W); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Double), DestinationType = typeof(IOpTuple4_Float))]
        public class DoubleToFloatRW4 : DoubleToFloat4, IOpTuple4_Float
        {
            public DoubleToFloatRW4(IOpTuple4_Double inner)
                : this(inner, v => (float)v, v => (double)v)
            {
            }

            public DoubleToFloatRW4(IOpTuple4_Double inner, DoubleToFloat converterBase, FloatToDouble converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple4_Double inner;
            private readonly FloatToDouble converter;

            public void Set(ITuple4 tuple)
            {
                ITuple4_Double _tuple = tuple.AsTupleDouble();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
            }

            public void Set(float x, float y, float z, float w)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z), this.converter(w));
            }

            float IOpTuple4_Float.X
            {
                set { this.inner.X = this.converter(value); }
            }

            float IOpTuple4_Float.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            float IOpTuple4_Float.Z
            {
                set { this.inner.Z = this.converter(value); }
            }

            float IOpTuple4_Float.W
            {
                set { this.inner.W = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Float), DestinationType = typeof(ITuple4_Double))]
        public class FloatToDouble4 : ITuple4_Double
        {
            public FloatToDouble4(ITuple4_Float inner)
                : this(inner, v => (double)v)
            {
            }

            public FloatToDouble4(ITuple4_Float inner, FloatToDouble converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple4_Float inner;
            private readonly FloatToDouble converter;

            public void Get(IOpTuple4 setter)
            {
                this.inner.Get(setter);
            }

            public double X
            {
                get { return this.converter(this.inner.X); }
            }

            public double Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public double Z
            {
                get { return this.converter(this.inner.Z); }
            }

            public double W
            {
                get { return this.converter(this.inner.W); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Float), DestinationType = typeof(IOpTuple4_Double))]
        public class FloatToDoubleRW4 : FloatToDouble4, IOpTuple4_Double
        {
            public FloatToDoubleRW4(IOpTuple4_Float inner)
                : this(inner, v => (double)v, v => (float)v)
            {
            }

            public FloatToDoubleRW4(IOpTuple4_Float inner, FloatToDouble converterBase, DoubleToFloat converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple4_Float inner;
            private readonly DoubleToFloat converter;

            public void Set(ITuple4 tuple)
            {
                ITuple4_Float _tuple = tuple.AsTupleFloat();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
            }

            public void Set(double x, double y, double z, double w)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z), this.converter(w));
            }

            double IOpTuple4_Double.X
            {
                set { this.inner.X = this.converter(value); }
            }

            double IOpTuple4_Double.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            double IOpTuple4_Double.Z
            {
                set { this.inner.Z = this.converter(value); }
            }

            double IOpTuple4_Double.W
            {
                set { this.inner.W = this.converter(value); }
            }
        }

        #endregion

        #region Double <-> Integer

        public delegate double IntegerToDouble(int v);

        public delegate int DoubleToInteger(double v);

        [ConvertHelper(SourceType1 = typeof(ITuple2_Double), DestinationType = typeof(ITuple2_Integer))]
        public class DoubleToInteger2 : ITuple2_Integer
        {
            public DoubleToInteger2(ITuple2_Double inner)
                : this(inner, v => (int)v)
            {
            }

            public DoubleToInteger2(ITuple2_Double inner, DoubleToInteger converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple2_Double inner;
            private readonly DoubleToInteger converter;

            public void Get(IOpTuple2 setter)
            {
                this.inner.Get(setter);
            }

            public int X
            {
                get { return this.converter(this.inner.X); }
            }

            public int Y
            {
                get { return this.converter(this.inner.Y); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple2_Double), DestinationType = typeof(IOpTuple2_Integer))]
        public class DoubleToIntegerRW2 : DoubleToInteger2, IOpTuple2_Integer
        {
            public DoubleToIntegerRW2(IOpTuple2_Double inner)
                : this(inner, v => (int)v, v => (double)v)
            {
            }

            public DoubleToIntegerRW2(IOpTuple2_Double inner, DoubleToInteger converterBase, IntegerToDouble converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple2_Double inner;
            private readonly IntegerToDouble converter;

            public void Set(ITuple2 tuple)
            {
                ITuple2_Double _tuple = tuple.AsTupleDouble();
                this.inner.Set(_tuple.X, _tuple.Y);
            }

            public void Set(int x, int y)
            {
                this.inner.Set(this.converter(x), this.converter(y));
            }

            int IOpTuple2_Integer.X
            {
                set { this.inner.X = this.converter(value); }
            }

            int IOpTuple2_Integer.Y
            {
                set { this.inner.Y = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple2_Integer), DestinationType = typeof(ITuple2_Double))]
        public class IntegerToDouble2 : ITuple2_Double
        {
            public IntegerToDouble2(ITuple2_Integer inner)
                : this(inner, v => (double)v)
            {
            }

            public IntegerToDouble2(ITuple2_Integer inner, IntegerToDouble converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple2_Integer inner;
            private readonly IntegerToDouble converter;

            public void Get(IOpTuple2 setter)
            {
                this.inner.Get(setter);
            }

            public double X
            {
                get { return this.converter(this.inner.X); }
            }

            public double Y
            {
                get { return this.converter(this.inner.Y); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple2_Integer), DestinationType = typeof(IOpTuple2_Double))]
        public class IntegerToDoubleRW2 : IntegerToDouble2, IOpTuple2_Double
        {
            public IntegerToDoubleRW2(IOpTuple2_Integer inner)
                : this(inner, v => (double)v, v => (int)v)
            {
            }

            public IntegerToDoubleRW2(IOpTuple2_Integer inner, IntegerToDouble converterBase, DoubleToInteger converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple2_Integer inner;
            private readonly DoubleToInteger converter;

            public void Set(ITuple2 tuple)
            {
                ITuple2_Integer _tuple = tuple.AsTupleInteger();
                this.inner.Set(_tuple.X, _tuple.Y);
            }

            public void Set(double x, double y)
            {
                this.inner.Set(this.converter(x), this.converter(y));
            }

            double IOpTuple2_Double.X
            {
                set { this.inner.X = this.converter(value); }
            }

            double IOpTuple2_Double.Y
            {
                set { this.inner.Y = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Double), DestinationType = typeof(ITuple3_Integer))]
        public class DoubleToInteger3 : ITuple3_Integer
        {
            public DoubleToInteger3(ITuple3_Double inner)
                : this(inner, v => (int)v)
            {
            }

            public DoubleToInteger3(ITuple3_Double inner, DoubleToInteger converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple3_Double inner;
            private readonly DoubleToInteger converter;

            public void Get(IOpTuple3 setter)
            {
                this.inner.Get(setter);
            }

            public int X
            {
                get { return this.converter(this.inner.X); }
            }

            public int Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public int Z
            {
                get { return this.converter(this.inner.Z); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Double), DestinationType = typeof(IOpTuple3_Integer))]
        public class DoubleToIntegerRW3 : DoubleToInteger3, IOpTuple3_Integer
        {
            public DoubleToIntegerRW3(IOpTuple3_Double inner)
                : this(inner, v => (int)v, v => (double)v)
            {
            }

            public DoubleToIntegerRW3(IOpTuple3_Double inner, DoubleToInteger converterBase, IntegerToDouble converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple3_Double inner;
            private readonly IntegerToDouble converter;

            public void Set(ITuple3 tuple)
            {
                ITuple3_Double _tuple = tuple.AsTupleDouble();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z);
            }

            public void Set(int x, int y, int z)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z));
            }

            int IOpTuple3_Integer.X
            {
                set { this.inner.X = this.converter(value); }
            }

            int IOpTuple3_Integer.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            int IOpTuple3_Integer.Z
            {
                set { this.inner.Z = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Integer), DestinationType = typeof(ITuple3_Double))]
        public class IntegerToDouble3 : ITuple3_Double
        {
            public IntegerToDouble3(ITuple3_Integer inner)
                : this(inner, v => (double)v)
            {
            }

            public IntegerToDouble3(ITuple3_Integer inner, IntegerToDouble converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple3_Integer inner;
            private readonly IntegerToDouble converter;

            public void Get(IOpTuple3 setter)
            {
                this.inner.Get(setter);
            }

            public double X
            {
                get { return this.converter(this.inner.X); }
            }

            public double Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public double Z
            {
                get { return this.converter(this.inner.Z); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Integer), DestinationType = typeof(IOpTuple3_Double))]
        public class IntegerToDoubleRW3 : IntegerToDouble3, IOpTuple3_Double
        {
            public IntegerToDoubleRW3(IOpTuple3_Integer inner)
                : this(inner, v => (double)v, v => (int)v)
            {
            }

            public IntegerToDoubleRW3(IOpTuple3_Integer inner, IntegerToDouble converterBase, DoubleToInteger converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple3_Integer inner;
            private readonly DoubleToInteger converter;

            public void Set(ITuple3 tuple)
            {
                ITuple3_Integer _tuple = tuple.AsTupleInteger();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z);
            }

            public void Set(double x, double y, double z)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z));
            }

            double IOpTuple3_Double.X
            {
                set { this.inner.X = this.converter(value); }
            }

            double IOpTuple3_Double.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            double IOpTuple3_Double.Z
            {
                set { this.inner.Z = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Double), DestinationType = typeof(ITuple4_Integer))]
        public class DoubleToInteger4 : ITuple4_Integer
        {
            public DoubleToInteger4(ITuple4_Double inner)
                : this(inner, v => (int)v)
            {
            }

            public DoubleToInteger4(ITuple4_Double inner, DoubleToInteger converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple4_Double inner;
            private readonly DoubleToInteger converter;

            public void Get(IOpTuple4 setter)
            {
                this.inner.Get(setter);
            }

            public int X
            {
                get { return this.converter(this.inner.X); }
            }

            public int Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public int Z
            {
                get { return this.converter(this.inner.Z); }
            }

            public int W
            {
                get { return this.converter(this.inner.W); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Double), DestinationType = typeof(IOpTuple4_Integer))]
        public class DoubleToIntegerRW4 : DoubleToInteger4, IOpTuple4_Integer
        {
            public DoubleToIntegerRW4(IOpTuple4_Double inner)
                : this(inner, v => (int)v, v => (double)v)
            {
            }

            public DoubleToIntegerRW4(IOpTuple4_Double inner, DoubleToInteger converterBase, IntegerToDouble converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple4_Double inner;
            private readonly IntegerToDouble converter;

            public void Set(ITuple4 tuple)
            {
                ITuple4_Double _tuple = tuple.AsTupleDouble();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
            }

            public void Set(int x, int y, int z, int w)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z), this.converter(w));
            }

            int IOpTuple4_Integer.X
            {
                set { this.inner.X = this.converter(value); }
            }

            int IOpTuple4_Integer.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            int IOpTuple4_Integer.Z
            {
                set { this.inner.Z = this.converter(value); }
            }

            int IOpTuple4_Integer.W
            {
                set { this.inner.W = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Integer), DestinationType = typeof(ITuple4_Double))]
        public class IntegerToDouble4 : ITuple4_Double
        {
            public IntegerToDouble4(ITuple4_Integer inner)
                : this(inner, v => (double)v)
            {
            }

            public IntegerToDouble4(ITuple4_Integer inner, IntegerToDouble converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple4_Integer inner;
            private readonly IntegerToDouble converter;

            public void Get(IOpTuple4 setter)
            {
                this.inner.Get(setter);
            }

            public double X
            {
                get { return this.converter(this.inner.X); }
            }

            public double Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public double Z
            {
                get { return this.converter(this.inner.Z); }
            }

            public double W
            {
                get { return this.converter(this.inner.W); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Integer), DestinationType = typeof(IOpTuple4_Double))]
        public class IntegerToDoubleRW4 : IntegerToDouble4, IOpTuple4_Double
        {
            public IntegerToDoubleRW4(IOpTuple4_Integer inner)
                : this(inner, v => (double)v, v => (int)v)
            {
            }

            public IntegerToDoubleRW4(IOpTuple4_Integer inner, IntegerToDouble converterBase, DoubleToInteger converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple4_Integer inner;
            private readonly DoubleToInteger converter;

            public void Set(ITuple4 tuple)
            {
                ITuple4_Integer _tuple = tuple.AsTupleInteger();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
            }

            public void Set(double x, double y, double z, double w)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z), this.converter(w));
            }

            double IOpTuple4_Double.X
            {
                set { this.inner.X = this.converter(value); }
            }

            double IOpTuple4_Double.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            double IOpTuple4_Double.Z
            {
                set { this.inner.Z = this.converter(value); }
            }

            double IOpTuple4_Double.W
            {
                set { this.inner.W = this.converter(value); }
            }
        }

        #endregion

        #region Double <-> Byte

        public delegate double ByteToDouble(byte v);

        public delegate byte DoubleToByte(double v);

        [ConvertHelper(SourceType1 = typeof(ITuple2_Double), DestinationType = typeof(ITuple2_Byte))]
        public class DoubleToByte2 : ITuple2_Byte
        {
            public DoubleToByte2(ITuple2_Double inner)
                : this(inner, v => (byte)v)
            {
            }

            public DoubleToByte2(ITuple2_Double inner, DoubleToByte converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple2_Double inner;
            private readonly DoubleToByte converter;

            public void Get(IOpTuple2 setter)
            {
                this.inner.Get(setter);
            }

            public byte X
            {
                get { return this.converter(this.inner.X); }
            }

            public byte Y
            {
                get { return this.converter(this.inner.Y); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple2_Double), DestinationType = typeof(IOpTuple2_Byte))]
        public class DoubleToByteRW2 : DoubleToByte2, IOpTuple2_Byte
        {
            public DoubleToByteRW2(IOpTuple2_Double inner)
                : this(inner, v => (byte)v, v => (double)v)
            {
            }

            public DoubleToByteRW2(IOpTuple2_Double inner, DoubleToByte converterBase, ByteToDouble converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple2_Double inner;
            private readonly ByteToDouble converter;

            public void Set(ITuple2 tuple)
            {
                ITuple2_Double _tuple = tuple.AsTupleDouble();
                this.inner.Set(_tuple.X, _tuple.Y);
            }

            public void Set(byte x, byte y)
            {
                this.inner.Set(this.converter(x), this.converter(y));
            }

            byte IOpTuple2_Byte.X
            {
                set { this.inner.X = this.converter(value); }
            }

            byte IOpTuple2_Byte.Y
            {
                set { this.inner.Y = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple2_Byte), DestinationType = typeof(ITuple2_Double))]
        public class ByteToDouble2 : ITuple2_Double
        {
            public ByteToDouble2(ITuple2_Byte inner)
                : this(inner, v => (double)v)
            {
            }

            public ByteToDouble2(ITuple2_Byte inner, ByteToDouble converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple2_Byte inner;
            private readonly ByteToDouble converter;

            public void Get(IOpTuple2 setter)
            {
                this.inner.Get(setter);
            }

            public double X
            {
                get { return this.converter(this.inner.X); }
            }

            public double Y
            {
                get { return this.converter(this.inner.Y); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple2_Byte), DestinationType = typeof(IOpTuple2_Double))]
        public class ByteToDoubleRW2 : ByteToDouble2, IOpTuple2_Double
        {
            public ByteToDoubleRW2(IOpTuple2_Byte inner)
                : this(inner, v => (double)v, v => (byte)v)
            {
            }

            public ByteToDoubleRW2(IOpTuple2_Byte inner, ByteToDouble converterBase, DoubleToByte converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple2_Byte inner;
            private readonly DoubleToByte converter;

            public void Set(ITuple2 tuple)
            {
                ITuple2_Byte _tuple = tuple.AsTupleByte();
                this.inner.Set(_tuple.X, _tuple.Y);
            }

            public void Set(double x, double y)
            {
                this.inner.Set(this.converter(x), this.converter(y));
            }

            double IOpTuple2_Double.X
            {
                set { this.inner.X = this.converter(value); }
            }

            double IOpTuple2_Double.Y
            {
                set { this.inner.Y = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Double), DestinationType = typeof(ITuple3_Byte))]
        public class DoubleToByte3 : ITuple3_Byte
        {
            public DoubleToByte3(ITuple3_Double inner)
                : this(inner, v => (byte)v)
            {
            }

            public DoubleToByte3(ITuple3_Double inner, DoubleToByte converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple3_Double inner;
            private readonly DoubleToByte converter;

            public void Get(IOpTuple3 setter)
            {
                this.inner.Get(setter);
            }

            public byte X
            {
                get { return this.converter(this.inner.X); }
            }

            public byte Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public byte Z
            {
                get { return this.converter(this.inner.Z); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Double), DestinationType = typeof(IOpTuple3_Byte))]
        public class DoubleToByteRW3 : DoubleToByte3, IOpTuple3_Byte
        {
            public DoubleToByteRW3(IOpTuple3_Double inner)
                : this(inner, v => (byte)v, v => (double)v)
            {
            }

            public DoubleToByteRW3(IOpTuple3_Double inner, DoubleToByte converterBase, ByteToDouble converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple3_Double inner;
            private readonly ByteToDouble converter;

            public void Set(ITuple3 tuple)
            {
                ITuple3_Double _tuple = tuple.AsTupleDouble();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z);
            }

            public void Set(byte x, byte y, byte z)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z));
            }

            byte IOpTuple3_Byte.X
            {
                set { this.inner.X = this.converter(value); }
            }

            byte IOpTuple3_Byte.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            byte IOpTuple3_Byte.Z
            {
                set { this.inner.Z = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Byte), DestinationType = typeof(ITuple3_Double))]
        public class ByteToDouble3 : ITuple3_Double
        {
            public ByteToDouble3(ITuple3_Byte inner)
                : this(inner, v => (double)v)
            {
            }

            public ByteToDouble3(ITuple3_Byte inner, ByteToDouble converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple3_Byte inner;
            private readonly ByteToDouble converter;

            public void Get(IOpTuple3 setter)
            {
                this.inner.Get(setter);
            }

            public double X
            {
                get { return this.converter(this.inner.X); }
            }

            public double Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public double Z
            {
                get { return this.converter(this.inner.Z); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Byte), DestinationType = typeof(IOpTuple3_Double))]
        public class ByteToDoubleRW3 : ByteToDouble3, IOpTuple3_Double
        {
            public ByteToDoubleRW3(IOpTuple3_Byte inner)
                : this(inner, v => (double)v, v => (byte)v)
            {
            }

            public ByteToDoubleRW3(IOpTuple3_Byte inner, ByteToDouble converterBase, DoubleToByte converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple3_Byte inner;
            private readonly DoubleToByte converter;

            public void Set(ITuple3 tuple)
            {
                ITuple3_Byte _tuple = tuple.AsTupleByte();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z);
            }

            public void Set(double x, double y, double z)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z));
            }

            double IOpTuple3_Double.X
            {
                set { this.inner.X = this.converter(value); }
            }

            double IOpTuple3_Double.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            double IOpTuple3_Double.Z
            {
                set { this.inner.Z = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Double), DestinationType = typeof(ITuple4_Byte))]
        public class DoubleToByte4 : ITuple4_Byte
        {
            public DoubleToByte4(ITuple4_Double inner)
                : this(inner, v => (byte)v)
            {
            }

            public DoubleToByte4(ITuple4_Double inner, DoubleToByte converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple4_Double inner;
            private readonly DoubleToByte converter;

            public void Get(IOpTuple4 setter)
            {
                this.inner.Get(setter);
            }

            public byte X
            {
                get { return this.converter(this.inner.X); }
            }

            public byte Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public byte Z
            {
                get { return this.converter(this.inner.Z); }
            }

            public byte W
            {
                get { return this.converter(this.inner.W); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Double), DestinationType = typeof(IOpTuple4_Byte))]
        public class DoubleToByteRW4 : DoubleToByte4, IOpTuple4_Byte
        {
            public DoubleToByteRW4(IOpTuple4_Double inner)
                : this(inner, v => (byte)v, v => (double)v)
            {
            }

            public DoubleToByteRW4(IOpTuple4_Double inner, DoubleToByte converterBase, ByteToDouble converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple4_Double inner;
            private readonly ByteToDouble converter;

            public void Set(ITuple4 tuple)
            {
                ITuple4_Double _tuple = tuple.AsTupleDouble();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
            }

            public void Set(byte x, byte y, byte z, byte w)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z), this.converter(w));
            }

            byte IOpTuple4_Byte.X
            {
                set { this.inner.X = this.converter(value); }
            }

            byte IOpTuple4_Byte.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            byte IOpTuple4_Byte.Z
            {
                set { this.inner.Z = this.converter(value); }
            }

            byte IOpTuple4_Byte.W
            {
                set { this.inner.W = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Byte), DestinationType = typeof(ITuple4_Double))]
        public class ByteToDouble4 : ITuple4_Double
        {
            public ByteToDouble4(ITuple4_Byte inner)
                : this(inner, v => (double)v)
            {
            }

            public ByteToDouble4(ITuple4_Byte inner, ByteToDouble converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple4_Byte inner;
            private readonly ByteToDouble converter;

            public void Get(IOpTuple4 setter)
            {
                this.inner.Get(setter);
            }

            public double X
            {
                get { return this.converter(this.inner.X); }
            }

            public double Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public double Z
            {
                get { return this.converter(this.inner.Z); }
            }

            public double W
            {
                get { return this.converter(this.inner.W); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Byte), DestinationType = typeof(IOpTuple4_Double))]
        public class ByteToDoubleRW4 : ByteToDouble4, IOpTuple4_Double
        {
            public ByteToDoubleRW4(IOpTuple4_Byte inner)
                : this(inner, v => (double)v, v => (byte)v)
            {
            }

            public ByteToDoubleRW4(IOpTuple4_Byte inner, ByteToDouble converterBase, DoubleToByte converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple4_Byte inner;
            private readonly DoubleToByte converter;

            public void Set(ITuple4 tuple)
            {
                ITuple4_Byte _tuple = tuple.AsTupleByte();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
            }

            public void Set(double x, double y, double z, double w)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z), this.converter(w));
            }

            double IOpTuple4_Double.X
            {
                set { this.inner.X = this.converter(value); }
            }

            double IOpTuple4_Double.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            double IOpTuple4_Double.Z
            {
                set { this.inner.Z = this.converter(value); }
            }

            double IOpTuple4_Double.W
            {
                set { this.inner.W = this.converter(value); }
            }
        }

        #endregion

        #region Float <-> Integer

        public delegate float IntegerToFloat(int v);

        public delegate int FloatToInteger(float v);

        [ConvertHelper(SourceType1 = typeof(ITuple2_Float), DestinationType = typeof(ITuple2_Integer))]
        public class FloatToInteger2 : ITuple2_Integer
        {
            public FloatToInteger2(ITuple2_Float inner)
                : this(inner, v => (int)v)
            {
            }

            public FloatToInteger2(ITuple2_Float inner, FloatToInteger converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple2_Float inner;
            private readonly FloatToInteger converter;

            public void Get(IOpTuple2 setter)
            {
                this.inner.Get(setter);
            }

            public int X
            {
                get { return this.converter(this.inner.X); }
            }

            public int Y
            {
                get { return this.converter(this.inner.Y); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple2_Float), DestinationType = typeof(IOpTuple2_Integer))]
        public class FloatToIntegerRW2 : FloatToInteger2, IOpTuple2_Integer
        {
            public FloatToIntegerRW2(IOpTuple2_Float inner)
                : this(inner, v => (int)v, v => (float)v)
            {
            }

            public FloatToIntegerRW2(IOpTuple2_Float inner, FloatToInteger converterBase, IntegerToFloat converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple2_Float inner;
            private readonly IntegerToFloat converter;

            public void Set(ITuple2 tuple)
            {
                ITuple2_Float _tuple = tuple.AsTupleFloat();
                this.inner.Set(_tuple.X, _tuple.Y);
            }

            public void Set(int x, int y)
            {
                this.inner.Set(this.converter(x), this.converter(y));
            }

            int IOpTuple2_Integer.X
            {
                set { this.inner.X = this.converter(value); }
            }

            int IOpTuple2_Integer.Y
            {
                set { this.inner.Y = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple2_Integer), DestinationType = typeof(ITuple2_Float))]
        public class IntegerToFloat2 : ITuple2_Float
        {
            public IntegerToFloat2(ITuple2_Integer inner)
                : this(inner, v => (float)v)
            {
            }

            public IntegerToFloat2(ITuple2_Integer inner, IntegerToFloat converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple2_Integer inner;
            private readonly IntegerToFloat converter;

            public void Get(IOpTuple2 setter)
            {
                this.inner.Get(setter);
            }

            public float X
            {
                get { return this.converter(this.inner.X); }
            }

            public float Y
            {
                get { return this.converter(this.inner.Y); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple2_Integer), DestinationType = typeof(IOpTuple2_Float))]
        public class IntegerToFloatRW2 : IntegerToFloat2, IOpTuple2_Float
        {
            public IntegerToFloatRW2(IOpTuple2_Integer inner)
                : this(inner, v => (float)v, v => (int)v)
            {
            }

            public IntegerToFloatRW2(IOpTuple2_Integer inner, IntegerToFloat converterBase, FloatToInteger converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple2_Integer inner;
            private readonly FloatToInteger converter;

            public void Set(ITuple2 tuple)
            {
                ITuple2_Integer _tuple = tuple.AsTupleInteger();
                this.inner.Set(_tuple.X, _tuple.Y);
            }

            public void Set(float x, float y)
            {
                this.inner.Set(this.converter(x), this.converter(y));
            }

            float IOpTuple2_Float.X
            {
                set { this.inner.X = this.converter(value); }
            }

            float IOpTuple2_Float.Y
            {
                set { this.inner.Y = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Float), DestinationType = typeof(ITuple3_Integer))]
        public class FloatToInteger3 : ITuple3_Integer
        {
            public FloatToInteger3(ITuple3_Float inner)
                : this(inner, v => (int)v)
            {
            }

            public FloatToInteger3(ITuple3_Float inner, FloatToInteger converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple3_Float inner;
            private readonly FloatToInteger converter;

            public void Get(IOpTuple3 setter)
            {
                this.inner.Get(setter);
            }

            public int X
            {
                get { return this.converter(this.inner.X); }
            }

            public int Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public int Z
            {
                get { return this.converter(this.inner.Z); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Float), DestinationType = typeof(IOpTuple3_Integer))]
        public class FloatToIntegerRW3 : FloatToInteger3, IOpTuple3_Integer
        {
            public FloatToIntegerRW3(IOpTuple3_Float inner)
                : this(inner, v => (int)v, v => (float)v)
            {
            }

            public FloatToIntegerRW3(IOpTuple3_Float inner, FloatToInteger converterBase, IntegerToFloat converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple3_Float inner;
            private readonly IntegerToFloat converter;

            public void Set(ITuple3 tuple)
            {
                ITuple3_Float _tuple = tuple.AsTupleFloat();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z);
            }

            public void Set(int x, int y, int z)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z));
            }

            int IOpTuple3_Integer.X
            {
                set { this.inner.X = this.converter(value); }
            }

            int IOpTuple3_Integer.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            int IOpTuple3_Integer.Z
            {
                set { this.inner.Z = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Integer), DestinationType = typeof(ITuple3_Float))]
        public class IntegerToFloat3 : ITuple3_Float
        {
            public IntegerToFloat3(ITuple3_Integer inner)
                : this(inner, v => (float)v)
            {
            }

            public IntegerToFloat3(ITuple3_Integer inner, IntegerToFloat converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple3_Integer inner;
            private readonly IntegerToFloat converter;

            public void Get(IOpTuple3 setter)
            {
                this.inner.Get(setter);
            }

            public float X
            {
                get { return this.converter(this.inner.X); }
            }

            public float Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public float Z
            {
                get { return this.converter(this.inner.Z); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Integer), DestinationType = typeof(IOpTuple3_Float))]
        public class IntegerToFloatRW3 : IntegerToFloat3, IOpTuple3_Float
        {
            public IntegerToFloatRW3(IOpTuple3_Integer inner)
                : this(inner, v => (float)v, v => (int)v)
            {
            }

            public IntegerToFloatRW3(IOpTuple3_Integer inner, IntegerToFloat converterBase, FloatToInteger converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple3_Integer inner;
            private readonly FloatToInteger converter;

            public void Set(ITuple3 tuple)
            {
                ITuple3_Integer _tuple = tuple.AsTupleInteger();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z);
            }

            public void Set(float x, float y, float z)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z));
            }

            float IOpTuple3_Float.X
            {
                set { this.inner.X = this.converter(value); }
            }

            float IOpTuple3_Float.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            float IOpTuple3_Float.Z
            {
                set { this.inner.Z = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Float), DestinationType = typeof(ITuple4_Integer))]
        public class FloatToInteger4 : ITuple4_Integer
        {
            public FloatToInteger4(ITuple4_Float inner)
                : this(inner, v => (int)v)
            {
            }

            public FloatToInteger4(ITuple4_Float inner, FloatToInteger converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple4_Float inner;
            private readonly FloatToInteger converter;

            public void Get(IOpTuple4 setter)
            {
                this.inner.Get(setter);
            }

            public int X
            {
                get { return this.converter(this.inner.X); }
            }

            public int Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public int Z
            {
                get { return this.converter(this.inner.Z); }
            }

            public int W
            {
                get { return this.converter(this.inner.W); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Float), DestinationType = typeof(IOpTuple4_Integer))]
        public class FloatToIntegerRW4 : FloatToInteger4, IOpTuple4_Integer
        {
            public FloatToIntegerRW4(IOpTuple4_Float inner)
                : this(inner, v => (int)v, v => (float)v)
            {
            }

            public FloatToIntegerRW4(IOpTuple4_Float inner, FloatToInteger converterBase, IntegerToFloat converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple4_Float inner;
            private readonly IntegerToFloat converter;

            public void Set(ITuple4 tuple)
            {
                ITuple4_Float _tuple = tuple.AsTupleFloat();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
            }

            public void Set(int x, int y, int z, int w)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z), this.converter(w));
            }

            int IOpTuple4_Integer.X
            {
                set { this.inner.X = this.converter(value); }
            }

            int IOpTuple4_Integer.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            int IOpTuple4_Integer.Z
            {
                set { this.inner.Z = this.converter(value); }
            }

            int IOpTuple4_Integer.W
            {
                set { this.inner.W = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Integer), DestinationType = typeof(ITuple4_Float))]
        public class IntegerToFloat4 : ITuple4_Float
        {
            public IntegerToFloat4(ITuple4_Integer inner)
                : this(inner, v => (float)v)
            {
            }

            public IntegerToFloat4(ITuple4_Integer inner, IntegerToFloat converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple4_Integer inner;
            private readonly IntegerToFloat converter;

            public void Get(IOpTuple4 setter)
            {
                this.inner.Get(setter);
            }

            public float X
            {
                get { return this.converter(this.inner.X); }
            }

            public float Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public float Z
            {
                get { return this.converter(this.inner.Z); }
            }

            public float W
            {
                get { return this.converter(this.inner.W); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Integer), DestinationType = typeof(IOpTuple4_Float))]
        public class IntegerToFloatRW4 : IntegerToFloat4, IOpTuple4_Float
        {
            public IntegerToFloatRW4(IOpTuple4_Integer inner)
                : this(inner, v => (float)v, v => (int)v)
            {
            }

            public IntegerToFloatRW4(IOpTuple4_Integer inner, IntegerToFloat converterBase, FloatToInteger converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple4_Integer inner;
            private readonly FloatToInteger converter;

            public void Set(ITuple4 tuple)
            {
                ITuple4_Integer _tuple = tuple.AsTupleInteger();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
            }

            public void Set(float x, float y, float z, float w)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z), this.converter(w));
            }

            float IOpTuple4_Float.X
            {
                set { this.inner.X = this.converter(value); }
            }

            float IOpTuple4_Float.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            float IOpTuple4_Float.Z
            {
                set { this.inner.Z = this.converter(value); }
            }

            float IOpTuple4_Float.W
            {
                set { this.inner.W = this.converter(value); }
            }
        }

        #endregion

        #region Float <-> Byte

        public delegate float ByteToFloat(byte v);

        public delegate byte FloatToByte(float v);

        [ConvertHelper(SourceType1 = typeof(ITuple2_Float), DestinationType = typeof(ITuple2_Byte))]
        public class FloatToByte2 : ITuple2_Byte
        {
            public FloatToByte2(ITuple2_Float inner)
                : this(inner, v => (byte)v)
            {
            }

            public FloatToByte2(ITuple2_Float inner, FloatToByte converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple2_Float inner;
            private readonly FloatToByte converter;

            public void Get(IOpTuple2 setter)
            {
                this.inner.Get(setter);
            }

            public byte X
            {
                get { return this.converter(this.inner.X); }
            }

            public byte Y
            {
                get { return this.converter(this.inner.Y); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple2_Float), DestinationType = typeof(IOpTuple2_Byte))]
        public class FloatToByteRW2 : FloatToByte2, IOpTuple2_Byte
        {
            public FloatToByteRW2(IOpTuple2_Float inner)
                : this(inner, v => (byte)v, v => (float)v)
            {
            }

            public FloatToByteRW2(IOpTuple2_Float inner, FloatToByte converterBase, ByteToFloat converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple2_Float inner;
            private readonly ByteToFloat converter;

            public void Set(ITuple2 tuple)
            {
                ITuple2_Float _tuple = tuple.AsTupleFloat();
                this.inner.Set(_tuple.X, _tuple.Y);
            }

            public void Set(byte x, byte y)
            {
                this.inner.Set(this.converter(x), this.converter(y));
            }

            byte IOpTuple2_Byte.X
            {
                set { this.inner.X = this.converter(value); }
            }

            byte IOpTuple2_Byte.Y
            {
                set { this.inner.Y = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple2_Byte), DestinationType = typeof(ITuple2_Float))]
        public class ByteToFloat2 : ITuple2_Float
        {
            public ByteToFloat2(ITuple2_Byte inner)
                : this(inner, v => (float)v)
            {
            }

            public ByteToFloat2(ITuple2_Byte inner, ByteToFloat converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple2_Byte inner;
            private readonly ByteToFloat converter;

            public void Get(IOpTuple2 setter)
            {
                this.inner.Get(setter);
            }

            public float X
            {
                get { return this.converter(this.inner.X); }
            }

            public float Y
            {
                get { return this.converter(this.inner.Y); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple2_Byte), DestinationType = typeof(IOpTuple2_Float))]
        public class ByteToFloatRW2 : ByteToFloat2, IOpTuple2_Float
        {
            public ByteToFloatRW2(IOpTuple2_Byte inner)
                : this(inner, v => (float)v, v => (byte)v)
            {
            }

            public ByteToFloatRW2(IOpTuple2_Byte inner, ByteToFloat converterBase, FloatToByte converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple2_Byte inner;
            private readonly FloatToByte converter;

            public void Set(ITuple2 tuple)
            {
                ITuple2_Byte _tuple = tuple.AsTupleByte();
                this.inner.Set(_tuple.X, _tuple.Y);
            }

            public void Set(float x, float y)
            {
                this.inner.Set(this.converter(x), this.converter(y));
            }

            float IOpTuple2_Float.X
            {
                set { this.inner.X = this.converter(value); }
            }

            float IOpTuple2_Float.Y
            {
                set { this.inner.Y = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Float), DestinationType = typeof(ITuple3_Byte))]
        public class FloatToByte3 : ITuple3_Byte
        {
            public FloatToByte3(ITuple3_Float inner)
                : this(inner, v => (byte)v)
            {
            }

            public FloatToByte3(ITuple3_Float inner, FloatToByte converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple3_Float inner;
            private readonly FloatToByte converter;

            public void Get(IOpTuple3 setter)
            {
                this.inner.Get(setter);
            }

            public byte X
            {
                get { return this.converter(this.inner.X); }
            }

            public byte Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public byte Z
            {
                get { return this.converter(this.inner.Z); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Float), DestinationType = typeof(IOpTuple3_Byte))]
        public class FloatToByteRW3 : FloatToByte3, IOpTuple3_Byte
        {
            public FloatToByteRW3(IOpTuple3_Float inner)
                : this(inner, v => (byte)v, v => (float)v)
            {
            }

            public FloatToByteRW3(IOpTuple3_Float inner, FloatToByte converterBase, ByteToFloat converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple3_Float inner;
            private readonly ByteToFloat converter;

            public void Set(ITuple3 tuple)
            {
                ITuple3_Float _tuple = tuple.AsTupleFloat();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z);
            }

            public void Set(byte x, byte y, byte z)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z));
            }

            byte IOpTuple3_Byte.X
            {
                set { this.inner.X = this.converter(value); }
            }

            byte IOpTuple3_Byte.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            byte IOpTuple3_Byte.Z
            {
                set { this.inner.Z = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Byte), DestinationType = typeof(ITuple3_Float))]
        public class ByteToFloat3 : ITuple3_Float
        {
            public ByteToFloat3(ITuple3_Byte inner)
                : this(inner, v => (float)v)
            {
            }

            public ByteToFloat3(ITuple3_Byte inner, ByteToFloat converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple3_Byte inner;
            private readonly ByteToFloat converter;

            public void Get(IOpTuple3 setter)
            {
                this.inner.Get(setter);
            }

            public float X
            {
                get { return this.converter(this.inner.X); }
            }

            public float Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public float Z
            {
                get { return this.converter(this.inner.Z); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Byte), DestinationType = typeof(IOpTuple3_Float))]
        public class ByteToFloatRW3 : ByteToFloat3, IOpTuple3_Float
        {
            public ByteToFloatRW3(IOpTuple3_Byte inner)
                : this(inner, v => (float)v, v => (byte)v)
            {
            }

            public ByteToFloatRW3(IOpTuple3_Byte inner, ByteToFloat converterBase, FloatToByte converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple3_Byte inner;
            private readonly FloatToByte converter;

            public void Set(ITuple3 tuple)
            {
                ITuple3_Byte _tuple = tuple.AsTupleByte();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z);
            }

            public void Set(float x, float y, float z)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z));
            }

            float IOpTuple3_Float.X
            {
                set { this.inner.X = this.converter(value); }
            }

            float IOpTuple3_Float.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            float IOpTuple3_Float.Z
            {
                set { this.inner.Z = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Float), DestinationType = typeof(ITuple4_Byte))]
        public class FloatToByte4 : ITuple4_Byte
        {
            public FloatToByte4(ITuple4_Float inner)
                : this(inner, v => (byte)v)
            {
            }

            public FloatToByte4(ITuple4_Float inner, FloatToByte converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple4_Float inner;
            private readonly FloatToByte converter;

            public void Get(IOpTuple4 setter)
            {
                this.inner.Get(setter);
            }

            public byte X
            {
                get { return this.converter(this.inner.X); }
            }

            public byte Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public byte Z
            {
                get { return this.converter(this.inner.Z); }
            }

            public byte W
            {
                get { return this.converter(this.inner.W); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Float), DestinationType = typeof(IOpTuple4_Byte))]
        public class FloatToByteRW4 : FloatToByte4, IOpTuple4_Byte
        {
            public FloatToByteRW4(IOpTuple4_Float inner)
                : this(inner, v => (byte)v, v => (float)v)
            {
            }

            public FloatToByteRW4(IOpTuple4_Float inner, FloatToByte converterBase, ByteToFloat converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple4_Float inner;
            private readonly ByteToFloat converter;

            public void Set(ITuple4 tuple)
            {
                ITuple4_Float _tuple = tuple.AsTupleFloat();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
            }

            public void Set(byte x, byte y, byte z, byte w)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z), this.converter(w));
            }

            byte IOpTuple4_Byte.X
            {
                set { this.inner.X = this.converter(value); }
            }

            byte IOpTuple4_Byte.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            byte IOpTuple4_Byte.Z
            {
                set { this.inner.Z = this.converter(value); }
            }

            byte IOpTuple4_Byte.W
            {
                set { this.inner.W = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Byte), DestinationType = typeof(ITuple4_Float))]
        public class ByteToFloat4 : ITuple4_Float
        {
            public ByteToFloat4(ITuple4_Byte inner)
                : this(inner, v => (float)v)
            {
            }

            public ByteToFloat4(ITuple4_Byte inner, ByteToFloat converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple4_Byte inner;
            private readonly ByteToFloat converter;

            public void Get(IOpTuple4 setter)
            {
                this.inner.Get(setter);
            }

            public float X
            {
                get { return this.converter(this.inner.X); }
            }

            public float Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public float Z
            {
                get { return this.converter(this.inner.Z); }
            }

            public float W
            {
                get { return this.converter(this.inner.W); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Byte), DestinationType = typeof(IOpTuple4_Float))]
        public class ByteToFloatRW4 : ByteToFloat4, IOpTuple4_Float
        {
            public ByteToFloatRW4(IOpTuple4_Byte inner)
                : this(inner, v => (float)v, v => (byte)v)
            {
            }

            public ByteToFloatRW4(IOpTuple4_Byte inner, ByteToFloat converterBase, FloatToByte converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple4_Byte inner;
            private readonly FloatToByte converter;

            public void Set(ITuple4 tuple)
            {
                ITuple4_Byte _tuple = tuple.AsTupleByte();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
            }

            public void Set(float x, float y, float z, float w)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z), this.converter(w));
            }

            float IOpTuple4_Float.X
            {
                set { this.inner.X = this.converter(value); }
            }

            float IOpTuple4_Float.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            float IOpTuple4_Float.Z
            {
                set { this.inner.Z = this.converter(value); }
            }

            float IOpTuple4_Float.W
            {
                set { this.inner.W = this.converter(value); }
            }
        }

        #endregion

        #region Integer <-> Byte

        public delegate int ByteToInteger(byte v);

        public delegate byte IntegerToByte(int v);

        [ConvertHelper(SourceType1 = typeof(ITuple2_Integer), DestinationType = typeof(ITuple2_Byte))]
        public class IntegerToByte2 : ITuple2_Byte
        {
            public IntegerToByte2(ITuple2_Integer inner)
                : this(inner, v => (byte)v)
            {
            }

            public IntegerToByte2(ITuple2_Integer inner, IntegerToByte converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple2_Integer inner;
            private readonly IntegerToByte converter;

            public void Get(IOpTuple2 setter)
            {
                this.inner.Get(setter);
            }

            public byte X
            {
                get { return this.converter(this.inner.X); }
            }

            public byte Y
            {
                get { return this.converter(this.inner.Y); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple2_Integer), DestinationType = typeof(IOpTuple2_Byte))]
        public class IntegerToByteRW2 : IntegerToByte2, IOpTuple2_Byte
        {
            public IntegerToByteRW2(IOpTuple2_Integer inner)
                : this(inner, v => (byte)v, v => (int)v)
            {
            }

            public IntegerToByteRW2(IOpTuple2_Integer inner, IntegerToByte converterBase, ByteToInteger converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple2_Integer inner;
            private readonly ByteToInteger converter;

            public void Set(ITuple2 tuple)
            {
                ITuple2_Integer _tuple = tuple.AsTupleInteger();
                this.inner.Set(_tuple.X, _tuple.Y);
            }

            public void Set(byte x, byte y)
            {
                this.inner.Set(this.converter(x), this.converter(y));
            }

            byte IOpTuple2_Byte.X
            {
                set { this.inner.X = this.converter(value); }
            }

            byte IOpTuple2_Byte.Y
            {
                set { this.inner.Y = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple2_Byte), DestinationType = typeof(ITuple2_Integer))]
        public class ByteToInteger2 : ITuple2_Integer
        {
            public ByteToInteger2(ITuple2_Byte inner)
                : this(inner, v => (int)v)
            {
            }

            public ByteToInteger2(ITuple2_Byte inner, ByteToInteger converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple2_Byte inner;
            private readonly ByteToInteger converter;

            public void Get(IOpTuple2 setter)
            {
                this.inner.Get(setter);
            }

            public int X
            {
                get { return this.converter(this.inner.X); }
            }

            public int Y
            {
                get { return this.converter(this.inner.Y); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple2_Byte), DestinationType = typeof(IOpTuple2_Integer))]
        public class ByteToIntegerRW2 : ByteToInteger2, IOpTuple2_Integer
        {
            public ByteToIntegerRW2(IOpTuple2_Byte inner)
                : this(inner, v => (int)v, v => (byte)v)
            {
            }

            public ByteToIntegerRW2(IOpTuple2_Byte inner, ByteToInteger converterBase, IntegerToByte converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple2_Byte inner;
            private readonly IntegerToByte converter;

            public void Set(ITuple2 tuple)
            {
                ITuple2_Byte _tuple = tuple.AsTupleByte();
                this.inner.Set(_tuple.X, _tuple.Y);
            }

            public void Set(int x, int y)
            {
                this.inner.Set(this.converter(x), this.converter(y));
            }

            int IOpTuple2_Integer.X
            {
                set { this.inner.X = this.converter(value); }
            }

            int IOpTuple2_Integer.Y
            {
                set { this.inner.Y = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Integer), DestinationType = typeof(ITuple3_Byte))]
        public class IntegerToByte3 : ITuple3_Byte
        {
            public IntegerToByte3(ITuple3_Integer inner)
                : this(inner, v => (byte)v)
            {
            }

            public IntegerToByte3(ITuple3_Integer inner, IntegerToByte converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple3_Integer inner;
            private readonly IntegerToByte converter;

            public void Get(IOpTuple3 setter)
            {
                this.inner.Get(setter);
            }

            public byte X
            {
                get { return this.converter(this.inner.X); }
            }

            public byte Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public byte Z
            {
                get { return this.converter(this.inner.Z); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Integer), DestinationType = typeof(IOpTuple3_Byte))]
        public class IntegerToByteRW3 : IntegerToByte3, IOpTuple3_Byte
        {
            public IntegerToByteRW3(IOpTuple3_Integer inner)
                : this(inner, v => (byte)v, v => (int)v)
            {
            }

            public IntegerToByteRW3(IOpTuple3_Integer inner, IntegerToByte converterBase, ByteToInteger converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple3_Integer inner;
            private readonly ByteToInteger converter;

            public void Set(ITuple3 tuple)
            {
                ITuple3_Integer _tuple = tuple.AsTupleInteger();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z);
            }

            public void Set(byte x, byte y, byte z)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z));
            }

            byte IOpTuple3_Byte.X
            {
                set { this.inner.X = this.converter(value); }
            }

            byte IOpTuple3_Byte.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            byte IOpTuple3_Byte.Z
            {
                set { this.inner.Z = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Byte), DestinationType = typeof(ITuple3_Integer))]
        public class ByteToInteger3 : ITuple3_Integer
        {
            public ByteToInteger3(ITuple3_Byte inner)
                : this(inner, v => (int)v)
            {
            }

            public ByteToInteger3(ITuple3_Byte inner, ByteToInteger converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple3_Byte inner;
            private readonly ByteToInteger converter;

            public void Get(IOpTuple3 setter)
            {
                this.inner.Get(setter);
            }

            public int X
            {
                get { return this.converter(this.inner.X); }
            }

            public int Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public int Z
            {
                get { return this.converter(this.inner.Z); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Byte), DestinationType = typeof(IOpTuple3_Integer))]
        public class ByteToIntegerRW3 : ByteToInteger3, IOpTuple3_Integer
        {
            public ByteToIntegerRW3(IOpTuple3_Byte inner)
                : this(inner, v => (int)v, v => (byte)v)
            {
            }

            public ByteToIntegerRW3(IOpTuple3_Byte inner, ByteToInteger converterBase, IntegerToByte converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple3_Byte inner;
            private readonly IntegerToByte converter;

            public void Set(ITuple3 tuple)
            {
                ITuple3_Byte _tuple = tuple.AsTupleByte();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z);
            }

            public void Set(int x, int y, int z)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z));
            }

            int IOpTuple3_Integer.X
            {
                set { this.inner.X = this.converter(value); }
            }

            int IOpTuple3_Integer.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            int IOpTuple3_Integer.Z
            {
                set { this.inner.Z = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Integer), DestinationType = typeof(ITuple4_Byte))]
        public class IntegerToByte4 : ITuple4_Byte
        {
            public IntegerToByte4(ITuple4_Integer inner)
                : this(inner, v => (byte)v)
            {
            }

            public IntegerToByte4(ITuple4_Integer inner, IntegerToByte converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple4_Integer inner;
            private readonly IntegerToByte converter;

            public void Get(IOpTuple4 setter)
            {
                this.inner.Get(setter);
            }

            public byte X
            {
                get { return this.converter(this.inner.X); }
            }

            public byte Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public byte Z
            {
                get { return this.converter(this.inner.Z); }
            }

            public byte W
            {
                get { return this.converter(this.inner.W); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Integer), DestinationType = typeof(IOpTuple4_Byte))]
        public class IntegerToByteRW4 : IntegerToByte4, IOpTuple4_Byte
        {
            public IntegerToByteRW4(IOpTuple4_Integer inner)
                : this(inner, v => (byte)v, v => (int)v)
            {
            }

            public IntegerToByteRW4(IOpTuple4_Integer inner, IntegerToByte converterBase, ByteToInteger converter)
                : base(inner, converterBase)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly IOpTuple4_Integer inner;
            private readonly ByteToInteger converter;

            public void Set(ITuple4 tuple)
            {
                ITuple4_Integer _tuple = tuple.AsTupleInteger();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
            }

            public void Set(byte x, byte y, byte z, byte w)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z), this.converter(w));
            }

            byte IOpTuple4_Byte.X
            {
                set { this.inner.X = this.converter(value); }
            }

            byte IOpTuple4_Byte.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            byte IOpTuple4_Byte.Z
            {
                set { this.inner.Z = this.converter(value); }
            }

            byte IOpTuple4_Byte.W
            {
                set { this.inner.W = this.converter(value); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Byte), DestinationType = typeof(ITuple4_Integer))]
        public class ByteToInteger4 : ITuple4_Integer
        {
            public ByteToInteger4(ITuple4_Byte inner)
                : this(inner, v => (int)v)
            {
            }

            public ByteToInteger4(ITuple4_Byte inner, ByteToInteger converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            private readonly ITuple4_Byte inner;
            private readonly ByteToInteger converter;

            public void Get(IOpTuple4 setter)
            {
                this.inner.Get(setter);
            }

            public int X
            {
                get { return this.converter(this.inner.X); }
            }

            public int Y
            {
                get { return this.converter(this.inner.Y); }
            }

            public int Z
            {
                get { return this.converter(this.inner.Z); }
            }

            public int W
            {
                get { return this.converter(this.inner.W); }
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Byte), DestinationType = typeof(IOpTuple4_Integer))]
        public class ByteToIntegerRW4 : ByteToInteger4, IOpTuple4_Integer
        {
            public ByteToIntegerRW4(IOpTuple4_Byte inner)
                : this(inner, v => (int)v, v => (byte)v)
            {
            }

            public ByteToIntegerRW4(IOpTuple4_Byte inner, ByteToInteger converterBase, IntegerToByte converter)
                : base(inner, converterBase)
            {
                this.converter = converter;
            }

            private readonly IOpTuple4_Byte inner;
            private readonly IntegerToByte converter;

            public void Set(ITuple4 tuple)
            {
                ITuple4_Byte _tuple = tuple.AsTupleByte();
                this.inner.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
            }

            public void Set(int x, int y, int z, int w)
            {
                this.inner.Set(this.converter(x), this.converter(y), this.converter(z), this.converter(w));
            }

            int IOpTuple4_Integer.X
            {
                set { this.inner.X = this.converter(value); }
            }

            int IOpTuple4_Integer.Y
            {
                set { this.inner.Y = this.converter(value); }
            }

            int IOpTuple4_Integer.Z
            {
                set { this.inner.Z = this.converter(value); }
            }

            int IOpTuple4_Integer.W
            {
                set { this.inner.W = this.converter(value); }
            }
        }

        #endregion

        #region Color Float <-> Byte

        [ConvertHelper(SourceType1 = typeof(ITuple3_Float), SourceType2 = typeof(IColor3), DestinationType = typeof(ITuple3_Byte), Weight = -1000)]
        public class ColorFloatToByte3 : FloatToByte3
        {
            public ColorFloatToByte3(ITuple3_Float inner)
                : base(inner, ColorUtils.ToChannelByte)
            {
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Float), SourceType2 = typeof(IOpColor3), DestinationType = typeof(IOpTuple3_Byte), Weight = -1000)]
        public class ColorFloatToByteRW3 : FloatToByteRW3
        {
            public ColorFloatToByteRW3(IOpTuple3_Float inner)
                : base(inner, ColorUtils.ToChannelByte, ColorUtils.ToChannelFloat)
            {
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Byte), SourceType2 = typeof(IColor3), DestinationType = typeof(ITuple3_Float), Weight = -1000)]
        public class ColorByteToFloat3 : ByteToFloat3
        {
            public ColorByteToFloat3(ITuple3_Byte inner)
                : base(inner, ColorUtils.ToChannelFloat)
            {
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple3_Byte), SourceType2 = typeof(IOpColor3), DestinationType = typeof(IOpTuple3_Float), Weight = -1000)]
        public class ColorByteToFloatRW3 : ByteToFloatRW3
        {
            public ColorByteToFloatRW3(IOpTuple3_Byte inner)
                : base(inner, ColorUtils.ToChannelFloat, ColorUtils.ToChannelByte)
            {
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Float), SourceType2 = typeof(IColor4), DestinationType = typeof(ITuple4_Byte), Weight = -1000)]
        public class ColorFloatToByte4 : FloatToByte4
        {
            public ColorFloatToByte4(ITuple4_Float inner)
                : base(inner, ColorUtils.ToChannelByte)
            {
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Float), SourceType2 = typeof(IOpColor4), DestinationType = typeof(IOpTuple4_Byte), Weight = -1000)]
        public class ColorFloatToByteRW4 : FloatToByteRW4
        {
            public ColorFloatToByteRW4(IOpTuple4_Float inner)
                : base(inner, ColorUtils.ToChannelByte, ColorUtils.ToChannelFloat)
            {
            }
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Byte), SourceType2 = typeof(IColor4), DestinationType = typeof(ITuple4_Float), Weight = -1000)]
        public class ColorByteToFloat4 : ByteToFloat4
        {
            public ColorByteToFloat4(ITuple4_Byte inner)
                : base(inner, ColorUtils.ToChannelFloat)
            {
            }
        }

        [ConvertHelper(SourceType1 = typeof(IOpTuple4_Byte), SourceType2 = typeof(IOpColor4), DestinationType = typeof(IOpTuple4_Float), Weight = -1000)]
        public class ColorByteToFloatRW4 : ByteToFloatRW4
        {
            public ColorByteToFloatRW4(IOpTuple4_Byte inner)
                : base(inner, ColorUtils.ToChannelFloat, ColorUtils.ToChannelByte)
            {
            }
        }

        #endregion
    }
}