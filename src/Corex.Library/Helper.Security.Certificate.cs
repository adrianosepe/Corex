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
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Corex.Exceptions.Assert;
using Corex.Library.Properties;

namespace Corex.Library;

public partial class XHelper
{
    public static class Security
    {
        public static class Certificate
        {
            public static X509Certificate2 RecoverCertificateBySerialNumber(
                string serialNumber, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.LocalMachine)
            {
                XContract.ArgIsNotNull(serialNumber, nameof(serialNumber));

                using var store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                return store
                    .Certificates
                    .Cast<X509Certificate2>()
                    .FirstOrDefault(certificate => serialNumber.Equals(certificate.SerialNumber, StringComparison.InvariantCultureIgnoreCase));
            }

#if !NETSTANDARD
            public static IEnumerable<X509Certificate2> ShowSelectionUIFromValidCertificates(X509SelectionFlag selectionModel = X509SelectionFlag.SingleSelection)
            {
                var uiMessage = selectionModel == X509SelectionFlag.SingleSelection
                    ? DbMessages.Certificate_ShowSelectionUIFromValidCertificates_Selecione_o_certificado_que_deseja_utilizar
                    : DbMessages.Certificate_ShowSelectionUIFromValidCertificates_Selecione_os_certificados_que_deseja_utilizar;

                var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                try
                {
                    var certificates = store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, true);
                    var selection = X509Certificate2UI.SelectFromCollection(
                        certificates, DbMessages.Certificate_ShowSelectionUIFromValidCertificates_Certificados_válidos_, uiMessage, selectionModel);

                    return selection.Cast<X509Certificate2>().ToArray();
                }
                finally
                {
                    store.Close();
                }
            }
#endif
        }
    }
}