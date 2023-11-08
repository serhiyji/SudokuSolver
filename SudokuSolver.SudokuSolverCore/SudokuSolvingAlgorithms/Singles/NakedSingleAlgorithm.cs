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
    public class NakedSingleAlgorithm : SudokuSolvingAlgorithm
    {
        public NakedSingleAlgorithm() 
        {
            this.TypeAlgorithm = TypeAlgorithmSolution.NakedSingleAlgorithm;    
        }
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    PosPoint pos_p = new PosPoint(i, j);
                    if (sudoku.IsNullInPosPoint(pos_p) && sudoku.IsOnePossibleValueInPosPoint(pos_p))
                    {
                        byte NewValue_ = sudoku.GetFirstValueSetInPosPoint(pos_p);
                        if (NewValue_ == 0) continue;
                        return new SolutionMethod(this.TypeAlgorithm, true)
                        {
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
