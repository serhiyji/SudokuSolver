using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Points;
using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore.Solution
{
    public static class SolutionMethodHandler
    {
        public static bool IsPosPointsInHorizontalLine(Arrange<PosPoint> arr)
        {
            if (arr.Count() == 1 || arr.Count() == 0) { return true; };
            bool total_i = true;
            for (int i = 0; i < arr.Count() - 1; i++)
            {
                total_i = ((arr[i].i == arr[i + 1].i) && total_i);
            }
            return total_i;
        }
        public static bool IsPosPointsInVerticalLine(Arrange<PosPoint> arr)
        {
            if (arr.Count() == 1 || arr.Count() == 0) { return true; };
            bool total_j = true;
            for (int i = 0; i < arr.Count() - 1; i++)
            {
                total_j = ((arr[i].j == arr[i + 1].j) && total_j);
            }
            return total_j;
        }
        public static bool IsOneSquareInArrPospoint(Arrange<PosPoint> arr)
        {
            if (arr.Count() == 1 || arr.Count() == 0) { return true; };
            bool total = true;
            for (int i = 0; i < arr.Count() - 1; i++)
            {
                total = (new PosSquare(arr[i]) == new PosSquare(arr[i + 1])) && total;
            }
            return total;
        }
        public static bool Solution_methods_Handler<TPointMatrix>(ref GridSudoku<TPointMatrix> matrix, SolutionMethod inter) where TPointMatrix : IPointMatrix, new()
        {
            if (inter.IsSingleValue)
            {
                matrix.SetValue(inter.PosPointNewValue, inter.NewValue);
                return true;
            }
            else
            {
                bool total = false;
                if (inter.IS.pos)
                {
                    for (int i = 0; i < inter.PosPoints.Count(); i++)
                    {
                        total = matrix.ClearValuesInSetInPosPoint(inter.PosPoints[i], matrix.GetPossValueInPosPoint(inter.PosPoints[i]) - inter.Values) || total;
                    }
                }

                total = (inter.IS.sq) ? (IsOneSquareInArrPospoint(inter.PosPoints)) ?
                    matrix.ClearValuesInSetInRange(RangeMatrix.FormSquare(new PosSquare(inter.PosPoints[0])), inter.Values, inter.PosPoints) || total : total : total;

                total = (inter.IS.hl) ? (IsPosPointsInHorizontalLine(inter.PosPoints)) ?
                    matrix.ClearValuesInSetInRange(RangeMatrix.FormHorizontalLine(inter.PosPoints[0].i), inter.Values, inter.PosPoints) || total : total : total;

                total = (inter.IS.vl) ? (IsPosPointsInVerticalLine(inter.PosPoints)) ?
                    matrix.ClearValuesInSetInRange(RangeMatrix.FormVerticalLine(inter.PosPoints[0].j), inter.Values, inter.PosPoints) || total : total : total;

                return total;
            }
            return false;
        }
        public static bool IsValid<TPointMatrix>(GridSudoku<TPointMatrix> matrix, SolutionMethod solution) where TPointMatrix : IPointMatrix, new()
        {
            if (!solution.IsSingleValue)
            {
                if (solution.PosPoints.Count == 0) { return false; }
                byte count = (byte)solution.PosPoints.Count;
                bool hl = SolutionMethodHandler.IsPosPointsInHorizontalLine(solution.PosPoints),
                    vl = SolutionMethodHandler.IsPosPointsInVerticalLine(solution.PosPoints),
                    sq = SolutionMethodHandler.IsOneSquareInArrPospoint(solution.PosPoints);
                foreach (byte item in solution.Values)
                {
                    if (hl && matrix.GetCountPossiblePosPointInRange(RangeMatrix.FormHorizontalLine(solution.PosPoints[0].i), item) > count && solution.IS.hl)
                    {
                        return true;
                    }
                    else if (vl && matrix.GetCountPossiblePosPointInRange(RangeMatrix.FormVerticalLine(solution.PosPoints[0].j), item) > count && solution.IS.vl)
                    {
                        return true;
                    }
                    else if (sq && matrix.GetCountPossiblePosPointInRange(RangeMatrix.FormSquare(new PosSquare(solution.PosPoints[0])), item) > count && solution.IS.sq)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
