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
using Corex.Library.I18n;

namespace Corex.Library.ComponentModel.Model.Present.Attributes.Base;

public class BaseDisplayAttribute : Attribute, IDisplayType
{
    private readonly LocalizableString _description = new LocalizableString(nameof(Description));
    private readonly LocalizableString _groupName = new LocalizableString(nameof(GroupName));
    private readonly LocalizableString _name = new LocalizableString(nameof(Name));

    private string _displayPattern;
    private Type _resourceType;

    public string Description
    {
        get => _description.Value;
        set
        {
            if (_description.Value != value)
            {
                _description.Value = value;
            }
        }
    }

    public string GroupName
    {
        get => _groupName.Value;
        set
        {
            if (_groupName.Value != value)
            {
                _groupName.Value = value;
            }
        }
    }

    public string Name
    {
        get => _name.Value;
        set
        {
            if (_name.Value != value)
            {
                _name.Value = value;
            }
        }
    }

    public Type ResourceType
    {
        get => _resourceType;
        set
        {
            if (_resourceType == value)
            {
                return;
            }

            _resourceType = value;

            _name.ResourceType = value;
            _description.ResourceType = value;
            _groupName.ResourceType = value;
        }
    }

    public override object TypeId => this;

    public virtual int Level { get; set; }

    public string DisplayPattern
    {
        get => _displayPattern;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                // ReSharper disable LocalizableElement
                if (!value.Contains("{"))
                {
                    value = "{" + value + "}";
                }
                // ReSharper restore LocalizableElement
            }

            _displayPattern = value;
        }
    }

    string IDisplayType.Description => GetDescription();

    string IDisplayType.Name => GetName();

    public string GetDescription() => _description.GetLocalizableValue();

    public string GetGroupName() => _groupName.GetLocalizableValue();

    public string GetName() => _name.GetLocalizableValue();
}