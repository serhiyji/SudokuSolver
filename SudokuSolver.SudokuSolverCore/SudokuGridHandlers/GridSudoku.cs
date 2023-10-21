using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Matrix;
using SudokuSolver.SudokuSolverCore.Points;
using SudokuSolver.SudokuSolverCore.Solution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.SudokuSolverCore.SudokuGridHandlers
{
    public class GridSudoku<TPointMatrix> : Matrix<TPointMatrix> where TPointMatrix : IPointMatrix, new()
    {
        #region GridSudoku        
        public GridSudoku() : base(false)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    this.matrix[i, j] = new TPointMatrix();
                    this.matrix[i, j].value = 0;
                    this.matrix[i, j].set = new Set<byte>();
                    this.matrix[i, j].Position = new PosPoint(i, j);
                }
            }
            
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
                        this.ClearValueInSetInRange(RangeMatrix.FormHorizontalLine(pos_p.i), value);
                        this.ClearValueInSetInRange(RangeMatrix.FormHorizontalLine(pos_p.j), value);
                        this.ClearValueInSetInRange(RangeMatrix.FormSquare(new PosSquare(pos_p)), value);
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
        public bool LoadSudoku(string data)
        {
            try
            {
                var dat = data.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (dat.Count() != SizeGridSudoku.CountCellsMatrix) return false;
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
        public List<TPointMatrix> GetPointMatrixInRange(RangeMatrix range)
        {
            List<TPointMatrix> Points = new List<TPointMatrix>();
            for (int i = range.Position1.i; i <= range.Position2.i; i++)
            {
                for (int j = range.Position1.j; j <= range.Position2.j; j++)
                {
                    Points.Add(matrix[i, j]);
                }
            }
            return Points;
        }
        public int GetCountValues()
        => this.GetPointMatrixInRange(RangeMatrix.GetRangeFullMatrix()).Where(p => p.value != 0).Count();

        private int GetNullValues(RangeMatrix range)
        => this.GetPointMatrixInRange(range).Where(p => p.value == 0).Count();

        public bool IsOneNullInRange(RangeMatrix range)
        => this.GetNullValues(range) == 1;

        public Set<byte> GetPossValueInPosPoint(PosPoint pos_p)
        => new Set<byte>(this.matrix[pos_p.i, pos_p.j].set);
        public bool IsNullInPosPoint(PosPoint pos_p)
        => this.matrix[pos_p.i, pos_p.j].value == 0;
        public bool IsOnePossibleValueInPosPoint(PosPoint pos_p)
        => this.matrix[pos_p.i, pos_p.j].set.Count() == 1;
        public byte GetFirstValueSetInPosPoint(PosPoint pos_p)
        => this.matrix[pos_p.i, pos_p.j].set.Count > 0 ? this.matrix[pos_p.i, pos_p.j].set[0] : (byte)0;

        public Arrange<PosPoint> GetPossPosPointsInRange(RangeMatrix range, byte value)
        => new Arrange<PosPoint>(this.GetPointMatrixInRange(range).Where(p => p.value == 0 && p.set.Contains(value)).Select(p => p.Position).ToArray());

        public byte GetCountPossiblePosPointInRange(RangeMatrix range, byte value)
        => (byte)this.GetPointMatrixInRange(range).Where(p => p.set.Contains(value)).Count();

        public PosPoint GetOneNullPosPointInRange(RangeMatrix range)
        {
            for (int i = range.Position1.i; i <= range.Position2.i; i++)
            {
                for (int j = range.Position1.j; j <= range.Position2.j; j++)
                {
                    if (this.matrix[i, j].value == 0)
                    {
                        return new PosPoint(i, j);
                    }
                }
            }
            return new PosPoint(0, 0);
        }

        public PosPoint GetFirstPossiblePosPointInRange(RangeMatrix range, byte value)
        {
            for (int i = range.Position1.i; i <= range.Position2.i; i++)
            {
                for (int j = range.Position1.j; j <= range.Position2.j; j++)
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

        public Arrange<PosPoint> GetPosPointsInRange(RangeMatrix range)
        => new Arrange<PosPoint>(this.GetPointMatrixInRange(range).Where(p => p.value == 0).Select(p => p.Position).ToArray());
        
        #endregion

        #region ClearHendlers
        public bool ClearValueInSetInRange(RangeMatrix range, byte value, Arrange<PosPoint> arr = default(Arrange<PosPoint>))
        {
            arr = (arr is null) ? new Arrange<PosPoint>() : arr;
            bool total = false;
            for (int i = range.Position1.i; i <= range.Position2.i; i++)
            {
                for (int j = range.Position1.j; j <= range.Position2.j; j++)
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
        public bool ClearValuesInSetInRange(RangeMatrix range, Set<byte> values, Arrange<PosPoint> arr = default(Arrange<PosPoint>))
        {
            bool total = false;
            for (int i = 0; i < values.Count(); i++)
            {
                total = this.ClearValueInSetInRange(range, values[i], arr) || total;
            }
            return total;
        }

        public bool ClearValueInPosPoint(PosPoint pos_p, byte value)
        => this.ClearValueInSetInRange(new RangeMatrix(pos_p, pos_p), value);

        public bool ClearValuesInSetInPosPoint(PosPoint pos_p, Set<byte> values)
        => this.ClearValuesInSetInRange(new RangeMatrix(pos_p, pos_p), values);

        public void SettingPosibleValue(PosPoint pos_p, byte value)
        {
            this.ClearValueInSetInRange(RangeMatrix.FormHorizontalLine(pos_p.i), value);
            this.ClearValueInSetInRange(RangeMatrix.FormVerticalLine(pos_p.j), value);
            this.ClearValueInSetInRange(RangeMatrix.FormSquare(new PosSquare(pos_p)), value);
            this.matrix[pos_p.i, pos_p.j].set = GetPossibleValuesInPosPoint(pos_p);
        }
        public void SettingPosibleValue(RangeMatrix range, byte value)
        {
            for (int i = range.Position1.i; i <= range.Position2.i; i++)
            {
                for (int j = range.Position1.j; j <= range.Position2.j; j++)
                {
                    if (matrix[i, j].value == 0 && this.GetPossibleValuesInPosPoint(new PosPoint(i, j)).Contains(value))
                    {
                        this.matrix[i, j].set += new Set<byte>(value);
                    }
                }
            }
        }
        #endregion

        #region SetStartValues
        public void SetPossibleValues()
        {
            for (int i = 0; i < SizeGridSudoku.SizeMatrixVertical; i++)
            {
                for (int j = 0; j < SizeGridSudoku.SizeMatrixHorizontal; j++)
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
            for (int i = 0; i < SizeGridSudoku.SizeMatrixVertical; i++)
            {
                for (int j = 0; j < SizeGridSudoku.SizeMatrixHorizontal; j++)
                {
                    this.SetValue(new PosPoint(i, j), 0);
                }
            }
            this.SetPossibleValues();
        }
        private Set<byte> GetPossibleValuesInPosPoint(PosPoint pos_p)
        {
            Set<byte> set1 = new Set<byte>(this.GetSetInRange(RangeMatrix.FormHorizontalLine(pos_p.i)));
            Set<byte> set2 = new Set<byte>(this.GetSetInRange(RangeMatrix.FormVerticalLine(pos_p.j)));
            Set<byte> set3 = new Set<byte>(this.GetSetInRange(RangeMatrix.FormSquare(new PosSquare(pos_p))));
            return (SizeGridSudoku.AllStandardPosibleValues - (set1 + set2 + set3));
        }
        private Set<byte> GetSetInRange(RangeMatrix range)
        => new Set<byte>(this.GetPointMatrixInRange(range).Where(p => p.value != 0).Select(p => p.value).ToArray());
        #endregion

    }
}
