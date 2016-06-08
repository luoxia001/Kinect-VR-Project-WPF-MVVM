namespace KinectProject.Navigation
{
    /// <summary>
    /// Interface for an INavigable exported part
    /// </summary>
    public interface IExportNavigableMetadata
    {
        /// <summary>
        /// Name of the navigation context.
        /// </summary>
        string NavigableContextName { get; }
    }
}
