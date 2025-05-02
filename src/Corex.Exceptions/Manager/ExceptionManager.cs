#region License

// Copyright(c) 2023 GrappTec
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using Corex.Exceptions.Internals;

namespace Corex.Exceptions.Manager;

public class ExceptionManager
{
    public static readonly ExceptionManager Instance;

    private IExceptionConfig _config;

    static ExceptionManager()
    {
        Instance = new ExceptionManager();
    }

    internal ExceptionManager()
    {
        DefaultErrorMessage = "Ocorreu um erro inesperado em algum processo interno. Tente novamente.";
    }

    public bool AllowSupportEmail { get; set; }

    public bool AttachmentsPrintScreen { get; set; }

    public bool AutoSendEmail { get; set; }

    public string DefaultErrorMessage { get; set; }

    public string EmailFrom { get; set; }

    public bool EmailSmtpAuth { get; set; }

    public int EmailSmtpPort { get; set; }

    public string EmailSmtpPwd { get; set; }

    public string EmailSmtpServer { get; set; }

    public string EmailSmtpUser { get; set; }

    public string EmailTo { get; set; }

    public string SupportEmail { get; set; }

    public event FormatEmailExceptionEventHandler FormatEmailException;

    public MailMessage CreateAutoMailMessage(string title, string body)
    {
        var message = new MailMessage(EmailFrom, EmailTo, title, body)
            {
                IsBodyHtml = true
            };

        if (!AttachmentsPrintScreen || !IsConfigRegistered())
        {
            return message;
        }

        var fileName = Path.ChangeExtension(Path.GetTempFileName(), "png");
        _config.CaptureScreen(fileName);

        message.Attachments.Add(new Attachment(fileName));

        return message;
    }

    public MailMessage CreateSupportMessage(string title, string body)
    {
        var message = new MailMessage(EmailFrom, SupportEmail ?? EmailTo, title, body)
            {
                IsBodyHtml = true
            };

        if (!AttachmentsPrintScreen || !IsConfigRegistered())
        {
            return message;
        }

        var fileName = Path.ChangeExtension(Path.GetTempFileName(), "png");
        _config.CaptureScreen(fileName);

        message.Attachments.Add(new Attachment(fileName));

        return message;
    }

    public static string DefaultFormatException(Exception exception)
    {
        var msg = Encoding.UTF8.GetString(XHelper.Resources.LoadResource(typeof(ExceptionManager).Assembly,
            "Manager.Resources.EmailErrorContent.html"));

        try
        {
            msg = msg.Replace("{NAME}", AppDomain.CurrentDomain.FriendlyName);
            msg = msg.Replace("{VERSION}", Assembly.GetEntryAssembly()?.GetName().Version.ToString());
            msg = msg.Replace("{MACHINE}", Environment.MachineName);
            msg = msg.Replace("{STACKTRACE}", FormatString(Environment.StackTrace, true));
            msg = msg.Replace("{EXMESSAGE}", FormatString(exception.Message, true));
            msg = msg.Replace("{EXCEPTIONS}",
                FormatString(GetExceptions(exception, 0), false));
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.ToString());
        }

        return msg;
    }

    public string FormatException(Exception exception, bool useDefaultLayout = false)
    {
        if (FormatEmailException == null || useDefaultLayout)
        {
            return DefaultFormatException(exception);
        }

        var e = new FormatEmailExceptionEventArgs(exception);
        FormatEmailException(this, e);

        return e.Output.ToString();
    }

    public void HandleException(Exception exception)
    {
        var newGuid = Guid.NewGuid();
        var title = $"Protocol ID: {newGuid}";
        var body = FormatException(exception);

        if (AutoSendEmail)
        {
            SendEmail(CreateAutoMailMessage(title, body));
        }

        if (IsConfigRegistered())
        {
            _config.Execute(exception, newGuid, CreateSupportMessage(title, body));
        }
    }

    public void RegisterExceptionConfig(IExceptionConfig config)
    {
        _config?.UnregisterManager(this);

        _config = config;
        _config?.RegisterManager(this);
    }

    public bool SendEmail(MailMessage message)
    {
        try
        {
            var mailClient = new SmtpClient(EmailSmtpServer, EmailSmtpPort);

            if (EmailSmtpAuth)
            {
                mailClient.EnableSsl = true;
                mailClient.Timeout = 10000;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = new NetworkCredential(EmailSmtpUser, EmailSmtpPwd);
            }

            mailClient.Send(message);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string FormatString(string message, bool formatNewLine)
    {
        if (!formatNewLine)
        {
            return message ?? string.Empty;
        }

        return (message ?? string.Empty).Replace(Environment.NewLine, "<br />");
    }

    private static string GetExceptions(Exception exception, int level)
    {
        var msg = Encoding.UTF8.GetString(XHelper.Resources.LoadResource(typeof(ExceptionManager).Assembly,
            "Manager.Resources.ExceptionContent.html"));

        msg = msg.Replace("{INNERLEVEL_PX}", GetInnerLevel(level));
        msg = msg.Replace("{EXMESSAGE}", exception.Message);
        msg = msg.Replace("{EXLEVEL}", level.ToString(CultureInfo.InvariantCulture));
        msg = msg.Replace("{EXTYPE}", exception.GetType().FullName);
        msg = msg.Replace("{EXMESSAGE}", FormatString(exception.Message, true));
        msg = msg.Replace("{EXSTACKTRACE}", FormatString(exception.StackTrace, true));
        msg = msg.Replace("{EXSOURCE}", FormatString(exception.Source, true));

        if (exception.InnerException != null)
        {
            msg += GetExceptions(exception.InnerException, level + 1);
        }

        return msg;
    }

    private static string GetInnerLevel(int level) => level <= 0 ? 0.ToString(CultureInfo.InvariantCulture) : (15 * level).ToString(CultureInfo.InvariantCulture);

    private bool IsConfigRegistered() => _config != null;
}