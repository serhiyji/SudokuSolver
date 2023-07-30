using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Points;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.SudokuSolverCore
{
    public enum AlgorithmSudokuSlover
    {
        None,
        Full_House, Naked_Single, Hidden_Single,
        Locked_Pair, Locked_Triple,
        Locked_Candidates_Type_Pointing, Locked_Candidates_Type_Claiming,
        Naked_Pair, Naked_Triple, Naked_Quadruple,
        Hidden_Pair, Hidden_Triple, Hidden_Quadruple
    }
    public partial class Intersections
    {
        public AlgorithmSudokuSlover algorithm { get; set; }
        public string NameMethodSlover { get; set; }
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

        public Intersections()
        {
            SetDefoltValues();
        }
        public Intersections(string nameMethodSlover = "", bool isSingleValue = false,
            byte newValue = 0, PosPoint posPointNewValue = null,
            Arrange<PosPoint> posPoints = null, Set<byte> values = null) : this()
        {
            NameMethodSlover = nameMethodSlover;
            IsSingleValue = isSingleValue;
            NewValue = newValue;
            PosPointNewValue = posPointNewValue;
            PosPoints = posPoints;
            this.values = values;
            this.algorithm = AlgorithmSudokuSlover.None;
            IS = (true, true, true, true);
            this.ChangedPosPoints = new List<PosPoint>();
        }

        public Intersections(Arrange<PosPoint> ArrPos, Set<byte> SetValues) : this()
        {
            this.PosPoints = ArrPos;
            this.values = SetValues;
        }

        public void SetDefoltValues()
        {
            NameMethodSlover = "";
            IsSingleValue = false;
            NewValue = 0;
            PosPointNewValue = new PosPoint();
            PosPoints = new Arrange<PosPoint>();
            values = new Set<byte>();
            IS = (true, true, true, true);
            this.algorithm = AlgorithmSudokuSlover.None;
            this.ChangedPosPoints = new List<PosPoint>();
        }
        public void SetValues(Intersections intersection)
        {
            this.NameMethodSlover = intersection.NameMethodSlover;
            this.IsSingleValue = intersection.IsSingleValue;
            this.NewValue = intersection.NewValue;
            this.PosPointNewValue.i = intersection.PosPointNewValue.i;
            this.PosPointNewValue.j = intersection.PosPointNewValue.j;
            this.PosPoints = intersection.PosPoints;
            this.values = intersection.values;
            this.IS = intersection.IS;
            this.algorithm = intersection.algorithm;
        }
        public void SelectSolution(ref BetterMatrix.BetterMatrix matrix)
        {
            this.ChangedPosPoints.Clear();
            if (IsSingleValue)
            {
                //matrix[this.PosPointNewValue].PossibleValues[this.NewValue - 1] = Point.GreenColor;
                this.ChangedPosPoints.Add(this.PosPointNewValue);
            }
            else
            {
                this.GreenPointsSet(matrix);
                this.RedPointsSet(matrix);
            }
        }
        public void DeSelectSolution(ref BetterMatrix.BetterMatrix matrix)
        {
            foreach (var item in this.ChangedPosPoints)
            {
                //matrix.matrix[item.i, item.j].SetToDefoltStatusItem();
            }
        }
        public bool IsValid(BetterMatrix.BetterMatrix matrix)
        {
            if (!this.IsSingleValue)
            {
                if(this.PosPoints.Count == 0) { return false; }
                byte count = (byte)this.PosPoints.Count;
                bool hl = SudokuSlover.IsPosPointsInHorizontalLine(this.PosPoints), 
                    vl = SudokuSlover.IsPosPointsInVerticalLine(this.PosPoints), 
                    sq = SudokuSlover.IsOneSquareInArrPospoint(this.PosPoints);
                foreach (byte item in this.values)
                {
                    if (hl && matrix.GetCountPossiblePosPointInHorizontalLine(PosPoints[0].i, item) > count && IS.hl)
                    {
                        return true;
                    }
                    else if (vl && matrix.GetCountPossiblePosPointInVerticalLine(PosPoints[0].j, item) > count && IS.vl)
                    {
                        return true;
                    }
                    else if (sq && matrix.GetCountPossiblePosPointInSquare(new PosSquare(this.PosPoints[0]), item) > count && IS.sq)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void GreenPointsSet(BetterMatrix.BetterMatrix matrix)
        {
            foreach (var item in this.PosPoints)
            {
                foreach (var val in this.values)
                {
                    if (matrix.matrix[item.i, item.j].set.Contains(val))
                    {
                        //matrix.matrix[item.i, item.j].PossibleValues[val - 1] = Point.GreenColor;
                    }
                }
                if (!this.ChangedPosPoints.Contains(item))
                {
                    this.ChangedPosPoints.Add(item);
                }
            }
        }
        private void RedPointsSet(BetterMatrix.BetterMatrix matrix)
        {
            if (IsSingleValue) return;
            bool hl = SudokuSlover.IsPosPointsInHorizontalLine(this.PosPoints),
                vl = SudokuSlover.IsPosPointsInVerticalLine(this.PosPoints),
                sq = SudokuSlover.IsOneSquareInArrPospoint(this.PosPoints);
            Set<PosPoint> pospoint = new Set<PosPoint>(this.PosPoints);
            Func<byte, Set<PosPoint>, bool> func = (value, RedPoints) =>
            {
                foreach (PosPoint item in RedPoints.Where(item => matrix[item].set.Contains(value)))
                {
                    //matrix.matrix[item.i, item.j].PossibleValues[value - 1] = Point.RedColor;
                    if (!this.ChangedPosPoints.Contains(item))
                    {
                        this.ChangedPosPoints.Add(item);
                    }
                }
                return false;
            };

            foreach (byte value in this.values)
            {
                if (hl)
                {
                    Set<PosPoint> RedPoints = new Set<PosPoint>(matrix.GetPossPosPointsInHorizontalLine(this.PosPoints[0].i, value)) - pospoint;
                    func?.Invoke(value, RedPoints);
                }
                if (vl)
                {
                    Set<PosPoint> RedPoints = new Set<PosPoint>(matrix.GetPossPosPointsInVerticalLine(this.PosPoints[0].j, value)) - pospoint;
                    func?.Invoke(value, RedPoints);
                }
                if (sq)
                {
                    Set<PosPoint> RedPoints = new Set<PosPoint>(matrix.GetPossPosPointsInSquare(new PosSquare(this.PosPoints[0]), value)) - pospoint;
                    func?.Invoke(value, RedPoints);
                }
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
