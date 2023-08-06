using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Points;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.SudokuSolverCore.Solution
{
    public enum AlgorithmSolutionMethod
    {
        None,
        Full_House, Naked_Single, Hidden_Single,
        Locked_Pair, Locked_Triple,
        Locked_Candidates_Type_Pointing, Locked_Candidates_Type_Claiming,
        Naked_Pair, Naked_Triple, Naked_Quadruple,
        Hidden_Pair, Hidden_Triple, Hidden_Quadruple
    }
    public partial class SolutionMethod
    {
        public AlgorithmSolutionMethod algorithm { get; set; }
        public bool IsSingleValue { get; set; }
        // 
        public byte NewValue { get; set; }
        public PosPoint PosPointNewValue { get; set; }
        //
        public (bool pos, bool sq, bool hl, bool vl) IS { get; set; }

        //

        public Arrange<PosPoint> PosPoints { get; set; }
        public Set<byte> values { get; set; }


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
            this.values = values;
            algorithm = AlgorithmSolutionMethod.None;
            IS = (true, true, true, true);
        }

        public void SetDefoltValues()
        {
            IsSingleValue = false;
            NewValue = 0;
            PosPointNewValue = new PosPoint();
            PosPoints = new Arrange<PosPoint>();
            values = new Set<byte>();
            IS = (true, true, true, true);
            algorithm = AlgorithmSolutionMethod.None;
        }
        public void SetValues(SolutionMethod intersection)
        {
            IsSingleValue = intersection.IsSingleValue;
            NewValue = intersection.NewValue;
            PosPointNewValue.i = intersection.PosPointNewValue.i;
            PosPointNewValue.j = intersection.PosPointNewValue.j;
            PosPoints = intersection.PosPoints;
            values = intersection.values;
            IS = intersection.IS;
            algorithm = intersection.algorithm;
        }
    }
}
