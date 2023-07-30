using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Solution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore
{
    public class SudokuSloverHandler
    {
        public BetterMatrix.BetterMatrix matrix { get; set; }
        private List<Func<SolutionMethod>> solution_methods;
        public SudokuSloverHandler(ref BetterMatrix.BetterMatrix matrix)
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

        private SolutionMethod Full_House()
        {
            // Horizontal Line
            for (int i = 0; i < 9; i++)
            {
                if (this.matrix.IsOneNullInHorizontalLine(i))
                {
                    PosPoint pos_p = this.matrix.GetPosPointNullHorizontalLine(i);
                    byte NewValue_ = this.matrix.GetFirstValueSetInPosPoint(pos_p);
                    if (NewValue_ == 0) continue;
                    return new SolutionMethod()
                    {
                        algorithm = AlgorithmSudokuSlover.Full_House,
                        IsSingleValue = true,
                        NewValue = NewValue_,
                        PosPointNewValue = pos_p
                    };
                }
            }
            // Verticall Line
            for (int i = 0; i < 9; i++)
            {
                if (this.matrix.IsOneNullInVerticallLine(i))
                {
                    PosPoint pos_p = this.matrix.GetPosPointNullVerticalLine(i);
                    byte NewValue_ = this.matrix.GetFirstValueSetInPosPoint(pos_p);
                    if (NewValue_ == 0) continue;
                    return new SolutionMethod()
                    {
                        algorithm = AlgorithmSudokuSlover.Full_House,
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
                    if (this.matrix.IsOneNullInSquare(pos_s))
                    {
                        PosPoint pos_p = this.matrix.GetPosPointNullSquare(pos_s);
                        byte NewValue_ = this.matrix.GetFirstValueSetInPosPoint(pos_p);
                        if (NewValue_ == 0) continue;
                        return new SolutionMethod()
                        {
                            algorithm = AlgorithmSudokuSlover.Full_House,
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
                            algorithm = AlgorithmSudokuSlover.Naked_Single,
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
                    if (this.matrix.GetCountPossiblePosPointInHorizontalLine(i, value) == 1)
                    {
                        return new SolutionMethod()
                        {
                            algorithm = AlgorithmSudokuSlover.Hidden_Single,
                            IsSingleValue = true,
                            NewValue = value,
                            PosPointNewValue = this.matrix.GetFirstPossiblePosPointInHorizontalLine(i, value)
                        };
                    }
                }
                for (int i = 0; i < 9; i++)
                {
                    if (this.matrix.GetCountPossiblePosPointInVerticalLine(i, value) == 1)
                    {
                        return new SolutionMethod()
                        {
                            algorithm = AlgorithmSudokuSlover.Hidden_Single,
                            IsSingleValue = true,
                            NewValue = value,
                            PosPointNewValue = this.matrix.GetFirstPossiblePosPointInVerticalLine(i, value)
                        };
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (this.matrix.GetCountPossiblePosPointInSquare(new PosSquare(i, j), value) == 1)
                        {
                            return new SolutionMethod()
                            {
                                algorithm = AlgorithmSudokuSlover.Hidden_Single,
                                IsSingleValue = true,
                                NewValue = value,
                                PosPointNewValue = this.matrix.GetFirstPossiblePosPointInSquare(new PosSquare(i, j), value)
                            };
                        }
                    }
                }
            }
            return null;
        }
        private SolutionMethod Locked_Pair()
        {
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    PosSquare pos_s = new PosSquare(si, sj);
                    SolutionMethod pair = this.matrix.GetLockedPairInSquare(pos_s);
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
                    PosSquare pos_s = new PosSquare(si, sj);
                    SolutionMethod tripl = this.matrix.GetLockedTripleInSquare(pos_s);
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
                    PosSquare pos_s = new PosSquare(i, j);
                    for (byte value = 1; value < 10; value++)
                    {
                        int size_ = this.matrix.GetCountPossiblePosPointInSquare(pos_s, value);
                        if (size_ == 3 || size_ == 2)
                        {
                            SolutionMethod intersection = new SolutionMethod()
                            {
                                algorithm = AlgorithmSudokuSlover.Locked_Candidates_Type_Pointing,
                                IsSingleValue = false,
                                PosPoints = this.matrix.GetPossPosPointsInSquare(pos_s, value),
                                values = new Set<byte>(value),
                                IS = (false, false, true, true)
                            };
                            if (intersection.IsValid(this.matrix))
                            {
                                return intersection;
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
                    int count_h = this.matrix.GetCountPossiblePosPointInHorizontalLine(i, value);
                    if (count_h <= 3 && count_h >= 2)
                    {
                        SolutionMethod intersection = new SolutionMethod()
                        {
                            algorithm = AlgorithmSudokuSlover.Locked_Candidates_Type_Claiming,
                            IsSingleValue = false,
                            PosPoints = this.matrix.GetPossPosPointsInHorizontalLine(i, value),
                            values = new Set<byte>(value),
                            IS = (false, true, false, false)
                        };
                        if (intersection.IsValid(this.matrix))
                        {
                            return intersection;
                        }
                    }

                    int count_v = this.matrix.GetCountPossiblePosPointInVerticalLine(i, value);
                    if (count_v <= 3 && count_v >= 2)
                    {
                        SolutionMethod intersection = new SolutionMethod()
                        {
                            IsSingleValue = false,
                            PosPoints = this.matrix.GetPossPosPointsInVerticalLine(i, value),
                            values = new Set<byte>(value),
                            IS = (false, true, false, false)
                        };
                        if (intersection.IsValid(this.matrix))
                        {
                            return intersection;
                        }
                    }
                }
            }
            return null;
        }
        private SolutionMethod Naked_Pair()
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod pair_h = this.matrix.GetLockedPairInHorizontalLine(i);
                if (!(pair_h is null)) { return pair_h; }
                SolutionMethod pair_v = this.matrix.GetLockedPairInVerticalLine(i);
                if (!(pair_v is null)) { return pair_v; }
            }
            return null;
        }
        private SolutionMethod Naked_Triple()
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod triple_h = this.matrix.GetLockedTripleInHorizontalLine(i);
                if (!(triple_h is null)) { return triple_h; }
                SolutionMethod triple_v = this.matrix.GetLockedTripleInVerticalLine(i);
                if (!(triple_v is null)) { return triple_v; }
            }
            return null;
        }
        private SolutionMethod Naked_Quadruple()
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod quadruple_h = this.matrix.GetNakedQuadrupleInHorizontalLine(i);
                if (!(quadruple_h is null)) { return quadruple_h; }
                SolutionMethod quadruple_v = this.matrix.GetNakedQuadrupleInVerticalLine(i);
                if (!(quadruple_v is null)) { return quadruple_v; }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    SolutionMethod quadruple_s = this.matrix.GetNakedQuadrupleInSquare(new PosSquare(i, j));
                    if (!(quadruple_s is null)) { return quadruple_s; }
                }
            }
            return null;
        }
        private SolutionMethod Hidden_Pair()
        {
            for (int i = 0; i < 9; i++)
            {

                SolutionMethod pair_h = this.matrix.GetHiddenPairInHorizontalLine(i);
                if (!(pair_h is null)) { return pair_h; }
                SolutionMethod pair_v = this.matrix.GetHiddenPairInVerticalLine(i);
                if (!(pair_v is null)) { return pair_v; }
            }
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    SolutionMethod pair = this.matrix.GetHiddenPairInSquare(new PosSquare(si, sj));
                    if (!(pair is null)) { return pair; }
                }
            }
            return null;
        }
        private SolutionMethod Hidden_Triple()
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod triple_h = this.matrix.GetHiddenTripleInHorizontalLine(i);
                if (!(triple_h is null)) { return triple_h; };
                SolutionMethod triple_v = this.matrix.GetHiddenTripleInVerticalLine(i);
                if (!(triple_v is null)) { return triple_v; };
            }
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    SolutionMethod triple = this.matrix.GetHiddenTripleInSquare(new PosSquare(si, sj));
                    if (!(triple is null)) { return triple; };
                }
            }
            return null;
        }
        private SolutionMethod Hidden_Quadruple()
        {
            for (int i = 0; i < 9; i++)
            {
                SolutionMethod quadruple_h = this.matrix.GetHiddenQuadrupleInHorizontalLine(i);
                if (!(quadruple_h is null)) { return quadruple_h; };
                SolutionMethod quadruple_v = this.matrix.GetHiddenQuadrupleInVerticalLine(i);
                if (!(quadruple_v is null)) { return quadruple_v; };
            }
            for (int si = 0; si < 3; si++)
            {
                for (int sj = 0; sj < 3; sj++)
                {
                    SolutionMethod quadruple_s = this.matrix.GetHiddenQuadrupleInSquare(new PosSquare(si, sj));
                    if(!(quadruple_s is null)) { return quadruple_s; };
                }
            }
            return null;
        }

        public SolutionMethod NextSlover()
        {
            foreach (Func<SolutionMethod> item in solution_methods)
            {
                SolutionMethod intersection = item.Invoke();
                if (!(intersection is null))
                {
                    return intersection;
                }
            }
            return null;
        }
    }
}
