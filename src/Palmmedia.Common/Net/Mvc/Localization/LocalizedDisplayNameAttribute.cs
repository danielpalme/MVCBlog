using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Palmmedia.Common.Net.Mvc.Localization
{
    /// <summary>
    /// A <see cref="DisplayNameAttribute"/> which can be used together with the 'LabelFor' and 'EditorFor'
    /// methods.
    /// It provides the possibility to use resources for localization, similar to other attributes from
    /// the 'System.ComponentModel.DataAnnotations' namespace.
    /// </summary>
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        /// <summary>
        /// The message resource accessor.
        /// </summary>
        private Func<string> messageResourceAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedDisplayNameAttribute"/> class.
        /// </summary>
        public LocalizedDisplayNameAttribute()
            : base(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedDisplayNameAttribute"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        public LocalizedDisplayNameAttribute(string displayName)
            : base(displayName)
        {
        }

        /// <summary>
        /// Gets or sets the name of the message resource.
        /// </summary>
        public string MessageResourceName { get; set; }

        /// <summary>
        /// Gets or sets the type of the message resource.
        /// </summary>
        public Type MessageResourceType { get; set; }

        /// <summary>
        /// Gets the display name for a property, event, or public void method that takes no arguments stored in this attribute.
        /// </summary>
        /// <value></value>
        /// <returns>The display name.</returns>
        public override string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.MessageResourceName) && this.MessageResourceType != null)
                {
                    this.SetResourceAccessorByPropertyLookup();
                    return this.messageResourceAccessor();
                }

                return base.DisplayName;
            }
        }

        /// <summary>
        /// Sets the resource accessor by property lookup.
        /// </summary>
        private void SetResourceAccessorByPropertyLookup()
        {
            if ((this.MessageResourceType == null) || string.IsNullOrEmpty(this.MessageResourceName))
            {
                throw new InvalidOperationException("Need Both ResourceType And ResourceName");
            }

            PropertyInfo property = this.MessageResourceType.GetProperty(this.MessageResourceName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            if (property != null)
            {
                MethodInfo getMethod = property.GetGetMethod(true);
                if ((getMethod == null) || (!getMethod.IsAssembly && !getMethod.IsPublic))
                {
                    property = null;
                }
            }

            if (property == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Resource Type Does Not Have Property", new object[] { this.MessageResourceType.FullName, this.MessageResourceName }));
            }

            if (property.PropertyType != typeof(string))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Resource Property Not String Type", new object[] { property.Name, this.MessageResourceType.FullName }));
            }

            this.messageResourceAccessor = delegate
            {
                return (string)property.GetValue(null, null);
            };
        }
    }
}
