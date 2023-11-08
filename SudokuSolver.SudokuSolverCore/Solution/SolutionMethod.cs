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
    
    public partial class SolutionMethod
    {
        public TypeAlgorithmSolution Algorithm { get; set; }
        public bool IsSingleValue { get; set; }
        // 
        public byte NewValue { get; set; }
        public PosPoint PosPointNewValue { get; set; }
        //
        public (bool pos, bool sq, bool hl, bool vl) IS { get; set; }

        //

        public Arrange<PosPoint> PosPoints { get; set; }
        public Set<byte> Values { get; set; }


        // 
        

        public SolutionMethod()
        {
            SetDefoltValues();
        }
        public SolutionMethod(bool isSingleValue = false,
            byte newValue = 0, PosPoint posPointNewValue = default(PosPoint),
            Arrange<PosPoint> posPoints = default(Arrange<PosPoint>), 
            Set<byte> values = default(Set<byte>)) : this()
        {
            IsSingleValue = isSingleValue;
            NewValue = newValue;
            PosPointNewValue = posPointNewValue;
            PosPoints = posPoints;
            this.Values = values;
            Algorithm = TypeAlgorithmSolution.None;
            IS = (true, true, true, true);
        }

        public void SetDefoltValues()
        {
            IsSingleValue = false;
            NewValue = 0;
            PosPointNewValue = new PosPoint();
            PosPoints = new Arrange<PosPoint>();
            Values = new Set<byte>();
            IS = (true, true, true, true);
            Algorithm = TypeAlgorithmSolution.None;
        }
        public void SetValues(SolutionMethod Solution_method)
        {
            IsSingleValue = Solution_method.IsSingleValue;
            NewValue = Solution_method.NewValue;
            PosPointNewValue.i = Solution_method.PosPointNewValue.i;
            PosPointNewValue.j = Solution_method.PosPointNewValue.j;
            PosPoints = Solution_method.PosPoints;
            Values = Solution_method.Values;
            IS = Solution_method.IS;
            Algorithm = Solution_method.Algorithm;
        }
    }
}
