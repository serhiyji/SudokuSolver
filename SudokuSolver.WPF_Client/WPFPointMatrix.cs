﻿using PropertyChanged;
using SudokuSolver.SudokuSolverCore.Collections;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.Points;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SudokuSolver.WPF_Client
{
    [AddINotifyPropertyChangedInterface]
    public class WPFPointMatrix : SudokuSolver.SudokuSolverCore.Points.IPointMatrix
    {
        public byte value { get; set; }
        public Set<byte> set { get; set; }
        public PosPoint Position { get; set; }

        public static SolidColorBrush EmptyColor = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        public static SolidColorBrush SelectedColor = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));

        public static SolidColorBrush GreenColor = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
        public static SolidColorBrush RedColor = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        public static SolidColorBrush BlueColor = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));

        public bool IsSelected { get; set; } = false;
        public string Value => (value == 0) ? "" : value.ToString();
        public SolidColorBrush Selected => IsSelected ? SelectedColor : EmptyColor;
        public string Val1 => (this.set.Contains(1)) ? "1" : "";
        public string Val2 => (this.set.Contains(2)) ? "2" : "";
        public string Val3 => (this.set.Contains(3)) ? "3" : "";
        public string Val4 => (this.set.Contains(4)) ? "4" : "";
        public string Val5 => (this.set.Contains(5)) ? "5" : "";
        public string Val6 => (this.set.Contains(6)) ? "6" : "";
        public string Val7 => (this.set.Contains(7)) ? "7" : "";
        public string Val8 => (this.set.Contains(8)) ? "8" : "";
        public string Val9 => (this.set.Contains(9)) ? "9" : "";

        public ObservableCollection<SolidColorBrush> PossibleValues { get; set; } = new ObservableCollection<SolidColorBrush>(Enumerable.Repeat(EmptyColor, 9));

        public void SetToDefoltStatusItem()
        {
            for (int i = 0; i < this.PossibleValues.Count; i++)
            {
                this.PossibleValues[i] = EmptyColor;
            }
        }
        public void SwapFromOtherPointMatrix<TPointMatrix>(ref TPointMatrix other_point) where TPointMatrix : IPointMatrix, new()
        {
            (this.value, other_point.value) = (this.value, other_point.value);
            (this.set, other_point.set) = (this.set, other_point.set);
            (this.Position, other_point.Position) = (this.Position, other_point.Position);
        }
    }
}
