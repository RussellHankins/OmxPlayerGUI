using System;
using System.Windows.Forms;

namespace OmxPlayerGui
{
	public class frmSong:Form
	{
		string _song;
		Button btnFirst;
		Button btnBack;
		Button btnNext;
		Button btnPause;
		Button btnStop;
		Button btnPlay;
		Button btnQuit;
		Button btnLouder;
		Button btnSofter;
		Label lblInfo;
		Label lblSong;
		cPlayer _player;
		DateTime? _ignoreButton;
		bool _closeWhenSongFinishes; // If the song plays when the program starts and there's no interaction, close when the program stops.
		public frmSong (string song)
		{
			ShouldIgnoreButton = false;
			_song = FindSong(song);
			InitializeComponent ();
			_closeWhenSongFinishes = false;
			if (!String.IsNullOrEmpty (_song)) {
				_closeWhenSongFinishes = true;
				PlaySong ();
			}
		}
		/// <summary>
		/// Extracts the song name from the command line.
		/// </summary>
		/// <returns>The song.</returns>
		/// <param name="song">Command line</param>
		private string FindSong(string song)
		{
			int search;
			const char DOUBLE_QUOTE = '"';
			search = song.IndexOf (".exe ", StringComparison.OrdinalIgnoreCase);
			song = song.Substring (search + 5);
			// Remove double quotes.
			if ((!String.IsNullOrEmpty(song)) && (song[0] == DOUBLE_QUOTE))
			{
				song = song.Substring (1);
			}
			if ((!String.IsNullOrEmpty(song)) && (song[song.Length-1] == DOUBLE_QUOTE))
			{
				song = song.Substring (0, song.Length - 1);
			}
			//song = "/home/pi/boot.bin/Shared/Mp3/ZZ Top - Cheap Sunglasses.mp3";
			return song;
		}
		private void InitializeComponent ()
		{
			Width = 440;
			Height = 160;
			Text = "OmxPlayer " + System.IO.Path.GetFileName(_song);
			ControlCreator.Add (Controls, out btnFirst, "btnFirst", "|<", 10, 10, 50, 25);
			btnFirst.AccessibleDescription = "Move to the beginning of the song (Up Arrow)";
			btnFirst.Click += btnFirst_Click;
			AddKeyEvents (btnFirst);

			ControlCreator.Add (Controls, out btnBack, "btnBack", "<-", 70, 10,50, 25);
			btnBack.AccessibleDescription = "Move back 30 seconds (Left Arrow)";
			btnBack.Click += btnBack_Click;
			AddKeyEvents (btnBack);

			ControlCreator.Add (Controls, out btnPlay, "btnPlay", "|>", 130, 10, 50, 25);
			btnPlay.AccessibleDescription = "Play (Space)";
			btnPlay.Click += btnPlay_Click;
			AddKeyEvents (btnPlay);

			ControlCreator.Add (Controls, out btnPause, "btnPause", "||", 200, 10, 50, 25);
			btnPause.AccessibleDescription = "Pause (Space)";
			btnPause.Click += btnPause_Click;
			AddKeyEvents (btnPause);

			ControlCreator.Add (Controls, out btnStop, "btnStop", "[]", 270, 10, 50, 25);
			btnStop.AccessibleDescription = "Stop (Q)";
			btnStop.Click += btnStop_Click;
			AddKeyEvents (btnStop);

			ControlCreator.Add (Controls, out btnNext, "btnNext", "->", 340, 10, 50, 25);
			btnNext.AccessibleDescription = "Move forward 30 seconds (Right Arrow)";
			btnNext.Click += btnNext_Click;
			AddKeyEvents (btnNext);

			ControlCreator.Add (Controls, out btnLouder, "btnLouder","Louder",10, 45, 100, 25);
			btnLouder.AccessibleDescription = "Increases volume (+)";
			btnLouder.Click += btnLouder_Click;
			AddKeyEvents (btnLouder);

			ControlCreator.Add (Controls, out btnSofter, "btnSofter", "Softer", 120, 45, 100, 25);
			btnSofter.AccessibleDescription = "Decreases volume (-)";
			btnSofter.Click += btnSofter_Click;
			AddKeyEvents (btnSofter);

			ControlCreator.Add (Controls, out btnQuit, "btnQuit", "X", 230, 45, 50, 25);
			btnQuit.AccessibleDescription = "Exits the program (Esc)";
			btnQuit.Click += btnQuit_Click;
			AddKeyEvents (btnQuit);

			ControlCreator.Add (Controls, out lblInfo, "lblInfo", "", 10, 75, Width - 20, 25);
			AddMouseEvents (btnFirst);
			AddMouseEvents (btnBack);
			AddMouseEvents (btnPlay);
			AddMouseEvents (btnPause);
			AddMouseEvents (btnStop);
			AddMouseEvents (btnNext);
			AddMouseEvents (btnQuit);
			AddMouseEvents (btnLouder);
			AddMouseEvents (btnSofter);

			ControlCreator.Add (Controls, out lblSong, "lblSong", "", 10, 110, Width - 20, 25);
			lblSong.Text = _song;

			this.FormClosing += Form_Closing;
			//this.KeyPress += Form_KeyPress;
			//this.PreviewKeyDown += Form_KeyDown;
			//this.KeyDown += Form_KeyDown;
			//myToolTip = new ToolTip ();
			//this.Controls.Add (myToolTip,"myToolTip");
			return;
		}

		void btnSofter_Click (object sender, EventArgs e)
		{
			if (ShouldIgnoreButton) {
				return;
			}
			if (IsPlaying) {
				_player.DecreaseVolume ();
			}
		}

		void btnLouder_Click (object sender, EventArgs e)
		{
			if (ShouldIgnoreButton) {
				return;
			}
			if (IsPlaying) {
				_player.IncreaseVolume ();
			}
		}
		/*protected override bool IsInputChar(char charCode)
		{
			if (charCode == 32) {
				return true;
			} else {
				return base.IsInputChar (charCode);
			}
		}
		protected override bool IsInputKey(Keys keyData)
		{
			if (keyData == Keys.Down || keyData == Keys.Up ||
				keyData == Keys.Left || keyData == Keys.Right)
			{
				return true;
			}
			else
			{
				return base.IsInputKey(keyData);
			}
		}*/

		private void AddKeyEvents(Button btn)
		{
			//btn.KeyDown += Form_KeyDown;
			//btn.KeyPress += Form_KeyPress;
			btn.PreviewKeyDown += Form_PreviewKeyDown;
		}

		private void btnBack_Click(Object sender,EventArgs e)
		{
			if (ShouldIgnoreButton) {
				return;
			}
			_closeWhenSongFinishes = false;
			if (IsPlaying) {
				_player.SkipBack ();
			}
		}
		private void btnNext_Click(Object sender,EventArgs e)
		{
			if (ShouldIgnoreButton) {
				return;
			}
			_closeWhenSongFinishes = false;
			if (IsPlaying) {
				_player.SkipForward ();
			}
		}
		private void Form_PreviewKeyDown(Object sender,PreviewKeyDownEventArgs e)
		{
			try
			{
				switch (e.KeyCode) {
				case Keys.Right:
					{					
						_closeWhenSongFinishes = false;
						if (IsPlaying) {
							_player.SkipForward ();
							e.IsInputKey = true;
						}
						break;
					}
				case Keys.Left:
					{					
						_closeWhenSongFinishes = false;
						if (IsPlaying) {
							_player.SkipBack ();
							e.IsInputKey = true;
						}
						break;
					}
				case Keys.Q:
					{
						e.IsInputKey = true;
						if (IsPlaying)
						{
							_closeWhenSongFinishes = false;
							Stop();
						}
						break;
					}
				case Keys.Escape:
					{
						e.IsInputKey = true;
						if (IsPlaying) {
							_closeWhenSongFinishes = false;
							Stop ();
						}
						Close ();
						break;
					}
				case Keys.Space:
					{
						e.IsInputKey = true;
						ShouldIgnoreButton = true;
						if (IsPlaying) {
							_player.Pause ();
						} else {
							PlaySong ();
						}
						break;
					}
				case Keys.OemMinus:
				case Keys.VolumeDown:
					{						
						e.IsInputKey = true;
						if (IsPlaying) {
							_player.DecreaseVolume ();
						}
						break;
					}
				case Keys.Oemplus:
				case Keys.VolumeUp:
					{
						e.IsInputKey = true;
						if (IsPlaying) {
							_player.IncreaseVolume ();
						}
						break;
					}
				case Keys.VolumeMute:
					{
						// Not implemented.
						break;
					}
				case Keys.Up:
					{
						e.IsInputKey = true;
						if (IsPlaying) {
							_player.SkipToTheBeginning ();
						}
						break;
					}
				case Keys.Down:
					{
						e.IsInputKey = true;
						Close ();
						break;
					}
				}
			} catch(Exception) {
			}
			return;
		}
		private bool IsPlaying
		{
			get {
				return ((_player != null) && (_player.IsPlaying ()));
			}
		}
		private void btnPause_Click(Object sender,EventArgs e)
		{
			if (ShouldIgnoreButton) {
				return;
			}
			if (IsPlaying)
			{
				_player.Pause ();
			}
		}
		private void btnQuit_Click(Object sender,EventArgs e)
		{
			try
			{
				if (ShouldIgnoreButton) {
					return;
				}
				_closeWhenSongFinishes = false;
				Stop ();
				Close ();
			} catch(Exception) {
			}
		}
		private void Form_Closing(Object sender,FormClosingEventArgs e)
		{
			try
			{
				Stop ();
			} catch(Exception) {
			}
		}
		private void btnFirst_Click(Object sender,EventArgs e)
		{
			try
			{
				if (ShouldIgnoreButton) {
					return;
				}
				if (IsPlaying) {
					_player.SkipToTheBeginning ();
				}
			} catch(Exception) {
			}
			return;
		}
		private void btnPlay_Click(Object sender,EventArgs e)
		{
			try
			{
				if (ShouldIgnoreButton) {
					return;
				}
				_closeWhenSongFinishes = false;
				PlaySong ();
			} catch(Exception) {
			}
			return;
		}
		private void PlaySong()
		{
			if (_player == null) {
				_player = new cPlayer ();
				_player.PlaySong (_song, "OMXPlayer Audio Jack", FinishedPlaying);
			} else {
				_player.Pause ();
			}
			return;
		}
		private void btnStop_Click(Object sender,EventArgs e)
		{
			try
			{
				if (ShouldIgnoreButton) {
					return;
				}
				_closeWhenSongFinishes = false;
				Stop ();
			} catch(Exception) {
			}
		}
		private void Stop()
		{			
			if (IsPlaying) {
				_player.Stop ();
				_player = null;
			}
			return;
		}
		private void FinishedPlaying()
		{
			_player = null;
			if (_closeWhenSongFinishes) {				
				Close ();
			}
		}
		private void AddMouseEvents(Button buttonToAddEventsTo)
		{
			buttonToAddEventsTo.MouseEnter += MouseEnter_Handler;
			buttonToAddEventsTo.MouseLeave += MouseLeave_Handler;
			return;
		}
		private void MouseEnter_Handler(Object sender,EventArgs e)
		{
			Button mouseButton = (Button)sender;
			lblInfo.Text = mouseButton.AccessibleDescription;
		}
		private void MouseLeave_Handler(Object sender,EventArgs e)
		{
			lblInfo.Text = "";
		}
		private bool ShouldIgnoreButton
		{
			get {
				if (_ignoreButton.HasValue) {
					if (DateTime.Now > _ignoreButton.Value) {
						_ignoreButton = null;
					}
				}					
				return (_ignoreButton.HasValue);
			}
			set {
				if (value) {
					// Ignore all buttons for the next 300 miliseconds.
					_ignoreButton = DateTime.Now.AddMilliseconds (300);
				} else {
					_ignoreButton = null;
				}
			}
		}
	}
}

