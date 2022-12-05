using System;

namespace RNGui.BuildInfo
{
    public class CompilationTimeStringAttribute : Attribute
    {
        private static string _name = null;

        public static string Name
        {
            get
            {
                if (_name == null)
                {
                    var n = nameof(CompilationTimeStringAttribute);
                    _name = n.Remove(n.Length - 9);
                }
                return _name;
            }
        }
    }
}