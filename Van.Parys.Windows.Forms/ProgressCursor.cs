using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.Windows.Forms;

namespace Van.Parys.Windows.Forms
{
    public class ProgressCursor : IProgressCursor
    {
        #region Constructors

        public ProgressCursor()
        {
        }

        #endregion

        #region Public/Protected Members

        /// <summary>
        /// Gives users possibility to custom draw the cursor
        /// </summary>
        public event EventHandler<CursorPaintEventArgs> CustomDrawCursor;

        #endregion

        #region Private Members

        /// <summary>
        /// Creates the cursor using P/Invoke
        /// </summary>
        /// <param name="bmp">The BMP to display as cursor</param>
        /// <param name="xHotSpot">The x hot spot.</param>
        /// <param name="yHotSpot">The y hot spot.</param>
        /// <returns></returns>
        private Cursor CreateCursor(Bitmap bmp, Point hotSpot)
        {
            IntPtr iconHandle = bmp.GetHicon();
            IconInfo iconInfo = new IconInfo();
            UnManagedMethodWrapper.GetIconInfo(iconHandle, ref iconInfo);
            iconInfo.xHotspot = hotSpot.X;
            iconInfo.yHotspot = hotSpot.Y;
            iconInfo.fIcon = false;
            iconHandle = UnManagedMethodWrapper.CreateIconIndirect(ref iconInfo);
            return new Cursor(iconHandle);
        }

        /// <summary>
        /// Generates the progress bitmap.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        private Bitmap GenerateProgressBitmap(double value, double max)
        {
            var bitmap = new Bitmap(32, 32);
            var gfx = Graphics.FromImage(bitmap);

            gfx.SmoothingMode = SmoothingMode.AntiAlias;

            var cursorPaintEventArgs = new CursorPaintEventArgs {Graphics = gfx, Handled = false, Max = Max, Value = Current};
            OnCustomDrawCursor(cursorPaintEventArgs);

            if (cursorPaintEventArgs.Handled == false)
            {
                cursorPaintEventArgs.DrawDefault();
            }

            return bitmap;
        }

        /// <summary>
        /// Raises the <see cref="E:CustomDrawCursor"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Sphinx.Base.Business.ProgressCursor.CursorPaintEventArgs"/> instance containing the event data.</param>
        private void OnCustomDrawCursor(CursorPaintEventArgs e)
        {
            EventHandler<CursorPaintEventArgs> handler = CustomDrawCursor;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Sets the cursor to the specified bitmap
        /// </summary>
        /// <param name="btmp">The BTMP.</param>
        private void SetCursor(Bitmap btmp)
        {
            CurrentCursor = CreateCursor(btmp, new Point(16, 16));
            Cursor.Current = CurrentCursor;
        }

        #endregion

        #region Implementation of IProgressCursor

        /// <summary>
        /// Gets or sets the current value of the progess, this is in relation to the Max value
        /// </summary>
        /// <value>
        /// The current value
        /// </value>
        public double Current { get; set; }

        /// <summary>
        /// Gets the current cursor.
        /// </summary>
        public Cursor CurrentCursor { get; private set; }

        /// <summary>
        /// Gets or sets the max.
        /// </summary>
        /// <value>
        /// The max.
        /// </value>
        public double Max { get; set; }

        /// <summary>
        /// Ends this progress cursor, and sets it back to the default
        /// </summary>
        public void End()
        {
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Increments to the specified value
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>If parameter value is greater then max, it is set to max</remarks>
        /// <returns></returns>
        public IProgressCursor IncrementTo(double value)
        {
            if (value > Max)
                value = Max;

            Current = value;

            SetCursor(GenerateProgressBitmap(Current, Max));

            return this;
        }

        /// <summary>
        /// Increments the current value with specified value
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IProgressCursor IncrementWith(double value)
        {
            if ((value + Current) > Max)
                value = Max - Current;

            Current += value;

            SetCursor(GenerateProgressBitmap(Current, Max));

            return this;
        }

        #endregion
        
        #region Nested type: CursorPaintEventArgs

        public class CursorPaintEventArgs : EventArgs
        {
            #region Constructors

            public CursorPaintEventArgs()
            {
                BorderPen = new Pen(Color.LightSlateGray);
                FillPen = new SolidBrush(Color.LimeGreen);
            }

            #endregion

            #region Properties

            public Graphics Graphics { get; set; }
            public Pen BorderPen { get; set; }
            public Brush FillPen { get; set; }
            public double Value { get; set; }
            public double Max { get; set; }
            public bool Handled { get; set; }

            #endregion

            #region Public/Protected Members

            /// <summary>
            /// Draws the default rendering, this is a circular progressbar showing percental progress in the middle
            /// </summary>
            public void DrawDefault()
            {
                int fontEmSize = 7;

                var totalWidth = (int) Graphics.VisibleClipBounds.Width;
                var totalHeight = (int) Graphics.VisibleClipBounds.Height;
                int margin_all = 2;
                var band_width = (int) (totalWidth*0.1887);

                int workspaceWidth = totalWidth - (margin_all*2);
                int workspaceHeight = totalHeight - (margin_all*2);
                var workspaceSize = new Size(workspaceWidth, workspaceHeight);

                var upperLeftWorkspacePoint = new Point(margin_all, margin_all);
                var upperLeftInnerEllipsePoint = new Point(upperLeftWorkspacePoint.X + band_width, upperLeftWorkspacePoint.Y + band_width);

                var innerEllipseSize = new Size(((totalWidth/2) - upperLeftInnerEllipsePoint.X)*2, ((totalWidth/2) - upperLeftInnerEllipsePoint.Y)*2);

                var outerEllipseRectangle = new Rectangle(upperLeftWorkspacePoint, workspaceSize);
                var innerEllipseRectangle = new Rectangle(upperLeftInnerEllipsePoint, innerEllipseSize);

                double valueMaxRatio = (Value/Max);
                var sweepAngle = (int) (valueMaxRatio*360);

                var defaultFont = new Font(SystemFonts.DefaultFont.FontFamily, fontEmSize, FontStyle.Regular);
                string format = string.Format("{0:00}", (int) (valueMaxRatio*100));
                SizeF measureString = Graphics.MeasureString(format, defaultFont);
                var textPoint = new PointF(upperLeftInnerEllipsePoint.X + ((innerEllipseSize.Width - measureString.Width)/2), upperLeftInnerEllipsePoint.Y + ((innerEllipseSize.Height - measureString.Height)/2));

                Graphics.Clear(Color.Transparent);

                Graphics.DrawEllipse(BorderPen, outerEllipseRectangle);
                Graphics.FillPie(FillPen, outerEllipseRectangle, 0, sweepAngle);

                Graphics.FillEllipse(new SolidBrush(Color.White), innerEllipseRectangle);
                Graphics.DrawEllipse(BorderPen, innerEllipseRectangle);

                Graphics.DrawString(format, defaultFont, FillPen, textPoint);
            }

            #endregion
        }

        #endregion
    }
}