using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Points;
using SudokuSolver.SudokuSolverCore.Solution;
using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore
{
    public class SudokuSloverHandler<TPointMatrix> where TPointMatrix : IPointMatrix, new()
    {
        public GridSudoku<TPointMatrix> matrix { get; set; }
        private List<Func<SolutionMethod>> solution_methods;
        public SudokuSloverHandler(ref GridSudoku<TPointMatrix> matrix)
        {
            this.matrix = matrix;
            this.matrix.SetPossibleValues();
            this.AddSolutionMethods();
        }
        private void AddSolutionMethods()
        {
            solution_methods = new List<Func<SolutionMethod>>() 
            {
                new Func<SolutionMethod>(this.Full_House),
                new Func<SolutionMethod>(this.Naked_Single),
                new Func<SolutionMethod>(this.Hidden_Single),
                new Func<SolutionMethod>(this.Locked_Pair),
                new Func<SolutionMethod>(this.Locked_Triple),
                new Func<SolutionMethod>(this.Locked_Candidates_Type_Pointing),
                new Func<SolutionMethod>(this.Locked_Candidates_Type_Claiming),
                new Func<SolutionMethod>(this.Naked_Pair),
                new Func<SolutionMethod>(this.Naked_Triple),
                new Func<SolutionMethod>(this.Naked_Quadruple),
                new Func<SolutionMethod>(this.Hidden_Pair),
                new Func<SolutionMethod>(this.Hidden_Triple),
                new Func<SolutionMethod>(this.Hidden_Quadruple),
            };
        }

        #region SearchAlgorithmsSolutionPrivate
        // Locked / Naked
        private SolutionMethod GetLockedPairInRange(RangeMatrix range)
        {
            Arrange<PosPoint> pos = this.matrix.GetPosPointsInRange(range);
            for (int i = 0; i < pos.Count(); i++)
            {
                for (int j = 0; j < pos.Count(); j++)
                {
                    if ((pos[i] != pos[j]) && (this.matrix[pos[i].i, pos[i].j].set == this.matrix[pos[j].i, pos[j].j].set)
                        && (this.matrix[pos[i].i, pos[i].j].set.Count() == 2))
                    {
                        SolutionMethod Solution_method = new SolutionMethod()
                        {
                            Algorithm = AlgorithmSolutionMethod.Locked_Pair,
                            IsSingleValue = false,
                            PosPoints = new Arrange<PosPoint>(pos[i], pos[j]),
                            Values = this.matrix.GetPossValueInPosPoint(pos[i])
                        };
                        if (SolutionMethodHandler.IsValid(this.matrix, Solution_method))
                        {
                            return Solution_method;
                        }
                    }
                }
            }
            return null;
        }
        private SolutionMethod GetLockedTripleInRange(RangeMatrix range)
        {
            Arrange<PosPoint> pos = this.matrix.GetPosPointsInRange(range);
            for (int i1 = 0; i1 < pos.Count(); i1++)
            {
                for (int i2 = 0; i2 < pos.Count(); i2++)
                {
                    for (int i3 = 0; i3 < pos.Count(); i3++)
                    {
                        if (pos[i1] != pos[i2] && pos[i2] != pos[i3] && pos[i1] != pos[i3])
                        {
                            Set<byte> set1 = this.matrix[pos[i1].i, pos[i1].j].set;
                            Set<byte> set2 = this.matrix[pos[i2].i, pos[i2].j].set;
                            Set<byte> set3 = this.matrix[pos[i3].i, pos[i3].j].set;
                            Set<byte> all = set1 + set2 + set3;
                            if ((set1.Count() >= 2 && set1.Count() <= 3) && (set2.Count() >= 2 && set2.Count() <= 3) && (set3.Count() >= 2 && set3.Count() <= 3) && all.Count() == 3)
                            {
                                SolutionMethod Solution_method = new SolutionMethod()
                                {
                                    Algorithm = AlgorithmSolutionMethod.Locked_Triple,
                                    IsSingleValue = false,
                                    PosPoints = new Arrange<PosPoint>(pos[i1], pos[i2], pos[i3]),
                                    Values = all
                                };
                                if (SolutionMethodHandler.IsValid(this.matrix, Solution_method))
                                {
                                    return Solution_method;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        // Naked
        private SolutionMethod GetNakedQuadrupleInRange(RangeMatrix range)
        {
            Arrange<PosPoint> pos = this.matrix.GetPosPointsInRange(range);
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
                                Set<byte> set1 = this.matrix[pos[i1].i, pos[i1].j].set;
                                Set<byte> set2 = this.matrix[pos[i2].i, pos[i2].j].set;
                                Set<byte> set3 = this.matrix[pos[i3].i, pos[i3].j].set;
                                Set<byte> set4 = this.matrix[pos[i4].i, pos[i4].j].set;
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
                                    if (SolutionMethodHandler.IsValid(this.matrix, Solution_method))
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
        // Hidden
        private SolutionMethod GetHiddenPairInRange(RangeMatrix range)
        {
            for (byte num1 = 0; num1 < 10; num1++)
            {
                for (byte num2 = 1; num2 < 10; num2++)
                {
                    if (num1 != num2)
                    {
                        int count_num1 = this.matrix.GetCountPossiblePosPointInRange(range, num1), count_num2 = this.matrix.GetCountPossiblePosPointInRange(range, num2);
                        if (count_num1 == 2 && count_num2 == 2)
                        {
                            Arrange<PosPoint> arr1 = this.matrix.GetPossPosPointsInRange(range, num1), arr2 = this.matrix.GetPossPosPointsInRange(range, num2);
                            if (arr1 == arr2)
                            {
                                SolutionMethod Solution_method = new SolutionMethod()
                                {
                                    Algorithm = AlgorithmSolutionMethod.Hidden_Pair,
                                    IsSingleValue = false,
                                    PosPoints = new Arrange<PosPoint>(arr1[0], arr1[1]),
                                    Values = new Set<byte>(num1, num2)
                                };
                                if (SolutionMethodHandler.IsValid(this.matrix, Solution_method))
                                {
                                    return Solution_method;
                                }
                            }
                        }
                    }

                }
            }
            return null;
        }
        private SolutionMethod GetHiddenTripleInRange(RangeMatrix range)
        {
            for (byte num1 = 0; num1 < 10; num1++)
            {
                for (byte num2 = 0; num2 < 10; num2++)
                {
                    for (byte num3 = 0; num3 < 10; num3++)
                    {
                        int count_num1 = this.matrix.GetCountPossiblePosPointInRange(range, num1);
                        int count_num2 = this.matrix.GetCountPossiblePosPointInRange(range, num2);
                        int count_num3 = this.matrix.GetCountPossiblePosPointInRange(range, num3);
                        if (num1 != num2 && num2 != num3 && num1 != num3 && count_num1 <= 3 && count_num1 >= 2 && count_num2 <= 3 && count_num2 >= 2 && count_num3 <= 3 && count_num3 >= 2)
                        {
                            Set<PosPoint> poss_num = new Set<PosPoint>(this.matrix.GetPossPosPointsInRange(range, num1)) + new Set<PosPoint>(this.matrix.GetPossPosPointsInRange(range, num2)) + new Set<PosPoint>(this.matrix.GetPossPosPointsInRange(range, num3));
                            if (poss_num.Count() == 3)
                            {
                                Set<byte> set_other = new Set<byte>();
                                Set<byte> values = new Set<byte>();
                                for (int i = range.Position1.i; i <= range.Position2.i; i++)
                                {
                                    for (int j = range.Position1.j; j <= range.Position2.j; j++)
                                    {
                                        PosPoint pos = new PosPoint(i, j);
                                        if (this.matrix[i, j].value == 0 && !poss_num.Contains(pos))
                                        {
                                            set_other += this.matrix[i, j].set;
                                        }
                                        if (poss_num.Contains(pos))
                                        {
                                            values += this.matrix[i, j].set;
                                        }
                                    }
                                }
                                values = values - set_other;
                                if (values.Count() == 3)
                                {
                                    SolutionMethod Solution_method = new SolutionMethod()
                                    {
                                        Algorithm = AlgorithmSolutionMethod.Hidden_Triple,
                                        IsSingleValue = false,
                                        PosPoints = poss_num,
                                        Values = values
                                    };
                                    if (SolutionMethodHandler.IsValid(this.matrix, Solution_method))
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
        private SolutionMethod GetHiddenQuadrupleInRange(RangeMatrix range)
        {
            for (byte num1 = 1; num1 < 10; num1++)
            {
                for (byte num2 = 1; num2 < 10; num2++)
                {
                    for (byte num3 = 1; num3 < 10; num3++)
                    {
                        for (byte num4 = 1; num4 < 10; num4++)
                        {
                            int count_num1 = this.matrix.GetCountPossiblePosPointInRange(range, num1);
                            int count_num2 = this.matrix.GetCountPossiblePosPointInRange(range, num2);
                            int count_num3 = this.matrix.GetCountPossiblePosPointInRange(range, num3);
                            int count_num4 = this.matrix.GetCountPossiblePosPointInRange(range, num4);
                            if (num1 != num2 && num2 != num3 && num1 != num3 && num1 != num4 && num2 != num4 && num3 != num4
                                && count_num1 <= 4 && count_num1 >= 2 && count_num2 <= 4 && count_num2 >= 2
                                && count_num3 <= 4 && count_num3 >= 2 && count_num4 <= 4 && count_num4 >= 2)
                            {
                                Set<PosPoint> poss_num = new Set<PosPoint>(this.matrix.GetPossPosPointsInRange(range, num1))
                                    + new Set<PosPoint>(this.matrix.GetPossPosPointsInRange(range, num2))
                                    + new Set<PosPoint>(this.matrix.GetPossPosPointsInRange(range, num3))
                                    + new Set<PosPoint>(this.matrix.GetPossPosPointsInRange(range, num4));
                                if (poss_num.Count() == 4)
                                {
                                    Set<byte> set_other = new Set<byte>();
                                    Set<byte> values = new Set<byte>();
                                    for (int i = range.Position1.i; i <= range.Position2.i; i++)
                                    {
                                        for (int j = range.Position1.j; j <= range.Position2.j; j++)
                                        {
                                            PosPoint pos = new PosPoint(i, j);
                                            if (this.matrix[i, j].value == 0 && !poss_num.Contains(pos))
                                            {
                                                set_other += this.matrix[i, j].set;
                                            }
                                            if (poss_num.Contains(pos))
                                            {
                                                values += this.matrix[i, j].set;
                                            }
                                        }
                                    }
                                    values = values - set_other;
                                    if (values.Count() == 4)
                                    {
                                        SolutionMethod Solution_method = new SolutionMethod()
                                        {
                                            Algorithm = AlgorithmSolutionMethod.Hidden_Quadruple,
                                            IsSingleValue = false,
                                            PosPoints = poss_num,
                                            Values = values
                                        };
                                        if (SolutionMethodHandler.IsValid(this.matrix, Solution_method))
                                        {
                                            return Solution_method;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region Singles
        private SolutionMethod Full_House()
        {
            // Horizontal Line
            for (int i = 0; i < 9; i++)
            {
                if (this.matrix.IsOneNullInRange(RangeMatrix.FormHorizontalLine(i)))
                {
                    PosPoint pos_p = this.matrix.GetOneNullPosPointInRange(RangeMatrix.FormHorizontalLine(i));
                    byte NewValue_ = this.matrix.GetFirstValueSetInPosPoint(pos_p);
                    if (NewValue_ == 0) continue;
                    return new SolutionMethod()
                    {
                        Algorithm = AlgorithmSolutionMethod.Full_House,
                        IsSingleValue = true,
                        NewValue = NewValue_,
                        PosPointNewValue = pos_p
                    };
                }
            }
            // Verticall Line
            for (int i = 0; i < 9; i++)
            {
                if (this.matrix.IsOneNullInRange(RangeMatrix.FormVerticalLine(i)))
                {
                    PosPoint pos_p = this.matrix.GetOneNullPosPointInRange(RangeMatrix.FormVerticalLine(i));
                    byte NewValue_ = this.matrix.GetFirstValueSetInPosPoint(pos_p);
                    if (NewValue_ == 0) continue;
                    return new SolutionMethod()
                    {
                        Algorithm = AlgorithmSolutionMethod.Full_House,
                        IsSingleValue = true,
                        NewValue = NewValue_,
                        PosPointNewValue = pos_p
                    };
                }
            }
            // Square
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    PosSquare pos_s = new PosSquare(i, j);
                    if (this.matrix.IsOneNullInRange(RangeMatrix.FormSquare(i, j)))
                    {
                        PosPoint pos_p = this.matrix.GetOneNullPosPointInRange(RangeMatrix.FormSquare(i, j));
                        byte NewValue_ = this.matrix.GetFirstValueSetInPosPoint(pos_p);
                        if (NewValue_ == 0) continue;
                        return new SolutionMethod()
                        {
                            Algorithm = AlgorithmSolutionMethod.Full_House,
                            IsSingleValue = true,
                            NewValue = NewValue_,
                            PosPointNewValue = pos_p
                        };
                    }
                }
            }
            return null;
        }
        private SolutionMethod Naked_Single()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    PosPoint pos_p = new PosPoint(i, j);
                    if (this.matrix.IsNullInPosPoint(pos_p) && this.matrix.IsOnePossibleValueInPosPoint(pos_p))
                    {
                        byte NewValue_ = this.matrix.GetFirstValueSetInPosPoint(pos_p);
                        if (NewValue_ == 0) continue;
                        return new SolutionMethod()
                        {
                            Algorithm = AlgorithmSolutionMethod.Naked_Single,
                            IsSingleValue = true,
                            NewValue = NewValue_,
                            PosPointNewValue = pos_p
                        };
                    }
                }
            }
            return null;
        }
        private SolutionMethod Hidden_Single()
        {
            for (byte value = 1; value < 10; value++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (this.matrix.GetCountPossiblePosPointInRange(RangeMatrix.FormHorizontalLine(i), value) == 1)
                    {
                        return new SolutionMethod()
                        {
                            Algorithm = AlgorithmSolutionMethod.Hidden_Single,
                            IsSingleValue = true,
                            NewValue = value,
                            PosPointNewValue = this.matrix.GetFirstPossiblePosPointInRange(RangeMatrix.FormHorizontalLine(i), value)
                        };
                    }
                }
                for (int i = 0; i < 9; i++)
                {
                    if (this.matrix.GetCountPossiblePosPointInRange(RangeMatrix.FormVerticalLine(i), value) == 1)
                    {
                        return new SolutionMethod()
                        {
                            Algorithm = AlgorithmSolutionMethod.Hidden_Single,
                            IsSingleValue = true,
                            NewValue = value,
                            PosPointNewValue = this.matrix.GetFirstPossiblePosPointInRange(RangeMatrix.FormVerticalLine(i), value)
                        };
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (this.matrix.GetCountPossiblePosPointInRange(RangeMatrix.FormSquare(i, j), value) == 1)
                        {
                            return new SolutionMethod()
                            {
                                Algorithm = AlgorithmSolutionMethod.Hidden_Single,
                                IsSingleValue = true,
                                NewValue = value,
                                PosPointNewValue = this.matrix.GetFirstPossiblePosPointInRange(RangeMatrix.FormSquare(i, j), value)
                            };
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region Intersections
        private SolutionMethod Locked_Pair()
        {
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    PosSquare pos_s = new PosSquare(si, sj);
                    SolutionMethod pair = this.GetLockedPairInRange(RangeMatrix.FormSquare(pos_s));
                    if (!(pair is null))
                    {
                        return pair;
                    }
                }
            }
            return null;
        }
        private SolutionMethod Locked_Triple()
        {
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    SolutionMethod tripl = this.GetLockedPairInRange(RangeMatrix.FormSquare(si, sj));
                    if (!(tripl is null)) { return tripl; };
                }
            }
            return null;
        }
        private SolutionMethod Locked_Candidates_Type_Pointing()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (byte value = 1; value < 10; value++)
                    {
                        int size_ = this.matrix.GetCountPossiblePosPointInRange(RangeMatrix.FormSquare(i, j), value);
                        if (size_ == 3 || size_ == 2)
                        {
                            SolutionMethod Solution_method = new SolutionMethod()
                            {
                                Algorithm = AlgorithmSolutionMethod.Locked_Candidates_Type_Pointing,
                                IsSingleValue = false,
                                PosPoints = this.matrix.GetPossPosPointsInRange(RangeMatrix.FormSquare(i, j), value),
                                Values = new Set<byte>(value),
                                IS = (false, false, true, true)
                            };
                            if (SolutionMethodHandler.IsValid(this.matrix, Solution_method))
                            {
                                return Solution_method;
                            }
                        }
                    }
                }
            }
            return null;
        }
        private SolutionMethod Locked_Candidates_Type_Claiming()
        {
            for (byte value = 1; value < 10; value++)
            {
                for (int i = 0; i < 9; i++)
                {
                    int count_h = this.matrix.GetCountPossiblePosPointInRange(RangeMatrix.FormHorizontalLine(i), value);
                    if (count_h <= 3 && count_h >= 2)
                    {
                        SolutionMethod Solution_method = new SolutionMethod()
                        {
                            Algorithm = AlgorithmSolutionMethod.Locked_Candidates_Type_Claiming,
                            IsSingleValue = false,
                            PosPoints = this.matrix.GetPossPosPointsInRange(RangeMatrix.FormHorizontalLine(i), value),
                            Values = new Set<byte>(value),
                            IS = (false, true, false, false)
                        };
                        if (SolutionMethodHandler.IsValid(this.matrix, Solution_method))
                        {
                            return Solution_method;
                        }
                    }

                    int count_v = this.matrix.GetCountPossiblePosPointInRange(RangeMatrix.FormVerticalLine(i), value);
                    if (count_v <= 3 && count_v >= 2)
                    {
                        SolutionMethod Solution_method = new SolutionMethod()
                        {
                            IsSingleValue = false,
                            PosPoints = this.matrix.GetPossPosPointsInRange(RangeMatrix.FormVerticalLine(i), value),
                            Values = new Set<byte>(value),
                            IS = (false, true, false, false)
                        };
                        if (SolutionMethodHandler.IsValid(this.matrix, Solution_method))
                        {
                            return Solution_method;
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region Subsets
        private SolutionMethod Naked_Pair()
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod pair_h = this.GetLockedPairInRange(RangeMatrix.FormHorizontalLine(i));
                if (!(pair_h is null)) { return pair_h; }
                SolutionMethod pair_v = this.GetLockedPairInRange(RangeMatrix.FormVerticalLine(i));
                if (!(pair_v is null)) { return pair_v; }
            }
            return null;
        }
        private SolutionMethod Naked_Triple()
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod triple_h = this.GetLockedTripleInRange(RangeMatrix.FormHorizontalLine(i));
                if (!(triple_h is null)) { return triple_h; }
                SolutionMethod triple_v = this.GetLockedTripleInRange(RangeMatrix.FormVerticalLine(i));
                if (!(triple_v is null)) { return triple_v; }
            }
            return null;
        }
        private SolutionMethod Naked_Quadruple()
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod quadruple_h = this.GetNakedQuadrupleInRange(RangeMatrix.FormHorizontalLine(i));
                if (!(quadruple_h is null)) { return quadruple_h; }
                SolutionMethod quadruple_v = this.GetNakedQuadrupleInRange(RangeMatrix.FormVerticalLine(i));
                if (!(quadruple_v is null)) { return quadruple_v; }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    SolutionMethod quadruple_s = this.GetNakedQuadrupleInRange(RangeMatrix.FormSquare(i, j));
                    if (!(quadruple_s is null)) { return quadruple_s; }
                }
            }
            return null;
        }
        private SolutionMethod Hidden_Pair()
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod pair_h = this.GetHiddenPairInRange(RangeMatrix.FormHorizontalLine(i));
                if (!(pair_h is null)) { return pair_h; }
                SolutionMethod pair_v = this.GetHiddenPairInRange(RangeMatrix.FormVerticalLine(i));
                if (!(pair_v is null)) { return pair_v; }
            }
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    SolutionMethod pair = this.GetHiddenPairInRange(RangeMatrix.FormSquare(si, sj));
                    if (!(pair is null)) { return pair; }
                }
            }
            return null;
        }
        private SolutionMethod Hidden_Triple()
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod triple_h = this.GetHiddenTripleInRange(RangeMatrix.FormHorizontalLine(i));
                if (!(triple_h is null)) { return triple_h; };
                SolutionMethod triple_v = this.GetHiddenTripleInRange(RangeMatrix.FormVerticalLine(i));
                if (!(triple_v is null)) { return triple_v; };
            }
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    SolutionMethod triple = this.GetNakedQuadrupleInRange(RangeMatrix.FormSquare(si, sj));
                    if (!(triple is null)) { return triple; };
                }
            }
            return null;
        }
        private SolutionMethod Hidden_Quadruple()
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod quadruple_h = this.GetHiddenQuadrupleInRange(RangeMatrix.FormHorizontalLine(i));
                if (!(quadruple_h is null)) { return quadruple_h; };
                SolutionMethod quadruple_v = this.GetHiddenQuadrupleInRange(RangeMatrix.FormVerticalLine(i));
                if (!(quadruple_v is null)) { return quadruple_v; };
            }
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    SolutionMethod quadruple_s = this.GetNakedQuadrupleInRange(RangeMatrix.FormSquare(si, sj));
                    if(!(quadruple_s is null)) { return quadruple_s; };
                }
            }
            return null;
        }
        #endregion

        public SolutionMethod NextSlover()
        {
            foreach (Func<SolutionMethod> item in solution_methods)
            {
                SolutionMethod Solution_method = item.Invoke();
                if (!(Solution_method is null))
                {
                    return Solution_method;
                }
            }
            return null;
        }
    }
}
