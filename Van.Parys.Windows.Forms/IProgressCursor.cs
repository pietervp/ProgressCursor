using System;
using System.Windows.Forms;

namespace Van.Parys.Windows.Forms
{
    public interface IProgressCursor
    {
        #region Properties

        Cursor CurrentCursor { get; }

        double Max { get; set; }

        double Current { get; set; }

        #endregion

        #region Public/Protected Members

        void End();

        IProgressCursor IncrementTo(double value);
        IProgressCursor IncrementWith(double value);

        event EventHandler<ProgressCursor.CursorPaintEventArgs> CustomDrawCursor;

        #endregion
    }
}