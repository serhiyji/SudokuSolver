using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Points;
using SudokuSolver.SudokuSolverCore.Solution;
using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms
{
    public abstract class SudokuSolvingAlgorithm
    {
        public TypeAlgorithmSolution TypeAlgorithm { get; protected set; } = TypeAlgorithmSolution.None;
        public bool IsExecute { get; set; } = true;
        public virtual SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku) where TPointMatrix : IPointMatrix, new()
        {
            throw new NotImplementedException();
        }
    }
}
