using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Points;
using SudokuSolver.SudokuSolverCore.Solution;
using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore
{
    public class SudokuSloverHandler<TPointMatrix> where TPointMatrix : IPointMatrix, new()
    {
        public GridSudoku<TPointMatrix> matrix { get; set; }
        private List<SudokuSolvingAlgorithm> solution_methods;
        public SudokuSloverHandler(ref GridSudoku<TPointMatrix> matrix)
        {
            this.matrix = matrix;
            this.matrix.SetPossibleValues();

            solution_methods = new List<SudokuSolvingAlgorithm>()
            {
                // Singles
                new SudokuSolvingAlgorithms.Singles.FullHouseAlgorithm(),
                new SudokuSolvingAlgorithms.Singles.NakedSingleAlgorithm(),
                new SudokuSolvingAlgorithms.Singles.HiddenSingleAlgorithm(),
                
                // Intersections
                new SudokuSolvingAlgorithms.Intersections.LockedPairAlgorithm(),
                new SudokuSolvingAlgorithms.Intersections.LockedTripleAlgorithm(),
                new SudokuSolvingAlgorithms.Intersections.LockedCandidatesTypePointingAlgorithm(),
                new SudokuSolvingAlgorithms.Intersections.LockedCandidatesTypeClaimingAlgorithm(),

                // Subsets
                new SudokuSolvingAlgorithms.Subsets.NakedPairAlgorithm(),
                new SudokuSolvingAlgorithms.Subsets.NakedTripleAlgorithm(),
                new SudokuSolvingAlgorithms.Subsets.NakedQuadrupleAlgorithm(),
                new SudokuSolvingAlgorithms.Subsets.HiddenPairAlgorithm(),
                new SudokuSolvingAlgorithms.Subsets.HiddenTripleAlgorithm(),
                new SudokuSolvingAlgorithms.Subsets.HiddenQuadrupleAlgorithm(),
            };
        }

        public SolutionMethod NextSlover()
        {
            foreach (SudokuSolvingAlgorithm item in solution_methods)
            {
                SolutionMethod Solution_method = item.Solve(this.matrix);
                if (!(Solution_method is null))
                {
                    return Solution_method;
                }
            }
            return null;
        }
    }
}
