namespace DevIO.Business.Models.Validations.Documents
{
    public class Utils
    {
        public static string RetornarApenasNumero(string value)
        {
            var onlyNumber = "";

            foreach (var s in value)
            {
                if (char.IsDigit(s)) onlyNumber += s;
            }

            return onlyNumber.Trim();
        }
    }
}
