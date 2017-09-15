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

using Essence.Util.Math.Double;
using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text;
using Essence.Util.Math;

namespace Essence.Geometry.Core.Double
{
    // http://www.geometrictools.com/LibMathematics/Algebra/Algebra.html
    // http://content.gpwiki.org/index.php/OpenGL:Tutorials:Using_Quaternions_to_represent_rotation
    // http://www.euclideanspace.com/maths/geometry/rotations/conversions/eulerToMatrix/index.htm
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Quaternion : IFormattable, IEpsilonEquatable<Quaternion>
    {
        public const string _W = "W";
        public const string _X = "X";
        public const string _Y = "Y";
        public const string _Z = "Z";

        public static Quaternion ZERO = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        public static Quaternion IDENTITY = new Quaternion(1.0f, 0.0f, 0.0f, 0.0f);

        public Quaternion(double[] values)
        {
            this.W = values[0];
            this.X = values[1];
            this.Y = values[2];
            this.Z = values[3];
        }

        public Quaternion(double w, double x, double y, double z)
        {
            this.W = w;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Quaternion(double w, Vector3d v)
        {
            this.W = w;
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
        }

        public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return this.W;
                    case 1:
                        return this.X;
                    case 2:
                        return this.Y;
                    case 3:
                        return this.Z;
                }
                throw new IndexOutOfRangeException();
            }
        }

        public readonly double W;
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public bool IsNormalized
        {
            get { return this.SquaredLength().EpsilonEquals(1.0); }
        }

        public bool IsValid
        {
            get { return !this.SquaredLength().EpsilonZero(); }
        }

        public bool IsZero
        {
            get { return this.EpsilonEquals(ZERO); }
        }

        public bool IsIdentity
        {
            get { return this.EpsilonEquals(IDENTITY); }
        }

        #region Arithmetic operations

        public static Quaternion operator +(Quaternion @this, Quaternion q)
        {
            return new Quaternion(@this.W + q.W,
                                  @this.X + q.X,
                                  @this.Y + q.Y,
                                  @this.Z + q.Z);
        }

        public static Quaternion operator -(Quaternion @this, Quaternion q)
        {
            return new Quaternion(@this.W - q.W,
                                  @this.X - q.X,
                                  @this.Y - q.Y,
                                  @this.Z - q.Z);
        }

        public static Quaternion operator *(Quaternion @this, Quaternion q)
        {
            // NOTE:  Multiplication is not generally commutative, so in most
            // cases p*q != q*p.
            return new Quaternion(@this.W * q.W - @this.X * q.X - @this.Y * q.Y - @this.Z * q.Z,
                                  @this.W * q.X + @this.X * q.W + @this.Y * q.Z - @this.Z * q.Y,
                                  @this.W * q.Y + @this.Y * q.W + @this.Z * q.X - @this.X * q.Z,
                                  @this.W * q.Z + @this.Z * q.W + @this.X * q.Y - @this.Y * q.X);
        }

        public static Quaternion operator *(Quaternion @this, double scalar)
        {
            return new Quaternion(scalar * @this.W,
                                  scalar * @this.X,
                                  scalar * @this.Y,
                                  scalar * @this.Z);
        }

        public static Quaternion operator *(double scalar, Quaternion @this)
        {
            return @this * scalar;
        }

        public static Quaternion operator /(Quaternion @this, double scalar)
        {
            if (scalar.EpsilonZero())
            {
                return new Quaternion(double.MaxValue,
                                      double.MaxValue,
                                      double.MaxValue,
                                      double.MaxValue);
            }

            double invScalar = 1.0 / scalar;
            return new Quaternion(invScalar * @this.W,
                                  invScalar * @this.X,
                                  invScalar * @this.Y,
                                  invScalar * @this.Z);
        }

        public static Quaternion operator -(Quaternion @this)
        {
            return new Quaternion(-@this.W,
                                  -@this.X,
                                  -@this.Y,
                                  -@this.Z);
        }

        #endregion

        #region Conversion between quaternions, matrices, and axis-angle

        public static Quaternion FromRotationMatrix(Matrix3x3d rot)
        {
            // Algorithm in Ken Shoemake's article in 1987 SIGGRAPH course notes
            // article "Quaternion Calculus and Fast Animation".

            int[] next = { 1, 2, 0 };

            double[] mTuple = new double[4];

            double trace = rot[0, 0] + rot[1, 1] + rot[2, 2];

            if (trace > 0.0)
            {
                // |w| > 1/2, may as well choose w > 1/2
                double root = Math.Sqrt(trace + 1.0); // 2w
                double root2 = 0.5 / root; // 1/(4w)

                mTuple[0] = 0.5 * root;
                mTuple[1] = (rot[2, 1] - rot[1, 2]) * root2;
                mTuple[2] = (rot[0, 2] - rot[2, 0]) * root2;
                mTuple[3] = (rot[1, 0] - rot[0, 1]) * root2;
            }
            else
            {
                // |w| <= 1/2
                int i = 0;
                if (rot[1, 1] > rot[0, 0])
                {
                    i = 1;
                }
                if (rot[2, 2] > rot[i, i])
                {
                    i = 2;
                }
                int j = next[i];
                int k = next[j];

                double root = Math.Sqrt(rot[i, i] - rot[j, j] - rot[k, k] + 1.0);
                double root2 = 0.5 / root;

                int[] quat = new int[] { 1, 2, 3 };

                mTuple[quat[i]] = 0.5 * root;
                mTuple[0] = (rot[k, j] - rot[j, k]) * root2;
                mTuple[quat[j]] = (rot[j, i] + rot[i, j]) * root2;
                mTuple[quat[k]] = (rot[k, i] + rot[i, k]) * root2;
            }
            return new Quaternion(mTuple);
        }

        public Matrix3x3d ToRotationMatrix()
        {
            double length2 = this.SquaredLength();

            double rlength2;
            if (!length2.EpsilonEquals(1.0))
            {
                rlength2 = 2.0 / length2;
            }
            else
            {
                rlength2 = 2.0;
            }

            double x2 = rlength2 * this.X;
            double y2 = rlength2 * this.Y;
            double z2 = rlength2 * this.Z;
            double xx = x2 * this.X;
            double xy = y2 * this.X;
            double xz = z2 * this.X;
            double yy = y2 * this.Y;
            double yz = z2 * this.Y;
            double zz = z2 * this.Z;
            double wx = x2 * this.W;
            double wy = y2 * this.W;
            double wz = z2 * this.W;

            return new Matrix3x3d(1.0 - (yy + zz), xy - wz, xz + wy,
                                  xy + wz, 1.0 - (xx + zz), yz - wx,
                                  xz - wy, yz + wx, 1.0 - (xx + yy));
        }

        public static Quaternion FromRotationMatrix(Vector3d[ /*3*/] rotColumn)
        {
            return FromRotationMatrix(new Matrix3x3d(rotColumn[0].X, rotColumn[1].X, rotColumn[2].X,
                                                     rotColumn[0].Y, rotColumn[1].Y, rotColumn[2].Y,
                                                     rotColumn[0].Z, rotColumn[1].Z, rotColumn[2].Z));
        }

        public void ToRotationMatrix(Vector3d[ /*3*/] rotColumn)
        {
            Matrix3x3d mat = this.ToRotationMatrix();
            rotColumn[0] = new Vector3d(mat[0, 0], mat[1, 0], mat[2, 0]);
            rotColumn[1] = new Vector3d(mat[0, 1], mat[1, 1], mat[2, 1]);
            rotColumn[2] = new Vector3d(mat[0, 2], mat[1, 2], mat[2, 2]);
        }

        public static Quaternion FromAxisAngle(Vector3d axis, double angle)
        {
            Contract.Requires(axis.IsUnit);

            // assert:  axis[] is unit length
            //
            // The quaternion representing the rotation is
            //   q = cos(A/2)+sin(A/2)*(x*i+y*j+z*k)

            double halfAngle = 0.5 * angle;
            double sn = Math.Sin(halfAngle);
            return new Quaternion(Math.Cos(halfAngle),
                                  sn * axis.X,
                                  sn * axis.Y,
                                  sn * axis.Z);
        }

        public void ToAxisAngle(out Vector3d axis, out double angle)
        {
            // The quaternion representing the rotation is
            //   q = cos(A/2)+sin(A/2)*(x*i+y*j+z*k)

            double sqrLength = this.X * this.X + this.Y * this.Y + this.Z * this.Z;

            if (sqrLength.EpsilonZero())
            {
                // Angle is 0 (mod 2*pi), so any axis will do.
                angle = 0.0;
                axis = new Vector3d(1.0, 0.0, 0.0);
                return;
            }

            angle = 2.0 * Math.Acos(this.W);
            double invLength = 1.0 / Math.Sqrt(sqrLength);
            axis = new Vector3d(this.X * invLength, this.Y * invLength, this.Z * invLength);
        }

        // Convert from Euler Angles
        public Quaternion FromEuler(double heading, double attitude, double bank)
        {
            // Assuming the angles are in radians.
            double c1 = Math.Cos(heading / 2);
            double s1 = Math.Sin(heading / 2);
            double c2 = Math.Cos(attitude / 2);
            double s2 = Math.Sin(attitude / 2);
            double c3 = Math.Cos(bank / 2);
            double s3 = Math.Sin(bank / 2);
            double c1c2 = c1 * c2;
            double s1s2 = s1 * s2;
            return new Quaternion(c1c2 * c3 - s1s2 * s3,
                                  c1c2 * s3 + s1s2 * c3,
                                  s1 * c2 * c3 + c1 * s2 * s3,
                                  c1 * s2 * c3 - s1 * c2 * s3);
        }

        public void ToEuler(out double heading, out double attitude, out double bank)
        {
            double sqw = this.W * this.W;
            double sqx = this.X * this.X;
            double sqy = this.Y * this.Y;
            double sqz = this.Z * this.Z;
            double unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            double test = this.X * this.Y + this.Z * this.W;

            if (test > 0.499 * unit)
            {
                // singularity at north pole
                heading = 2 * Math.Atan2(this.X, this.W);
                attitude = Math.PI / 2;
                bank = 0;
                return;
            }

            if (test < -0.499 * unit)
            {
                // singularity at south pole
                heading = -2 * Math.Atan2(this.X, this.W);
                attitude = -Math.PI / 2;
                bank = 0;
                return;
            }

            heading = Math.Atan2(2 * this.Y * this.W - 2 * this.X * this.Z, sqx - sqy - sqz + sqw);
            attitude = Math.Asin(2 * test / unit);
            bank = Math.Atan2(2 * this.X * this.W - 2 * this.Y * this.Z, -sqx + sqy - sqz + sqw);
        }

        #endregion

        #region Functions of a quaternion

        // length of 4-tuple
        public double Length()
        {
            return Math.Sqrt(this.SquaredLength());
        }

        // squared length of 4-tuple
        public double SquaredLength()
        {
            return this.W * this.W + this.X * this.X + this.Y * this.Y + this.Z * this.Z;
        }

        // dot product of 4-tuples
        public double Dot(Quaternion q)
        {
            return this.W * q.W + this.X * q.X + this.Y * q.Y + this.Z * q.Z;
        }

        public Quaternion Normalize(double epsilon = MathUtils.ZERO_TOLERANCE)
        {
            double length;
            return this.Normalize(out length, epsilon);
        }

        public Quaternion Normalize(out double length, double epsilon = MathUtils.ZERO_TOLERANCE)
        {
            double sqLength = this.SquaredLength();

            // Ya esta normalizado.
            if (sqLength.EpsilonEquals(1.0))
            {
                length = 1.0;
                return this;
            }

            if (sqLength.EpsilonZero(epsilon))
            {
                length = 0.0;
                return new Quaternion(0.0, 0.0, 0.0, 0.0);
            }

            length = Math.Sqrt(sqLength);

            double invLength = 1.0 / length;
            return new Quaternion(this.W * invLength,
                                  this.X * invLength,
                                  this.Y * invLength,
                                  this.Z * invLength);
        }

        // apply to non-zero quaternion
        public Quaternion Inverse()
        {
            double norm = this.SquaredLength();

            if (norm.EpsilonZero())
            {
                // Return an invalid result to flag the error.
                return new Quaternion(0.0, 0.0, 0.0, 0.0);
            }

            double invNorm = 1.0 / norm;
            return new Quaternion(this.W * invNorm,
                                  -this.X * invNorm,
                                  -this.Y * invNorm,
                                  -this.Z * invNorm);
        }

        // negate x, y, and z terms
        public Quaternion Conjugate()
        {
            return new Quaternion(this.W, -this.X, -this.Y, -this.Z);
        }

        // apply to quaternion with w = 0
        public Quaternion Exp()
        {
            // If q = A*(x*i+y*j+z*k) where (x,y,z) is unit length, then
            // exp(q) = cos(A)+sin(A)*(x*i+y*j+z*k).  If sin(A) is near zero,
            // use exp(q) = cos(A)+A*(x*i+y*j+z*k) since A/sin(A) has limit 1.

            double angle = Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
            double sn = Math.Sin(angle);
            double cn = Math.Cos(angle);

            if (sn.EpsilonEquals(0, MathUtils.ZERO_TOLERANCE))
            {
                return new Quaternion(cn, this.X, this.Y, this.Z);
            }

            double coeff = sn / angle;
            return new Quaternion(cn,
                                  coeff * this.X,
                                  coeff * this.Y,
                                  coeff * this.Z);
        }

        // apply to unit-length quaternion
        public Quaternion Log()
        {
            // If q = cos(A)+sin(A)*(x*i+y*j+z*k) where (x,y,z) is unit length, then
            // log(q) = A*(x*i+y*j+z*k).  If sin(A) is near zero, use log(q) =
            // sin(A)*(x*i+y*j+z*k) since sin(A)/A has limit 1.

            if (Math.Abs(this.W) >= 1.0)
            {
                return new Quaternion(0, this.X, this.Y, this.Z);
            }

            double angle = Math.Acos(this.W);
            double sn = Math.Sin(angle);

            if (sn.EpsilonEquals(0, MathUtils.ZERO_TOLERANCE))
            {
                return new Quaternion(0, this.X, this.Y, this.Z);
            }

            double coeff = angle / sn;
            return new Quaternion(0,
                                  coeff * this.X,
                                  coeff * this.Y,
                                  coeff * this.Z);
        }

        #endregion

        public Vector3d Rotate2(Vector3d vec)
        {
            Vector3d aux = vec.Unit;

            Quaternion result = this * new Quaternion(0, aux) * this.Conjugate();
            return new Vector3d(result.X, result.Y, result.Z);
        }

        // Rotation of a vector by a quaternion.
        public Vector3d Rotate(Vector3d vec)
        {
            // Given a vector u = (x0,y0,z0) and a unit length quaternion
            // q = <w,x,y,z>, the vector v = (x1,y1,z1) which represents the
            // rotation of u by q is v = q*u*q^{-1} where * indicates quaternion
            // multiplication and where u is treated as the quaternion <0,x0,y0,z0>.
            // Note that q^{-1} = <w,-x,-y,-z>, so no real work is required to
            // invert q.  Now
            //
            //   q*u*q^{-1} = q*<0,x0,y0,z0>*q^{-1}
            //     = q*(x0*i+y0*j+z0*k)*q^{-1}
            //     = x0*(q*i*q^{-1})+y0*(q*j*q^{-1})+z0*(q*k*q^{-1})
            //
            // As 3-vectors, q*i*q^{-1}, q*j*q^{-1}, and 2*k*q^{-1} are the columns
            // of the rotation matrix computed in Quaternion<double>::ToRotationMatrix.
            // The vector v is obtained as the product of that rotation matrix with
            // vector u.  As such, the quaternion representation of a rotation
            // matrix requires less space than the matrix and more time to compute
            // the rotated vector.  Typical space-time tradeoff...

            Matrix3x3d rot = this.ToRotationMatrix();
            return rot * vec;
        }

        // Compute a quaternion that rotates unit-length vector V1 to unit-length
        // vector V2.  The rotation is about the axis perpendicular to both V1 and
        // V2, with angle of that between V1 and V2.  If V1 and V2 are parallel,
        // any axis of rotation will do, such as the permutation (z2,x2,y2), where
        // V2 = (x2,y2,z2).
        public Quaternion Align(Vector3d v1, Vector3d v2)
        {
            // If V1 and V2 are not parallel, the axis of rotation is the unit-length
            // vector U = Cross(V1,V2)/Length(Cross(V1,V2)).  The angle of rotation,
            // A, is the angle between V1 and V2.  The quaternion for the rotation is
            // q = cos(A/2) + sin(A/2)*(ux*i+uy*j+uz*k) where U = (ux,uy,uz).
            //
            // (1) Rather than extract A = acos(Dot(V1,V2)), multiply by 1/2, then
            //     compute sin(A/2) and cos(A/2), we reduce the computational costs by
            //     computing the bisector B = (V1+V2)/Length(V1+V2), so cos(A/2) =
            //     Dot(V1,B).
            //
            // (2) The rotation axis is U = Cross(V1,B)/Length(Cross(V1,B)), but
            //     Length(Cross(V1,B)) = Length(V1)*Length(B)*sin(A/2) = sin(A/2), in
            //     which case sin(A/2)*(ux*i+uy*j+uz*k) = (cx*i+cy*j+cz*k) where
            //     C = Cross(V1,B).
            //
            // If V1 = V2, then B = V1, cos(A/2) = 1, and U = (0,0,0).  If V1 = -V2,
            // then B = 0.  This can happen even if V1 is approximately -V2 using
            // floating point arithmetic, since Vector3::Normalize checks for
            // closeness to zero and returns the zero vector accordingly.  The test
            // for exactly zero is usually not recommend for floating point
            // arithmetic, but the implementation of Vector3::Normalize guarantees
            // the comparison is robust.  In this case, the A = pi and any axis
            // perpendicular to V1 may be used as the rotation axis.

            Vector3d bisector = (v1 + v2).Unit;

            double cosHalfAngle = v1.Dot(bisector);

            if (!cosHalfAngle.EpsilonZero())
            {
                Vector3d cross = v1.Cross(bisector);
                return new Quaternion(cosHalfAngle,
                                      cross.X,
                                      cross.Y,
                                      cross.Z);
            }

            if (Math.Abs(v1[0]) >= Math.Abs(v1[1]))
            {
                // V1.x or V1.z is the largest magnitude component.
                double invLength = 1 / Math.Sqrt(v1[0] * v1[0] + v1[2] * v1[2]);
                return new Quaternion(0.0,
                                      -v1[2] * invLength,
                                      0.0,
                                      +v1[0] * invLength);
            }
            else
            {
                // V1.y or V1.z is the largest magnitude component.
                double invLength = 1 / Math.Sqrt(v1[1] * v1[1] + v1[2] * v1[2]);
                return new Quaternion(0.0,
                                      0.0,
                                      +v1[2] * invLength,
                                      -v1[1] * invLength);
            }
        }

        public static Quaternion Lookat(Vector3d d)
        {
            return Lookat(d.X, d.Y, d.Z);
        }

        public static Quaternion Lookat(double x, double y, double z)
        {
            // s is a scaling factor to ensure quat remains unit length
            double s = Math.Sqrt(1 / (x * x + y * y + z * z));
            double xx = x * s;
            double yy = y * s;
            double zz = z * s;
            return new Quaternion(1, xx, yy, zz);
        }

        #region private

        private const double PIOVER180 = Math.PI / 180;

        #endregion

        #region Object

        public override string ToString()
        {
            return this.ToString("F3", null);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Quaternion))
            {
                return false;
            }

            Quaternion q = (Quaternion)obj;
            return this.EpsilonEquals(q, MathUtils.ZERO_TOLERANCE);
        }

        public override int GetHashCode()
        {
            // http://www.jarvana.com/jarvana/view/org/apache/lucene/lucene-spatial/2.9.3/lucene-spatial-2.9.3-sources.jar!/org/apache/lucene/spatial/geometry/shape/Vector2D.java
            const int prime = 31;
            int hash = 1;
            unchecked
            {
                hash = prime * hash + this.W.GetHashCode();
                hash = prime * hash + this.X.GetHashCode();
                hash = prime * hash + this.Y.GetHashCode();
                hash = prime * hash + this.Z.GetHashCode();
            }
            return hash;
        }

        #endregion

        #region IEpsilonEquatable<Vector2d>

        /// <summary>
        ///     Comprueba si este vector es igual a <c>vector</c>
        ///     con un error <c>epsilon</c>.
        /// </summary>
        /// <param name="vector">Vector.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si son iguales.</returns>
        public bool EpsilonEquals(Quaternion q, double epsilon = MathUtils.EPSILON)
        {
            return (this.W.EpsilonEquals(q.W, epsilon)
                    && this.X.EpsilonEquals(q.X, epsilon)
                    && this.Y.EpsilonEquals(q.Y, epsilon)
                    && this.Z.EpsilonEquals(q.Z, epsilon));
        }

        #endregion

        #region IFormattable

        /// <summary>
        ///     Formatea este vector segun el proveedor indicado.
        ///     Busca en <c>IFormatProvider</c> los formatos:
        ///     <c>ICustomFormatter</c>: delega el formateo en este objeto.
        ///     <c>VectorFormatInfo</c>: Obtiene la informacion de formato.
        /// </summary>
        public string ToString(string formato, IFormatProvider proveedor)
        {
            if (proveedor != null)
            {
                ICustomFormatter formatter = proveedor.GetFormat(this.GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    return formatter.Format(formato, this, proveedor);
                }
            }

            char f = 'G';
            if (!string.IsNullOrEmpty(formato))
            {
                formato = formato.ToUpper();
                f = char.ToUpper(formato[0]);
            }

            switch (f)
            {
                case 'G':
                default:
                    return new StringBuilder()
                        .AppendFormat("W: ").AppendLine(this.W.ToString("F3"))
                        .AppendFormat("X: ").AppendLine(this.X.ToString("F3"))
                        .AppendFormat("Y: ").AppendLine(this.Y.ToString("F3"))
                        .AppendFormat("Z: ").AppendLine(this.Z.ToString("F3"))
                        .ToString();
            }
        }

        #endregion
    }
}