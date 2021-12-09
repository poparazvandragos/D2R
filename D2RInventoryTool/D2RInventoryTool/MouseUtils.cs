using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows; // Or use whatever point class you like for the implicit cast operator

namespace D2RInventoryTool
{
    /// <summary>
    /// Struct representing a point.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public static implicit operator Point(POINT point)
        {
            return new Point(point.X, point.Y);
        }

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class MouseUtils
    {
        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            //GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            bool success = GetCursorPos(out lpPoint);
            if (success)
                return lpPoint;
            else
                return new POINT()
                {
                    X = -1,
                    Y = -1
                };
        }

        public static void SetMousePosition(Point point)
        {
            POINT p = new POINT((int)point.X, (int)point.Y);
            //ClientToScreen(this.Handle, ref p);
            SetCursorPos(p.X, p.Y);
        }
    }
}
