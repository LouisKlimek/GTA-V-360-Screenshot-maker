using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using System.Windows.Forms;
using System.Drawing;
using GTA.Native;
using System.IO;
using System.Drawing.Imaging;

namespace _360Pictures
{
    public class pictures : Script
    {
		private string basePath = "360Pictures";

		private int cFov = 50;
		private bool cPlayerVisible = false;
		private bool cHideHud = true;
		private bool cSaveCoords = true;
		private bool autoScreenshoting = false;
		private float takeScreenshotsEveryXmeters = 30f;

		private Ped character;
		private Vector3 lastScreenshotPosition;

		public pictures()
        {
			// Exectus at start time
			this.Tick += onTick;
			this.KeyUp += onKeyUp;
			this.KeyDown += onKeyDown;

			character = Game.Player.Character;
		}

        private void onTick(object sender, EventArgs e)
        {
			// Executes every Frame
			Vector3 currentPosition = character.Position;

			if (this.autoScreenshoting && Vector3.Distance(currentPosition, lastScreenshotPosition) > takeScreenshotsEveryXmeters)
			{
				lastScreenshotPosition = currentPosition;

				Take360Screenshot();
			}

		}
        private void onKeyUp(object sender, KeyEventArgs e)
        {
            // Executes if the Player releases a Key

        }
        private void onKeyDown(object sender, KeyEventArgs e)
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
			string screnshotPath = "";
			Vector3 position = character.Position;
			Vector3 rotation = character.Rotation;
			//Vector3 rotation = new Vector3(0, 0, 0);

			#region Create base Folder if it doesn't exist yet
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

			#region Create the screenshot Folder if it doesn't exist yet
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
			camera1.Rotation = new Vector3(0, 0, 0);
			World.RenderingCamera = camera1;
			Script.Wait(50);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p1-1.jpg"), ImageFormat.Jpeg);

			camera1.Rotation = new Vector3(0, 0, 78.9f);
			World.RenderingCamera = camera1;
			Script.Wait(50);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p1-2.jpg"), ImageFormat.Jpeg);

			camera1.Rotation = new Vector3(0, 0, (78.95f * 2f));
			World.RenderingCamera = camera1;
			Script.Wait(50);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p1-3.jpg"), ImageFormat.Jpeg);

			camera1.Rotation = new Vector3(0, 0, (78.95f * 3f));
			World.RenderingCamera = camera1;
			Script.Wait(50);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p1-4.jpg"), ImageFormat.Jpeg);

			camera1.Rotation = new Vector3(0, 0, (78.95f * 4f));
			World.RenderingCamera = camera1;
			Script.Wait(50);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p1-5.jpg"), ImageFormat.Jpeg);


			
			#region Make the Screenshots
			Camera camera = World.CreateCamera(position, rotation, this.cFov);
			camera.Position = position;
			camera.Rotation = new Vector3(rotation.X + 80f, rotation.Y, rotation.Z);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p1.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p2.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p3.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p4.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p5.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p6.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p7.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p8.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p9.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p10.jpg"), ImageFormat.Jpeg);
			camera.Position = position;
			camera.Rotation = new Vector3(rotation.X + 40f, rotation.Y, rotation.Z);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p11.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p12.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p13.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p14.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p15.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p16.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p17.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p18.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p19.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p20.jpg"), ImageFormat.Jpeg);
			camera.Position = position;
			camera.Rotation = rotation;
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p21.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p22.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p23.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p24.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p25.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p26.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p27.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p28.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p29.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p30.jpg"), ImageFormat.Jpeg);
			camera.Position = position;
			camera.Rotation = new Vector3(rotation.X - 40f, rotation.Y, rotation.Z);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p31.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p32.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p33.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p34.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p35.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p36.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p37.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p38.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p39.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p40.jpg"), ImageFormat.Jpeg);
			camera.Position = position;
			camera.Rotation = new Vector3(rotation.X - 80f, rotation.Y, rotation.Z);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p41.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p42.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p43.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p44.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p45.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p46.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p47.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p48.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p49.jpg"), ImageFormat.Jpeg);
			camera.Rotation = new Vector3(camera.Rotation.X, camera.Rotation.Y, camera.Rotation.Z + 35f);
			World.RenderingCamera = camera;
			Script.Wait(15);
			this.TakeScreenShot().Save(Path.Combine(screnshotPath, "p50.jpg"), ImageFormat.Jpeg);
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
