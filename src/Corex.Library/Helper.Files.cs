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

// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using Corex.Exceptions.Assert;
using Corex.Exceptions.Base;
using Corex.Library.Files;
using JetBrains.Annotations;

namespace Corex.Library;

public partial class XHelper
{
    public static class Files
    {
        public static string AddSuffix(string fileName, string sufix)
        {
            var extension = Flow.FlowMustBeNotNull(Path.GetExtension(fileName));

            return Path.ChangeExtension(fileName, sufix + extension.Remove(0, 1));
        }

        public static void CreateEmptyFile(string fileName)
        {
            XContract.ArgIsNotNull(fileName, nameof(fileName));

            File.Create(fileName).Dispose();
        }

        public static string CreateFileNameFromDateTime(DateTime? dateTime = null, string prefix = null)
        {
            dateTime ??= DateTime.Now;

            return prefix + dateTime.Value.ToString("dd-MM-yyyy-HHmmss", CultureInfo.InvariantCulture);
        }

        public static string CreateFileNameFromDateTime(FileExtension fileExtension, DateTime? dateTime = null, string prefix = null)
        {
            XContract.ArgIsNotNull(fileExtension, nameof(fileExtension));

            return prefix + fileExtension.ChangeExtension(CreateFileNameFromDateTime(dateTime));
        }

        public static string CreateFileNameFromGuid(Guid? id = null)
        {
            id ??= Guid.NewGuid();

            return id.Value.ToString();
        }

        public static string CreateFileNameFromGuid(FileExtension fileExtension, Guid? id = null)
        {
            XContract.ArgIsNotNull(fileExtension, nameof(fileExtension));

            return fileExtension.ChangeExtension(CreateFileNameFromGuid(id));
        }

        public static string CreateFileNameOnTempFolder(FileExtension fileExtension)
        {
            XContract.ArgIsNotNull(fileExtension, nameof(fileExtension));

            return fileExtension.ChangeExtension(Path.GetTempFileName());
        }

        public static string CreateFileNameOnUserDocuments(FileExtension fileExtension, DateTime? dateTime = null, string prefix = null)
        {
            XContract.ArgIsNotNull(fileExtension, nameof(fileExtension));

            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            return fileExtension.ChangeExtension(Path.Combine(myDocuments, CreateFileNameFromDateTime(dateTime, prefix)));
        }

        public static void MoveVersioningIfExists(string originalFileName, string destinationDirectory)
        {
            var fileName = Path.GetFileName(originalFileName);
            var destination = Path.Combine(destinationDirectory, fileName ?? throw new XInvalidOperationException());

            if (File.Exists(destination))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                var extension = Path.GetExtension(fileName);

                var attempt = 0;
                do
                {
                    attempt++;
                    var sufix = attempt == 1 ? string.Empty : $" ({attempt})";
                    destination = Path.Combine(destinationDirectory, $"{fileNameWithoutExtension} - Copy{sufix}{extension}");
                } while (File.Exists(destination));
            }

            File.Move(originalFileName, destination);
        }

        public static void TryDelete(params string[] files)
        {
            foreach (var file in files)
            {
                try
                {
                    if (!Strings.HasData(file))
                    {
                        continue;
                    }

                    File.Delete(file);
                }
                catch (Exception e)
                {
                    XDebug.OnException(e);
                }
            }
        }

        public static void Write([NotNull] string filePath, [NotNull] Stream stream)
        {
            XContract.ArgIsNotNull(filePath, nameof(filePath));
            XContract.ArgIsNotNull(stream, nameof(stream));

            stream.Seek(0, SeekOrigin.Begin);

            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            stream.CopyTo(fileStream);
        }

        public static void Write([NotNull] string filePath, [NotNull] Action<FileStream> action)
        {
            XContract.ArgIsNotNull(filePath, nameof(filePath));
            XContract.ArgIsNotNull(action, nameof(action));

            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            action(fileStream);
        }

        public static void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);

        public static class Read
        {
            public static DataTable CvAsDataTable(string strFilePath, char separator = ',')
            {
                var dt = new DataTable();
                using (var sr = new StreamReader(strFilePath))
                {
                    var headers = sr.ReadLine()?.Split(separator);
                    if (headers == null)
                    {
                        return null;
                    }

                    foreach (var header in headers)
                    {
                        dt.Columns.Add(header);
                    }

                    while (!sr.EndOfStream)
                    {
                        var rows = sr.ReadLine()?.Split(separator);
                        if (rows == null)
                        {
                            continue;
                        }

                        var dr = dt.NewRow();
                        for (var i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                        }

                        dt.Rows.Add(dr);
                    }
                }

                return dt;
            }

            public static IEnumerable<string> ReadAllLines(Stream pStream, Encoding encoding)
            {
                using var stream = pStream;
                using var reader = new StreamReader(stream, encoding);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }

            public static IEnumerable<string> ReadAllLines(string fileName, Encoding encoding) => ReadAllLines(new FileStream(fileName, FileMode.Open), encoding);
        }
    }
}