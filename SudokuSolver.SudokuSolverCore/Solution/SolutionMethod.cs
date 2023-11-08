using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms;

namespace SudokuSolver.SudokuSolverCore.Solution
{
    public class SolutionMethod
    {
        // Base
        public TypeAlgorithmSolution Algorithm { get; set; } = TypeAlgorithmSolution.None;
        public bool IsSingleValue { get; set; } = true;

        // For single value 
        public byte NewValue { get; set; } = 0;
        public PosPoint PosPointNewValue { get; set; } = new PosPoint();

        // For mult values
        public (bool pos, bool sq, bool hl, bool vl) IS { get; set; } = (true, true, true, true);
        public Arrange<PosPoint> PosPoints { get; set; } = new Arrange<PosPoint>();
        public Set<byte> Values { get; set; } = new Set<byte>();

        public SolutionMethod()
        {
            
        }
        public SolutionMethod(TypeAlgorithmSolution typeAlgorithmSolution, bool isSingleValue)
        {
            this.Algorithm = typeAlgorithmSolution;
            this.IsSingleValue = isSingleValue;
        }
        public SolutionMethod(
            TypeAlgorithmSolution typeAlgorithmSolution, bool isSingleValue,
            byte newValue, PosPoint posPointnewValue
            ) : this(typeAlgorithmSolution, isSingleValue)
        {
            this.NewValue = newValue;
            this.PosPointNewValue = posPointnewValue;
        }
        public SolutionMethod(
            TypeAlgorithmSolution typeAlgorithmSolution, bool isSingleValue,
            Arrange<PosPoint> posPoints, Set<byte> values, (bool pos, bool sq, bool hl, bool vl) iS
            ) : this(typeAlgorithmSolution, isSingleValue)
        {
            this.PosPoints = posPoints;
            this.Values = values;
            this.IS = iS;
        }

        public void SetDefoltValues()
        {
            this.IsSingleValue = false;
            this.NewValue = 0;
            this.PosPointNewValue = new PosPoint();
            this.PosPoints = new Arrange<PosPoint>();
            this.Values = new Set<byte>();
            this.IS = (true, true, true, true);
            this.Algorithm = TypeAlgorithmSolution.None;
        }
        public void SetValues(SolutionMethod Solution_method)
        {
            this.IsSingleValue = Solution_method.IsSingleValue;
            this.NewValue = Solution_method.NewValue;
            this.PosPointNewValue.i = Solution_method.PosPointNewValue.i;
            this.PosPointNewValue.j = Solution_method.PosPointNewValue.j;
            this.PosPoints = Solution_method.PosPoints;
            this.Values = Solution_method.Values;
            this.IS = Solution_method.IS;
            this.Algorithm = Solution_method.Algorithm;
        }
    }
}
