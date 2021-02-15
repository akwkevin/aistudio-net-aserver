using EFCore.Sharding;
using System;

namespace Coldairarrow.Util
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PhysicDeleteTypeAttribute : Attribute
    {
        public PhysicDeleteTypeAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CachTypeAttribute : Attribute
    {
        public CachTypeAttribute() { }

    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ExpandByDateModeTypeAttribute : Attribute
    {
        public ExpandByDateModeTypeAttribute(ExpandByDateMode mode)
        {
            Mode = mode;
        }

        private ExpandByDateMode _Mode = ExpandByDateMode.PerMonth;
        public ExpandByDateMode Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PushMessageTypeAttribute : Attribute
    {
        public PushMessageTypeAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BatchSaveTypeAttribute : Attribute
    {
        public BatchSaveTypeAttribute() { }
    }
}
