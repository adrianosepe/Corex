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

namespace Corex.Library.ComponentModel.Model.Validation.Annotations;

public enum EDataType
{
    Custom = 0,

    DateTime = 1,
    Date = 2,
    Time = 3,
    Duration = 4,
    PhoneNumber = 5,
    Currency = 6,
    Text = 7,
    Html = 8,
    MultilineText = 9,
    EmailAddress = 10,
    Password = 11,
    Url = 12,
    ImageUrl = 13,
    CreditCard = 14,
    PostalCode = 15,
    Upload = 16,

    PrimaryKey = 20,
    ForeignKey = 21,
    Xml = 22
}