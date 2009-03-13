/**
 * HTC AudioManager Controller
 * By Daniel15 - http://d15.biz/
 */
using System;
using System.Text;
using System.Windows.Forms;

namespace HTCAudioManagerControl
{
	class main
	{
		const string APP_NAME = "HTC AudioManager Control";

		static int Main(string[] args)
		{
			AudioManagerControl amc;

			// Firstly make sure we know what we're doing
			if (args.Length == 0)
			{
				ShowUsage();
				return 1;
			}

			try
			{
				amc = new AudioManagerControl();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Could not connect to AudioManager - " + ex.Message, APP_NAME);
				return 2;
			}

			switch (args[0])
			{
				case "playpause":
					amc.PlayPause();
					break;
				case "stop":
					amc.Pause();
					break;
				case "prev":
					amc.Prev();
					break;
				case "next":
					amc.Next();
					break;
				default:
					MessageBox.Show("Invalid option: " + args[1], APP_NAME);
					break;
			}

			return 0;
		}

		static void ShowUsage()
		{
			string message = @"Valid options:
playpause - Play/Pause the current song
stop - Stop the current song
prev - Go back to the previous track
next - Go to the next track

By Daniel15 - http://d15.biz/";
			MessageBox.Show(message, APP_NAME);
		}
	}
}
