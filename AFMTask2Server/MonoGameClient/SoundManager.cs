using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameClient
{
    public static class SoundManager
    {
        public static SoundEffect BallWallCollisionSoundEffect;
        public static SoundEffect PaddleBallCollisionSoundEffect;

        public static void LoadSounds(ContentManager Content)
        {
            BallWallCollisionSoundEffect = Content.Load<SoundEffect> ("Sounds/BallWallCollision");
            PaddleBallCollisionSoundEffect = Content.Load<SoundEffect>("Sounds/PaddleBallCollision");
        }
    }
}
