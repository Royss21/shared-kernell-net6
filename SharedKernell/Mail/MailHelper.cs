
namespace SharedKernell.Mail
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Options;
    using System.Net;
    using System.Net.Mail;

    public class MailHelper : IMailHelper
    {
        private readonly MailConfiguration _mailConfig;

        public MailHelper(IOptions<MailConfiguration> mailConfig)
        {
            _mailConfig = mailConfig.Value;
        }

        public async Task Send(Mail mail)
        {
            using (var client = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = _mailConfig.UserName,
                    Password = _mailConfig.Password 
                };

                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = credential;
                client.Host = _mailConfig.Host;
                client.Port = _mailConfig.Port;
                client.EnableSsl = _mailConfig.EnableSsl;
                client.UseDefaultCredentials = false;

                using (var mailMessage = new MailMessage())
                {
                    if (mail.Mails is not null && mail.Mails.Any())
                    {
                        foreach (var address in mail.Mails) 
                            mailMessage.To.Add(address);
                    }
                    else 
                        throw new Exception("Debes incluir al menos una dirección de correo destinatario.");

                    mailMessage.From = new MailAddress(_mailConfig.Address.Mail, _mailConfig.Address.ShowName);
                    mailMessage.Subject = mail.Subject;
                    mailMessage.Body = mail.Body;
                    mailMessage.IsBodyHtml = true;

                    if (mail.MailsCopy is not null && mail.MailsCopy.Any())
                    {
                        foreach (var bccAddress in mail.MailsCopy) 
                            mailMessage.Bcc.Add(bccAddress);
                    }

                    if (mail.Files is not null && mail.Files.Any())
                        foreach (var file in mail.Files)
                            if (string.IsNullOrWhiteSpace(file.Path.Trim()))
                                mailMessage.Attachments.Add(new Attachment(file.Path));
                            else
                                mailMessage.Attachments.Add(new Attachment(file.Stream, file.Name));

                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    try
                    {
                        await client.SendMailAsync(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Ha ocurrido un problema al intentar enviar el correo.", ex);
                    }
                }
            }
        }

        public string RenderViewToString(Controller controller, string viewName, Object model)
        {
            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = GetView(viewEngine, controller, viewName);
                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }

        private ViewEngineResult GetView(IViewEngine viewEngine, Controller controller, string viewName)
        {
            var hostingEnv = controller.HttpContext.RequestServices.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
            return viewEngine.GetView(hostingEnv.WebRootPath, viewName, true);
        }
    }
}