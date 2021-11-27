namespace DevIO.Api.Extensions
{
    public class AppSettings
    {
        /// <summary>
        /// Chave de criptografia do token
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Expiração em horas, qauntas horas o token estará disponível até perder a validade.
        /// </summary>
        public int expiresHours { get; set; }
        /// <summary>
        /// Emissor, quem emite o token no caso a nossa aplicação
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Valido em, em quais urls o token é válido.
        /// </summary>
        public string ValidIn { get; set; }
    }
}
