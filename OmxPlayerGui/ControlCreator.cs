using System;
using System.Windows.Forms;

namespace OmxPlayerGui
{
	public static class ControlCreator
	{
		public static void Add(this Control.ControlCollection collection,out TextBox box,string id, string text, int left, int top, int width, int height)
		{
			box = new TextBox();
			box.Text = text;
			AddControl (collection,box,id,left,top,width,height);
			return;
		}
		public static void Add (this Control.ControlCollection collection,out ContextMenu menu, string id)
		{
			menu = new ContextMenu ();
			menu.Name = id;
		}
		public static void Add(this Control.ControlCollection collection,out ContextMenu menu,string id, MenuItem[] menuitems)
		{
			menu = new ContextMenu (menuitems);
			menu.Name = id;
			return;
		}
		public static void Add(this Control.ControlCollection collection,out TreeView box
		                                   , string id, int left, int top, int width, int height)
		{
			box = new TreeView();
			AddControl(collection,box,id,left,top,width,height);
			return;
		}
		public static void Add(this Control.ControlCollection collection,out GroupBox box,string id, string text, int left, int top, int width, int height)
		{
			box = new GroupBox();
			box.Text = text;
			AddControl (collection,box,id,left,top,width,height);
			return;
		}
		public static void Add(this Control.ControlCollection collection,out Button box,string id, string text, int left, int top, int width, int height)
		{
			box = new Button();
			box.Text = text;
			AddControl (collection,box,id,left,top,width,height);
			return;
		}
		public static void Add(this Control.ControlCollection collection,out Label box,string id, string text, int left, int top, int width, int height)
		{
			box = new Label();
			box.Text = text;
			AddControl (collection,box,id,left,top,width,height);
			return;
		}
		public static void Add(this Control.ControlCollection collection,out CheckBox box,string id, string text, int left, int top, int width, int height)
		{
			box = new CheckBox();
			box.Text = text;
			AddControl (collection,box,id,left,top,width,height);
			return;
		}
		public static void Add(this Control.ControlCollection collection,out ListBox box,string id, int left, int top, int width, int height)
		{
			box = new ListBox();
			AddControl (collection,box,id,left,top,width,height);
			return;
		}
		public static void Add(this Control.ControlCollection collection,out CheckedListBox box,string id, int left, int top, int width, int height)
		{
			box = new CheckedListBox();
			AddControl (collection,box,id,left,top,width,height);
			return;
		}

		public static void Add(this Control.ControlCollection collection,out DataGrid box,string id, int left, int top, int width, int height)
		{
			box = new DataGrid();
			AddControl (collection,box,id,left,top,width,height);
			return;
		}
		public static void Add(this Control.ControlCollection collection,out DataGridView box,string id, int left, int top, int width, int height)
		{
			box = new DataGridView();
			AddControl (collection,box,id,left,top,width,height);
			return;
		}

		private static void AddControl(Control.ControlCollection collection,Control box,string id, int left, int top, int width, int height)
		{
			box.Name = id;
			box.Left = left;
			box.Top = top;
			box.Width = width;
			box.Height = height;
			collection.Add(box);
			return;
		}
		
	}
}

