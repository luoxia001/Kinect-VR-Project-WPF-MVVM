namespace KinectProject.Utilities
{
    /// <summary>
    /// Represents a possible state of a prompt UI experience.
    /// </summary>
    public enum PromptState
    {
        /// <summary>
        /// Preconditions to show the prompt have not been met.
        /// </summary>
        Hidden,

        /// <summary>
        /// Prompt is being shown to user.
        /// </summary>
        Prompting,

        /// <summary>
        /// Prompt preconditions have been met, but prompt has been dismissed.
        /// </summary>
        Dismissed
    }
}
