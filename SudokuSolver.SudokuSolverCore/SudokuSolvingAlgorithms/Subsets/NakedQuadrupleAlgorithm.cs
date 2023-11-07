using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Points;
using SudokuSolver.SudokuSolverCore.Solution;
using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms.Subsets
{
    public class NakedQuadrupleAlgorithm : SudokuSolvingAlgorithm
    {
        private SolutionMethod GetNakedQuadrupleInRange<TPointMatrix>(GridSudoku<TPointMatrix> sudoku, RangeMatrix range) where TPointMatrix : IPointMatrix, new()
        {
            Arrange<PosPoint> pos = sudoku.GetPosPointsInRange(range);
            for (int i1 = 0; i1 < pos.Count(); i1++)
            {
                for (int i2 = 0; i2 < pos.Count(); i2++)
                {
                    for (int i3 = 0; i3 < pos.Count(); i3++)
                    {
                        for (int i4 = 0; i4 < pos.Count(); i4++)
                        {
                            if (pos[i1] != pos[i2] && pos[i2] != pos[i3] && pos[i1] != pos[i3]
                             && pos[i3] != pos[i4] && pos[i1] != pos[i4] && pos[i2] != pos[i4])
                            {
                                Set<byte> set1 = sudoku[pos[i1].i, pos[i1].j].set;
                                Set<byte> set2 = sudoku[pos[i2].i, pos[i2].j].set;
                                Set<byte> set3 = sudoku[pos[i3].i, pos[i3].j].set;
                                Set<byte> set4 = sudoku[pos[i4].i, pos[i4].j].set;
                                Set<byte> all = set1 + set2 + set3 + set4;
                                if ((set1.Count() >= 2 && set1.Count() <= 4) && (set2.Count() >= 2 && set2.Count() <= 4)
                                 && (set3.Count() >= 2 && set3.Count() <= 4) && (set4.Count() >= 2 && set4.Count() <= 4) && all.Count() == 4)
                                {
                                    SolutionMethod Solution_method = new SolutionMethod()
                                    {
                                        Algorithm = AlgorithmSolutionMethod.Naked_Quadruple,
                                        IsSingleValue = false,
                                        PosPoints = new Arrange<PosPoint>(pos[i1], pos[i2], pos[i3], pos[i4]),
                                        Values = all
                                    };
                                    if (SolutionMethodHandler.IsValid(sudoku, Solution_method))
                                    {
                                        return Solution_method;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        public override SolutionMethod? Solve<TPointMatrix>(GridSudoku<TPointMatrix> sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod quadruple_h = this.GetNakedQuadrupleInRange(sudoku, RangeMatrix.FormHorizontalLine(i));
                if (!(quadruple_h is null)) { return quadruple_h; }
                SolutionMethod quadruple_v = this.GetNakedQuadrupleInRange(sudoku, RangeMatrix.FormVerticalLine(i));
                if (!(quadruple_v is null)) { return quadruple_v; }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    SolutionMethod quadruple_s = this.GetNakedQuadrupleInRange(sudoku, RangeMatrix.FormSquare(i, j));
                    if (!(quadruple_s is null)) { return quadruple_s; }
                }
            }
            return null;
        }
    }
}
