using Microsoft.Win32;
using SudokuSolver.SudokuSolverCore.BetterMatrix;
using SudokuSolver.SudokuSolverCore.Points;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SudokuSolver.WPF_Client
{
    public class SudokuSavingHandler : SudokuSolver.Extensions.Singleton<SudokuSavingHandler>
    {
        public bool IsSudokuFromFile { get; private set; }
        public string FullPath { get; private set; }
        public bool IsSudokuFromDatabase { get; private set; }
        public int IdSudokuInDatabase { get; private set; }

        public SudokuSavingHandler()
        {
            this.IsSudokuFromFile = false;
            this.FullPath = "";
            this.IsSudokuFromDatabase = false;
            this.IdSudokuInDatabase = -1;
        }
        public void SaveAsSudokuInFile<TPointMatrix>(ref BetterMatrix<TPointMatrix> matrix) where TPointMatrix : IPointMatrix, new()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                this.FullPath = saveFileDialog.FileName;
                this.SaveToFile(this.FullPath, matrix.SaveSudoku());
                this.SwapPropAccess(File: true, Database: false);
            }
        }
        public void SaveSudokuInFile<TPointMatrix>(ref BetterMatrix<TPointMatrix> matrix) where TPointMatrix : IPointMatrix, new()
        {
            this.SaveToFile(this.FullPath, matrix.SaveSudoku());
        }
        public void LoadSudokuFromFile<TPointMatrix>(ref BetterMatrix<TPointMatrix> matrix) where TPointMatrix : IPointMatrix, new()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                this.FullPath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(this.FullPath);
                if (matrix.LoadSudoku(fileContent))
                {
                    this.SwapPropAccess(File: true, Database: false);
                }
                else
                {
                    MessageBox.Show("Судоку не була завантажена з файлу / Файл не відповідє стандатру або був пошкоджений");
                }
            }
        }
        public void SaveAsSudokuInDataBase<TPointMatrix>(string nameSudoku, ref BetterMatrix<TPointMatrix> matrix) where TPointMatrix : IPointMatrix, new()
        {
            try
            {
                int newid = DatabaseHandler.Instance.SaveSudoku(nameSudoku, ref matrix);
                if (newid >= 0)
                {
                    this.IdSudokuInDatabase = newid;
                    this.SwapPropAccess(File: false, Database: true);
                }
            }
            catch (Exceptions.ExceptionConnectDatabase ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SaveSudokuInDataBase<TPointMatrix>(ref BetterMatrix<TPointMatrix> matrix) where TPointMatrix : IPointMatrix, new()
        {
            try
            {
                DatabaseHandler.Instance.SaveSudoku(this.IdSudokuInDatabase, ref matrix);
            }
            catch (Exceptions.ExceptionConnectDatabase ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void LoadSudokuFromDataBase<TPointMatrix>(int idsudoku, ref BetterMatrix<TPointMatrix> matrix) where TPointMatrix : IPointMatrix, new()
        {
            int newid = DatabaseHandler.Instance.LoadSudoku(idsudoku, ref matrix);
            if (newid >= 0)
            {
                this.IdSudokuInDatabase = newid;
                this.SwapPropAccess(File: false, Database: true);
            }
        }
        private void SaveToFile(string filePath, string data)
        {
            try
            {
                File.WriteAllText(filePath, data);
            }
            catch (Exception) { }
        }
        private void SwapPropAccess(bool File = false, bool Database = false)
        {
            this.IsSudokuFromFile = File;
            this.IsSudokuFromDatabase = Database;
        }
        public void ToFalse() => SwapPropAccess(false, false);
    }
}
