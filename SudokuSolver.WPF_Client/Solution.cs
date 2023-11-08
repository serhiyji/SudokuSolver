using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SudokuSolver.SudokuSolverCore.Solution;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms;
using System.Security.Cryptography;

namespace SudokuSolver.WPF_Client
{
    [AddINotifyPropertyChangedInterface]
    public class Solution : SudokuSolver.Extensions.Singleton<Solution>
    {
        public SolutionMethod Solution_method { get; set; }
        public bool IsExecute { get; set; }
        private List<PosPoint> ChangedPosPoints;
        public Solution()
        {
            Solution_method = new SolutionMethod();
            IsExecute = false;
            this.ChangedPosPoints = new List<PosPoint>();
        }
        public void SelectSolution(ref GridSudoku<WPFPointMatrix> matrix)
        {
            ChangedPosPoints.Clear();
            if (Solution_method.IsSingleValue)
            {
                matrix[Solution_method.PosPointNewValue].PossibleValues[Solution_method.NewValue - 1] = WPFPointMatrix.GreenColor;
                ChangedPosPoints.Add(Solution_method.PosPointNewValue);
            }
            else
            {
                GreenPointsSet(ref matrix);
                RedPointsSet(matrix);
            }
        }
        public void DeSelectSolution(ref GridSudoku<WPFPointMatrix> matrix)
        {
            foreach (var item in ChangedPosPoints)
            {
                matrix.matrix[item.i, item.j].SetToDefoltStatusItem();
            }
        }

        private void GreenPointsSet(ref GridSudoku<WPFPointMatrix> matrix)
        {
            foreach (var item in Solution_method.PosPoints)
            {
                foreach (var val in Solution_method.Values)
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
        private void RedPointsSet(GridSudoku<WPFPointMatrix> matrix)
        {
            if (Solution_method.IsSingleValue) return;
            bool hl = SolutionMethodHandler.IsPosPointsInHorizontalLine(Solution_method.PosPoints),
                vl = SolutionMethodHandler.IsPosPointsInVerticalLine(Solution_method.PosPoints),
                sq = SolutionMethodHandler.IsOneSquareInArrPospoint(Solution_method.PosPoints);
            Set<PosPoint> pospoint = new Set<PosPoint>(Solution_method.PosPoints);
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

            foreach (byte value in Solution_method.Values)
            {
                if (hl)
                {
                    //GetPossPosPointsInRange
                    Set<PosPoint> RedPoints = new Set<PosPoint>(matrix.GetPossPosPointsInRange(RangeMatrix.FormHorizontalLine(Solution_method.PosPoints[0].i), value)) - pospoint;
                    func?.Invoke(value, RedPoints);
                }
                if (vl)
                {
                    Set<PosPoint> RedPoints = new Set<PosPoint>(matrix.GetPossPosPointsInRange(RangeMatrix.FormVerticalLine(Solution_method.PosPoints[0].j), value)) - pospoint;
                    func?.Invoke(value, RedPoints);
                }
                if (sq)
                {
                    Set<PosPoint> RedPoints = new Set<PosPoint>(matrix.GetPossPosPointsInRange(RangeMatrix.FormSquare(new PosSquare(Solution_method.PosPoints[0])), value)) - pospoint;
                    func?.Invoke(value, RedPoints);
                }
            }
        }
    }
}
