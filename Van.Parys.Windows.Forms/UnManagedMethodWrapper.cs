using System.Linq;
using System;
using System.Runtime.InteropServices;

namespace Van.Parys.Windows.Forms
{
    public sealed class UnManagedMethodWrapper
    {
        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo iconInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr iconHandle, ref IconInfo iconInfo);
    }
}