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
    /// <summary>
    ///     Trasnsformacion 2D a partir de una matriz.
    /// </summary>
    public sealed class Transform2DMatrix : Transform2D
    {
        public Transform2DMatrix(Matrix3x3d matrix)
            : this(new Matrix2x3d(matrix), true)
        {
        }

        public Transform2DMatrix(Matrix2x3d matrix, bool share = true)
        {
            this.matrix = (share ? matrix : matrix.Clone());
        }

        public Transform2DMatrix(double a, double b, double tx, double c, double d, double ty)
        {
            this.matrix = new Matrix2x3d(a, b, tx,
                                         c, d, ty);
        }

        public override IVector2D Transform(IVector2D v)
        {
            return this.matrix.Mul(v.ToVector2d());
        }

        public override IPoint2D Transform(IPoint2D p)
        {
            return this.matrix.Mul(p.ToPoint2d());
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

            Matrix2x3d result = this.matrix.Clone();
            result.Mul(tmatrix.matrix);
            return new Transform2DMatrix(result, true);
        }

        public override ITransform2D Inv
        {
            get
            {
                if (this.inv == null)
                {
                    Matrix2x3d aux = this.matrix.Clone();
                    aux.Inv();
                    this.inv = new Transform2DMatrix(aux);
                    this.inv.inv = this;
                }
                return this.inv;
            }
        }

        public override bool IsIdentity
        {
            get { return this.matrix.IsIdentity; }
        }

        #region private

        private readonly Matrix2x3d matrix;
        private Transform2DMatrix inv;

        #endregion
    }
}