using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Трансформационна матрица за 2D
    /// </summary>
    public class Matrix
    {
        public float[,] m; // ред, колона

        /// <summary>
        /// Матрица за транслация
        /// </summary>
        /// <param name="dx">транслация по x</param>
        /// <param name="dy">транслация по y</param>
        public Matrix(float dx, float dy)
        {
            m[0, 0] = 1;
            m[0, 1] = 0;
            m[0, 2] = dx;

            m[1, 0] = 0;
            m[1, 1] = 1;
            m[1, 2] = dy;

            m[2, 0] = 0;
            m[2, 1] = 0;
            m[2, 2] = 1;
        }

        /// <summary>
        /// Матрица за мащабиране
        /// </summary>
        /// <param name="scale">мащаба</param>
        public Matrix(float scale)
        {
            m[0, 0] = scale;
            m[0, 1] = 0;
            m[0, 2] = 0;

            m[1, 0] = 0;
            m[1, 1] = scale;
            m[1, 2] = 0;

            m[2, 0] = 0;
            m[2, 1] = 0;
            m[2, 2] = scale == 0.0f ? 0 : 1;
        }

        public static Matrix Multiply(Matrix a, Matrix b)
        {
            Matrix result = new Matrix(0.0f);
            for(int rowIdx = 0; rowIdx < 3; rowIdx++)
            {
                for(int colIdx = 0; colIdx < 3; colIdx++)
                {
                    for (int k = 0; k < 3; k++) result.m[rowIdx, colIdx] += a.m[rowIdx, k] * b.m[k, colIdx];
                }
            }
            return result;
        }
    }
}
