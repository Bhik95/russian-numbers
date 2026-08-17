namespace RussianNumbers
{
    public enum CaseAfterNumeral
    {
        NominativeSingular = 0, // 1, 21, 31, 41, ... (no 11) -> Nominative Singular
        GenitiveSingular, // 2, 3, 4, 22, 23, 24, 32, 33, 34, ... (no 12, 13, 14) -> Genitive Singular
        GenitivePlural // all other situations
    }
}
