using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Solution;
using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms.Singles
{
    public class HiddenSingleAlgorithm : SudokuSolvingAlgorithm
    {
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            for (byte value = 1; value < 10; value++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (sudoku.GetCountPossiblePosPointInRange(RangeMatrix.FormHorizontalLine(i), value) == 1)
                    {
                        return new SolutionMethod()
                        {
                            Algorithm = AlgorithmSolutionMethod.Hidden_Single,
                            IsSingleValue = true,
                            NewValue = value,
                            PosPointNewValue = sudoku.GetFirstPossiblePosPointInRange(RangeMatrix.FormHorizontalLine(i), value)
                        };
                    }
                }
                for (int i = 0; i < 9; i++)
                {
                    if (sudoku.GetCountPossiblePosPointInRange(RangeMatrix.FormVerticalLine(i), value) == 1)
                    {
                        return new SolutionMethod()
                        {
                            Algorithm = AlgorithmSolutionMethod.Hidden_Single,
                            IsSingleValue = true,
                            NewValue = value,
                            PosPointNewValue = sudoku.GetFirstPossiblePosPointInRange(RangeMatrix.FormVerticalLine(i), value)
                        };
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (sudoku.GetCountPossiblePosPointInRange(RangeMatrix.FormSquare(i, j), value) == 1)
                        {
                            return new SolutionMethod()
                            {
                                Algorithm = AlgorithmSolutionMethod.Hidden_Single,
                                IsSingleValue = true,
                                NewValue = value,
                                PosPointNewValue = sudoku.GetFirstPossiblePosPointInRange(RangeMatrix.FormSquare(i, j), value)
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
