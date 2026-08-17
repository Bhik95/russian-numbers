namespace RussianNumbers
{
    public static class RussianGrammarHelpers
    {

        public static CaseAfterNumeral GetCaseAfterNumeral(long n)
        {
            int lastTwoDigits = (int)(n % 100);

            if (lastTwoDigits >= 11 && lastTwoDigits <= 19)
                return CaseAfterNumeral.GenitivePlural;

            int lastDigit = (int)(n % 10);

            if (lastDigit == 1)
                return CaseAfterNumeral.NominativeSingular;
            else if (lastDigit >= 2 && lastDigit <= 4)
                return CaseAfterNumeral.GenitiveSingular;

            return CaseAfterNumeral.GenitivePlural;
        }
    }
}