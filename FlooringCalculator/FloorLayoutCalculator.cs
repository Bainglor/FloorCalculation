using Serilog;
using System.Numerics;

namespace FlooringCalculator
{
    /// <summary>
    /// The FloorLayoutCalculator is specific to the calculation of the floor layout.
    /// The permutation generation is left to the PermutationCalculator.
    /// The validation functions are here because they are specific to the floor layout calculation.
    /// The are passed via the FUNC delegate to the PermutationCalculator.
    /// </summary>

    public class FloorLayoutCalculator
    {
        List<int> _boards;
        ILogger _logger;
        private int _min;
        private int _max;
        private PermutationCalculator _permutationCalculator;

        public FloorLayoutCalculator(List<int> boards, ILogger logger, PermutationCalculator permutationCalculator)
        {
            _boards = boards;
            _logger = logger;
            _permutationCalculator = permutationCalculator;
        }

        //For testing to return full list
        //public ICollection<ICollection<IList<int>>> GetFloorLayout(int length, int width)
        //{
        //    MinMax(length);
        //    var floorDimension = length.ToString() + "x" + width.ToString();

        //    _logger.Information("******************************************************Start Floor Layout Calculation for :" + floorDimension) ;
        //    var boardPermutations = GetBoardPermutations(_max, _min, _boards, length);
        //    var floorPermutations = GetFloorPermutations(boardPermutations, width, length);
        //    _logger.Information("********************************************************End Floor Layout Calculation for :" + floorDimension);
        //    return floorPermutations;
        //}

        public BigInteger GetFloorLayoutCount(int length, int width)
        {
            MinMax(length);
            var floorDimension = length.ToString() + "x" + width.ToString();

            _logger.Information("******************************************************Start Floor Layout Calculation for :" + floorDimension);
            var boardPermutations = GetBoardPermutations(_max, _min, _boards, length);
            var floorPermutationsCount = GetFloorPermutationsCount(boardPermutations, width, length);
            _logger.Information("********************************************************End Floor Layout Calculation for :" + floorDimension);
            return floorPermutationsCount;
        }

        private void MinMax(int length)
        {
            var maxBoard = _boards.Max();
            var minBoard = _boards.Min();


            _max = length / minBoard;
            _min = length / maxBoard;
        }

        private BigInteger GetFloorPermutationsCount(IList<IList<int>> boardPermutations, int width, int length)
        {
            _logger.Information("******Floor Permutations");
            var validFloorLayoutsCount = _permutationCalculator.PermutationsWithRepetitionsCount(boardPermutations, width, length, ValidateFloor);

            return validFloorLayoutsCount;
        }

        //For testing to return full list
        //private ICollection<ICollection<IList<int>>> GetFloorPermutations(IList<IList<int>> boardPermutations, int width, int length)
        //{
        //    _logger.Information("******Floor Permutations");
        //    var validFloorLayouts = _permutationCalculator.PermutationsWithRepetitions(boardPermutations, width, length, ValidateFloor) ;
        //    _logger.Information("*****Valid FloorLayouts, Total: " + validFloorLayouts.Count.ToString());
        //    foreach (var floorLayout in validFloorLayouts)
        //    {
        //        _logger.Information(Utility.CreateLogMessage(floorLayout));
        //    }

        //    return validFloorLayouts;
        //}

        private IList<IList<int>> GetBoardPermutations(int max, int min, List<int> boardLengths, int length)
        {
            IList<IList<int>> validRows = new List<IList<int>>();
            _logger.Information("******Board Permutations");
            for (int y = min; y <= max; y++)
            {
                var returnList = _permutationCalculator.PermutationsWithRepetitions(boardLengths, y, length, ValidateRowLength);
                foreach (var item in returnList)
                {
                    validRows.Add(item);
                }
            }
            _logger.Information("*****Valid Rows: " + validRows.Count);
            //var validRows = GetPermutationsWithTotal(allPermutations, length);
            _logger.Information(Utility.CreateLogMessage(validRows));
            return validRows;
        }

        //Validation Function for boards. In this class because it is specific to it.
        public bool ValidateRowLength(IList<int> permutations, int total)
        {
            bool validRow = false;

            if (permutations.Sum() == total)
            {
                validRow = true;
            }

            return validRow;
        }

        //Validation Function for boards. In this class because it is specific to it.
        private bool ValidateFloor(IList<IList<int>> floorLayout, int length)
        {
            bool floorVerified = true;
            for (int i = 0; i < (floorLayout.Count - 1); i++)
            {
                var currentRow = floorLayout[i];
                var nextRow = floorLayout[i + 1];
                if (JointsLineUp(currentRow, nextRow, length))
                {
                    floorVerified = false;
                    break;
                }
            }
            return floorVerified;
        }

        private bool JointsLineUp(IList<int> currentRow, IList<int> nextRow, int length)
        {
            bool jointsLineUp = false;
            if (currentRow.Count == 0)
                return jointsLineUp;

            var currentRunningTotal = GetJointSums(currentRow, length);
            var nextRunningTotal = GetJointSums(nextRow, length);
            var jointLineCount = currentRunningTotal.Intersect(nextRunningTotal).Count();

            if (jointLineCount != 0)
                jointsLineUp = true;

            return jointsLineUp;
        }

        private IList<int> GetJointSums(IList<int> boards, int length)
        {
            var boardSum = new List<int>();
            int boardRunningTotal = 0;
            foreach (var board in boards)
            {
                boardRunningTotal = boardRunningTotal + board;
                //Do not want final total as this will always equal the length.
                //Which is exempt for joint lineup.
                if (boardRunningTotal != length)
                {
                    boardSum.Add(boardRunningTotal);
                }
            }

            return boardSum;
        }
    }
}
