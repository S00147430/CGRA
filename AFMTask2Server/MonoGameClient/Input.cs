using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameClient
{
    public static class Input
    {
        public static List<GestureSample> Gestures;
 
        static Input()
        {
            Gestures = new List<GestureSample>();
        }

        public static Vector2 GetKeyboardInputDirection(PlayerIndex playerIndex)
        {
            Vector2 direction = Vector2.Zero;
            KeyboardState keyboardState = Keyboard.GetState(playerIndex);

            if (playerIndex == PlayerIndex.One)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                    direction.Y += -1;
                if (keyboardState.IsKeyDown(Keys.S))
                    direction.Y += 1;
                if (keyboardState.IsKeyDown(Keys.A))
                    direction.X += -1;
                if (keyboardState.IsKeyDown(Keys.D))
                    direction.X += 1;
            }

            if (playerIndex == PlayerIndex.Two)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                    direction.Y += -1;
                if (keyboardState.IsKeyDown(Keys.Down))
                    direction.Y += 1;
            }
            return direction;
        }

        public static void ProcessTouchInput(out Vector2 player1Velocity, out Vector2 player2Velocity)
        {
            Gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                Gestures.Add(TouchPanel.ReadGesture());
            }
            player1Velocity = Vector2.Zero;
            player2Velocity = Vector2.Zero;

            foreach (GestureSample gestureSample in Gestures)
            {
                if (gestureSample.GestureType == GestureType.FreeDrag)
                {
                    if (gestureSample.Position.X > 0 && gestureSample.Position.X<= Game1.ScreenWidth / 2)
                        player1Velocity.Y += gestureSample.Delta.Y;
                    if (gestureSample.Position.X >= Game1.ScreenWidth / 2 && gestureSample.Position.X<= Game1.ScreenWidth)
                        player2Velocity.Y += gestureSample.Delta.Y;
                }
            }
        }
    }
}