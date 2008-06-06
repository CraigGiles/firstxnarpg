using System;
using System.Collections.Generic;
using System.Text;

using Helper;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class Console : GameScreen
    {
        Texture2D background;
        float alpha = 150f;

        // Menu related fields
        private string output = "Type A Command";
        private string command = "";
        private List<string> log = new List<string>();

        // Input related fields
        private Keys[] keys = { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, 
                                Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, 
                                Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, 
                                Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, 
                                Keys.Y, Keys.Z };

        #region Constructor(s)
        public Console()
        {
            this.Name = "Console";
            this.HasFocus = true;
            this.IsPopup = true;
        }

        public override void LoadContent()
        {
            background = Globals.Content.Load<Texture2D>(@"Graphics/Debug/Console");

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Updates console
        /// </summary>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update(bool coveredByOtherScreen)
        {
            //checks to see if tilde is pressed to remove console
            CheckForExitConsole();

            //gets users input
            GetInput();

            //executes any issued commands
            CheckForExecuteCommand();

            base.Update(coveredByOtherScreen);
        }

        /// <summary>
        /// Renders console to screen
        /// </summary>
        public override void Draw()
        {
            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Texture, SaveStateMode.None);
            Globals.Batch.Draw(background,
                new Rectangle(0, 0, Globals.Graphics.GraphicsDevice.Viewport.Width, 85),
                new Color(1, 1, 1, (byte)alpha));

            Globals.Batch.DrawString(Globals.Font,
                "Console: Type a command",
                new Vector2(5, 5),
                Color.White);

            Globals.Batch.DrawString(Globals.Font,
                output,
                new Vector2(5, 30),
                Color.White);

            Globals.Batch.DrawString(Globals.Font,
                command,
                new Vector2(5, 60),
                Color.White);
            
            Globals.Batch.End();
            base.Draw();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks to see if the tilde key is pressed, and exits
        /// console if necessary
        /// </summary>
        private void CheckForExitConsole()
        {
            if (Globals.Input.IsKeyPressed(Keys.OemTilde))
            {
                this.HasFocus = false;
                ScreenManager.RemoveScreen(this);
            }
        }

        /// <summary>
        /// Gets user input and converts all keys pressed to
        /// characters in the console
        /// </summary>
        private void GetInput()
        {
            //Checks all keys for update
            foreach (Keys k in keys)
            {
                if (Globals.Input.IsKeyPressed(k))
                    command += k;
            }

            if (Globals.Input.IsKeyPressed(Keys.NumPad0) || Globals.Input.IsKeyPressed(Keys.D0))
                command += "0";

            if (Globals.Input.IsKeyPressed(Keys.NumPad1) || Globals.Input.IsKeyPressed(Keys.D1))
                command += "1";

            if (Globals.Input.IsKeyPressed(Keys.NumPad2) || Globals.Input.IsKeyPressed(Keys.D2))
                command += "2";

            if (Globals.Input.IsKeyPressed(Keys.NumPad3) || Globals.Input.IsKeyPressed(Keys.D3))
                command += "3";

            if (Globals.Input.IsKeyPressed(Keys.NumPad4) || Globals.Input.IsKeyPressed(Keys.D4))
                command += "4";

            if (Globals.Input.IsKeyPressed(Keys.NumPad5) || Globals.Input.IsKeyPressed(Keys.D5))
                command += "5";

            if (Globals.Input.IsKeyPressed(Keys.NumPad6) || Globals.Input.IsKeyPressed(Keys.D6))
                command += "6";

            if (Globals.Input.IsKeyPressed(Keys.NumPad7) || Globals.Input.IsKeyPressed(Keys.D7))
                command += "7";

            if (Globals.Input.IsKeyPressed(Keys.NumPad8) || Globals.Input.IsKeyPressed(Keys.D8))
                command += "8";

            if (Globals.Input.IsKeyPressed(Keys.NumPad9) || Globals.Input.IsKeyPressed(Keys.D9))
                command += "9";

            if (Globals.Input.IsKeyPressed(Keys.OemMinus) && Globals.Input.IsKeyHeld(Keys.LeftShift))
                command += "_";

            //if Delete is pressed, clear last key
            if (Globals.Input.IsKeyPressed(Keys.Delete) || Globals.Input.IsKeyPressed(Keys.Back))
            {
                string newCommand = "";
                for (int i = 0; i < command.Length - 1; i++)
                {
                    newCommand += command[i];
                }

                command = null;
                command = (string)newCommand.Clone();
            }

            if (Globals.Input.IsKeyPressed(Keys.Space))
                command += " ";
        }

        /// <summary>
        /// Checks to see if a command has been executed
        /// </summary>
        private void CheckForExecuteCommand()
        {
            //if enter is pressed, execte command
            if (Globals.Input.IsKeyPressed(Keys.Enter))
            {
                string execute = command;
                command = "";
                ExecuteCommand(execute);
            }
        }

        #endregion

        /// <summary>
        /// Executes any commands issued via the console
        /// </summary>
        /// <param name="command">console command to execute</param>
        private void ExecuteCommand(string command)
        {
            int value = -1;

            //Attempts to parse the command. If unsuccessful, the
            //value int will change back to -1
            if (!int.TryParse(command.Remove(0, command.Length - 2), out value))
                value = -1;


            command.ToLower();

            #region Commands List

            if (command.StartsWith("cfg", true, null))
                CheckConfigCommands(command, value);

            else if (command.StartsWith("fps", true, null))
                CheckFpsCommands(command, value);

            else if (command.StartsWith("map", true, null))
                CheckMapEditorCommands(command, value);

            else if (command.StartsWith("Quit", true, null))
                Globals.Game.Exit();


                        

            #endregion

            command = "";
        }

        private void CheckConfigCommands(string command, int value)
        {
            if (command.StartsWith("cfg_save", true, null))
            {
                output = "Saving Config.cfg";
                Settings.SaveSettings();
            }

            else if (command.StartsWith("cfg_load", true, null))
            {
                output = "Loading Config.cfg";
                Settings.LoadSettings();
            }
        }

        private void CheckFpsCommands(string command, int value)
        {
            if (command.StartsWith("fps_show", true, null))
            {
                output = "FPS: 0) off - 1) on (Default: 0)";
                FPSCounter.ConsoleValue = value;
            }

            else if (command.StartsWith("fps_scale", true, null))
            {
                output = "FPS_TextSize: Default 2";
                FPSCounter.DrawScale = value;
            }
        }

        private void CheckMapEditorCommands(string command, int value)
        {
            if (command.StartsWith("map_editor", true, null))
            {
                if (value == 1)
                {
                    ScreenManager.RemoveScreen(this);
                    ScreenManager.AddScreen(new MapEditor());
                }

                if (value == 0)
                {
                    ScreenManager.RemoveScreen("MapEditor");
                }
            }

            #region map_TileSize
            else if (command.StartsWith("map_TileSize", true, null))
            {
                if (value == -1)
                    output = "Current Tile Size: " + Globals.TileEngine.Map.TileHeight + " x " + Globals.TileEngine.Map.TileWidth;
                else
                {
                    output = "Adjusting tile size to " + value + " x " + value;
                    Globals.TileEngine.Map.AdjustTileSize(value);
                }
            }
            #endregion

            #region map_NewName
            else if (command.StartsWith("map_NewName", true, null))
            {

                try
                {
                    string newName = command.Remove(0, 12);
                    output = "New Map Name: " + newName;
                    Globals.TileEngine.Map.Name = newName;
                }
                catch
                {
                }

            }
            #endregion

            #region map_newmap


            if (command.StartsWith("map_newmap", true, null))
            {
                command += " ";
                string tileSet = command.Remove(0, 11).ToLower();

                if (tileSet.ToString().Trim() == "field")
                    Globals.TileEngine.CreateNewMap(Tileset.Field);
            }

            #endregion

            #region map_save
            else if (command.StartsWith("map_save", true, null))
            {
                Globals.TileEngine.SaveMap();
            }
            #endregion

            #region map_load 
            else if (command.StartsWith("map_load", true, null))
            {
                string name = command.Remove(0, 8).Trim();
                Globals.TileEngine.LoadMap(name);
            }
            #endregion

            else if (command.StartsWith("map_", true, null))
            {
            }

        }

        

    }
}
