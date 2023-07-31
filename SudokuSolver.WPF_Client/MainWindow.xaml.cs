﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SudokuSolver.WPF_Client
{
    public partial class MainWindow : Window
    {
        private ViewModel model;
        public MainWindow()
        {
            InitializeComponent();
            this.model = new ViewModel();
            this.model.CloseWindow += (s, e) => { this.Close(); };
            this.DataContext = this.model;
        }
    }
}
