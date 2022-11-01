namespace SharedKernell.Mail
{
    public interface IMailHelper
    {
        Task Send(Mail mail);
    }
}