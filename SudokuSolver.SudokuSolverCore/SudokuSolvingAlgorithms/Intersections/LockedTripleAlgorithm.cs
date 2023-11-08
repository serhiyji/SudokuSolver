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
    public class LockedTripleAlgorithm : SudokuSolvingAlgorithm
    {
        public LockedTripleAlgorithm()
        {
            this.TypeAlgorithm = TypeAlgorithmSolution.LockedTripleAlgorithm;
        }
        public SolutionMethod GetLockedTripleInRange<TPointMatrix>(GridSudoku<TPointMatrix> sudoku, RangeMatrix range) where TPointMatrix : IPointMatrix, new()
        {
            Arrange<PosPoint> pos = sudoku.GetPosPointsInRange(range);
            for (int i1 = 0; i1 < pos.Count(); i1++)
            {
                for (int i2 = 0; i2 < pos.Count(); i2++)
                {
                    for (int i3 = 0; i3 < pos.Count(); i3++)
                    {
                        if (pos[i1] != pos[i2] && pos[i2] != pos[i3] && pos[i1] != pos[i3])
                        {
                            Set<byte> set1 = sudoku[pos[i1].i, pos[i1].j].set;
                            Set<byte> set2 = sudoku[pos[i2].i, pos[i2].j].set;
                            Set<byte> set3 = sudoku[pos[i3].i, pos[i3].j].set;
                            Set<byte> all = set1 + set2 + set3;
                            if ((set1.Count() >= 2 && set1.Count() <= 3) && (set2.Count() >= 2 && set2.Count() <= 3) && (set3.Count() >= 2 && set3.Count() <= 3) && all.Count() == 3)
                            {
                                SolutionMethod Solution_method = new SolutionMethod(this.TypeAlgorithm, false)
                                {
                                    PosPoints = new Arrange<PosPoint>(pos[i1], pos[i2], pos[i3]),
                                    Values = all
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
            return null;
        }
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    SolutionMethod tripl = this.GetLockedTripleInRange(sudoku, RangeMatrix.FormSquare(si, sj));
                    if (!(tripl is null)) { return tripl; };
                }
            }
            return null;
        }
    }
}
