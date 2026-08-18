using System.Text;
using System.Text.RegularExpressions;

namespace RussianNumbers;

internal static class Program
{
    private static readonly Regex RangeRegex = new Regex(@"^(\-?\d+)\.\.(\-?\d+)$"); //-1..10 -> a=-1, b=10 | 3..4 -> a=3, b=4

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
            ProcessInputLine(args[0], GenderNumber.Masculine, true);
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
                line = nameof(GenderNumber.Masculine);
        } while (!Enum.TryParse(line, out genderNumber));


        Console.WriteLine("Type a number or range [-4..10] to get it in Russian. Or write \"challenge\" if you want to test your knowledge!");
        do
        {
            line = Console.ReadLine();

            if (line == null)
                return -3;

            ProcessInputLine(line, genderNumber, useStressMarkers.Value);

        } while (true);
    }

    private static void ProcessInputLine(string line, GenderNumber genderNumber, bool includeStressMarkers)
    {
        if (line.Equals("challenge"))
        {
            Challenge(genderNumber, includeStressMarkers);
        }
        else if (long.TryParse(line, out long number))
        {
            Console.WriteLine(RussianNumberUtils.GetRussianNumberString(number, genderNumber, includeStressMarkers));
        }
        else if (RangeRegex.IsMatch(line))
        {
            var match = RangeRegex.Match(line);

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

    private static void Challenge(GenderNumber genderNumber, bool includeStressMark)
    {
        Console.WriteLine("Challenge accepted. Let's test your knowledge.");

        string? line;

        long nMin;
        do
        {
            Console.WriteLine("Enter the minimum number:");
            line = Console.ReadLine();
            if (long.TryParse(line, out nMin))
            {
                break;
            }
        } while (true);

        long nMax;
        do
        {
            Console.WriteLine("Enter the maximum number:");
            line = Console.ReadLine();
            if (long.TryParse(line, out nMax))
            {
                break;
            }
        } while (true);

        Random r = new Random(HashCode.Combine(DateTime.UtcNow.Ticks));

        Console.WriteLine("Try to read the following numbers. Press enter to continue or type \"exit\" to exit.");
        do
        {
            long n = r.NextInt64(nMin, nMax + 1);
            Console.WriteLine(n);
            
            line = Console.ReadLine();

            Console.WriteLine(RussianNumberUtils.GetRussianNumberString(n, genderNumber, includeStressMark));

        } while (line != "exit");

        Console.WriteLine("Exited challenge.");
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