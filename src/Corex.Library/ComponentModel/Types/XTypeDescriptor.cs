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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Corex.Library.ComponentModel.Attributes.ViewDesign;
using Corex.Library.ComponentModel.Model;
using Corex.Library.ComponentModel.Model.Business;
using Corex.Library.ComponentModel.Model.Business.Enums;
using Corex.Library.ComponentModel.Model.Present;
using Corex.Library.ComponentModel.Model.Present.Attributes;
using Corex.Library.ComponentModel.Model.Validation.Behaviors;
using Corex.Library.Properties;

namespace Corex.Library.ComponentModel.Types;

public class XTypeDescriptor : IXTypeDescriptor
{
    private static readonly XTypeDescriptorCache Cache;
    private readonly Lazy<IDisplayType> _lazyDisplayTypeAttribute;

    private readonly Lazy<Guid> _lazyViewMetadataID;
    private readonly Lazy<PropertyDescriptor[]> _propertyDescriptorCollection;

    /* #region .cctor & .ctor */
    static XTypeDescriptor()
    {
        Cache = new XTypeDescriptorCache();
    }

    protected XTypeDescriptor(Type modelType)
    {
        ModelType = modelType;

        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
        modelType?.GetCustomAttributes(true);

        _lazyDisplayTypeAttribute = new Lazy<IDisplayType>(() => GetDisplayType(ModelType));

        _propertyDescriptorCollection = new Lazy<PropertyDescriptor[]>(
            () => TypeDescriptor.GetProperties(ModelType).Cast<PropertyDescriptor>().ToArray());

        _lazyViewMetadataID = new Lazy<Guid>(
            () => XHelper.Reflections.Attributes.Get<ViewAttribute>(ModelType)?.ID ?? Guid.Empty);
    }

    public Guid DesignID => _lazyViewMetadataID.Value;

    public IDisplayType DisplayType => _lazyDisplayTypeAttribute.Value;

    public bool IsFormattedDisplayModel => DisplayType.DisplayPattern != null;

    
    public IEnumerable<PropertyDescriptor> PropertiesOfMetadataPropertyDescriptor
        => Properties.Where(propertyDescriptor => propertyDescriptor.GetType().Name.Contains("MetadataPropertyDescriptorWrapper"));
    /* #endregion .cctor & .ctor */

    /* #region Properties */
    public Type ModelType { get; }

    public string ModelDisplayName => DisplayType.Name;

    public IEnumerable<PropertyDescriptor> Properties => _propertyDescriptorCollection.Value;

    public PropertyDescriptor GetDefaultLookupProperty()
    {
        var propDescriptionCollection = _propertyDescriptorCollection.Value;

        var query =
            propDescriptionCollection.Where(
                    descriptor => descriptor.Attributes.OfType<LookupDisplayAttribute>().Any())
                .Select(descriptor => descriptor)
                .ToArray();

        if (query.Length == 0)
        {
            return propDescriptionCollection.First();
        }

        var displayName =
            query.FirstOrDefault(
                descriptor =>
                    descriptor.Attributes.OfType<LookupDisplayAttribute>()
                        .FirstOrDefault(attribute => attribute.ConfigureAsDisplayName) != null);

        return displayName ?? query[0];
    }

    public object GetDefaultLookupPropertyValue(object obj)
    {
        var propertyInfo = obj.GetType().GetProperty(GetDefaultLookupProperty().Name);

        return propertyInfo?.GetValue(obj);
    }
    /* #endregion Properties */

    /* #region Model */
    public string GetModelDisplayName(object obj)
    {
        if (obj == null)
        {
            return null;
        }

        var displayValue = IsFormattedDisplayModel
            ? XHelper.Strings.Formater.Interpolation(DisplayType.DisplayPattern, obj)
            // ReSharper disable LocalizableElement
            : $"{GetPrimaryKeyPropertyValue(obj)} - {GetDefaultLookupPropertyValue(obj)}";
        // ReSharper restore LocalizableElement

        var entityWithSituation = obj.As<IEntityWorkWithSituation>();
        if (entityWithSituation != null && entityWithSituation.Situacao == ELogicalDelete.Inactive)
        {
            // ReSharper disable LocalizableElement
            return $"{displayValue} - <<{DbMessages.XTypeDescriptor_Inactive}>>";
            // ReSharper restore LocalizableElement
        }

        return displayValue;
    }

    public PropertyDescriptor GetPrimaryKeyProperty()
    {
        const string pkName = Entity.Metadata.DefaultPrimaryKeyColumnName;

        return _propertyDescriptorCollection.Value.First(descriptor => descriptor.Name == pkName);
    }

    public object GetPrimaryKeyPropertyValue(object obj)
    {
        var propertyInfo = obj.GetType().GetProperty(GetPrimaryKeyProperty().Name);

        Debug.Assert(propertyInfo != null, nameof(propertyInfo) + " != null");

        return propertyInfo.GetValue(obj);
    }

    public PropertyDescriptor GetPropertyDescriptor(string propertyName)
    {
        return _propertyDescriptorCollection.Value.FirstOrDefault(descriptor => descriptor.Name == propertyName);
    }

    public string GetPropertyDisplayName(PropertyDescriptor propertyDescription, bool formatRequiredAsHtml = true)
    {
        if (propertyDescription == null)
        {
            return string.Empty;
        }

        var displayName = XHelper.Models.GetDisplayName(propertyDescription);
        if (formatRequiredAsHtml && IsPropertyRequired(propertyDescription))
        {
            // ReSharper disable LocalizableElement
            displayName += " <color='red'>*</color>";
            // ReSharper restore LocalizableElement
        }

        return displayName;
    }
    /* #endregion Model */

    /* #region Property */
    public PropertyInfo GetPropertyInfo(string propertyName)
    {
        var propertyDescriptor = GetPropertyDescriptor(propertyName);
        return propertyDescriptor?.ComponentType.GetProperty(propertyName);
    }

    public bool IsPropertyRequired(PropertyDescriptor propertyDescriptor) => BehaviorIdentifier.IsPropertyRequired(propertyDescriptor);

    public static XTypeDescriptor Get(Type type)
    {
        return Cache.Get(
            type,
            t =>
                {
                    var newType = typeof(XTypeDescriptor<>).MakeGenericType(type);

                    return (XTypeDescriptor) Activator.CreateInstance(newType, true);
                });
    }

    public static XTypeDescriptor Get<T>() => Cache.Get(typeof(T), type => new XTypeDescriptor<T>());

    public static IDisplayType GetDisplayType(Type modelType)
    {
        var descriptors = TypeDescriptor
            .GetAttributes(modelType)
            .OfType<IDisplayType>()
            .ToArrayEfficient();

        return descriptors
                   .OrderByDescending(type => type.Level)
                   .ThenByDescending(type => type.Name)
                   .FirstOrDefault()
               ??
               new TypeDisplayAttribute
                   {
                       Name = modelType.Name
                   };
    }

    public static XTypeDescriptor GetGenericExtension(Type typeOfDescriptor, Type modelType)
    {
        var newType = typeOfDescriptor.MakeGenericType(modelType);

        return Cache.Get(
            newType, t => (XTypeDescriptor) Activator.CreateInstance(newType, true));
    }

    public string GetPropertyDisplayName(string propertyName, bool formatRequiredAsHtml = true)
    {
        return GetPropertyDisplayName(Properties.FirstOrDefault(descriptor => descriptor.Name == propertyName), formatRequiredAsHtml);
    }

    public string GetPropertyDisplayNameFormated(PropertyDescriptor propertyDescription, string displayName, bool formatRequiredAsHtml = true)
    {
        if (propertyDescription == null)
        {
            return displayName;
        }

        if (formatRequiredAsHtml && IsPropertyRequired(propertyDescription))
        {
            // ReSharper disable LocalizableElement
            displayName += " <color='red'>*</color>";
            // ReSharper restore LocalizableElement
        }

        return displayName;
    }

    /* #endregion Property */
}