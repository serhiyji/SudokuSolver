using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SudokuSolverCore.SudokuSolvingAlgorithms
{
    public enum TypeAlgorithmSolution
    {
        None,
        FullHouseAlgorithm,
        NakedSingleAlgorithm,
        HiddenSingleAlgorithm,
        LockedPairAlgorithm,
        LockedTripleAlgorithm,
        LockedCandidatesTypePointingAlgorithm,
        LockedCandidatesTypeClaimingAlgorithm,
        NakedPairAlgorithm,
        NakedTripleAlgorithm,
        NakedQuadrupleAlgorithm,
        HiddenPairAlgorithm,
        HiddenTripleAlgorithm,
        HiddenQuadrupleAlgorithm,
        StandardBruteForceAlgorithm
    }
}
