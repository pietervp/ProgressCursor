using System;
using System.Linq;

namespace Van.Parys.Windows.Forms
{
    public class CursorHelper
    {
        #region Public/Protected Members

        /// <summary>
        /// Starts the progress cursor.
        /// </summary>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static IProgressCursor StartProgressCursor(double max = 1.0)
        {
            var progressCursor = new ProgressCursor {Max = max, Current = 0};
            return progressCursor;
        }

        #endregion
    }
}