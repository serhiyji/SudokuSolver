using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Matrix;
using SudokuSolver.SudokuSolverCore.Points;
using SudokuSolver.SudokuSolverCore.Solution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.SudokuSolverCore.BetterMatrix
{
    public partial class BetterMatrix<TPointMatrix> : Matrix<TPointMatrix> where TPointMatrix : IPointMatrix, new()
    {
        #region BetterMatrix
        private readonly Matrix<byte> __example = new Matrix<byte>();
        public BetterMatrix() : base(false)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    this.matrix[i, j] = new TPointMatrix();
                    this.matrix[i, j].value = 0;
                    this.matrix[i, j].Position = new PosPoint(i, j);
                }
            }
            __example.matrix = new byte[,]
            {
                { 1, 2, 3, 7, 8, 9, 4, 5, 6 },
                { 4, 5, 6, 1, 2, 3, 7, 8, 9 },
                { 7, 8, 9, 4, 5, 6, 1, 2, 3 },

                { 2, 3, 1, 8, 9, 7, 5, 6, 4 },
                { 5, 6, 4, 2, 3, 1, 8, 9, 7 },
                { 8, 9, 7, 5, 6, 4, 2, 3, 1 },

                { 3, 1, 2, 6, 4, 5, 9, 7, 8 },
                { 6, 4, 5, 9, 7, 8, 3, 1, 2 },
                { 9, 7, 8, 3, 1, 2, 6, 4, 5 }
            };
        }
        public void SetValue(PosPoint pos_p, byte value)
        {
            byte last_value = matrix[pos_p.i, pos_p.j].value;
            if (value == 0)
            {
                if (last_value != 0)
                {
                    this.matrix[pos_p.i, pos_p.j].value = 0;
                    this.SettingPosibleValue(pos_p, last_value);
                }
            }
            else
            {
                if (this.matrix[pos_p.i, pos_p.j].value == 0)
                {
                    if (this.matrix[pos_p.i, pos_p.j].set.Contains(value))
                    {
                        this.matrix[pos_p.i, pos_p.j].value = value;
                        this.matrix[pos_p.i, pos_p.j].set -= new Set<byte>(1, 2, 3, 4, 5, 6, 7, 8, 9);
                        this.ClearValueInSetInHorizontalLine(pos_p.i, value);
                        this.ClearValueInSetInVerticalLine(pos_p.j, value);
                        this.ClearValueInSetInSquare(new PosSquare(pos_p), value);
                    }
                }
                else
                {
                    this.SetValue(pos_p, 0);
                    this.SetValue(pos_p, value);
                }
            }
        }
        public void Fill(ref Matrix<int> mat)
        {
            for (int i = 0; i < mat.matrix.GetLength(0); i++)
            {
                for (int j = 0; j < mat.matrix.GetLength(1); j++)
                {
                    this.matrix[i, j].value = (byte)mat.matrix[i, j];
                }
            }
        }
        public void GenerateNewSudoku(int count = 0)
        {
            Random rand = new Random();
            this.ClearMatrix();
            Matrix<byte> mat = __example;
            mat.ToMixMatrix();
            for (int c = 0; c < count; c++)
            {
                int i = rand.Next(0, 9), j = rand.Next(0, 9);
                if (this.matrix[i, j].value == 0)
                {
                    this.SetValue(new PosPoint(i, j), mat.matrix[i, j]);
                }
                else
                {
                    c--;
                }
            }
            this.SetPossibleValues();
        }
        public bool LoadSudoku(string data)
        {
            try
            {
                var dat = data.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (dat.Count() != size * size) return false;
                int k = 0;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        this.SetValue(new PosPoint(i, j), byte.Parse(dat[k]));
                        k++;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string SaveSudoku()
        {
            string res = "";
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    res += matrix[i, j].value + ":";
                }
            }
            return res;
        }
        #endregion

        #region AuxiliaryAlgorithms
        public List<TPointMatrix> GetPointMatrixInRange(PosPoint pos1, PosPoint pos2)
        {
            List<TPointMatrix> Points = new List<TPointMatrix>();
            for (int i = pos1.i; i <= pos2.i; i++)
            {
                for (int j = pos1.j; j <= pos2.j; j++)
                {
                    Points.Add(matrix[i, j]);
                }
            }
            return Points;
        }
        public int GetCountValues()
        => this.GetPointMatrixInRange(new PosPoint(0, 0), new PosPoint(9, 9)).Where(p => p.value != 0).Count();

        private int GetNullValues(PosPoint pos1, PosPoint pos2)
        => this.GetPointMatrixInRange(pos1, pos2).Where(p => p.value == 0).Count();

        private bool IsOneNullInRange(PosPoint pos1, PosPoint pos2)
        => this.GetNullValues(pos1, pos2) == 1;
        
        public bool IsOneNullInHorizontalLine(int index)
        => this.IsOneNullInRange(new PosPoint(index, 0), new PosPoint(index, size - 1));
        public bool IsOneNullInVerticallLine(int index)
        => this.IsOneNullInRange(new PosPoint(0, index), new PosPoint(size - 1, index));
        public bool IsOneNullInSquare(PosSquare pos_s)
        => this.IsOneNullInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2));

        public Set<byte> GetPossValueInPosPoint(PosPoint pos_p)
        => new Set<byte>(this.matrix[pos_p.i, pos_p.j].set);
        public bool IsNullInPosPoint(PosPoint pos_p)
        => this.matrix[pos_p.i, pos_p.j].value == 0;
        public bool IsOnePossibleValueInPosPoint(PosPoint pos_p)
        => this.matrix[pos_p.i, pos_p.j].set.Count() == 1;
        public byte GetFirstValueSetInPosPoint(PosPoint pos_p)
        => this.matrix[pos_p.i, pos_p.j].set.Count > 0 ? this.matrix[pos_p.i, pos_p.j].set[0] : (byte)0;

        private Arrange<PosPoint> GetPossPosPointsInRange(PosPoint pos1, PosPoint pos2, byte value)
        => new Arrange<PosPoint>(this.GetPointMatrixInRange(pos1, pos2).Where(p => p.value == 0 && p.set.Contains(value)).Select(p => p.Position).ToArray());
        
        public Arrange<PosPoint> GetPossPosPointsInHorizontalLine(int index, byte value)
        => this.GetPossPosPointsInRange(new PosPoint(index, 0), new PosPoint(index, size - 1), value);
        public Arrange<PosPoint> GetPossPosPointsInVerticalLine(int index, byte value)
        => this.GetPossPosPointsInRange(new PosPoint(0, index), new PosPoint(size - 1, index), value);
        public Arrange<PosPoint> GetPossPosPointsInSquare(PosSquare pos_s, byte value)
        => this.GetPossPosPointsInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2), value);

        private byte GetCountPossiblePosPointInRange(PosPoint pos1, PosPoint pos2, byte value)
        => (byte)this.GetPointMatrixInRange(pos1, pos2).Where(p => p.set.Contains(value)).Count();

        public byte GetCountPossiblePosPointInHorizontalLine(int index, byte value)
        => this.GetCountPossiblePosPointInRange(new PosPoint(index, 0), new PosPoint(index, size - 1), value);
        public byte GetCountPossiblePosPointInVerticalLine(int index, byte value)
        => this.GetCountPossiblePosPointInRange(new PosPoint(0, index), new PosPoint(size - 1, index), value);
        public byte GetCountPossiblePosPointInSquare(PosSquare pos_s, byte value)
        => this.GetCountPossiblePosPointInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2), value);

        private PosPoint GetOneNullPosPointInRange(PosPoint pos1, PosPoint pos2)
        {
            for (int i = pos1.i; i <= pos2.i; i++)
            {
                for (int j = pos1.j; j <= pos2.j; j++)
                {
                    if (this.matrix[i, j].value == 0)
                    {
                        return new PosPoint(i, j);
                    }
                }
            }
            return new PosPoint(0, 0);
        }

        public PosPoint GetPosPointNullHorizontalLine(int index)
        => this.GetOneNullPosPointInRange(new PosPoint(index, 0), new PosPoint(index, size - 1));
        public PosPoint GetPosPointNullVerticalLine(int index)
        => this.GetOneNullPosPointInRange(new PosPoint(0, index), new PosPoint(size - 1, index));
        public PosPoint GetPosPointNullSquare(PosSquare pos_s)
        => this.GetOneNullPosPointInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2));

        private PosPoint GetFirstPossiblePosPointInRange(PosPoint pos1, PosPoint pos2, byte value)
        {
            for (int i = pos1.i; i <= pos2.i; i++)
            {
                for (int j = pos1.j; j <= pos2.j; j++)
                {
                    if (this.matrix[i, j].value == 0)
                    {
                        if (this.matrix[i, j].set.Contains(value))
                        {
                            return new PosPoint(i, j);
                        }
                    }
                }
            }
            return new PosPoint(0, 0);
        }

        public PosPoint GetFirstPossiblePosPointInHorizontalLine(int index, byte value)
        => this.GetFirstPossiblePosPointInRange(new PosPoint(index, 0), new PosPoint(index, size - 1), value);
        public PosPoint GetFirstPossiblePosPointInVerticalLine(int index, byte value)
        => this.GetFirstPossiblePosPointInRange(new PosPoint(0, index), new PosPoint(size - 1, index), value);
        public PosPoint GetFirstPossiblePosPointInSquare(PosSquare pos_s, byte value)
        => this.GetFirstPossiblePosPointInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2), value);

        private Arrange<PosPoint> GetPosPointsInRange(PosPoint pos1, PosPoint pos2)
        => new Arrange<PosPoint>(this.GetPointMatrixInRange(pos1, pos2).Where(p => p.value == 0).Select(p => p.Position).ToArray());
        
        #endregion

        #region ClearHendlers
        private bool ClearValueInSetInRange(PosPoint pos1, PosPoint pos2, byte value, Arrange<PosPoint> arr = default(Arrange<PosPoint>))
        {
            arr = (arr is null) ? new Arrange<PosPoint>() : arr;
            bool total = false;
            for (int i = pos1.i; i <= pos2.i; i++)
            {
                for (int j = pos1.j; j <= pos2.j; j++)
                {
                    if ((this.matrix[i, j].value == 0) && !(arr.Contains(new PosPoint(i, j))))
                    {
                        int size_start = this.matrix[i, j].set.Count();
                        this.matrix[i, j].set -= new Set<byte>(value);
                        if (size_start != this.matrix[i, j].set.Count())
                        {
                            total = true;
                        }
                    }
                }
            }
            return total;
        }
        private bool ClearValuesInSetInRange(PosPoint pos1, PosPoint pos2, Set<byte> values, Arrange<PosPoint> arr = default(Arrange<PosPoint>))
        {
            bool total = false;
            for (int i = 0; i < values.Count(); i++)
            {
                total = this.ClearValueInSetInRange(pos1, pos2, values[i], arr) || total;
            }
            return total;
        }

        public bool ClearValueInSetInHorizontalLine(int index, byte value, Arrange<PosPoint> arr = default(Arrange<PosPoint>))
        => this.ClearValueInSetInRange(new PosPoint(index, 0), new PosPoint(index, size - 1), value, arr);
        public bool ClearValueInSetInVerticalLine(int index, byte value, Arrange<PosPoint> arr = default(Arrange<PosPoint>))
        => this.ClearValueInSetInRange(new PosPoint(0, index), new PosPoint(size - 1, index), value, arr);
        public bool ClearValueInSetInSquare(PosSquare pos_s, byte value, Arrange<PosPoint> arr = default(Arrange<PosPoint>))
        => this.ClearValueInSetInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2), value, arr);
        public bool ClearValueInPosPoint(PosPoint pos_p, byte value)
        => this.ClearValueInSetInRange(pos_p, pos_p, value);

        public bool ClearValuesInSetInHorizontalLine(int index, Set<byte> values, Arrange<PosPoint> arr = default(Arrange<PosPoint>))
        => this.ClearValuesInSetInRange(new PosPoint(index, 0), new PosPoint(index, size - 1), values, arr);
        public bool ClearValuesInSetInVerticalLine(int index, Set<byte> values, Arrange<PosPoint> arr = default(Arrange<PosPoint>))
        => this.ClearValuesInSetInRange(new PosPoint(0, index), new PosPoint(size - 1, index), values, arr);
        public bool ClearValuesInSetInSquare(PosSquare pos_s, Set<byte> values, Arrange<PosPoint> arr = default(Arrange<PosPoint>))
        => this.ClearValuesInSetInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2), values, arr);
        public bool ClearValuesInSetInPosPoint(PosPoint pos_p, Set<byte> values)
        => this.ClearValuesInSetInRange(pos_p, pos_p, values);

        public void SettingPosibleValue(PosPoint pos_p, byte value)
        {
            this.SettingPosibleValueInHorizontalLine(pos_p.i, value);
            this.SettingPosibleValueInVerticalLine(pos_p.j, value);
            this.SettingPosibleValueInSquare(new PosSquare(pos_p), value);
            this.matrix[pos_p.i, pos_p.j].set = GetPossibleValuesInPosPoint(pos_p);
        }
        private void SettingPosibleValue(PosPoint pos1, PosPoint pos2, byte value)
        {
            for (int i = pos1.i; i <= pos2.i; i++)
            {
                for (int j = pos1.j; j <= pos2.j; j++)
                {
                    if (matrix[i, j].value == 0 && this.GetPossibleValuesInPosPoint(new PosPoint(i, j)).Contains(value))
                    {
                        this.matrix[i, j].set += new Set<byte>(value);
                    }
                }
            }
        }
        private void SettingPosibleValueInHorizontalLine(int index, byte value)
        { this.SettingPosibleValue(new PosPoint(index, 0), new PosPoint(index, size - 1), value); }
        private void SettingPosibleValueInVerticalLine(int index, byte value)
        { this.SettingPosibleValue(new PosPoint(0, index), new PosPoint(size - 1, index), value); }
        private void SettingPosibleValueInSquare(PosSquare pos_s, byte value)
        { this.SettingPosibleValue(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2), value); }
        #endregion

        #region SearchAlgorithmsSolutionPrivate
        // Locked / Naked
        private SolutionMethod GetLockedPairInRange(PosPoint pos1, PosPoint pos2)
        {
            Arrange<PosPoint> pos = this.GetPosPointsInRange(pos1, pos2);
            for (int i = 0; i < pos.Count(); i++)
            {
                for (int j = 0; j < pos.Count(); j++)
                {
                    if ((pos[i] != pos[j]) && (this.matrix[pos[i].i, pos[i].j].set == this.matrix[pos[j].i, pos[j].j].set)
                        && (this.matrix[pos[i].i, pos[i].j].set.Count() == 2))
                    {
                        SolutionMethod intersection = new SolutionMethod()
                        {
                            algorithm = AlgorithmSolutionMethod.Locked_Pair,
                            IsSingleValue = false,
                            PosPoints = new Arrange<PosPoint>(pos[i], pos[j]),
                            values = this.GetPossValueInPosPoint(pos[i])
                        };
                        if (SolutionMethodHandler.IsValid(this, intersection))
                        {
                            return intersection;
                        }
                    }
                }
            }
            return null;
        }
        private SolutionMethod GetLockedTripleInRange(PosPoint pos1, PosPoint pos2)
        {
            Arrange<PosPoint> pos = this.GetPosPointsInRange(pos1, pos2);
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
                                SolutionMethod intersection = new SolutionMethod()
                                {
                                    algorithm = AlgorithmSolutionMethod.Locked_Triple,
                                    IsSingleValue = false,
                                    PosPoints = new Arrange<PosPoint>(pos[i1], pos[i2], pos[i3]),
                                    values = all
                                };
                                if (SolutionMethodHandler.IsValid(this, intersection))
                                {
                                    return intersection;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        // Naked
        private SolutionMethod GetNakedQuadrupleInRange(PosPoint pos1, PosPoint pos2)
        {
            Arrange<PosPoint> pos = this.GetPosPointsInRange(pos1, pos2);
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
                                    SolutionMethod intersection = new SolutionMethod()
                                    {
                                        algorithm = AlgorithmSolutionMethod.Naked_Quadruple,
                                        IsSingleValue = false,
                                        PosPoints = new Arrange<PosPoint>(pos[i1], pos[i2], pos[i3], pos[i4]),
                                        values = all
                                    };
                                    if (SolutionMethodHandler.IsValid(this, intersection))
                                    {
                                        return intersection;
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
        private SolutionMethod GetHiddenPairInRange(PosPoint pos1, PosPoint pos2)
        {
            for (byte num1 = 0; num1 < 10; num1++)
            {
                for (byte num2 = 1; num2 < 10; num2++)
                {
                    if (num1 != num2)
                    {
                        int count_num1 = this.GetCountPossiblePosPointInRange(pos1, pos2, num1), count_num2 = this.GetCountPossiblePosPointInRange(pos1, pos2, num2);
                        if (count_num1 == 2 && count_num2 == 2)
                        {
                            Arrange<PosPoint> arr1 = this.GetPossPosPointsInRange(pos1, pos2, num1), arr2 = this.GetPossPosPointsInRange(pos1, pos2, num2);
                            if (arr1 == arr2)
                            {
                                SolutionMethod intersection = new SolutionMethod()
                                {
                                    algorithm = AlgorithmSolutionMethod.Hidden_Pair,
                                    IsSingleValue = false,
                                    PosPoints = new Arrange<PosPoint>(arr1[0], arr1[1]),
                                    values = new Set<byte>(num1, num2)
                                };
                                if (SolutionMethodHandler.IsValid(this, intersection))
                                {
                                    return intersection;
                                }
                            }
                        }
                    }

                }
            }
            return null;
        }
        private SolutionMethod GetHiddenTripleInRange(PosPoint pos1, PosPoint pos2)
        {
            for (byte num1 = 0; num1 < 10; num1++)
            {
                for (byte num2 = 0; num2 < 10; num2++)
                {
                    for (byte num3 = 0; num3 < 10; num3++)
                    {
                        int count_num1 = this.GetCountPossiblePosPointInRange(pos1, pos2, num1);
                        int count_num2 = this.GetCountPossiblePosPointInRange(pos1, pos2, num2);
                        int count_num3 = this.GetCountPossiblePosPointInRange(pos1, pos2, num3);
                        if (num1 != num2 && num2 != num3 && num1 != num3 && count_num1 <= 3 && count_num1 >= 2 && count_num2 <= 3 && count_num2 >= 2 && count_num3 <= 3 && count_num3 >= 2)
                        {
                            Set<PosPoint> poss_num = new Set<PosPoint>(this.GetPossPosPointsInRange(pos1, pos2, num1)) + new Set<PosPoint>(this.GetPossPosPointsInRange(pos1, pos2, num2)) + new Set<PosPoint>(this.GetPossPosPointsInRange(pos1, pos2, num3));
                            if (poss_num.Count() == 3)
                            {
                                Set<byte> set_other = new Set<byte>();
                                Set<byte> values = new Set<byte>();
                                for (int i = pos1.i; i <= pos2.i; i++)
                                {
                                    for (int j = pos1.j; j <= pos2.j; j++)
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
                                    SolutionMethod intersection = new SolutionMethod()
                                    {
                                        algorithm = AlgorithmSolutionMethod.Hidden_Triple,
                                        IsSingleValue = false,
                                        PosPoints = poss_num,
                                        values = values
                                    };
                                    if (SolutionMethodHandler.IsValid(this, intersection))
                                    {
                                        return intersection;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        private SolutionMethod GetHiddenQuadrupleInRange(PosPoint pos1, PosPoint pos2)
        {
            for (byte num1 = 1; num1 < 10; num1++)
            {
                for (byte num2 = 1; num2 < 10; num2++)
                {
                    for (byte num3 = 1; num3 < 10; num3++)
                    {
                        for (byte num4 = 1; num4 < 10; num4++)
                        {
                            int count_num1 = this.GetCountPossiblePosPointInRange(pos1, pos2, num1);
                            int count_num2 = this.GetCountPossiblePosPointInRange(pos1, pos2, num2);
                            int count_num3 = this.GetCountPossiblePosPointInRange(pos1, pos2, num3);
                            int count_num4 = this.GetCountPossiblePosPointInRange(pos1, pos2, num4);
                            if (num1 != num2 && num2 != num3 && num1 != num3 && num1 != num4 && num2 != num4 && num3 != num4
                                && count_num1 <= 4 && count_num1 >= 2 && count_num2 <= 4 && count_num2 >= 2
                                && count_num3 <= 4 && count_num3 >= 2 && count_num4 <= 4 && count_num4 >= 2)
                            {
                                Set<PosPoint> poss_num = new Set<PosPoint>(this.GetPossPosPointsInRange(pos1, pos2, num1))
                                    + new Set<PosPoint>(this.GetPossPosPointsInRange(pos1, pos2, num2))
                                    + new Set<PosPoint>(this.GetPossPosPointsInRange(pos1, pos2, num3))
                                    + new Set<PosPoint>(this.GetPossPosPointsInRange(pos1, pos2, num4));
                                if (poss_num.Count() == 4)
                                {
                                    Set<byte> set_other = new Set<byte>();
                                    Set<byte> values = new Set<byte>();
                                    for (int i = pos1.i; i <= pos2.i; i++)
                                    {
                                        for (int j = pos1.j; j <= pos2.j; j++)
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
                                        SolutionMethod intersection = new SolutionMethod()
                                        {
                                            algorithm = AlgorithmSolutionMethod.Hidden_Quadruple,
                                            IsSingleValue = false,
                                            PosPoints = poss_num,
                                            values = values
                                        };
                                        if (SolutionMethodHandler.IsValid(this, intersection))
                                        {
                                            return intersection;
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

        #region SearchAlgorithmsSolutionPublic
        public SolutionMethod GetLockedPairInHorizontalLine(int index)
        => this.GetLockedPairInRange(new PosPoint(index, 0), new PosPoint(index, size - 1));
        public SolutionMethod GetLockedPairInVerticalLine(int index)
        => this.GetLockedPairInRange(new PosPoint(0, index), new PosPoint(size - 1, index));
        public SolutionMethod GetLockedPairInSquare(PosSquare pos_s)
        => this.GetLockedPairInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2));
        // Locked Triple
        public SolutionMethod GetLockedTripleInHorizontalLine(int index)
        => this.GetLockedTripleInRange(new PosPoint(index, 0), new PosPoint(index, size - 1));
        public SolutionMethod GetLockedTripleInVerticalLine(int index)
        => this.GetLockedTripleInRange(new PosPoint(0, index), new PosPoint(size - 1, index));
        public SolutionMethod GetLockedTripleInSquare(PosSquare pos_s)
        => this.GetLockedTripleInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2));
        // Hiden pair
        public SolutionMethod GetHiddenPairInHorizontalLine(int index)
        => this.GetHiddenPairInRange(new PosPoint(index, 0), new PosPoint(index, size - 1));
        public SolutionMethod GetHiddenPairInVerticalLine(int index)
        => this.GetHiddenPairInRange(new PosPoint(0, index), new PosPoint(size - 1, index));
        public SolutionMethod GetHiddenPairInSquare(PosSquare pos_s)
        => this.GetHiddenPairInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2));
        // Hiden Truple
        public SolutionMethod GetHiddenTripleInHorizontalLine(int index)
        => this.GetHiddenTripleInRange(new PosPoint(index, 0), new PosPoint(index, size - 1));
        public SolutionMethod GetHiddenTripleInVerticalLine(int index)
        => this.GetHiddenTripleInRange(new PosPoint(0, index), new PosPoint(size - 1, index));
        public SolutionMethod GetHiddenTripleInSquare(PosSquare pos_s)
        => this.GetHiddenTripleInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2));
        // Naked Quadruple
        public SolutionMethod GetNakedQuadrupleInHorizontalLine(int index)
        => this.GetNakedQuadrupleInRange(new PosPoint(index, 0), new PosPoint(index, size - 1));
        public SolutionMethod GetNakedQuadrupleInVerticalLine(int index)
        => this.GetNakedQuadrupleInRange(new PosPoint(0, index), new PosPoint(size - 1, index));
        public SolutionMethod GetNakedQuadrupleInSquare(PosSquare pos_s)
        => this.GetNakedQuadrupleInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2));
        // Hiden Quadruple
        public SolutionMethod GetHiddenQuadrupleInHorizontalLine(int index)
        => this.GetHiddenQuadrupleInRange(new PosPoint(index, 0), new PosPoint(index, size - 1));
        public SolutionMethod GetHiddenQuadrupleInVerticalLine(int index)
        => this.GetHiddenQuadrupleInRange(new PosPoint(0, index), new PosPoint(size - 1, index));
        public SolutionMethod GetHiddenQuadrupleInSquare(PosSquare pos_s)
        => this.GetHiddenQuadrupleInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2));
        #endregion

        #region SetStartValues
        public void SetPossibleValues()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (this.matrix[i, j].value == 0)
                    {
                        this.matrix[i, j].set = this.GetPossibleValuesInPosPoint(new PosPoint(i, j));
                    }
                }
            }
        }
        public void ClearMatrix()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    //this.matrix[i, j].SetToDefoltStatusItem();
                    this.SetValue(new PosPoint(i, j), 0);
                }
            }
            this.SetPossibleValues();
        }
        private Set<byte> GetPossibleValuesInPosPoint(PosPoint pos_p)
        {
            Set<byte> set1 = new Set<byte>(this.GetSetHorizontalLine(pos_p.i));
            Set<byte> set2 = new Set<byte>(this.GetSetVerticalLine(pos_p.j));
            Set<byte> set3 = new Set<byte>(this.GetSetSquare(new PosSquare(pos_p)));
            return (new Set<byte>(1, 2, 3, 4, 5, 6, 7, 8, 9) - (set1 + set2 + set3));
        }
        private Set<byte> GetSetInRange(PosPoint pos1, PosPoint pos2)
        => new Set<byte>(this.GetPointMatrixInRange(pos1, pos2).Where(p => p.value != 0).Select(p => p.value).ToArray());
     
        private Set<byte> GetSetHorizontalLine(int index)
        => this.GetSetInRange(new PosPoint(index, 0), new PosPoint(index, size - 1));
        private Set<byte> GetSetVerticalLine(int index)
        => this.GetSetInRange(new PosPoint(0, index), new PosPoint(size - 1, index));
        private Set<byte> GetSetSquare(PosSquare pos_s)
        => this.GetSetInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2));
        #endregion

    }
}
