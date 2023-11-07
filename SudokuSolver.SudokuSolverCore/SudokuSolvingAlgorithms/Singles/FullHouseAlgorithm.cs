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
    public class FullHouseAlgorithm : SudokuSolvingAlgorithm
    {
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            // Horizontal Line
            for (int i = 0; i < 9; i++)
            {
                if (sudoku.IsOneNullInRange(RangeMatrix.FormHorizontalLine(i)))
                {
                    PosPoint pos_p = sudoku.GetOneNullPosPointInRange(RangeMatrix.FormHorizontalLine(i));
                    byte NewValue_ = sudoku.GetFirstValueSetInPosPoint(pos_p);
                    if (NewValue_ == 0) continue;
                    return new SolutionMethod()
                    {
                        Algorithm = AlgorithmSolutionMethod.Full_House,
                        IsSingleValue = true,
                        NewValue = NewValue_,
                        PosPointNewValue = pos_p
                    };
                }
            }
            // Verticall Line
            for (int i = 0; i < 9; i++)
            {
                if (sudoku.IsOneNullInRange(RangeMatrix.FormVerticalLine(i)))
                {
                    PosPoint pos_p = sudoku.GetOneNullPosPointInRange(RangeMatrix.FormVerticalLine(i));
                    byte NewValue_ = sudoku.GetFirstValueSetInPosPoint(pos_p);
                    if (NewValue_ == 0) continue;
                    return new SolutionMethod()
                    {
                        Algorithm = AlgorithmSolutionMethod.Full_House,
                        IsSingleValue = true,
                        NewValue = NewValue_,
                        PosPointNewValue = pos_p
                    };
                }
            }
            // Square
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (sudoku.IsOneNullInRange(RangeMatrix.FormSquare(i, j)))
                    {
                        PosPoint pos_p = sudoku.GetOneNullPosPointInRange(RangeMatrix.FormSquare(i, j));
                        byte NewValue_ = sudoku.GetFirstValueSetInPosPoint(pos_p);
                        if (NewValue_ == 0) continue;
                        return new SolutionMethod()
                        {
                            Algorithm = AlgorithmSolutionMethod.Full_House,
                            IsSingleValue = true,
                            NewValue = NewValue_,
                            PosPointNewValue = pos_p
                        };
                    }
                }
            }
            return null;
        }
    }
}
