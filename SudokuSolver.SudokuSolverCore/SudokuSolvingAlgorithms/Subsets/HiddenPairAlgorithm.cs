using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Points;
using SudokuSolver.SudokuSolverCore.Solution;
using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms.Subsets
{
    public class HiddenPairAlgorithm : SudokuSolvingAlgorithm
    {
        public HiddenPairAlgorithm()
        {
            this.TypeAlgorithm = TypeAlgorithmSolution.HiddenPairAlgorithm;
        }
        private SolutionMethod GetHiddenPairInRange<TPointMatrix>(GridSudoku<TPointMatrix> sudoku, RangeMatrix range) where TPointMatrix : IPointMatrix, new()
        {
            for (byte num1 = 0; num1 < 10; num1++)
            {
                for (byte num2 = 1; num2 < 10; num2++)
                {
                    if (num1 != num2)
                    {
                        int count_num1 = sudoku.GetCountPossiblePosPointInRange(range, num1), count_num2 = sudoku.GetCountPossiblePosPointInRange(range, num2);
                        if (count_num1 == 2 && count_num2 == 2)
                        {
                            Arrange<PosPoint> arr1 = sudoku.GetPossPosPointsInRange(range, num1), arr2 = sudoku.GetPossPosPointsInRange(range, num2);
                            if (arr1 == arr2)
                            {
                                SolutionMethod Solution_method = new SolutionMethod(this.TypeAlgorithm, false)
                                {
                                    PosPoints = new Arrange<PosPoint>(arr1[0], arr1[1]),
                                    Values = new Set<byte>(num1, num2)
                                };
                                if (SolutionMethodHandler.IsValid(sudoku, Solution_method))
                                {
                                    return Solution_method;
                                }
                            }
                        }
                    }

                }
            }
            return null;
        }
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod pair_h = this.GetHiddenPairInRange(sudoku, RangeMatrix.FormHorizontalLine(i));
                if (!(pair_h is null)) { return pair_h; }
                SolutionMethod pair_v = this.GetHiddenPairInRange(sudoku, RangeMatrix.FormVerticalLine(i));
                if (!(pair_v is null)) { return pair_v; }
            }
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    SolutionMethod pair = this.GetHiddenPairInRange(sudoku, RangeMatrix.FormSquare(si, sj));
                    if (!(pair is null)) { return pair; }
                }
            }
            return null;
        }
    }
}
