using System.Text;

namespace RussianNumbers;

public static class RussianNumberWritingUtils
{
    private static readonly string[] Scales1DigitMasculine = ["ноль", "оди́н", "два", "три", "четы́ре", "пять", "шесть", "семь", "во́семь", "де́вять"];
    private static readonly string[] Scales1DigitFeminine = ["ноль", "одна́", "две", "три", "четы́ре", "пять", "шесть", "семь", "во́семь", "де́вять"];
    private static readonly string[] Scales1DigitNeuter = ["ноль", "одно́", "два", "три", "четы́ре", "пять", "шесть", "семь", "во́семь", "де́вять"];
    private static readonly string[] Scales1DigitPlural = ["ноль", "одни", "два", "три", "четы́ре", "пять", "шесть", "семь", "во́семь", "де́вять"];
    private static readonly string[] ScalesTeens = ["оди́ннадцать", "двена́дцать", "трина́дцать", "четы́рнадцать", "пятна́дцать", "шестна́дцать", "семна́дцать", "восемна́дцать", "девятна́дцать"];
    private static readonly string[] Scales2Digits = ["де́сять", "два́дцать", "три́дцать", "со́рок", "пятьдеся́т", "шестьдеся́т", "се́мьдесят", "во́семьдесят", "девяно́сто"];
    private static readonly string[] Scales3Digits = ["сто", "две́сти", "три́ста", "четы́реста", "пятьсо́т", "шестьсо́т", "семьсо́т", "восемьсо́т", "девятьсо́т"];

    private static readonly string[] ThousandWordCases = ["ты́сяча", "ты́сячи", "ты́сяч"];
    private static readonly string[] MillionWordCases = ["миллио́н", "миллио́на", "миллио́нов"];
    private static readonly string[] BillionWordCases = ["миллиа́рд", "миллиа́рда", "миллиа́рдов"];
    
    private const string RussianStressMark = "́";
    
    public static void WriteRussianNumber(long n, GenderNumber genderNumber, bool includeStressMarks, StringBuilder sb)
    {
        WriteSign(n, genderNumber, sb);
        
        if(!includeStressMarks)
            sb.Replace(RussianStressMark, "");
    }

    private static void WriteSign(long n, GenderNumber genderNumber, StringBuilder sb)
    {
        if (n < 0)
        {
            n = -n;
            sb.Append("ми́нус ");
        }

        WriteDigits10To12(n, genderNumber, sb);
    }

    private static void WriteScaleWithRemainder(int baseValue, string baseText, Action<int, GenderNumber, StringBuilder> remainderWriter, int n, GenderNumber genderNumber, StringBuilder sb)
    {
        if (n == baseValue)
        {
            sb.Append(baseText);
            return;
        }

        sb.Append(baseText);
        sb.Append(' ');

        remainderWriter(n - baseValue, genderNumber, sb);
    }

    // 1- 999 Billions
    private static void WriteDigits10To12(long n, GenderNumber genderNumber, StringBuilder sb)
    {
        if (n < 0 || n >= 1_000_000_000_000)
            throw new ArgumentOutOfRangeException(nameof(n));

        if (n < 1_000_000_000)
        {
            WriteDigits7To9((int)n, genderNumber, sb);
            return;
        }

        int nLeft = (int)(n / 1_000_000_000);
        int nRight = (int)(n % 1_000_000_000);

        WriteDigit3(nLeft, GenderNumber.Masculine, sb); // миллиа́рд is masculine and must be conjugated accordingly

        CaseAfterNumeral caseAfterNumeral = RussianGrammarHelpers.GetCaseAfterNumeral(nLeft);
        string billionWordConjugated = BillionWordCases[(int)caseAfterNumeral];

        sb.Append(' ');
        sb.Append(billionWordConjugated);
        sb.Append(' ');

        if (nRight > 0)
        {
            WriteDigits7To9(nRight, genderNumber, sb);
            sb.Append(' ');
        }
    }

    // 1-999 Millions
    private static void WriteDigits7To9(int n, GenderNumber genderNumber, StringBuilder sb)
    {
        if (n < 0 || n >= 1_000_000_000)
            throw new ArgumentOutOfRangeException(nameof(n));

        if (n < 1_000_000)
        {
            WriteDigits4To6(n, genderNumber, sb);
            return;
        }

        int nLeft = n / 1_000_000;
        int nRight = n % 1_000_000;

        WriteDigit3(nLeft, GenderNumber.Masculine, sb); // миллион is masculine and must be conjugated accordingly

        CaseAfterNumeral caseAfterNumeral = RussianGrammarHelpers.GetCaseAfterNumeral(nLeft);
        string millionWordConjugated = MillionWordCases[(int)caseAfterNumeral];

        sb.Append(' ');
        sb.Append(millionWordConjugated);
        sb.Append(' ');

        if (nRight > 0)
        {
            WriteDigits4To6(nRight, genderNumber, sb);
            sb.Append(' ');
        }
    }

    // 1-999 thousands
    private static void WriteDigits4To6(int n, GenderNumber genderNumber, StringBuilder sb)
    {
        if (n < 0 || n >= 1_000_000)
            throw new ArgumentOutOfRangeException(nameof(n));

        if (n < 1000)
        {
            WriteDigit3(n, genderNumber, sb);
            return;
        }

        int nLeft = n / 1000;
        int nRight = n % 1000;

        WriteDigit3(nLeft, GenderNumber.Feminine, sb); // ты́сяча is feminine and must be conjugated accordingly
        
        CaseAfterNumeral caseAfterNumeral = RussianGrammarHelpers.GetCaseAfterNumeral(nLeft);
        string thousandWordConjugated = ThousandWordCases[(int)caseAfterNumeral];

        sb.Append(' ');
        sb.Append(thousandWordConjugated);
        sb.Append(' ');

        if (nRight > 0)
        {
            WriteDigit3(nRight, genderNumber, sb);
            sb.Append(' ');
        }
    }
    
    private static void WriteDigit3(int n, GenderNumber genderNumber, StringBuilder sb)
    {
        if (n < 0 || n >= 1000)
            throw new ArgumentOutOfRangeException(nameof(n));

        if (n < 100)
        {
            WriteDigit2(n, genderNumber, sb);
            return;
        }

        int digit3 = n / 100;
        
        WriteScaleWithRemainder(digit3 * 100, Scales3Digits[digit3 - 1], WriteDigit2, n, genderNumber, sb);
    }

    
    private static void WriteDigit2(int n, GenderNumber genderNumber, StringBuilder sb)
    {
        if (n < 0 || n >= 100)
            throw new ArgumentOutOfRangeException(nameof(n));

        if (n < 10)
        {
            WriteDigit1(n, genderNumber, sb);
            return;
        }

        if (n >= 11 && n <= 19)
        {
            sb.Append(ScalesTeens[n - 11]);
            return;
        }

        int digit2 = n / 10;
        
        WriteScaleWithRemainder(digit2 * 10, Scales2Digits[digit2 - 1], WriteDigit1, n, genderNumber, sb);
    }

    private static void WriteDigit1(int n, GenderNumber genderNumber, StringBuilder sb)
    {
        if (n < 0 || n >= 10)
            throw new ArgumentOutOfRangeException(nameof(n));

        sb.Append(genderNumber switch
        {
            GenderNumber.Masculine => Scales1DigitMasculine[n],
            GenderNumber.Feminine => Scales1DigitFeminine[n],
            GenderNumber.Neuter => Scales1DigitNeuter[n],
            GenderNumber.Plural => Scales1DigitPlural[n],
            _ => Scales1DigitMasculine[n],
        });
    }
}