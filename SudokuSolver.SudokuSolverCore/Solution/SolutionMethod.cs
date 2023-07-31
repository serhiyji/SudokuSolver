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
        private List<PosPoint> ChangedPosPoints;

        public SolutionMethod()
        {
            SetDefoltValues();
        }
        public SolutionMethod(bool isSingleValue = false,
            byte newValue = 0, PosPoint posPointNewValue = null,
            Arrange<PosPoint> posPoints = null, Set<byte> values = null) : this()
        {
            IsSingleValue = isSingleValue;
            NewValue = newValue;
            PosPointNewValue = posPointNewValue;
            PosPoints = posPoints;
            this.values = values;
            algorithm = AlgorithmSolutionMethod.None;
            IS = (true, true, true, true);
            ChangedPosPoints = new List<PosPoint>();
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
            ChangedPosPoints = new List<PosPoint>();
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
        public void SelectSolution(ref BetterMatrix.BetterMatrix matrix)
        {
            ChangedPosPoints.Clear();
            if (IsSingleValue)
            {
                //matrix[this.PosPointNewValue].PossibleValues[this.NewValue - 1] = Point.GreenColor;
                ChangedPosPoints.Add(PosPointNewValue);
            }
            else
            {
                GreenPointsSet(matrix);
                RedPointsSet(matrix);
            }
        }
        public void DeSelectSolution(ref BetterMatrix.BetterMatrix matrix)
        {
            foreach (var item in ChangedPosPoints)
            {
                //matrix.matrix[item.i, item.j].SetToDefoltStatusItem();
            }
        }
        public bool IsValid(BetterMatrix.BetterMatrix matrix)
        {
            if (!IsSingleValue)
            {
                if (PosPoints.Count == 0) { return false; }
                byte count = (byte)PosPoints.Count;
                bool hl = SolutionMethodHandler.IsPosPointsInHorizontalLine(PosPoints),
                    vl = SolutionMethodHandler.IsPosPointsInVerticalLine(PosPoints),
                    sq = SolutionMethodHandler.IsOneSquareInArrPospoint(PosPoints);
                foreach (byte item in values)
                {
                    if (hl && matrix.GetCountPossiblePosPointInHorizontalLine(PosPoints[0].i, item) > count && IS.hl)
                    {
                        return true;
                    }
                    else if (vl && matrix.GetCountPossiblePosPointInVerticalLine(PosPoints[0].j, item) > count && IS.vl)
                    {
                        return true;
                    }
                    else if (sq && matrix.GetCountPossiblePosPointInSquare(new PosSquare(PosPoints[0]), item) > count && IS.sq)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void GreenPointsSet(BetterMatrix.BetterMatrix matrix)
        {
            foreach (var item in PosPoints)
            {
                foreach (var val in values)
                {
                    if (matrix.matrix[item.i, item.j].set.Contains(val))
                    {
                        //matrix.matrix[item.i, item.j].PossibleValues[val - 1] = Point.GreenColor;
                    }
                }
                if (!ChangedPosPoints.Contains(item))
                {
                    ChangedPosPoints.Add(item);
                }
            }
        }
        private void RedPointsSet(BetterMatrix.BetterMatrix matrix)
        {
            if (IsSingleValue) return;
            bool hl = SolutionMethodHandler.IsPosPointsInHorizontalLine(PosPoints),
                vl = SolutionMethodHandler.IsPosPointsInVerticalLine(PosPoints),
                sq = SolutionMethodHandler.IsOneSquareInArrPospoint(PosPoints);
            Set<PosPoint> pospoint = new Set<PosPoint>(PosPoints);
            Func<byte, Set<PosPoint>, bool> func = (value, RedPoints) =>
            {
                foreach (PosPoint item in RedPoints.Where(item => matrix[item].set.Contains(value)))
                {
                    //matrix.matrix[item.i, item.j].PossibleValues[value - 1] = Point.RedColor;
                    if (!ChangedPosPoints.Contains(item))
                    {
                        ChangedPosPoints.Add(item);
                    }
                }
                return false;
            };

            foreach (byte value in values)
            {
                if (hl)
                {
                    Set<PosPoint> RedPoints = new Set<PosPoint>(matrix.GetPossPosPointsInHorizontalLine(PosPoints[0].i, value)) - pospoint;
                    func?.Invoke(value, RedPoints);
                }
                if (vl)
                {
                    Set<PosPoint> RedPoints = new Set<PosPoint>(matrix.GetPossPosPointsInVerticalLine(PosPoints[0].j, value)) - pospoint;
                    func?.Invoke(value, RedPoints);
                }
                if (sq)
                {
                    Set<PosPoint> RedPoints = new Set<PosPoint>(matrix.GetPossPosPointsInSquare(new PosSquare(PosPoints[0]), value)) - pospoint;
                    func?.Invoke(value, RedPoints);
                }
            }
        }
    }
}
