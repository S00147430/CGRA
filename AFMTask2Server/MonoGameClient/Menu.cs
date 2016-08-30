using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameClient
{
    public class Menu
    {
        KeyboardState keyboard;
        KeyboardState prevKeyboard;

        MouseState mouse;
        MouseState prevMouse;

        public Game1 game { get; set; }
        SpriteFont spriteFont;
        int selected = 0;

        List<string> buttonList = new List<string>();

        public Menu()
        {
            //Adds buttons to write and select on the menu screen.
            buttonList.Add("Play");
            buttonList.Add("Login");
            buttonList.Add("Register");
            buttonList.Add("Exit");
        }

        public void LoadContent(ContentManager content)
        {
            spriteFont = content.Load<SpriteFont>("InGameFont");
        }

        //Update Game 1 class from menu class.
        public void Update(GameTime gameTime, Game1 game)
        {
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();

            if (CheckKeyboard(Keys.Up))
            {
                if (selected >= 0)
                    selected--;
            }

            if (CheckKeyboard(Keys.Down))
            {
                if (selected < buttonList.Count)
                    selected++;
            }

            if (CheckKeyboard(Keys.Enter))
            {
                if (selected == 0)
                {
                    Game1.GameState = null;
                }

                else if (selected == 3)
                {
                    game.Exit();
                }
           
                else if (selected == 1)
                {

                }
            }

            if (CheckKeyboard(Keys.Back))
            {

            }

            prevMouse = mouse;
            prevKeyboard = keyboard;
        }

        public bool CheckMouse()
        {
            return (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released);
        }

        public bool CheckKeyboard(Keys key)
        {
            return (keyboard.IsKeyDown(key) && !prevKeyboard.IsKeyDown(key));
        }

        public void Draw(SpriteBatch spriteBatch, Viewport viewport)
        {
            Color colour;
            int linePadding = 0;

            spriteBatch.Begin();
            for (int i = 0; i < buttonList.Count; i++)
            {
                colour = (i == selected) ? Color.Yellow : Color.Red;
                spriteBatch.DrawString(spriteFont, buttonList[i], new Vector2((viewport.Width / 10), (viewport.Height / 10) + (spriteFont.LineSpacing + linePadding) * i), colour);
            }
            spriteBatch.End();
        }
    }
}