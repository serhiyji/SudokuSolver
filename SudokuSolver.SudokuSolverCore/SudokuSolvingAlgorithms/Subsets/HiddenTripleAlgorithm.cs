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
    public class HiddenTripleAlgorithm : SudokuSolvingAlgorithm
    {
        public HiddenTripleAlgorithm()
        {
            this.TypeAlgorithm = TypeAlgorithmSolution.HiddenTripleAlgorithm;
        }
        private SolutionMethod GetHiddenTripleInRange<TPointMatrix>(GridSudoku<TPointMatrix> sudoku, RangeMatrix range) where TPointMatrix : IPointMatrix, new()
        {
            for (byte num1 = 0; num1 < 10; num1++)
            {
                for (byte num2 = 0; num2 < 10; num2++)
                {
                    for (byte num3 = 0; num3 < 10; num3++)
                    {
                        int count_num1 = sudoku.GetCountPossiblePosPointInRange(range, num1);
                        int count_num2 = sudoku.GetCountPossiblePosPointInRange(range, num2);
                        int count_num3 = sudoku.GetCountPossiblePosPointInRange(range, num3);
                        if (num1 != num2 && num2 != num3 && num1 != num3 && count_num1 <= 3 && count_num1 >= 2 && count_num2 <= 3 && count_num2 >= 2 && count_num3 <= 3 && count_num3 >= 2)
                        {
                            Set<PosPoint> poss_num = new Set<PosPoint>(sudoku.GetPossPosPointsInRange(range, num1)) + new Set<PosPoint>(sudoku.GetPossPosPointsInRange(range, num2)) + new Set<PosPoint>(sudoku.GetPossPosPointsInRange(range, num3));
                            if (poss_num.Count() == 3)
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
                                if (values.Count() == 3)
                                {
                                    SolutionMethod Solution_method = new SolutionMethod()
                                    {
                                        Algorithm = this.TypeAlgorithm,
                                        IsSingleValue = false,
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
            return null;
        }
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod triple_h = this.GetHiddenTripleInRange(sudoku, RangeMatrix.FormHorizontalLine(i));
                if (!(triple_h is null)) { return triple_h; };
                SolutionMethod triple_v = this.GetHiddenTripleInRange(sudoku, RangeMatrix.FormVerticalLine(i));
                if (!(triple_v is null)) { return triple_v; };
            }
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    SolutionMethod triple = this.GetHiddenTripleInRange(sudoku, RangeMatrix.FormSquare(si, sj));
                    if (!(triple is null)) { return triple; };
                }
            }
            return null;
        }
    }
}
