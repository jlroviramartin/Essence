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
    public sealed class Transform2DMatrix : Transform2D
    {
        public Transform2DMatrix(Matrix2x3d matrix, bool share = true)
        {
            this.Matrix = (share ? matrix : matrix.Clone());
        }

        public Transform2DMatrix(double a, double b, double tx,
                                 double c, double d, double ty)
        {
            this.Matrix = new Matrix2x3d(a, b, tx,
                                         c, d, ty);
        }

        public override IVector2D Transform(IVector2D v)
        {
            return this.Matrix.Mul(v.ToVector2d());
        }

        public override IPoint2D Transform(IPoint2D p)
        {
            return this.Matrix.Mul(p.ToPoint2d());
        }

        public override ITransform2D Concat(ITransform2D transform)
        {
            if (transform is Transform2DIdentity)
            {
                return transform;
            }

            Transform2DMatrix tmatrix = transform as Transform2DMatrix;
            if (tmatrix == null)
            {
                throw new NotImplementedException();
            }

            Matrix2x3d result = this.Matrix.Clone();
            result.Mul(tmatrix.Matrix);
            return new Transform2DMatrix(result, true);
        }

        public override ITransform2D Inv
        {
            get
            {
                if (this.inv == null)
                {
                    Matrix2x3d aux = this.Matrix.Clone();
                    aux.Inv();
                    this.inv = new Transform2DMatrix(aux);
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

        public Matrix2x3d Matrix { get; }

        #region private

        private Transform2DMatrix inv;

        #endregion
    }
}