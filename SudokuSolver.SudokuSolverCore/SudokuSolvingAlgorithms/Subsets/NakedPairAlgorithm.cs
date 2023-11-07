using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Solution;
using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms.Subsets
{
    public class NakedPairAlgorithm : SudokuSolvingAlgorithm
    {
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod pair_h = new Intersections.LockedPairAlgorithm().GetLockedPairInRange(sudoku, RangeMatrix.FormHorizontalLine(i));
                if (!(pair_h is null)) { return pair_h; }
                SolutionMethod pair_v = new Intersections.LockedPairAlgorithm().GetLockedPairInRange(sudoku, RangeMatrix.FormVerticalLine(i));
                if (!(pair_v is null)) { return pair_v; }
            }
            return null;
        }
    }
}
