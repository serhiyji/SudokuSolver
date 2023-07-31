using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SudokuSloverHendler;
using Sudoku_Slover.Expansion;

namespace Sudoku_Slover
{
    [AddINotifyPropertyChangedInterface]
    public class Solution : SingletonClass<Solution>
    {
        public Intersections Intersection { get; set; }
        public bool IsExecute { get; set; }
        public Solution()
        {
            Intersection = new Intersections();
            IsExecute = false;
        }
    }
}
