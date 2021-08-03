using System;
using System.Linq;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing;

using GTA;
using GTA.Math;
using GTA.Native;

namespace _360Pictures
{
    public class Pictures : Script
    {
		private readonly string basePath = "360Pictures";

		private readonly int cFov = 50;
		private readonly bool cPlayerVisible = false;
		private readonly bool cSaveCoords = true;
		private bool autoScreenshoting = false;
		private readonly float takeScreenshotsEveryXmeters = 30f;

		private readonly Ped character;
		private Vector3 lastScreenshotPosition;

		public Pictures()
        {
			// Executes at start time
			this.Tick += OnTick;
			this.KeyUp += OnKeyUp;
			this.KeyDown += OnKeyDown;

			character = Game.Player.Character;
		}

        private void OnTick(object sender, EventArgs e)
        {
			// Executes every Frame
			Vector3 currentPosition = character.Position;

			if (this.autoScreenshoting && Vector3.Distance(currentPosition, lastScreenshotPosition) > takeScreenshotsEveryXmeters)
			{
				lastScreenshotPosition = currentPosition;

				Take360Screenshot();
			}

		}
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            // Executes if the Player releases a Key

        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
			// Executes if the Player presses a Key
			if (e.KeyCode == Keys.NumPad1)
			{
				Take360Screenshot();
			}
			else if (e.KeyCode == Keys.NumPad2)
			{
				if (this.autoScreenshoting)
				{
					this.autoScreenshoting = false;
					UI.Notify("Auto 360° Screenshots deactivated.");
				}
				else
				{
					this.autoScreenshoting = true;
					lastScreenshotPosition = character.Position;

					UI.Notify("Auto 360° Screenshots activated. [Every " + takeScreenshotsEveryXmeters.ToString() + " meters.]");
				}
			}else if (e.KeyCode == Keys.NumPad3)
			{
				Vector3 position = character.Position;
				Vector3 rotation = character.Rotation;
				UI.Notify(position.X.ToString() + " " + position.Y.ToString() + " " + position.Z.ToString() + " " + rotation.X.ToString() + " " + rotation.Y.ToString() + " " + rotation.Z.ToString());
			}
		}

		public void Take360Screenshot()
        {
			string screnshotPath;
			Vector3 position = character.Position;
			Vector3 rotation = character.Rotation;
			//Vector3 rotation = new Vector3(0, 0, 0);

			#region Create base folder if it doesn't exist yet
			if (!Directory.Exists(basePath))
			{
				Directory.CreateDirectory(basePath);
			}
			#endregion

			#region Choose the Path where screenshots should be saved
			if (this.cSaveCoords)
			{
				screnshotPath = Path.Combine(basePath, position.X.ToString() + "_" + position.Y.ToString() + "_" + position.Z.ToString() + "_" + rotation.X.ToString() + "_" + rotation.Y.ToString() + "_" + rotation.Z.ToString());
			}
			else
			{
				screnshotPath = Path.Combine(basePath, DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
			}
			#endregion

			#region Create the screenshot folder if it doesn't exist yet
			if (!Directory.Exists(screnshotPath))
			{
				Directory.CreateDirectory(screnshotPath);
			}
			#endregion

			Game.TimeScale = 0f; // Pause Game Time
			Game.Player.LastVehicle.IsVisible = this.cPlayerVisible; // Make Vehicle Insible
			Game.Player.Character.IsVisible = this.cPlayerVisible; // Make Player Insible
			this.HideHud(true); // Hide the HUD


			Camera camera1 = World.CreateCamera(position, rotation, 50);
			camera1.Position = position;

			for (float i = 0f; i < 5f; i++)
            {
				camera1.Rotation = new Vector3(0, 0, 78.9f * i);
				World.RenderingCamera = camera1;
				Script.Wait(50);
				this.TakeScreenShot().Save(Path.Combine(screnshotPath, string.Format("p1-{0}.jpg", i + 1) ), ImageFormat.Jpeg);
			}

			
			#region Make the Screenshots
			Camera camera = World.CreateCamera(position, rotation, this.cFov);

			for (int i = 1; i < 51; i++)
            {
				if (new[] { 1, 11, 21, 31, 41 }.Contains(i))
                {
					camera.Position = position;

					float shiftedX = 0;

					switch (i)
                    {
						case 1:
							shiftedX = rotation.X + 80f;
							break;
						case 11:
							shiftedX = rotation.X + 40f;
							break;
						case 21:
							// Do nothing
							break;
						case 31:
							shiftedX = rotation.X - 40f;
							break;
						case 41:
							shiftedX = rotation.X - 80f;
							break;
						default:
							throw new ArgumentException("Invalid shift case");
					}

					camera.Rotation = new Vector3(shiftedX, rotation.Y, rotation.Z);
				}
				else
                {
					camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
                }

				World.RenderingCamera = camera;
				Script.Wait(15);
				this.TakeScreenShot().Save(Path.Combine(screnshotPath, string.Format("p{0}.jpg", i) ), ImageFormat.Jpeg);
			}
			#endregion
			


			// Return Camera back to default position
			World.DestroyAllCameras();
			World.RenderingCamera = null;

			// Reverse things done for screenshoting
			Game.TimeScale = 1f; //Un-Pause Game
			Game.Player.LastVehicle.IsVisible = !this.cPlayerVisible; // Make Vehicle Visible again
			Game.Player.Character.IsVisible = !this.cPlayerVisible; // Make Player Visible again
			this.HideHud(false); // Un-Hide the HUD

			//UI.Notify("360° Screenshot saved.");
		}


		public Bitmap TakeScreenShot()
        {
            Size blockRegionSize = new Size(Game.ScreenResolution.Width, Game.ScreenResolution.Height);
            Bitmap bitmap = new Bitmap(Game.ScreenResolution.Width, Game.ScreenResolution.Height);
            Graphics.FromImage(bitmap).CopyFromScreen(new Point(0, 0), new Point(0, 0), blockRegionSize);
            return bitmap;
        }

		public void HideHud(bool hideOrShow)
        {
			Function.Call(Hash.DISPLAY_RADAR, !hideOrShow);
			Function.Call(Hash.HIDE_HELP_TEXT_THIS_FRAME, hideOrShow);
			Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME, hideOrShow);
			Function.Call(Hash.DISPLAY_AREA_NAME, hideOrShow);
			Function.Call(Hash.SET_POLICE_RADAR_BLIPS, !hideOrShow);
		}
    }
}
