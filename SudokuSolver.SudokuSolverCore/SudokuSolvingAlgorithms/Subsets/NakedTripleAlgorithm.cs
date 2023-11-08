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
    public class NakedTripleAlgorithm : SudokuSolvingAlgorithm
    {
        public NakedTripleAlgorithm()
        {
            this.TypeAlgorithm = TypeAlgorithmSolution.NakedTripleAlgorithm;
        }
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod triple_h = new Intersections.LockedTripleAlgorithm().GetLockedTripleInRange(sudoku, RangeMatrix.FormHorizontalLine(i));
                if (!(triple_h is null)) { return triple_h; }
                SolutionMethod triple_v = new Intersections.LockedTripleAlgorithm().GetLockedTripleInRange(sudoku, RangeMatrix.FormVerticalLine(i));
                if (!(triple_v is null)) { return triple_v; }
            }
            return null;
        }
    }
}
