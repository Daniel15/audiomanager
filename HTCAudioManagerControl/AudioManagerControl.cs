/**
 * HTC AudioManager Controll
 * By Daniel15 - http://d15.biz/
 */
using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;

namespace HTCAudioManagerControl
{
	#region HTCAudioManagerException
	public class HTCAudioManagerException : Exception
	{
		public HTCAudioManagerException() { }
		public HTCAudioManagerException(string message) : base(message) { }
		public HTCAudioManagerException(string message, Exception inner) : base(message, inner) { }
	}
	#endregion
	
	class AudioManagerControl
	{
		[DllImport("coredll.dll", EntryPoint = "FindWindowW", SetLastError = true)]
		private static extern IntPtr FindWindowCE(string lpClassName, string lpWindowName);

		[DllImport("coredll.dll", CharSet = CharSet.Auto, SetLastError = false)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Constants for commands
		private const int WM_USER_SONG = 0x401;
		private enum CommandType {next = 0x5, prev = 0x7, play = 0x12, pause = 0x3}

		// Constants for registry stuff
		// TODO: Find out the other statuses, and if this is a bitmask
		private enum AudioControllerStatus {paused = 5} 

		private IntPtr _handle;

		/// <summary>
		/// Constructor for AudioManagerController. We ttry to get the window
		/// handle of the internal AudioManager window.
		/// </summary>
		public AudioManagerControl()
		{
			try
			{
				_handle = FindWindowCE("AUDIO_MANAGER_ENG", null);
				if (_handle == IntPtr.Zero)
					throw new Exception("Cannot find AudioManager window");

				Debug.WriteLine(String.Format("Found AudioManager window {0}", _handle), "HTCAudioManagerControl");
			}
			catch (Exception e)
			{
				throw new HTCAudioManagerException("Could not connect to AudioManager", e);
			}
		}

		/// <summary>
		/// Send a command to AudioManager
		/// </summary>
		/// <param name="command">Command to send</param>
		private void _SendCommand(CommandType command)
		{
			Debug.WriteLine(String.Format("Sending command {0} ({1})", command, (int)command), "HTCAudioManagerControl");
			SendMessage(_handle, WM_USER_SONG, new IntPtr((int)command), IntPtr.Zero);
		}

		public void Play()
		{
			_SendCommand(CommandType.play);
		}

		public void Pause()
		{
			_SendCommand(CommandType.pause);
		}

		public void Next()
		{
			_SendCommand(CommandType.next);
		}

		public void Prev()
		{
			_SendCommand(CommandType.prev);
		}

		public void PlayPause()
		{
			// Are we paused?
			RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"Software\HTC\AudioManager_Eng\information");
			AudioControllerStatus status = (AudioControllerStatus)regKey.GetValue("status");
			Debug.WriteLine(String.Format("Current status = {0}", status), "HTCAudioManagerControl");

			if (status == AudioControllerStatus.paused)
				Play();
			else
				Pause();

		}
	}
}
