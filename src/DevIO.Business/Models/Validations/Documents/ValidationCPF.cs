using System;
using System.Linq;

namespace DevIO.Business.Models.Validations.Documents
{
    public class ValidationCPF
    {
        public const int TamanhoCPF = 11;

        public static bool Validar(string number)
        {
            var numbers = Utils.RetornarApenasNumero(number);

            if (!ValidarTamanho(numbers))
            {
                return false;
            }

            return !PossuiDigitosRepetidos(numbers) && TemDigitosValidos(numbers);
        }

        private static bool ValidarTamanho(string number)
        {
            return number.Length == TamanhoCPF;
        }

        private static bool PossuiDigitosRepetidos(string number)
        {
            string[] invalidNumbers =
            {
                "00000000000000",
                "11111111111111",
                "22222222222222",
                "33333333333333",
                "44444444444444",
                "55555555555555",
                "66666666666666",
                "77777777777777",
                "88888888888888",
                "99999999999999"
            };

            return invalidNumbers.Contains(number);
        }

        private static bool TemDigitosValidos(string documento)
        {
            var number = documento.Substring(0, TamanhoCPF - 2);

            var digitoVerificador = new DigitoVerificador(number)
                .ComMultiplicadoresDeAte(2, 11)
                .Substituindo("0", 10, 11);

            var firstDigit = digitoVerificador.CalculaDigito();
            digitoVerificador.AddDigito(firstDigit);
            var secondDigit = digitoVerificador.CalculaDigito();

            return string.Concat(firstDigit, secondDigit) == documento.Substring(TamanhoCPF - 2, 2);
        }
    }
}
