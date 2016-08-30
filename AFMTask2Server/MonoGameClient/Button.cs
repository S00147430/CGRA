using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameClient
{
    public class Button
    {
        Texture2D texture;
        Vector2 postiion;
        Rectangle rectangle;

        Color colour = new Color(255, 255, 255, 255);

        bool down;

        public bool isClicked;

        public Button()
        {

        }
    }
}
