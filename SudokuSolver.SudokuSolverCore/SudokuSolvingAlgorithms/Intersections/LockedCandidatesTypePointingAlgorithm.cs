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
    public class LockedCandidatesTypePointingAlgorithm : SudokuSolvingAlgorithm
    {
        public LockedCandidatesTypePointingAlgorithm()
        {
            this.TypeAlgorithm = TypeAlgorithmSolution.LockedCandidatesTypePointingAlgorithm;
        }
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (byte value = 1; value < 10; value++)
                    {
                        int size_ = sudoku.GetCountPossiblePosPointInRange(RangeMatrix.FormSquare(i, j), value);
                        if (size_ == 3 || size_ == 2)
                        {
                            SolutionMethod Solution_method = new SolutionMethod()
                            {
                                Algorithm = this.TypeAlgorithm,
                                IsSingleValue = false,
                                PosPoints = sudoku.GetPossPosPointsInRange(RangeMatrix.FormSquare(i, j), value),
                                Values = new Set<byte>(value),
                                IS = (false, false, true, true)
                            };
                            if (SolutionMethodHandler.IsValid(sudoku, Solution_method))
                            {
                                return Solution_method;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
