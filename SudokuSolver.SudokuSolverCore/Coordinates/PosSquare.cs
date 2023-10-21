using System;

namespace SudokuSolver.SudokuSolverCore.Coordinates
{
    public class PosSquare
    {
        public int i { get; set; }
        public int j { get; set; }
        // Ctor
        #region Ctor
        public PosSquare(int i_ = 0, int j_ = 0)
        {
            i = i_;
            j = j_;
        }
        public PosSquare(PosPoint pos)
        {
            i = pos.i / SizeGridSudoku.SizeSquareHorizontal;
            j = pos.j / SizeGridSudoku.SizeSquareVertical;
        }
        #endregion

        // operators ==, !=
        #region operators ==, !=
        public static bool operator ==(PosSquare pos1, PosSquare pos2)
        {
            return pos1.Equals(pos2);
        }
        public static bool operator !=(PosSquare pos1, PosSquare pos2)
        {
            return !pos1.Equals(pos2);
        }
        public override bool Equals(object obj)
        {
            return obj is PosSquare pos && i == pos.i && j == pos.j;
        }
        #endregion
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
