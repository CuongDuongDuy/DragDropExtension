// Copyright (C) Microsoft Corporation. All Rights Reserved.
// This code released under the terms of the Microsoft Public License
// (Ms-PL, http://opensource.org/licenses/ms-pl.html).

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace VirtualFileDataObjectDemo
{
    public partial class Window1 : Window
    {
        // From Windows SDK header files
        private const string CFSTR_INETURLA = "UniformResourceLocator";

        public Window1()
        {
            InitializeComponent();

            // Attach to interesting events
            Text.MouseLeftButtonDown += new MouseButtonEventHandler(Text_MouseButtonDown);
            Text.MouseRightButtonDown += new MouseButtonEventHandler(Text_MouseButtonDown);
            TextUrl.MouseLeftButtonDown += new MouseButtonEventHandler(TextUrl_MouseButtonDown);
            TextUrl.MouseRightButtonDown += new MouseButtonEventHandler(TextUrl_MouseButtonDown);
            VirtualFile.MouseLeftButtonDown += new MouseButtonEventHandler(VirtualFile_MouseButtonDown);
            VirtualFile.MouseRightButtonDown += new MouseButtonEventHandler(VirtualFile_MouseButtonDown);
            TextUrlVirtualFile.MouseLeftButtonDown += new MouseButtonEventHandler(TextUrlVirtualFile_MouseButtonDown);
            TextUrlVirtualFile.MouseRightButtonDown += new MouseButtonEventHandler(TextUrlVirtualFile_MouseButtonDown);
        }

        private void Text_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var virtualFileDataObject = new VirtualFileDataObject();

            // Provide simple text (in the form of a NULL-terminated ANSI string)
            virtualFileDataObject.SetData(
                (short)(DataFormats.GetDataFormat(DataFormats.Text).Id),
                Encoding.Default.GetBytes("This is some sample text\0"));

            DoDragDropOrClipboardSetDataObject(e.ChangedButton, Text, virtualFileDataObject, DragDropEffects.Copy);
        }

        private void TextUrl_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var virtualFileDataObject = new VirtualFileDataObject();

            // Provide simple text and an URL in priority order
            // (both in the form of a NULL-terminated ANSI string)
            virtualFileDataObject.SetData(
                (short)(DataFormats.GetDataFormat(CFSTR_INETURLA).Id),
                Encoding.Default.GetBytes("http://blogs.msdn.com/delay/\0"));
            virtualFileDataObject.SetData(
                (short)(DataFormats.GetDataFormat(DataFormats.Text).Id),
                Encoding.Default.GetBytes("http://blogs.msdn.com/delay/\0"));

            DoDragDropOrClipboardSetDataObject(e.ChangedButton, TextUrl, virtualFileDataObject, DragDropEffects.Copy);
        }

        private void VirtualFile_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var virtualFileDataObject = new VirtualFileDataObject(
                null,
                (vfdo) =>
                {
                    if (DragDropEffects.Move == vfdo.PerformedDropEffect)
                    {
                        // Hide the element that was moved (or cut)
                        // BeginInvoke ensures UI operations happen on the right thread
                        Dispatcher.BeginInvoke((Action)(() => VirtualFile.Visibility = Visibility.Hidden));
                    }
                });

            // Provide a virtual file (generated on demand) containing the letters 'a'-'z'
            virtualFileDataObject.SetData(new VirtualFileDataObject.FileDescriptor[]
            {
                new VirtualFileDataObject.FileDescriptor
                {
                    Name = "Alphabet.txt",
                    Length = 26,
                    ChangeTimeUtc = DateTime.Now.AddDays(-1),
                    StreamContents = stream =>
                        {
                            var contents = Enumerable.Range('a', 26).Select(i => (byte)i).ToArray();
                            stream.Write(contents, 0, contents.Length);
                        }
                },
            });

            DoDragDropOrClipboardSetDataObject(e.ChangedButton, TextUrl, virtualFileDataObject, DragDropEffects.Move);
        }

        private void TextUrlVirtualFile_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = "cmd.exe",
                Arguments =
                    "/C copy /b \"C:\\Users\\Public\\Pictures\\Sample Pictures\\Desert.jpg\" \"C:\\Users\\Public\\Pictures\\Sample Pictures\\Desert1.jpg\""
            };
            process.StartInfo = startInfo;
            process.Start();

            //var virtualFileDataObject = new VirtualFileDataObject(
            //    // BeginInvoke ensures UI operations happen on the right thread
            //    (vfdo) => Dispatcher.BeginInvoke((Action)(() => BusyScreen.Visibility = Visibility.Visible)),
            //    (vfdo) => Dispatcher.BeginInvoke((Action)(() => BusyScreen.Visibility = Visibility.Collapsed)));

            // Provide a virtual file (downloaded on demand), its URL, and descriptive text
            //virtualFileDataObject.SetData(new VirtualFileDataObject.FileDescriptor[]
            //{
            //    new VirtualFileDataObject.FileDescriptor
            //    {
            //        Name = "DelaysBlog.xml",
            //        StreamContents = stream =>
            //            {
            //                using(var webClient = new WebClient())
            //                {
            //                    var data = webClient.DownloadData("http://blogs.msdn.com/delay/rss.xml");
            //                    stream.Write(data, 0, data.Length);
            //                }
            //            }
            //    },
            //});
            //virtualFileDataObject.SetData(
            //    (short)(DataFormats.GetDataFormat(CFSTR_INETURLA).Id),
            //    Encoding.Default.GetBytes("http://blogs.msdn.com/delay/rss.xml\0"));
            //virtualFileDataObject.SetData(
            //    (short)(DataFormats.GetDataFormat(DataFormats.Text).Id),
            //    Encoding.Default.GetBytes("[The RSS feed for Delay's Blog]\0"));

            //DoDragDropOrClipboardSetDataObject(e.ChangedButton, TextUrl, virtualFileDataObject, DragDropEffects.Copy);
        }

        private static void DoDragDropOrClipboardSetDataObject(MouseButton button, DependencyObject dragSource, VirtualFileDataObject virtualFileDataObject, DragDropEffects allowedEffects)
        {
            try
            {
                if (button == MouseButton.Left)
                {
                    // Left button is used to start a drag/drop operation
                    VirtualFileDataObject.DoDragDrop(dragSource, virtualFileDataObject, allowedEffects);
                }
                else if (button == MouseButton.Right)
                {
                    // Right button is used to copy to the clipboard
                    // Communicate the preferred behavior to the destination
                    virtualFileDataObject.PreferredDropEffect = allowedEffects;
                    Clipboard.SetDataObject(virtualFileDataObject);
                }
            }
            catch (COMException)
            {
                // Failure; no way to recover
            }
        }
    }
}
