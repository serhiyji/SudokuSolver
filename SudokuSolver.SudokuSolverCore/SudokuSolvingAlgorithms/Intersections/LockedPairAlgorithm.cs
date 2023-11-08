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

namespace SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms.Intersections
{
    public class LockedPairAlgorithm : SudokuSolvingAlgorithm
    {
        public LockedPairAlgorithm()
        {
            this.TypeAlgorithm = TypeAlgorithmSolution.LockedPairAlgorithm;
        }
        public SolutionMethod GetLockedPairInRange<TPointMatrix>(GridSudoku<TPointMatrix> sudoku, RangeMatrix range) where TPointMatrix : IPointMatrix, new()
        {
            Arrange<PosPoint> pos = sudoku.GetPosPointsInRange(range);
            for (int i = 0; i < pos.Count(); i++)
            {
                for (int j = 0; j < pos.Count(); j++)
                {
                    if ((pos[i] != pos[j]) && (sudoku[pos[i].i, pos[i].j].set == sudoku[pos[j].i, pos[j].j].set)
                        && (sudoku[pos[i].i, pos[i].j].set.Count() == 2))
                    {
                        SolutionMethod Solution_method = new SolutionMethod()
                        {
                            Algorithm = this.TypeAlgorithm,
                            IsSingleValue = false,
                            PosPoints = new Arrange<PosPoint>(pos[i], pos[j]),
                            Values = sudoku.GetPossValueInPosPoint(pos[i])
                        };
                        if (SolutionMethodHandler.IsValid(sudoku, Solution_method))
                        {
                            return Solution_method;
                        }
                    }
                }
            }
            return null;
        }
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    PosSquare pos_s = new PosSquare(si, sj);
                    SolutionMethod pair = this.GetLockedPairInRange(sudoku, RangeMatrix.FormSquare(pos_s));
                    if (!(pair is null))
                    {
                        return pair;
                    }
                }
            }
            return null;
        }
    }
}
