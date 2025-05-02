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
using System.Dynamic;

namespace Corex.Library.Dynamic;

public class ExpanderObject : DynamicObject
{
    public Dictionary<string, object> ObjectDictionary;

    public ExpanderObject()
    {
        ObjectDictionary = new Dictionary<string, object>();
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        if (ObjectDictionary.TryGetValue(binder.Name, out var val))
        {
            result = val;
            return true;
        }

        result = null;
        return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        try
        {
            ObjectDictionary[binder.Name] = value;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}