using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OmxPlayerGui
{
	public class MainClass
	{
		public static void Main (string[] args)
		{			
			frmSong song;
			if (args.Length > 0) {
				song = new frmSong (System.Environment.CommandLine);
				Application.Run (song);
			} else {
				song = new frmSong ("test.exe /home/pi/boot.bin/Shared/Mp3/Taboo - I Dream Of You Tonight.mp3");
				Application.Run (song);
			}
			return;
		}
	}
}

