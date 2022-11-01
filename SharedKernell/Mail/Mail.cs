namespace SharedKernell.Mail
{
    public class Mail
    {
        /// <summary>
        /// Lista direcciones de correo destinatarios
        /// </summary>
        public List<string> Mails { get; set; }
        /// <summary>
        /// Lista de direcciones de correo ocultos que recibirán una copia
        /// </summary>
        public List<string> MailsCopy { get; set; }

        /// <summary>
        /// Asunto
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Mensaje que se incluirá en el cuerpo del correo
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Cuerpo de correo
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Lista de archivos
        /// </summary>
        public List<MailFiles> Files { get; set; }
    }
}