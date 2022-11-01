namespace SharedKernell.Mail
{
    /// <summary>
    /// Documento de Email
    /// </summary>
    public class MailFiles
    {
        /// <summary>
        /// Ruta de documento (opcional)
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// Stream de documento (opcional)
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// Nombre de documento (opcional)
        /// </summary>
        public string Name { get; set; }
    }
}