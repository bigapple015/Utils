using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Common.Utils
{
    public static class QuickEditMode
    {
        /// <summary>
        /// Disable quick Edit mode
        /// </summary>
        public static void DisableQuickEditMode()
        {
            IntPtr hStdin = GetStdHandle(STD_INPUT_HANDLE);
            uint mode;
            if (GetConsoleMode(hStdin, out mode))
            {
                mode &= ~ENABLE_QUICK_EDIT_MODE;
                SetConsoleMode(hStdin, mode);
            }
        }

        /// <summary>
        /// The standard input device.  Initially, this is the console input buffer.
        /// </summary>
        public const int STD_INPUT_HANDLE = -10;
        /// <summary>
        /// This flag enanles the user to mouse to select and edit text
        /// To enable this mode, use ENABLE_QUICK_EDIT_MODE | ENABLE_ENTENDED_FLAGS. To disable this mode, use ENABLE_ENTENDED_FLAGS.
        /// </summary>
        public const uint ENABLE_QUICK_EDIT_MODE = 0x0040;

        /// <summary>
        /// Retrieves a handle to the specified standard device
        /// </summary>
        /// <param name="nStdHandle"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);
        /// <summary>
        /// Retrieve the current input mode of a console's input buffer or the current mode of a console screen buffer.
        /// </summary>
        /// <param name="hConsoleHandle"></param>
        /// <param name="lpMode"></param>
        /// <returns>If the function succeeds, the return value is true</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
        /// <summary>
        /// Sets the input mode of a console's input buffer or the output mode of a console screen buffer
        /// </summary>
        /// <param name="hConsoleHandle"></param>
        /// <param name="dwMode"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    }
}
