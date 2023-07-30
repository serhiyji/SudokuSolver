using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Matrix;
using SudokuSolver.SudokuSolverCore.Points;
using SudokuSolver.SudokuSolverCore.Solution;
using System;

namespace SudokuSolver.SudokuSolverCore.BetterMatrix
{
    public partial class BetterMatrix : Matrix<Point>
    {
        public SolutionMethod GetLockedPairInHorizontalLine(int index)
        { return this.GetLockedPairInRange(new PosPoint(index, 0), new PosPoint(index, size - 1)); }
        public SolutionMethod GetLockedPairInVerticalLine(int index)
        { return this.GetLockedPairInRange(new PosPoint(0, index), new PosPoint(size - 1, index)); }
        public SolutionMethod GetLockedPairInSquare(PosSquare pos_s)
        { return this.GetLockedPairInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2)); }
        // Locked Triple
        public SolutionMethod GetLockedTripleInHorizontalLine(int index)
        { return this.GetLockedTripleInRange(new PosPoint(index, 0), new PosPoint(index, size - 1)); }
        public SolutionMethod GetLockedTripleInVerticalLine(int index)
        { return this.GetLockedTripleInRange(new PosPoint(0, index), new PosPoint(size - 1, index)); }
        public SolutionMethod GetLockedTripleInSquare(PosSquare pos_s)
        { return this.GetLockedTripleInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2)); }
        // Hiden pair
        public SolutionMethod GetHiddenPairInHorizontalLine(int index)
        { return this.GetHiddenPairInRange(new PosPoint(index, 0), new PosPoint(index, size - 1)); }
        public SolutionMethod GetHiddenPairInVerticalLine(int index)
        { return this.GetHiddenPairInRange(new PosPoint(0, index), new PosPoint(size - 1, index)); }
        public SolutionMethod GetHiddenPairInSquare(PosSquare pos_s)
        { return this.GetHiddenPairInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2)); }
        // Hiden Truple
        public SolutionMethod GetHiddenTripleInHorizontalLine(int index)
        { return this.GetHiddenTripleInRange(new PosPoint(index, 0), new PosPoint(index, size - 1)); }
        public SolutionMethod GetHiddenTripleInVerticalLine(int index)
        { return this.GetHiddenTripleInRange(new PosPoint(0, index), new PosPoint(size - 1, index)); }
        public SolutionMethod GetHiddenTripleInSquare(PosSquare pos_s)
        { return this.GetHiddenTripleInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2)); }
        // Naked Quadruple
        public SolutionMethod GetNakedQuadrupleInHorizontalLine(int index)
        { return this.GetNakedQuadrupleInRange(new PosPoint(index, 0), new PosPoint(index, size - 1)); }
        public SolutionMethod GetNakedQuadrupleInVerticalLine(int index)
        { return this.GetNakedQuadrupleInRange(new PosPoint(0, index), new PosPoint(size - 1, index)); }
        public SolutionMethod GetNakedQuadrupleInSquare(PosSquare pos_s)
        { return this.GetNakedQuadrupleInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2)); }
        // Hiden Quadruple
        public SolutionMethod GetHiddenQuadrupleInHorizontalLine(int index)
        { return this.GetHiddenQuadrupleInRange(new PosPoint(index, 0), new PosPoint(index, size - 1)); }
        public SolutionMethod GetHiddenQuadrupleInVerticalLine(int index)
        { return this.GetHiddenQuadrupleInRange(new PosPoint(0, index), new PosPoint(size - 1, index)); }
        public SolutionMethod GetHiddenQuadrupleInSquare(PosSquare pos_s)
        { return this.GetHiddenQuadrupleInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2)); }
    }
}
