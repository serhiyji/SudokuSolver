using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SudokuSolver.SudokuSolverCore.Solution;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.BetterMatrix;

namespace SudokuSolver.WPF_Client
{
    [AddINotifyPropertyChangedInterface]
    public class Solution : SudokuSolver.Extensions.Singleton<Solution>
    {
        public SolutionMethod Intersection { get; set; }
        public bool IsExecute { get; set; }
        private List<PosPoint> ChangedPosPoints;
        public Solution()
        {
            Intersection = new SolutionMethod();
            IsExecute = false;
            this.ChangedPosPoints = new List<PosPoint>();
        }
        public void SelectSolution(ref BetterMatrix<WPFPointMatrix> matrix)
        {
            ChangedPosPoints.Clear();
            if (Intersection.IsSingleValue)
            {
                matrix[Intersection.PosPointNewValue].PossibleValues[Intersection.NewValue - 1] = WPFPointMatrix.GreenColor;
                ChangedPosPoints.Add(Intersection.PosPointNewValue);
            }
            else
            {
                GreenPointsSet(ref matrix);
                RedPointsSet(matrix);
            }
        }
        public void DeSelectSolution(ref BetterMatrix<WPFPointMatrix> matrix)
        {
            foreach (var item in ChangedPosPoints)
            {
                matrix.matrix[item.i, item.j].SetToDefoltStatusItem();
            }
        }

        private void GreenPointsSet(ref BetterMatrix<WPFPointMatrix> matrix)
        {
            foreach (var item in Intersection.PosPoints)
            {
                foreach (var val in Intersection.values)
                {
                    if (matrix.matrix[item.i, item.j].set.Contains(val))
                    {
                        matrix.matrix[item.i, item.j].PossibleValues[val - 1] = WPFPointMatrix.GreenColor;
                    }
                }
                if (!ChangedPosPoints.Contains(item))
                {
                    ChangedPosPoints.Add(item);
                }
            }
        }
        private void RedPointsSet(BetterMatrix<WPFPointMatrix> matrix)
        {
            if (Intersection.IsSingleValue) return;
            bool hl = SolutionMethodHandler.IsPosPointsInHorizontalLine(Intersection.PosPoints),
                vl = SolutionMethodHandler.IsPosPointsInVerticalLine(Intersection.PosPoints),
                sq = SolutionMethodHandler.IsOneSquareInArrPospoint(Intersection.PosPoints);
            Set<PosPoint> pospoint = new Set<PosPoint>(Intersection.PosPoints);
            Func<byte, Set<PosPoint>, bool> func = (value, RedPoints) =>
            {
                foreach (PosPoint item in RedPoints.Where(item => matrix.matrix[item.i, item.j].set.Contains(value)))
                {
                    matrix.matrix[item.i, item.j].PossibleValues[value - 1] = WPFPointMatrix.RedColor;
                    if (!ChangedPosPoints.Contains(item))
                    {
                        ChangedPosPoints.Add(item);
                    }
                }
                return false;
            };

            foreach (byte value in Intersection.values)
            {
                if (hl)
                {
                    Set<PosPoint> RedPoints = new Set<PosPoint>(matrix.GetPossPosPointsInHorizontalLine(Intersection.PosPoints[0].i, value)) - pospoint;
                    func?.Invoke(value, RedPoints);
                }
                if (vl)
                {
                    Set<PosPoint> RedPoints = new Set<PosPoint>(matrix.GetPossPosPointsInVerticalLine(Intersection.PosPoints[0].j, value)) - pospoint;
                    func?.Invoke(value, RedPoints);
                }
                if (sq)
                {
                    Set<PosPoint> RedPoints = new Set<PosPoint>(matrix.GetPossPosPointsInSquare(new PosSquare(Intersection.PosPoints[0]), value)) - pospoint;
                    func?.Invoke(value, RedPoints);
                }
            }
        }
    }
}
