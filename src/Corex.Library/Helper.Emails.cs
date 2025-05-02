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
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace Corex.Library;

public partial class XHelper
{
    public class Emails
    {
        public static void SaveMailMessage(MailMessage msg, string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.Create);
            ExportToEml(msg, fs);
        }

        private static void ExportToEml(MailMessage msg, Stream str)
        {
            using var client = new SmtpClient();
            var path = Path.Combine(Path.GetTempPath(), $"Corex.Temp\\{Guid.NewGuid()}\\");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            client.UseDefaultCredentials = true;
            client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            client.PickupDirectoryLocation = path;
            client.Send(msg);

            var filePath = Directory.GetFiles(path).Single();
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                fs.CopyTo(str);
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}