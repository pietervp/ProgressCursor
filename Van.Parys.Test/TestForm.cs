using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Van.Parys.Windows.Forms;

namespace Van.Parys.Test
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

            changeCursorButton.Click += delegate
                             {
                                 var progressCursor = Van.Parys.Windows.Forms.CursorHelper.StartProgressCursor(100);

                                 progressCursor.CustomDrawCursor += progressCursor_CustomDrawCursor;

                                 for (int i = 0; i < 100; i++)
                                 {
                                     progressCursor.IncrementTo(i);

                                     //simulate some work
                                     Thread.Sleep(100);
                                 }
                                    
                                 progressCursor.End();
                             };
        }

        void progressCursor_CustomDrawCursor(object sender, ProgressCursor.CursorPaintEventArgs e)
        {
            e.DrawDefault();
            e.Graphics.DrawString("Test", SystemFonts.DefaultFont, Brushes.Black, 0,0);
            e.Handled = true;
        }
    }
}
