using SudokuSolver.SudokuSolverCore.Coordinates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SudokuSolver.SudokuSolverCore.Matrix
{
    public class Matrix<T> where T : new()
    {
        public T[,] matrix { get; protected set; }

        public Matrix(bool IsSetDefaultValues = true)
        {
            this.matrix = new T[SizeGridSudoku.SizeMatrixVertical, SizeGridSudoku.SizeMatrixHorizontal];
            if (IsSetDefaultValues)
            {
                SetDelfaultValues();
            }
        }
        public Matrix(T[,] data, bool IsSetDefaultValues = true) : this(IsSetDefaultValues)
        {
            this.matrix = data;
        }
        public void SetDelfaultValues()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    System.Reflection.ConstructorInfo constructor = typeof(T).GetConstructor(Type.EmptyTypes);
                    T obj = ReferenceEquals(constructor, null) ? default : (T)constructor.Invoke(new object[0]);
                    matrix[i, j] = obj;
                }
            }
        }
        public void SetValue(PosPoint pos, T value)
        {
            matrix[pos.i, pos.j] = value;
        }
        public T this[int i, int j]
        {
            get
            {
                return matrix[i, j];
            }
            protected set
            {
                matrix[i, j] = value;
            }
        }
        public T this[PosPoint pos_p]
        {
            get
            {
                return matrix[pos_p.i, pos_p.j];
            }
            protected set
            {
                matrix[pos_p.i, pos_p.j] = value;
            }
        }        
        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    res += matrix[i, j].ToString() + "  ";
                }
                res += "\n";
            }
            return res;
        }
    }
}
