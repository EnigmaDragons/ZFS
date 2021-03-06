﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDragons.Core.Engine;

namespace MonoDragons.Core.Graphics
{
    public class CometTexture
    {
        private readonly Color _color;
        private readonly bool _isHorizontal;

        public CometTexture(Color color, bool isHorizontal)
        {
            _color = color;
            _isHorizontal = isHorizontal;
        }

        public Texture2D Create()
        {
            var length = 18;
            var data = new Color[1 * 18];
            
            for (var i = 1; i < data.Length; i++)
                data[i] = new Color(_color, 255 * i / length);
            var height = _isHorizontal ? 1 : length;
            var width = _isHorizontal ? length : 1;

            var texture = new Texture2D(CurrentGame.TheGame.GraphicsDevice, width, height);
            texture.SetData(data);
            return texture;
        }
    }
}
