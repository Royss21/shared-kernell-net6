namespace SharedKernell.Mail
{
    public class MailConfiguration
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public AddresMailConfiguration Address { get; set; }
    }

    public class AddresMailConfiguration
    {
        public string Mail { get; set; }
        public string ShowName { get; set; }
    }
}