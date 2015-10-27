using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.IO;

namespace ExplorerDragAndDrop
{
	/// <summary>
	/// Drag and drop, cut/copy & paste with Explorer sample.
	/// 
	/// Paul Tallett, 30-Apr-06 (http://blogs.msdn.com/ptallett/)
	/// 
	/// Use the code any way you like - no warrenties given.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader nameColumnHeader;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem cutMenuItem;
		private System.Windows.Forms.MenuItem copyMenuItem;
		private System.Windows.Forms.MenuItem pasteMenuItem;
		private System.Windows.Forms.MenuItem refreshMenuItem;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		const int ALT = 32;
		const int CTRL = 8;
		const int SHIFT = 4;

		public Form1()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.listView1 = new System.Windows.Forms.ListView();
			this.nameColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.cutMenuItem = new System.Windows.Forms.MenuItem();
			this.copyMenuItem = new System.Windows.Forms.MenuItem();
			this.pasteMenuItem = new System.Windows.Forms.MenuItem();
			this.refreshMenuItem = new System.Windows.Forms.MenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.AllowDrop = true;
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.nameColumnHeader});
			this.listView1.ContextMenu = this.contextMenu1;
			this.listView1.Location = new System.Drawing.Point(8, 8);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(512, 288);
			this.listView1.TabIndex = 0;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.DragOver += new System.Windows.Forms.DragEventHandler(this.listView1_DragOver);
			this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
			this.listView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView1_ItemDrag);
			// 
			// nameColumnHeader
			// 
			this.nameColumnHeader.Text = "Name";
			this.nameColumnHeader.Width = 483;
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.cutMenuItem,
																						 this.copyMenuItem,
																						 this.pasteMenuItem,
																						 this.refreshMenuItem});
			// 
			// cutMenuItem
			// 
			this.cutMenuItem.Index = 0;
			this.cutMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.cutMenuItem.Text = "Cut";
			this.cutMenuItem.Click += new System.EventHandler(this.cutMenuItem_Click);
			// 
			// copyMenuItem
			// 
			this.copyMenuItem.Index = 1;
			this.copyMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.copyMenuItem.Text = "Copy";
			this.copyMenuItem.Click += new System.EventHandler(this.copyMenuItem_Click);
			// 
			// pasteMenuItem
			// 
			this.pasteMenuItem.Index = 2;
			this.pasteMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.pasteMenuItem.Text = "Paste";
			this.pasteMenuItem.Click += new System.EventHandler(this.pasteMenuItem_Click);
			// 
			// refreshMenuItem
			// 
			this.refreshMenuItem.Index = 3;
			this.refreshMenuItem.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.refreshMenuItem.Text = "Refresh";
			this.refreshMenuItem.Click += new System.EventHandler(this.refreshMenuItem_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(8, 304);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(512, 32);
			this.label1.TabIndex = 1;
			this.label1.Text = "Drag and drop these files into/out of explorer. BE CAREFUL! It really does move/c" +
				"opy the files. Use CTRL + SHIFT to modify the behaviour.";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(8, 344);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(512, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "You can also use right click cut/copy && paste.";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(528, 366);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.label2);
			this.Name = "Form1";
			this.Text = "Drag and drop files into Explorer";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private string homeFolder = "";
		private string homeDisk = "";
		private FileSystemWatcher fsw;

		private void Form1_Load(object sender, System.EventArgs e)
		{
			// Get the bin/Debug folder, then go up two and find our "Files" folder
			string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			folder = Path.GetDirectoryName(folder);
			folder = Path.GetDirectoryName(folder);
			homeFolder =  folder + "\\Files";
			homeDisk = Path.GetPathRoot(homeFolder).ToUpper();		// C:\ or D:\
			this.Text = "Files in " + homeFolder;

			RefreshView();

			// Need to watch the folder for cut & paste operations as we get no notification when paste happens
			fsw = new FileSystemWatcher(homeFolder, "*.*");
			fsw.Changed += new FileSystemEventHandler(fsw_Changed);
			fsw.Deleted += new FileSystemEventHandler(fsw_Changed);
			fsw.Created += new FileSystemEventHandler(fsw_Changed);
			fsw.EnableRaisingEvents = true;
		}

		// FileSystemWatcher calls on another thread
		private delegate void ChangeHandler(object sender, FileSystemEventArgs e);

		/// <summary>
		/// Occurs when files change in our folder. Refresh if needed
		/// </summary>
		private void fsw_Changed(object sender, FileSystemEventArgs e)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new ChangeHandler(fsw_Changed), new object[] {sender, e});
				return;
			}

			string[] files = Directory.GetFiles(homeFolder);
			if (files.Length != listView1.Items.Count)			// You probably want to do something better
			{
				listView1.Items.Clear();
				foreach (string file in files)
				{
					listView1.Items.Add(file);
				}
			}
		}

		/// <summary>
		/// Routine to refresh the display
		/// </summary>
		private void RefreshView()
		{
			listView1.Items.Clear();
			string[] files = Directory.GetFiles(homeFolder);
			foreach (string file in files)
			{
				listView1.Items.Add(file);
			}
		}

		/// <summary>
		/// Routine to get the current selection from the listview
		/// </summary>
		/// <returns>Seletced items or null if no selection</returns>
		private string[] GetSelection()
		{
			if (listView1.SelectedItems.Count == 0)
				return null;

			string[] files = new string[listView1.SelectedItems.Count];
			int i = 0;
			foreach (ListViewItem item in listView1.SelectedItems)
			{
				files[i++] = item.Text;
			}
			return files;
		}

#region Drag and drop
		/// <summary>
		/// Called when we start dragging an item out of our listview
		/// </summary>
		private void listView1_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			string[] files = GetSelection();
			if(files != null)
			{
				DoDragDrop(new DataObject(DataFormats.FileDrop, files), DragDropEffects.Copy | DragDropEffects.Move /* | DragDropEffects.Link */);
				RefreshView();
			}
		}

		/// <summary>
		/// Called when someone drags something over our listview
		/// </summary>
		private void listView1_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			// Determine whether file data exists in the drop data. If not, then
			// the drop effect reflects that the drop cannot occur.
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) 
			{
				e.Effect = DragDropEffects.None;
				return;
			}

			// Set the effect based upon the KeyState.
			// Can't get links to work - Use of Ole1 services requiring DDE windows is disabled
//			if ((e.KeyState & (CTRL | ALT)) == (CTRL | ALT) &&
//				(e.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link) 
//			{
//				e.Effect = DragDropEffects.Link;
//			}
//			
//			else if ((e.KeyState & ALT) == ALT && 
//				(e.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link) 
//			{
//				e.Effect = DragDropEffects.Link;
//
//			} 
//			else
			if ((e.KeyState & SHIFT) == SHIFT && 
				(e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move) 
			{
				e.Effect = DragDropEffects.Move;

			} 
			else if ((e.KeyState & CTRL) == CTRL && 
				(e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy) 
			{
				e.Effect = DragDropEffects.Copy;
			} 
			else if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)  
			{
				// By default, the drop action should be move, if allowed.
				e.Effect = DragDropEffects.Move;

				// Implement the rather strange behaviour of explorer that if the disk
				// is different, then default to a COPY operation
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (files.Length > 0 && !files[0].ToUpper().StartsWith(homeDisk) &&			// Probably better ways to do this
				(e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy) 
					e.Effect = DragDropEffects.Copy;
			} 
			else
				e.Effect = DragDropEffects.None;

			// This is an example of how to get the item under the mouse
			Point pt = listView1.PointToClient(new Point(e.X, e.Y));
			ListViewItem itemUnder = listView1.GetItemAt(pt.X, pt.Y);
		}

		/// <summary>
		/// Somebody dropped something on our listview - perform the action
		/// </summary>
		private void listView1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			// Can only drop files, so check
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) 
			{
				return;
			}

			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string file in files)
			{
				string dest = homeFolder + "\\" + Path.GetFileName(file);
				bool isFolder = Directory.Exists(file);
				bool isFile = File.Exists(file);
				if (!isFolder && !isFile)				// Ignore if it doesn't exist
					continue;

				try
				{
					switch(e.Effect)
					{
						case DragDropEffects.Copy:
							if(isFile)					// TODO: Need to handle folders
								File.Copy(file, dest, false);
							break;
						case DragDropEffects.Move:
							if (isFile)
								File.Move(file, dest);
							break;
						case DragDropEffects.Link:		// TODO: Need to handle links
							break;
					}
				}
				catch(IOException ex)
				{
					MessageBox.Show(this, "Failed to perform the specified operation:\n\n" + ex.Message, "File operation failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
			}

			RefreshView();
		}
#endregion

#region Cut/Copy & Paste

		/// <summary>
		/// Cut context menu option
		/// </summary>
		private void cutMenuItem_Click(object sender, System.EventArgs e)
		{
			CopyToClipboard(true);
		}

		/// <summary>
		/// Copy context menu option
		/// </summary>
		private void copyMenuItem_Click(object sender, System.EventArgs e)
		{
			CopyToClipboard(false);
		}

		/// <summary>
		/// Write files to clipboard (from http://blogs.wdevs.com/idecember/archive/2005/10/27/10979.aspx)
		/// </summary>
		/// <param name="cut">True if cut, false if copy</param>
		void CopyToClipboard(bool cut)
		{
			string[] files = GetSelection();
			if(files != null)
			{
				IDataObject data = new DataObject(DataFormats.FileDrop, files);
				MemoryStream memo = new MemoryStream(4);
				byte[] bytes = new byte[]{(byte)(cut ? 2 : 5), 0, 0, 0};
				memo.Write(bytes, 0, bytes.Length);
				data.SetData("Preferred DropEffect", memo);
				Clipboard.SetDataObject(data);
			}
		}

		/// <summary>
		/// Paste context menu option
		/// </summary>
		private void pasteMenuItem_Click(object sender, System.EventArgs e)
		{
			IDataObject data = Clipboard.GetDataObject();
			if (!data.GetDataPresent(DataFormats.FileDrop))
				return;

			string[] files = (string[])data.GetData(DataFormats.FileDrop);
			MemoryStream stream = (MemoryStream)data.GetData("Preferred DropEffect", true);
			int flag = stream.ReadByte();
			if (flag != 2 && flag != 5)
				return;
			bool cut = (flag == 2);
			foreach (string file in files)
			{
				string dest = homeFolder + "\\" + Path.GetFileName(file);
				try
				{
					if(cut)
						File.Move(file, dest);
					else
						File.Copy(file, dest, false);
				}
				catch(IOException ex)
				{
					MessageBox.Show(this, "Failed to perform the specified operation:\n\n" + ex.Message, "File operation failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
			}

			RefreshView();
		}
#endregion

		/// <summary>
		/// Refresh menu option
		/// </summary>
		private void refreshMenuItem_Click(object sender, System.EventArgs e)
		{
			RefreshView();
		}
	}
}
