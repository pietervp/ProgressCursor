ProgressCursor
==============
Example
-------
![Alt text](http://www.codeproject.com/KB/progress/progresscursor/cursor.png)

Using the code
--------------

	var progressCursor = Van.Parys.Windows.Forms.CursorHelper.StartProgressCursor(100);

	for (int i = 0; i < 100; i++)
	{
	 progressCursor.IncrementTo(i);

	 //do some work
	}

	progressCursor.End();
	
	
The library also has some points of extensibility, by handling the 'EventHandler<CursorPaintEventArgs> CustomDrawCursor' event. By handling this event, the developer can choose to extend the default behaviour by running the DrawDefault method on the CursorPaintEventArgs instance (1-2).

	...
	progressCursor.CustomDrawCursor += progressCursor_CustomDrawCursor;
	...

	void progressCursor_CustomDrawCursor(object sender, 
						ProgressCursor.CursorPaintEventArgs e)
	{
		e.DrawDefault();
		
		//add text to the default drawn cursor
		e.Graphics.DrawString("Test", 
				   SystemFonts.DefaultFont, Brushes.Black, 0,0);
		
		//set Handled to true, or else nothing will happen,
		//and default painting is done
		e.Handled = true;
	}
	
	
IProgressCursor also implements IDisposable, which makes the 'using' statement valid on this interface. The advantage is that no custom exception handling has to be done to ensure the End() method is called on the ProgressCursor. An example of the usage is found in 1-3.

	using (var progressCursor = CursorHelper.StartProgressCursor(100))
	{
		for (int i = 0; i < 100; i++)
		{
			progressCursor.IncrementTo(i);

			//simulate some work
		}
	}
	1-3 ProgressCursor implements IDisposable

Why implement IDisposable 
-------------------------

A classic usage of the default cursor classes would be like this:

	private void DoStuff()
	{
		Cursor.Current = Cursors.WaitCursor;

		try
		{
			//do heavy duty stuff here...
		}
		finally 
		{
			Cursor.Current = Cursors.Default;
		}
	}
	
If one wouldn't implement the cursor change like this, the cursor could 'hang' and stay 'WaitCursor'. To avoid this Try Finally coding style, I implemented IDisposable on the IProgressCursor like this (2-2):

	public ProgressCursor(Cursor originalCursor)
	{
		OriginalCursor = originalCursor;
	}

	~ProgressCursor()
	{
		Dispose();
	}

	public void Dispose()
	{
		End();
	}

	public void End()
	{
		Cursor.Current = OriginalCursor;
	}
	
How it works
------------

Creating a custom cursor 

Basically, all the 'heavy lifting' is done by two imported user32.dll methods. These can be found in the class UnManagedMethodWrapper (what would be the right name for this class?).

	public sealed class UnManagedMethodWrapper
	{
		[DllImport("user32.dll")]
		public static extern IntPtr CreateIconIndirect(ref IconInfo iconInfo);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetIconInfo(IntPtr iconHandle, ref IconInfo iconInfo);
	}
	
These methods are called in CreateCursor:

	private Cursor CreateCursor(Bitmap bmp, Point hotSpot)
	{
		//gets the 'icon-handle' of the bitmap
		//(~.net equivalent of bmp as Icon)
		IntPtr iconHandle = bmp.GetHicon();
		IconInfo iconInfo = new IconInfo();
		
		//fill the IconInfo structure with data from the iconHandle
		UnManagedMethodWrapper.GetIconInfo(iconHandle, ref iconInfo);
		
		//set hotspot coordinates
		iconInfo.xHotspot = hotSpot.X;
		iconInfo.yHotspot = hotSpot.Y;
		
		//indicate that this is a cursor, not an icon
		iconInfo.fIcon = false;
		
		//actually create the cursor
		iconHandle = 
		  UnManagedMethodWrapper.CreateIconIndirect(ref iconInfo);
		
		//return managed Cursor object
		return new Cursor(iconHandle);
	}
	
What does it (try to) solve
---------------------------

End users tend to have the impression to be waiting longer on a process with no progress visualization, then a process with progress indication. 