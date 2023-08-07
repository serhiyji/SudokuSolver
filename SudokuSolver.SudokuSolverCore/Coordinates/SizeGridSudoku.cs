using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore.Coordinates
{
    public static class SizeGridSudoku
    {
        public static byte SizeMatrixHorizontal { get; private set; } = 9;
        public static byte SizeMatrixVertical { get; private set; } = 9;
        public static byte SizeSquareHorizontal { get; private set; } = 3;
        public static byte SizeSquareVertical { get; private set; } = 3;
        public static byte CountSqareHorizontal { get; private set; } = 3;
        public static byte CountSqareVertical { get; private set; } = 3;
        public static Set<byte> AllStandardPosibleValues { get; private set; } = new Set<byte>(1, 2, 3, 4, 5, 6, 7, 8, 9);
        public static Matrix<byte> ExampleSudoku { get; private set; }
        public static void SetTo6x6Grid()
        {
            SizeMatrixHorizontal = 6;
            SizeMatrixVertical = 6;
            SizeSquareHorizontal = 3;
            SizeSquareVertical = 2;
            CountSqareHorizontal = 2;
            CountSqareVertical = 3;
            AllStandardPosibleValues = new Set<byte>(1, 2, 3, 4, 5, 6);
            ExampleSudoku = new Matrix<byte>();
            ExampleSudoku.matrix = new byte[,]
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
        public static void SetTo9x9Grid()
        {
            SizeMatrixHorizontal = 9;
            SizeMatrixVertical = 9;
            SizeSquareHorizontal = 3;
            SizeSquareVertical = 3;
            CountSqareHorizontal = 3;
            CountSqareVertical = 3;
            AllStandardPosibleValues = new Set<byte>(1, 2, 3, 4, 5, 6, 7, 8, 9);
        }
    }
}
