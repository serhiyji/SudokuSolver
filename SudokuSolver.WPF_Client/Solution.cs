using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SudokuSolver.SudokuSolverCore.Solution;

namespace SudokuSolver.WPF_Client
{
    [AddINotifyPropertyChangedInterface]
    public class Solution : SudokuSolver.Extensions.Singleton<Solution>
    {
        public SolutionMethod Intersection { get; set; }
        public bool IsExecute { get; set; }
        public Solution()
        {
            Intersection = new SolutionMethod();
            IsExecute = false;
        }
    }
}
