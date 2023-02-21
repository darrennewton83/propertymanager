namespace PropertyManager.Shared.contact
{
    /// <summary>
    /// Represents a contact
    /// </summary>
    public interface IContact
    {
        /// <summary>
        /// The title of the contact
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The firstname of the contact
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// The last name of the contact
        /// </summary>
        public string LastName { get; }
    }
}
