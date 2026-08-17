using RussianNumbers;
using System.Text;

internal class Program
{
    private static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        
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


        Console.WriteLine("Type a number to get it in Russian");
        do
        {
            line = Console.ReadLine();

            if(long.TryParse(line, out long number)){
                string numberStr = RussianNumberUtils.GetRussianNumberString(number, genderNumber);

                if (!useStressMarkers.Value)
                    numberStr = numberStr.Replace("́", "");

                Console.WriteLine(numberStr);
            }
            else
            {
                Console.WriteLine("Not valid number.");
            }

        } while (line != null);

        return 0;
    }

    
}