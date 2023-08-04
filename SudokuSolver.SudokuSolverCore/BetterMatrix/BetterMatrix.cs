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
    public class BetterMatrix<TPointMatrix> : Matrix<TPointMatrix> where TPointMatrix : IPointMatrix, new()
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

        public Arrange<PosPoint> GetPossPosPointsInRange(PosPoint pos1, PosPoint pos2, byte value)
        => new Arrange<PosPoint>(this.GetPointMatrixInRange(pos1, pos2).Where(p => p.value == 0 && p.set.Contains(value)).Select(p => p.Position).ToArray());
        
        public Arrange<PosPoint> GetPossPosPointsInHorizontalLine(int index, byte value)
        => this.GetPossPosPointsInRange(new PosPoint(index, 0), new PosPoint(index, size - 1), value);
        public Arrange<PosPoint> GetPossPosPointsInVerticalLine(int index, byte value)
        => this.GetPossPosPointsInRange(new PosPoint(0, index), new PosPoint(size - 1, index), value);
        public Arrange<PosPoint> GetPossPosPointsInSquare(PosSquare pos_s, byte value)
        => this.GetPossPosPointsInRange(new PosPoint(pos_s.i * 3, pos_s.j * 3), new PosPoint(pos_s.i * 3 + 2, pos_s.j * 3 + 2), value);

        public byte GetCountPossiblePosPointInRange(PosPoint pos1, PosPoint pos2, byte value)
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

        public Arrange<PosPoint> GetPosPointsInRange(PosPoint pos1, PosPoint pos2)
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
