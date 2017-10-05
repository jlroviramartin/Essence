using System;

namespace Essence.Geometry.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ConvertHelperAttribute : Attribute
    {
        public Type SourceType1 { get; set; }
        public Type SourceType2 { get; set; }
        public Type DestinationType { get; set; }
        public float Weight { get; set; }
    }
}