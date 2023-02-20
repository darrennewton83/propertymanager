using Service.property;

namespace Service.tenancy
{
    /// <summary>
    /// Represents a tenancy
    /// </summary>
    public interface ITenancy
    {
        /// <summary>
        /// The property the tenancy is for
        /// </summary>
        IProperty Property { get; }

        /// <summary>
        /// The start date of the tenancy
        /// </summary>
        public DateOnly StartDate { get; }

        /// <summary>
        /// The end date of the tenancy
        /// </summary>
        public DateOnly EndDate { get; }
    }
}
