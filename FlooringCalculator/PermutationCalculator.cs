using Serilog;
using System.Collections.ObjectModel;
using System.Numerics;

namespace FlooringCalculator
{
    /// <summary>
    /// The function is to generate all possible permutations for a 
    /// different inputs.
    /// The permutations generated can be reduced by passing a validation
    /// function in the public method call. If the function is null, all
    /// permutations will be returned.
    /// The main operation is via recursive functions. 
    /// If a generated value does not pass validation it is not added to the output and
    /// there will be no reference to it. It therefore should go out of scope and be
    /// available for garrbage collection. 
    /// The count function maintains no references at all.
    /// BigInteger was used as the return because the value for 32x10 is
    /// 3329^10 = 1.671621512572349E+35
    /// </summary>

    public class PermutationCalculator
    {
        private ILogger _logger;
        private BigInteger _validCount;

        public PermutationCalculator(ILogger logger)
        {
            _logger = logger;
        }

        private Func<IList<IList<int>>, int, bool>? _validateTable;

        public BigInteger PermutationsWithRepetitionsCount(IList<IList<int>> input,
                                                           int take,
                                                           int length,
                                                           Func<IList<IList<int>>, int, bool>? validate)
        {
            _validateTable = validate;
            IList<IList<int>> itemRow = new List<IList<int>>();

            for (int i = 0; i < take; i++)
            {
                itemRow.Add(null);
            }
            //Number of items in the the set n to the power of selected elements of the set.
            _logger.Information("Total Table Permutations : " + input.Count + "^" + itemRow.Count + " = " + Math.Pow(input.Count, itemRow.Count).ToString());
            PermutationsWithRepetitionsCount(input, itemRow, 0, length);

            return _validCount;
        }

        public ICollection<ICollection<IList<int>>> PermutationsWithRepetitions(IList<IList<int>> input,
                                                                                int take,
                                                                                int length,
                                                                                Func<IList<IList<int>>, int, bool>? validate)
        {
            _validateTable = validate;
            ICollection<ICollection<IList<int>>> output = new Collection<ICollection<IList<int>>>();
            IList<IList<int>> itemRow = new List<IList<int>>();

            for(int i = 0; i < take; i++)
            {
                itemRow.Add(null);
            }
            //Number of items in the the set n to the power of selected elements of the set.
            _logger.Information("Total Table Permutations : " + input.Count  + "^" + itemRow.Count + " = " + Math.Pow(input.Count, itemRow.Count).ToString());
            PermutationsWithRepetitions(output, input, itemRow, 0, length);

            return output;
        }

        private void PermutationsWithRepetitions(ICollection<ICollection<IList<int>>> output, 
                                                                         IEnumerable<IEnumerable<int>> input,
                                                                         IList<IList<int>> itemRow,
                                                                         int rowCount, 
                                                                         int length)
        {
            if (rowCount < itemRow.Count)
            {
                var enumerable = input.ToList();
                foreach (var row in enumerable)
                {
                    itemRow[rowCount] = (IList<int>)row;
                    PermutationsWithRepetitions(output, enumerable, itemRow, rowCount + 1, length);
                }
            }
            else
            {
                _logger.Information(Utility.CreateLogMessage(itemRow));
                if (_validateTable != null && _validateTable(itemRow, length))
                {
                    var deepCopy = DeepCopy(itemRow);
                    output.Add(deepCopy);
                }
                else if (_validateTable == null)
                {
                    var deepCopy = DeepCopy(itemRow);
                    output.Add(deepCopy);
                }
            }
        }

        private void PermutationsWithRepetitionsCount(IEnumerable<IEnumerable<int>> input,
                                                      IList<IList<int>> itemRow,
                                                      int rowCount,
                                                      int length)
        {
            if (rowCount < itemRow.Count)
            {
                var enumerable = input.ToList();
                foreach (var row in enumerable)
                {
                    itemRow[rowCount] = (IList<int>)row;
                    PermutationsWithRepetitionsCount(enumerable, itemRow, rowCount + 1, length);
                }
            }
            else
            {
                _logger.Information(Utility.CreateLogMessage(itemRow));
                if (_validateTable != null && _validateTable(itemRow, length))
                {
                    var deepCopy = DeepCopy(itemRow);
                    _validCount++;
                }
                else if (_validateTable == null)
                {
                    var deepCopy = DeepCopy(itemRow);
                    _validCount++;
                }
            }
        }
        public ICollection<IList<int>> DeepCopy(IList<IList<int>> inputList)
        {
            ICollection<IList<int>> outputList = new Collection<IList<int>>();

            foreach(var otterList in inputList)
            {
                var containerList = new List<int>(otterList.Count);
                for(int i = 0; i < otterList.Count; i++)
                {
                    containerList.Add(otterList[i]);
                }
                outputList.Add(containerList);
            }

            return outputList;
        }

        private Func<IList<int>, int, bool>? _validateRow;

        public ICollection<IList<int>> PermutationsWithRepetitions(IList<int> input,
                                                                   int take,
                                                                   int length,
                                                                   Func<IList<int>, int,bool>? validate)
        {
            _validateRow = validate;
            ICollection<IList<int>> output = new Collection<IList<int>>();
            IList<int> item = new int[take];

            //Number of items in the the set n to the power of selected elements of the set.
            _logger.Information("Total Row Permutations  : " + input.Count + "^" + item.Count  + " = " + Math.Pow(input.Count, item.Count).ToString());
            PermutationsWithRepetitions(output, input, item, 0, length);

            return output;
        }

        private void PermutationsWithRepetitions(ICollection<IList<int>> output, 
                                                                        IEnumerable<int> input,
                                                                        IList<int> item,
                                                                        int count,
                                                                        int length)
        {
            if (count < item.Count)
            {
                var enumerable = input.ToList();
                foreach (var symbol in enumerable)
                {
                    item[count] = symbol;
                    PermutationsWithRepetitions(output, enumerable, item, count + 1, length);
                }
            }
            else
            {
                _logger.Information(Utility.CreateLogMessage(item));
                if (_validateRow != null && _validateRow(item, length))
                {
                    var row = new List<int>(item);
                    output.Add(row);
                }
                else if (_validateRow == null)
                {
                    //No validation function just add all permutations.
                    var row = new List<int>(item);
                    output.Add(row);
                }
                
            }
        }

    }
}
