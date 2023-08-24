using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using System.Security.Principal;

namespace SudokuSolver.SudokuSolverCore.Points
{
    public interface IPointMatrix
    {
        public byte value { get; set; }
        public Set<byte> set { get; set; }
        public PosPoint Position { get; set; }

    }
    public partial class PointMatrix : IPointMatrix
    {
        public byte value { get; set; }
        public Set<byte> set { get; set; }
        public PosPoint Position { get; set; }

        public PointMatrix(byte v)
        {
            this.value = v;
            this.set = new Set<byte>();
        }
        public PointMatrix() : this(0) { }
        public static bool operator ==(PointMatrix p1, byte value)
        {
            return p1.Equals(new PointMatrix(value));
        }
        public static bool operator !=(PointMatrix p1, byte value)
        {
            return !p1.Equals(new PointMatrix(value));
        }
        public override string ToString()
        {
            return $"{value}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            if (!(obj is PointMatrix)) { return false; }
            PointMatrix other = (PointMatrix)obj;
            return value == other.value;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
