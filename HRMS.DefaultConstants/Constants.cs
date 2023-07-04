using System;

namespace HRMS.DefaultConstants
{
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///   Contains a listing of constants used throughout the application
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////
    public sealed class Constants
    {
        /// <summary>
        /// The value used to represent a null DateTime value
        /// </summary>
        public static byte[] NullByte = null;

        /// <summary>
        /// The value used to represent a null DateTime value
        /// </summary>
        public static DateTime NullDateTime = DateTime.Parse( "01/01/1900");

        /// <summary>
        /// The value used to represent a null decimal value
        /// </summary>
        public static decimal NullDecimal = 0;

        /// <summary>
        /// The value used to represent a null double value
        /// </summary>
        public static double NullDouble = 0;

        /// <summary>
        /// The value used to represent a null Guid value
        /// </summary>
        public static Guid NullGuid = Guid.Empty;

        /// <summary>
        /// The value used to represent a null int value
        /// </summary>
        public static int NullInt = -1;//int.MinValue;

        /// <summary>
        /// The value used to represent a null long value
        /// </summary>
        public static long NullLong = 0;

        /// <summary>
        /// The value used to represent a null float value
        /// </summary>
        public static float NullFloat = 0;

        /// <summary>
        /// The value used to represent a null string value
        /// </summary>
        public static string NullString = string.Empty;

        /// <summary>
        /// The value used to represent a null bool value
        /// </summary>
        public static bool? NullBool = null;

        /// <summary>
        /// Maximum DateTime value allowed by SQL Server
        /// </summary>
        public static DateTime SqlMaxDate = DateTime.Now;

        /// <summary>
        /// Minimum DateTime value allowed by SQL Server
        /// </summary>
        public static DateTime SqlMinDate = DateTime.Now;
    }
}