using System.Text;

namespace RussianNumbers
{
    public class RussianNumberWriter
    {
        private GenderNumber _genderNumber = GenderNumber.Masculine;
        private bool _includeStressMarks;

        public GenderNumber GenderNumber
        {
            get => _genderNumber;
            set => _genderNumber = value;
        }

        public bool IncludeStressMarks
        {
            get => _includeStressMarks;
            set => _includeStressMarks = value;
        }

        public string Write(long n)
        {
            StringBuilder sb = new StringBuilder();
            
            RussianNumberWritingUtils.WriteRussianNumber(n, _genderNumber, _includeStressMarks, sb);

            return sb.ToString();
        }
    }
}
