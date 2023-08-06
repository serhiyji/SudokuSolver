using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Matrix;
using SudokuSolver.SudokuSolverCore.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore.SudokuGridHandlers
{
    public class GenarateSudokuGrid<TPointMatrix> where TPointMatrix : IPointMatrix, new()
    {
        private GridSudoku<TPointMatrix> matrix {  get; set; }
        private readonly Matrix<byte> __example = new Matrix<byte>();
        public GenarateSudokuGrid(ref GridSudoku<TPointMatrix> matrix)
        {
            this.matrix = matrix;
            __example.matrix = new byte[,]
            {
                { 1, 2, 3, 7, 8, 9, 4, 5, 6 },
                { 4, 5, 6, 1, 2, 3, 7, 8, 9 },
                { 7, 8, 9, 4, 5, 6, 1, 2, 3 },

                { 2, 3, 1, 8, 9, 7, 5, 6, 4 },
                { 5, 6, 4, 2, 3, 1, 8, 9, 7 },
                { 8, 9, 7, 5, 6, 4, 2, 3, 1 },

                { 3, 1, 2, 6, 4, 5, 9, 7, 8 },
                { 6, 4, 5, 9, 7, 8, 3, 1, 2 },
                { 9, 7, 8, 3, 1, 2, 6, 4, 5 }
            };
        }

        private void SwapCollomsInMatrix(int col1, int col2)
        {
            if (col1 == col2) return;
            if (col1 < 0 || col2 < 0 || col1 > 8 || col2 > 8) return;
            for (int i = 0; i < 9; i++)
            {
                this.matrix[i, col1].SwapFromOtherPointMatrix(ref this.matrix.matrix[i, col2]);
            }
        }
        private void SwapRowsInMatrix(int row1, int row2)
        {
            if (row1 == row2) return;
            if (row1 < 0 || row2 < 0 || row1 > 8 || row2 > 8) return;
            for (int i = 0; i < 9; i++)
            {
                this.matrix.matrix[row1, i].SwapFromOtherPointMatrix(ref this.matrix.matrix[row2, i]);
            }
        }
        private void SwapColBlocksMatrix(int col1, int col2)
        {
            if (col1 == col2) return;
            if (col1 < 0 || col2 < 0 || col1 > 2 || col2 > 2) return;
            int icol1 = col1 * 3 + 2;
            for (int i = col1 * 3, j = col2 * 3; i <= icol1; i++, j++)
            {
                this.SwapCollomsInMatrix(i, j);
            }
        }
        private void SwapRowBlocksMatrix(int row1, int row2)
        {
            if (row1 == row2) return;
            if (row1 < 0 || row2 < 0 || row1 > 2 || row2 > 2) return;
            int irow1 = row1 * 3 + 2;
            for (int i = row1 * 3, j = row2 * 3; i <= irow1; i++, j++)
            {
                this.SwapRowsInMatrix(i, j);
            }
        }
        private int Rand(Random rand, int start, int stop, int not)
        {
            int val;
            do
            {
                val = rand.Next(start, stop);
            } while (val == not);
            return val;
        }
        private void ToMixMatrix()
        {
            Random random = new Random();
            for (int i = 0; i < 3; i++)
            {
                int jr = this.Rand(random, 0, 3, i);
                this.SwapRowBlocksMatrix(jr, i);
                int jc = this.Rand(random, 0, 3, i);
                this.SwapColBlocksMatrix(jc, i);
            }
            for (int block = 0; block < 3; block++)
            {
                int start = block * 3, stop = block * 3 + 3;
                for (int i = start; i < stop; i++)
                {
                    int jr = this.Rand(random, start, stop, i);
                    this.SwapRowsInMatrix(jr, i);
                    int jc = this.Rand(random, start, stop, i);
                    this.SwapCollomsInMatrix(jc, i);
                }
            }
        }
        public void GenerateNewSudoku(int count = 0)
        {
            Random rand = new Random();
            this.matrix.ClearMatrix();
            this.ToMixMatrix();
            for (int c = 0; c < count; c++)
            {
                int i = rand.Next(0, 9), j = rand.Next(0, 9);
                if (this.matrix[i, j].value == 0)
                {
                    this.matrix.SetValue(new PosPoint(i, j), __example.matrix[i, j]);
                }
                else
                {
                    c--;
                }
            }
            this.matrix.SetPossibleValues();
        }
    }
}
