using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Points;
using SudokuSolver.SudokuSolverCore.Solution;
using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms.Subsets
{
    public class HiddenQuadrupleAlgorithm : SudokuSolvingAlgorithm
    {
        public HiddenQuadrupleAlgorithm()
        {
            this.TypeAlgorithm = TypeAlgorithmSolution.HiddenQuadrupleAlgorithm;
        }
        private SolutionMethod GetHiddenQuadrupleInRange<TPointMatrix>(GridSudoku<TPointMatrix> sudoku, RangeMatrix range) where TPointMatrix : IPointMatrix, new()
        {
            for (byte num1 = 1; num1 < 10; num1++)
            {
                for (byte num2 = 1; num2 < 10; num2++)
                {
                    for (byte num3 = 1; num3 < 10; num3++)
                    {
                        for (byte num4 = 1; num4 < 10; num4++)
                        {
                            int count_num1 = sudoku.GetCountPossiblePosPointInRange(range, num1);
                            int count_num2 = sudoku.GetCountPossiblePosPointInRange(range, num2);
                            int count_num3 = sudoku.GetCountPossiblePosPointInRange(range, num3);
                            int count_num4 = sudoku.GetCountPossiblePosPointInRange(range, num4);
                            if (num1 != num2 && num2 != num3 && num1 != num3 && num1 != num4 && num2 != num4 && num3 != num4
                                && count_num1 <= 4 && count_num1 >= 2 && count_num2 <= 4 && count_num2 >= 2
                                && count_num3 <= 4 && count_num3 >= 2 && count_num4 <= 4 && count_num4 >= 2)
                            {
                                Set<PosPoint> poss_num = new Set<PosPoint>(sudoku.GetPossPosPointsInRange(range, num1))
                                    + new Set<PosPoint>(sudoku.GetPossPosPointsInRange(range, num2))
                                    + new Set<PosPoint>(sudoku.GetPossPosPointsInRange(range, num3))
                                    + new Set<PosPoint>(sudoku.GetPossPosPointsInRange(range, num4));
                                if (poss_num.Count() == 4)
                                {
                                    Set<byte> set_other = new Set<byte>();
                                    Set<byte> values = new Set<byte>();
                                    for (int i = range.Position1.i; i <= range.Position2.i; i++)
                                    {
                                        for (int j = range.Position1.j; j <= range.Position2.j; j++)
                                        {
                                            PosPoint pos = new PosPoint(i, j);
                                            if (sudoku[i, j].value == 0 && !poss_num.Contains(pos))
                                            {
                                                set_other += sudoku[i, j].set;
                                            }
                                            if (poss_num.Contains(pos))
                                            {
                                                values += sudoku[i, j].set;
                                            }
                                        }
                                    }
                                    values = values - set_other;
                                    if (values.Count() == 4)
                                    {
                                        SolutionMethod Solution_method = new SolutionMethod(this.TypeAlgorithm, false)
                                        {
                                            PosPoints = poss_num,
                                            Values = values
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
                }
            }
            return null;
        }
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod quadruple_h = this.GetHiddenQuadrupleInRange(sudoku, RangeMatrix.FormHorizontalLine(i));
                if (!(quadruple_h is null)) { return quadruple_h; };
                SolutionMethod quadruple_v = this.GetHiddenQuadrupleInRange(sudoku, RangeMatrix.FormVerticalLine(i));
                if (!(quadruple_v is null)) { return quadruple_v; };
            }
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    SolutionMethod quadruple_s = this.GetHiddenQuadrupleInRange(sudoku, RangeMatrix.FormSquare(si, sj));
                    if (!(quadruple_s is null)) { return quadruple_s; };
                }
            }
            return null;
        }
    }
}
