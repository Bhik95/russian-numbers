namespace RussianNumbers
{
    public static class RussianNumberUtils
    {

        // 1: оди́н/одна́/одно́/одни
        // 2: два/две/два/два
        private static readonly string[] masculineDigits = ["ноль", "оди́н", "два", "три", "четы́ре", "пять", "шесть", "семь", "во́семь", "де́вять"];
        private static readonly string[] feminineDigits = ["ноль", "одна́", "две", "три", "четы́ре", "пять", "шесть", "семь", "во́семь", "де́вять"];
        private static readonly string[] neuterDigits = ["ноль", "одно́", "два", "три", "четы́ре", "пять", "шесть", "семь", "во́семь", "де́вять"];
        private static readonly string[] neuterPlural = ["ноль", "одни", "два", "три", "четы́ре", "пять", "шесть", "семь", "во́семь", "де́вять"];

        private static readonly string[] thousandWordCases = ["ты́сяча", "ты́сячи", "ты́сяч"];
        private static readonly string[] millionWordCases = ["миллио́н", "миллио́на", "миллио́нов"];
        private static readonly string[] billionWordCases = ["миллиа́рд", "миллиа́рда", "миллиа́рдов"];

        public static string GetRussianNumberString(long n, GenderNumber genderNumber)
        {
            if (n < 0) return "минус " + GetRussianNumberString(-n, genderNumber);

            return GetD10toD12(n, genderNumber);
        }

        private static string GetDXWithDXminus1Helper(int refN, string refStr, Func<int, GenderNumber, string> next, int n, GenderNumber genderNumber)
        {
            if (n == refN) return refStr;
            else return refStr + " " + next(n - refN, genderNumber);
        }

        // 1- 999 Billions
        private static string GetD10toD12(long n, GenderNumber genderNumber)
        {
            if (n < 0 || n >= 1_000_000_000_000)
                throw new ArgumentOutOfRangeException(nameof(n));

            if (n < 1_000_000_000) return GetD7toD9((int)n, genderNumber);

            int n_left = (int)(n / 1_000_000_000);
            int n_right = (int)(n % 1_000_000_000);

            string leftStr = GetD3(n_left, GenderNumber.Masculine); // миллиа́рд  is masculine and must be conjugated accordingly
            string rightStr = n_right == 0 ? "" : GetD7toD9(n_right, genderNumber);

            CaseAfterNumeral caseAfterNumeral = RussianGrammarHelpers.GetCaseAfterNumeral(n_left);

            string billionWordConjugated = billionWordCases[(int)caseAfterNumeral];

            return $"{leftStr} {billionWordConjugated} {rightStr}".Trim();
        }

        // 1-999 Millions
        private static string GetD7toD9(int n, GenderNumber genderNumber)
        {
            if (n < 0 || n >= 1_000_000_000)
                throw new ArgumentOutOfRangeException(nameof(n));

            if (n < 1_000_000) return GetD4toD6(n, genderNumber);

            int n_left = n / 1_000_000;
            int n_right = n % 1_000_000;

            string leftStr = GetD3(n_left, GenderNumber.Masculine); // миллион is masculine and must be conjugated accordingly
            string rightStr = n_right == 0 ? "" : GetD4toD6(n_right, genderNumber);

            CaseAfterNumeral caseAfterNumeral = RussianGrammarHelpers.GetCaseAfterNumeral(n_left);

            string millionWordConjugated = millionWordCases[(int)caseAfterNumeral];

            return $"{leftStr} {millionWordConjugated} {rightStr}".Trim();
        }

        // 1-999 thousands
        private static string GetD4toD6(int n, GenderNumber genderNumber)
        {
            if (n < 0 || n >= 1_000_000)
                throw new ArgumentOutOfRangeException(nameof(n));

            if (n < 1000) return GetD3(n, genderNumber);

            int n_left = n / 1000;
            int n_right = n % 1000;

            string leftStr = GetD3(n_left, GenderNumber.Feminine); // ты́сяча is feminine and must be conjugated accordingly
            string rightStr = n_right == 0 ? "" : GetD3(n_right, genderNumber);

            CaseAfterNumeral caseAfterNumeral = RussianGrammarHelpers.GetCaseAfterNumeral(n_left);

            string thousandWordConjugated = thousandWordCases[(int)caseAfterNumeral];

            return $"{leftStr} {thousandWordConjugated} {rightStr}".Trim();
        }

        private static string GetD3(int n, GenderNumber genderNumber)
        {
            if (n < 0 || n >= 1000)
                throw new ArgumentOutOfRangeException(nameof(n));

            if (n < 100) return GetD2(n, genderNumber);

            if (n >= 900) return GetDXWithDXminus1Helper(900, "девятьсо́т", GetD2, n, genderNumber);
            else if (n >= 800) return GetDXWithDXminus1Helper(800, "восемьсо́т", GetD2, n, genderNumber);
            else if (n >= 700) return GetDXWithDXminus1Helper(700, "семьсо́т", GetD2, n, genderNumber);
            else if (n >= 600) return GetDXWithDXminus1Helper(600, "шестьсо́т", GetD2, n, genderNumber);
            else if (n >= 500) return GetDXWithDXminus1Helper(500, "пятьсо́т", GetD2, n, genderNumber);
            else if (n >= 400) return GetDXWithDXminus1Helper(400, "четы́реста", GetD2, n, genderNumber);
            else if (n >= 300) return GetDXWithDXminus1Helper(300, "три́ста", GetD2, n, genderNumber);
            else if (n >= 200) return GetDXWithDXminus1Helper(200, "две́сти", GetD2, n, genderNumber);

            // if >= 100
            return GetDXWithDXminus1Helper(100, "сто", GetD2, n, genderNumber);
        }

        private static string GetD2(int n, GenderNumber genderNumber)
        {
            if (n < 0 || n >= 100)
                throw new ArgumentOutOfRangeException(nameof(n));

            if (n < 10) return GetD1(n, genderNumber);

            if (n >= 90) return GetDXWithDXminus1Helper(90, "девяно́сто", GetD1, n, genderNumber);
            else if (n >= 80) return GetDXWithDXminus1Helper(80, "во́семьдесят", GetD1, n, genderNumber);
            else if (n >= 70) return GetDXWithDXminus1Helper(70, "се́мьдесят", GetD1, n, genderNumber);
            else if (n >= 60) return GetDXWithDXminus1Helper(60, "шестьдеся́т", GetD1, n, genderNumber);
            else if (n >= 50) return GetDXWithDXminus1Helper(50, "пятьдеся́т", GetD1, n, genderNumber);
            else if (n >= 40) return GetDXWithDXminus1Helper(40, "со́рок", GetD1, n, genderNumber);
            else if (n >= 30) return GetDXWithDXminus1Helper(30, "три́дцать", GetD1, n, genderNumber);
            else if (n >= 20) return GetDXWithDXminus1Helper(20, "два́дцать", GetD1, n, genderNumber);
            else if (n == 19) return "девятна́дцать";
            else if (n == 18) return "восемна́дцать";
            else if (n == 17) return "семна́дцать";
            else if (n == 16) return "шестна́дцать";
            else if (n == 15) return "пятна́дцать";
            else if (n == 14) return "четы́рнадцать";
            else if (n == 13) return "трина́дцать";
            else if (n == 12) return "двена́дцать";
            else if (n == 11) return "оди́ннадцать";

            // if 10
            return "де́сять";
        }

        private static string GetD1(int n, GenderNumber genderNumber)
        {
            if (n < 0 || n >= 10)
                throw new ArgumentOutOfRangeException(nameof(n));

            return genderNumber switch
            {
                GenderNumber.Masculine => masculineDigits[n],
                GenderNumber.Feminine => feminineDigits[n],
                GenderNumber.Neuter => neuterDigits[n],
                GenderNumber.Plural => neuterPlural[n],
                _ => masculineDigits[n],
            };
        }
    }
}
