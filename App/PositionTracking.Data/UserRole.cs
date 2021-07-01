using System;
namespace PositionTracking.Data
{
    public enum UserRole
    {
        /// <summary> Admin </summary>

        View = 1,

        Edit,

        Add,

        Admin = int.MaxValue,

    }
}
