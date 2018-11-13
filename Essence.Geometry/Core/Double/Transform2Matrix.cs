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

namespace Essence.Geometry.Core.Double
{
    /// <summary>2D transform using a matrix.</summary>
    public sealed class Transform2Matrix : Transform2
    {
        public Transform2Matrix(Matrix3x3d matrix)
        {
            this.Matrix = new Matrix2x3d(matrix.M00, matrix.M01, matrix.M02,
                                         matrix.M10, matrix.M11, matrix.M12);
        }

        public Transform2Matrix(Matrix2x3d matrix, bool share = true)
        {
            this.Matrix = (share ? matrix : matrix.Clone());
        }

        public Transform2Matrix(double a, double b, double tx,
                                double c, double d, double ty)
        {
            this.Matrix = new Matrix2x3d(a, b, tx,
                                         c, d, ty);
        }

        public Matrix2x3d Matrix { get; }

        #region private

        private Transform2Matrix inv;

        #endregion

        #region Transform2

        public override Vector2d Transform(Vector2d v)
        {
            return this.Matrix.Mul(v);
        }

        public override Point2d Transform(Point2d p)
        {
            return this.Matrix.Mul(p);
        }

        #endregion

        #region ITransform2

        public override IVector2 Transform(IVector2 v)
        {
            return this.Matrix.Mul(v);
        }

        public override void Transform(IVector2 v, IOpVector2 vout)
        {
            this.Matrix.Mul(v, vout);
        }

        public override IPoint2 Transform(IPoint2 v)
        {
            return this.Matrix.Mul(v);
        }

        public override void Transform(IPoint2 p, IOpPoint2 pout)
        {
            this.Matrix.Mul(p, pout);
        }

        public override ITransform2 Concat(ITransform2 transform)
        {
            if (transform is Transform2Identity)
            {
                return transform;
            }

            Transform2Matrix tmatrix = transform as Transform2Matrix;
            if (tmatrix == null)
            {
                throw new NotImplementedException();
            }

            Matrix2x3d result = this.Matrix.Clone();
            result.Mul(tmatrix.Matrix);
            return new Transform2Matrix(result, true);
        }

        public override ITransform2 Inv
        {
            get
            {
                if (this.inv == null)
                {
                    Matrix2x3d aux = this.Matrix.Clone();
                    aux.Inv();
                    this.inv = new Transform2Matrix(aux);
                    this.inv.inv = this;
                }
                return this.inv;
            }
        }

        public override bool IsIdentity
        {
            get { return this.Matrix.IsIdentity; }
        }

        public override void GetMatrix(Matrix3x3d matrix)
        {
            matrix.Set(this.Matrix);
        }

        #endregion
    }
}