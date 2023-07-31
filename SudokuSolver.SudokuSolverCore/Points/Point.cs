using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;

namespace SudokuSolver.SudokuSolverCore.Points
{
    public partial class Point
    {
        public byte value { get; set; }
        public Set<byte> set { get; set; }
        public bool IsSelected { get; internal set; }

        public Point(byte v)
        {
            this.value = v;
            this.set = new Set<byte>();
        }
        public Point() : this(0) { }
        public static bool operator ==(Point p1, byte value)
        {
            return p1.Equals(new Point(value));
        }
        public static bool operator !=(Point p1, byte value)
        {
            return !p1.Equals(new Point(value));
        }
        public override string ToString()
        {
            return $"{value}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            if (!(obj is Point)) { return false; }
            Point other = (Point)obj;
            return value == other.value;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
