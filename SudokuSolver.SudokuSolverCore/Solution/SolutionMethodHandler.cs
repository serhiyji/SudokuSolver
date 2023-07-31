using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Points;
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
        public static bool Intersections_Handler(ref BetterMatrix.BetterMatrix<PointMatrix> matrix, SolutionMethod inter)
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
                        total = matrix.ClearValuesInSetInPosPoint(inter.PosPoints[i], matrix.GetPossValueInPosPoint(inter.PosPoints[i]) - inter.values) || total;
                    }
                }

                total = (inter.IS.sq) ? (IsOneSquareInArrPospoint(inter.PosPoints)) ?
                    matrix.ClearValuesInSetInSquare(new PosSquare(inter.PosPoints[0]), inter.values, inter.PosPoints) || total : total : total;

                total = (inter.IS.hl) ? (IsPosPointsInHorizontalLine(inter.PosPoints)) ?
                    matrix.ClearValuesInSetInHorizontalLine(inter.PosPoints[0].i, inter.values, inter.PosPoints) || total : total : total;

                total = (inter.IS.vl) ? (IsPosPointsInVerticalLine(inter.PosPoints)) ?
                    matrix.ClearValuesInSetInVerticalLine(inter.PosPoints[0].j, inter.values, inter.PosPoints) || total : total : total;

                return total;
            }
            return false;
        }
    }
}
