using RussianNumbers;
using System.Text;
using System.Text.RegularExpressions;

internal class Program
{
    private static Regex _rangeRegex = new Regex(@"^(\-?\d+)\.\.(\-?\d+)$"); //-1..10 -> a=-1, b=10 | 3..4 -> a=3, b=4

    private static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
     
        if(args.Length > 1)
        {
            Console.WriteLine($"Unexpected number of arguments: {args.Length}");
            PrintUsage();
            return -1;
        }

        if(args.Length == 1)
        {
            PrintFromNumberOrRange(args[0], GenderNumber.Masculine, true);
            return 0;
        }

        string? line;

        bool? useStressMarkers = null;
        do
        {
            Console.WriteLine("Use stress markers? (y/N)?");
            line = Console.ReadLine();
            if (line == null)
                return -1;

            line = line.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(line)) useStressMarkers = false;
            else if (line.Equals("y")) useStressMarkers = true;
            else if (line.Equals("n")) useStressMarkers = false;
        } while (!useStressMarkers.HasValue);
        

        GenderNumber genderNumber;
        do
        {
            Console.WriteLine("Choose gender/number (MASCULINE/Feminine/Neuter/Plural)?");
            line = Console.ReadLine();

            if (line == null)
                return -2;

            if (string.IsNullOrWhiteSpace(line))
                line = GenderNumber.Masculine.ToString();
        } while (!Enum.TryParse(line, out genderNumber));


        Console.WriteLine("Type a number or range [-4..10] to get it in Russian");
        do
        {
            line = Console.ReadLine();

            if (line == null)
                return -3;

            PrintFromNumberOrRange(line, genderNumber, useStressMarkers.Value);

        } while (line != null);

        return 0;
    }

    private static void PrintFromNumberOrRange(string line, GenderNumber genderNumber, bool includeStressMarkers)
    {
        if (long.TryParse(line, out long number))
        {
            Console.WriteLine(RussianNumberUtils.GetRussianNumberString(number, genderNumber, includeStressMarkers));
        }
        else if (_rangeRegex.IsMatch(line))
        {
            var match = _rangeRegex.Match(line);

            long nStart = long.Parse(match.Groups[1].Value);
            long nEnd = long.Parse(match.Groups[2].Value);

            if (nStart <= nEnd)
            {
                for (long i = nStart; i <= nEnd; i++)
                {
                    Console.WriteLine(RussianNumberUtils.GetRussianNumberString(i, genderNumber, includeStressMarkers));
                }
            }
            else
            {
                for (long i = nStart; i >= nEnd; i--)
                {
                    Console.WriteLine(RussianNumberUtils.GetRussianNumberString(i, genderNumber, includeStressMarkers));
                }
            }

        }
        else
        {
            Console.WriteLine("Not valid number.");
        }
    }

    private static void PrintUsage()
    {
        Console.WriteLine($"{System.Diagnostics.Process.GetCurrentProcess().ProcessName} [Range] [Number]");
        Console.WriteLine("  [Range]\t\tA range expressed like -4..10, -3..-1 or 2..40");
        Console.WriteLine("  [Number]\t\tA single long integer number");
        Console.WriteLine("");
        Console.WriteLine("  Do not specify any parameter to start the program in interactive mode.");
    }
}