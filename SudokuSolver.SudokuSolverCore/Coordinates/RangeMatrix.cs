using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore.Coordinates
{
    public class RangeMatrix
    {
        public PosPoint Position1 { get; private set; }
        public PosPoint Position2 { get; private set; }
        public RangeMatrix()
        {
            this.Position1 = new PosPoint();
            this.Position2 = new PosPoint();
        }
        public RangeMatrix(PosPoint pos1, PosPoint pos2)
        {
            this.Position1 = pos1;
            this.Position2 = pos2;
        }
        public RangeMatrix(int pos1_i, int pos1_j, int pos2_i, int pos2_j)
        {
            this.Position1 = new PosPoint(pos1_i, pos1_j);
            this.Position2 = new PosPoint(pos2_i, pos2_j);
        }

        public static RangeMatrix GetRangeFormHorizontalLine(int index)
            => new RangeMatrix(new PosPoint(index, 0), new PosPoint(index, SizeGridSudoku.SizeMatrixHorizontal - 1));
        public static RangeMatrix GetRangeFormVerticalLine(int index)
            => new RangeMatrix(new PosPoint(0, index), new PosPoint(SizeGridSudoku.SizeMatrixVertical - 1, index));
        public static RangeMatrix GetRangeFormSquare(PosSquare pos_s)
            => new RangeMatrix(
                new PosPoint(pos_s.i * SizeGridSudoku.CountSqareVertical, pos_s.j * SizeGridSudoku.CountSqareHorizontal), 
                new PosPoint(pos_s.i * SizeGridSudoku.CountSqareVertical + SizeGridSudoku.SizeSquareVertical - 1, pos_s.j * SizeGridSudoku.CountSqareHorizontal + SizeGridSudoku.SizeSquareHorizontal - 1)
            );
        public static RangeMatrix GetRangeFullMatrix() 
            => new RangeMatrix(new PosPoint(0, 0), new PosPoint(SizeGridSudoku.SizeMatrixVertical, SizeGridSudoku.SizeMatrixHorizontal));

    }
}
