namespace KinectProject.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    /// <summary>
    /// MEF export attribute that defines a contract that the exported part 
    /// is of type INavigable and exposes a unique name.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ExportNavigableAttribute : ExportAttribute, IExportNavigableMetadata
    {
        public ExportNavigableAttribute()
            : base(typeof(INavigableContext))
        {
        }

        /// <summary>
        /// Name of the navigation context.
        /// </summary>
        public string NavigableContextName { get; set; }
    }
}
