using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameClient
{
    public class Ball: GameObject
    {
        public Vector2 Velocity;
        public Random random;

        public Ball()
        {
            random = new Random();
        }

        public void Launch(float speed)
        {
            Position = new Vector2(Game1.ScreenWidth / 2 - Texture.Width / 2, Game1.ScreenHeight / 2 - Texture.Height / 2);
            // get a random + or - 60 degrees angle to the right
            float rotation = (float)(Math.PI / 2 + (random.NextDouble() * (Math.PI / 1.5f) - Math.PI / 3));

            Velocity.X = (float)Math.Sin(rotation);
            Velocity.Y = (float)Math.Cos(rotation);

            // 50% chance whether it launches left or right
            if (random.Next(2) == 1)
            {
                Velocity.X *= -1; //launch to the left
            }

            Velocity *= speed;
        }

        public void CheckWallCollision()
        {
            if (Position.Y < 0)
                {
                Position.Y = 0;
                Velocity.Y *= -1;
                SoundManager.BallWallCollisionSoundEffect.Play();
            }
            if (Position.Y + Texture.Height > Game1.ScreenHeight)
                {
                Position.Y = Game1.ScreenHeight - Texture.Height;
                Velocity.Y *= -1;
                SoundManager.BallWallCollisionSoundEffect.Play();
            }
        }

        public override void Move(Vector2 amount)
        {
            base.Move(amount);
            CheckWallCollision();
        }
    }
}
