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

namespace SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms.BruteForce
{
    public class StandardBruteForceAlgorithm : SudokuSolvingAlgorithm
    {
        public StandardBruteForceAlgorithm()
        {
            this.TypeAlgorithm = TypeAlgorithmSolution.StandardBruteForceAlgorithm;
        }

        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            this.BruteForceWithSet(sudoku);
            return null;
        }

        private bool BruteForceWithSet<TPointmatrix>(GridSudoku<TPointmatrix> sudoku) where TPointmatrix : IPointMatrix, new()
        {
            int minPossibleValues = 10;
            PosPoint minPoint = new PosPoint(-1, -1);
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (sudoku[row, col].value == 0)
                    {
                        int possibleValuesCount = sudoku[row, col].set.Count;
                        if (possibleValuesCount < minPossibleValues)
                        {
                            minPossibleValues = possibleValuesCount;
                            minPoint.i = row;
                            minPoint.j = col;
                        }
                    }
                }
            }

            if (minPoint.i == -1)
            {
                return true;
            }

            foreach (byte value in sudoku[minPoint].set)
            {
                sudoku.SetValue(minPoint, value);
                if (BruteForceWithSet(sudoku))
                {
                    return true;
                }
                sudoku.SetValue(minPoint, 0);
            }

            return false;
        }
    }
}
