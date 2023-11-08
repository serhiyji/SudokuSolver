using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Solution;
using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms.Intersections
{
    public class LockedCandidatesTypeClaimingAlgorithm : SudokuSolvingAlgorithm
    {
        public LockedCandidatesTypeClaimingAlgorithm()
        {
            this.TypeAlgorithm = TypeAlgorithmSolution.LockedCandidatesTypeClaimingAlgorithm;
        }
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            for (byte value = 1; value < 10; value++)
            {
                for (int i = 0; i < 9; i++)
                {
                    int count_h = sudoku.GetCountPossiblePosPointInRange(RangeMatrix.FormHorizontalLine(i), value);
                    if (count_h <= 3 && count_h >= 2)
                    {
                        SolutionMethod Solution_method = new SolutionMethod()
                        {
                            Algorithm = this.TypeAlgorithm,
                            IsSingleValue = false,
                            PosPoints = sudoku.GetPossPosPointsInRange(RangeMatrix.FormHorizontalLine(i), value),
                            Values = new Set<byte>(value),
                            IS = (false, true, false, false)
                        };
                        if (SolutionMethodHandler.IsValid(sudoku, Solution_method))
                        {
                            return Solution_method;
                        }
                    }

                    int count_v = sudoku.GetCountPossiblePosPointInRange(RangeMatrix.FormVerticalLine(i), value);
                    if (count_v <= 3 && count_v >= 2)
                    {
                        SolutionMethod Solution_method = new SolutionMethod()
                        {
                            IsSingleValue = false,
                            PosPoints = sudoku.GetPossPosPointsInRange(RangeMatrix.FormVerticalLine(i), value),
                            Values = new Set<byte>(value),
                            IS = (false, true, false, false)
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
    }
}
