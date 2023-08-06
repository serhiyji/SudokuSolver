using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using System.Windows.Controls;
using System.Windows;
using SudokuSolver.SudokuSolverCore.Coordinates;
using SudokuSolver.SudokuSolverCore.SudokuGridHandlers;
using SudokuSolver.SudokuSolverCore;
using SudokuSolver.SudokuSolverCore.Solution;
using SudokuSolver.SudokuSolverCore.Points;

namespace SudokuSolver.WPF_Client
{
    [AddINotifyPropertyChangedInterface]
    public partial class ViewModel
    {
        // Matrix
        private CursorPosition cursorPosition;
        private GridSudoku<WPFPointMatrix> matrix;
        private SudokuSloverHandler<WPFPointMatrix> slover;
        public SolutionMethod Intersection => Solution.Instance.Intersection;
        public bool IsExecute => Solution.Instance.IsExecute;
        private ObservableCollection<WPFPointMatrix> points { get; set; }
        public IEnumerable<WPFPointMatrix> Points => points;

        // Window control
        public event EventHandler CloseWindow;

        // SudokusList
        private ObservableCollection<Database.Entities.SavingSudoku> sudokus { get; set; }
        public IEnumerable<Database.Entities.SavingSudoku> Sudokus => sudokus;

        private Database.Entities.SavingSudoku selectedSudoku;
        public Database.Entities.SavingSudoku SelectedSudoku
        {
            get { return selectedSudoku; }
            set { selectedSudoku = value; }
        }
        public bool IsSelectedItemSudokus => SelectedSudoku != null;
        public Visibility VisibilityListSudokus { get; set; }
        public string TextBtnOpenCloseSudoku => this.VisibilityListSudokus == Visibility.Hidden ? "Відкрити список судоку" : "Закрити список судоку";
        public string nameSudokuInput { get; set; }

        // Buttons

        private RelayCommand SignInUpCommand;
        private RelayCommand SignOutCommand;

        public ICommand SignInUpCmd => SignInUpCommand;
        public ICommand SignOutCmd => SignOutCommand;

        private RelayCommand NextHintCommand;
        private RelayCommand ExecuteCommand;
        private RelayCommand SloveUpToCommand;
        private RelayCommand CancelCommand;

        public ICommand NextHintCmd => NextHintCommand;
        public ICommand ExecuteCmd => ExecuteCommand;
        public ICommand SloveUpToCmd => SloveUpToCommand;
        public ICommand CancelCmd => CancelCommand;


        private RelayCommand ClearMatrixCommand;

        public ICommand ClearMatrixCmd => ClearMatrixCommand;

        private RelayCommand UpCommand;
        private RelayCommand DownCommand;
        private RelayCommand LeftCommand;
        private RelayCommand RightCommand;

        public ICommand UpCmd => UpCommand;
        public ICommand DownCmd => DownCommand;
        public ICommand LeftCmd => LeftCommand;
        public ICommand RightCmd => RightCommand;

        private RelayCommand NumPad0Command;
        private RelayCommand NumPad1Command;
        private RelayCommand NumPad2Command;
        private RelayCommand NumPad3Command;
        private RelayCommand NumPad4Command;
        private RelayCommand NumPad5Command;
        private RelayCommand NumPad6Command;
        private RelayCommand NumPad7Command;
        private RelayCommand NumPad8Command;
        private RelayCommand NumPad9Command;

        public ICommand NumPad0Cmd => NumPad0Command;
        public ICommand NumPad1Cmd => NumPad1Command;
        public ICommand NumPad2Cmd => NumPad2Command;
        public ICommand NumPad3Cmd => NumPad3Command;
        public ICommand NumPad4Cmd => NumPad4Command;
        public ICommand NumPad5Cmd => NumPad5Command;
        public ICommand NumPad6Cmd => NumPad6Command;
        public ICommand NumPad7Cmd => NumPad7Command;
        public ICommand NumPad8Cmd => NumPad8Command;
        public ICommand NumPad9Cmd => NumPad9Command;

        private RelayCommand NewRandomSudokuCommand;
        private RelayCommand OpenSudokuFromFileCommand;
        private RelayCommand SaveSudokuInFileCommand;
        private RelayCommand SaveAsSudokuInFileCommand;
        private RelayCommand OpenSudokuFromDataBaseCommand;
        private RelayCommand SaveSudokuInDataBaseCommand;
        private RelayCommand SaveAsSudokuInDataBaseCommand;
        private RelayCommand OpenCloseListSudokusCommand;
        private RelayCommand UpdateListSavingSudukusCommand;
        private RelayCommand DeleteItemFromSudokusCommand;
        private RelayCommand QuitCommand;

        public ICommand NewRandomSudokuCmd => NewRandomSudokuCommand;
        public ICommand OpenSudokuFromFileCmd => OpenSudokuFromFileCommand;
        public ICommand SaveSudokuInFileCmd => SaveSudokuInFileCommand;
        public ICommand SaveAsSudokuInFileCmd => SaveAsSudokuInFileCommand;
        public ICommand OpenSudokuFromDataBaseCmd => OpenSudokuFromDataBaseCommand;
        public ICommand SaveSudokuInDataBaseCmd => SaveSudokuInDataBaseCommand;
        public ICommand SaveAsSudokuInDataBaseCmd => SaveAsSudokuInDataBaseCommand;
        public ICommand OpenCloseListSudokusCmd => OpenCloseListSudokusCommand;
        public ICommand UpdateListSavingSudukusCmd => UpdateListSavingSudukusCommand;
        public ICommand DeleteItemFromSudokusCmd => DeleteItemFromSudokusCommand;
        public ICommand QuitCmd => QuitCommand;
    }
}
