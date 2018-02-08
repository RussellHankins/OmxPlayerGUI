using System;
using System.Diagnostics;
using System.Threading;

namespace OmxPlayerGui
{
	public class cPlayer
	{
		private Process _player;
		private const string PlayerPath = "/usr/bin/omxplayer";
		private const int DefaultVolume = 16;
		private int _volume;
		private Action _finishedPlaying;

		public cPlayer ()
		{
			_player = null;
			_volume = DefaultVolume;
			_finishedPlaying = null;
		}
		public void Dispose()
		{
			if (_player != null) {
				_player.Dispose ();
				_player = null;
			}
			return;
		}
		public string[] Modes()
		{
			string[] modeList;
			modeList = new String[4];
			modeList[1] = "OMXPlayer Audio Jack";
			modeList[2] = "OMXPlayer HDMI Port";
			modeList[3] = "OMXPlayer Default Output";
			return modeList;
		}
		public void PlaySong (string song,string mode,Action finishedPlaying)
		{
			ProcessStartInfo myProcessStartInfo;
			Thread waitingThread;
			ThreadStart start;
			string parameters;
			string vol;
			Action WaitingToStop = () => {
				bool exists;
				//bool adjustVolume;
				//int oldVolume;

				try
				{
					//adjustVolume = true;
					while(true)
					{
						Thread.MemoryBarrier();
						exists = ((_player != null) && (_finishedPlaying != null));
						Thread.MemoryBarrier();
						if (!exists)
						{
							break;
						}
						if (_player.WaitForExit (500))
						{
							break;
						}
						/*if (adjustVolume)
						{
							adjustVolume = false;
							oldVolume = _volume;
							_volume = DefaultVolume;
							Volume = oldVolume;
						}*/
					}
					if (_finishedPlaying != null)
					{
						_finishedPlaying ();
					}
				}catch(Exception)
				{
				}
				_player = null;
				waitingThread = null;
				return;
			};

			_finishedPlaying = finishedPlaying;
			_player = new System.Diagnostics.Process ();
			myProcessStartInfo = new System.Diagnostics.ProcessStartInfo (PlayerPath);
			_player.StartInfo = myProcessStartInfo;
			switch (mode) {
			case "OMXPlayer Audio Jack":
				{
					parameters = "-o local {0}\"{1}\"";
					break;
				}
			case "OMXPlayer HDMI Port":
				{
					parameters = "-o hdmi {0}\"{1}\"";
					break;
				}
			default:
				{
					parameters = "{0}\"{1}\"";
					break;
				}
			}
			vol = "";
			if (_volume != DefaultVolume) {
				vol = String.Format ("--vol {0} ", (_volume - DefaultVolume) * 300);
			}

			myProcessStartInfo.Arguments = String.Format (parameters, vol,song);
			myProcessStartInfo.RedirectStandardInput = true;
			myProcessStartInfo.RedirectStandardOutput = true;
			myProcessStartInfo.CreateNoWindow = true;
			myProcessStartInfo.UseShellExecute = false;

			_player.OutputDataReceived += new DataReceivedEventHandler(PlayerOutputHandler);
			_player.Start ();
			_player.BeginOutputReadLine ();
			if (_finishedPlaying != null)
			{
				start = new ThreadStart(WaitingToStop);
				waitingThread = new Thread(start);
				waitingThread.IsBackground = true;
				waitingThread.Priority = ThreadPriority.BelowNormal;
				waitingThread.Start();
			}

			//_player.WaitForExit (100);
			return;
		}
		public int MinVolume()
		{
			return 0;
		}
		public int MaxVolume()
		{
			return 31;
		}
		public int Volume 
		{ 
			get 
			{
				return _volume; 
			}
			set 
			{
				int newVolume = value;
				int currentVolume = _volume;

				while (newVolume > currentVolume) {
					IncreaseVolume ();
					currentVolume++;
					Thread.Sleep (50);
				}
				while (newVolume < currentVolume) {
					DecreaseVolume ();
					currentVolume--;
					Thread.Sleep (50);
				}
			}
		}
		private void PlayerOutputHandler(object sendingProcess
			, DataReceivedEventArgs outLine)
		{
			// Don't show Have a nice day ;)
			return;
		}
		public void Stop()
		{	
			if (_player != null) {
				SendKey ('q');
			}
			return;
		}
		public bool IsPlaying()
		{
			return ((_player != null) && (!_player.HasExited));
		}
		public void Pause()
		{
			SendKey (' ');
			return;
		}
		public void UnPause()
		{
			Pause ();
			return;
		}
		public void SkipToTheBeginning() {
			SendHex ("1B5B42");
			return;
		}
		public void IncreaseVolume()
		{
			if (_volume < MaxVolume ()) {
				_volume++;
				SendKey ('+');
			}
			return;
		}
		public void DecreaseVolume()
		{
			if (_volume > MinVolume ()) {
				_volume--;
				SendKey ('-');
			}
			return;
		}
		public void SkipForward()
		{
			// http://subupi.blogspot.com/2012/10/piping-across-shell-sessions-to-control.html
			SendHex ("1B5B43");
			return;
		}
		public void SkipBack()
		{
			SendHex ("1B5B44");
			return;
		}
		/// <summary>
		/// Sends several keys as hex to OmxPlayer.
		/// </summary>
		/// <param name="hex">Hex.</param>
		private void SendHex(string hex)
		{
			// 386-290-9553
			var output = new System.Text.StringBuilder();
			System.Func<char,int> GetHex = (x) => {
				var value = "0123456789ABCDEFabcdef".IndexOf(x);
				if (value > 15) {
					value -= 6;
				}
				return value; // http://subupi.blogspot.com/2012/10/piping-across-shell-sessions-to-control.html
			};

			for (int loop = 0; loop < hex.Length; loop += 2) {
				output.Append((char)(GetHex(hex[loop]) * 16 + GetHex(hex[loop+1])));
			}
			try
			{
				if (_player != null) {
					_player.StandardInput.Write(output.ToString());
				}
			} catch(Exception) {
				//System.Windows.Forms.MessageBox.Show (ex.Message);
			}
			return;
		}
		/// <summary>
		/// Sends one key to OmxPlayer.
		/// </summary>
		/// <param name="key">Key.</param>
		private void SendKey(char key)
		{
			try
			{
				if (_player != null) {
					_player.StandardInput.Write(key);
				}
			} catch(Exception) {
				//System.Windows.Forms.MessageBox.Show (ex.Message);
			}
			return;
		}
	}
}

