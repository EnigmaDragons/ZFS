using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTiled.Tiled.TmxLoading
{
    public class Tmx
    {
        public int Width { get; }
        public int Height { get; }
        public int TileWidth { get; }
        public int TileHeight { get; }
        public List<Tsx> Tilesets { get; }
        public List<TmxLayer> Layers { get; } = new List<TmxLayer>();

        public Tmx(GraphicsDevice device, string mapDir, string tmxFileName)
        {
            var doc = XDocument.Load(Path.Combine("Content", mapDir, tmxFileName));
            var map = doc.Element(XName.Get("map"));
            Width = new XValue(map, "width").AsInt();
            Height = new XValue(map, "height").AsInt();
            TileWidth = new XValue(map, "tilewidth").AsInt();
            TileHeight = new XValue(map, "tileheight").AsInt();
            Tilesets = map.Elements(XName.Get("tileset"))
                .Select(x =>
                {
                    try
                    {
                        var src = x;
                        if (!x.HasElements)
                        {
                            var path = Path.GetFullPath(Path.Combine("Content", mapDir, new XValue(x, "source").AsString()));
                            var tsxDoc = XDocument.Load(path);
                            src = tsxDoc.Element(XName.Get("tileset"));
                        }
                        return new Tsx(device, new XValue(x, "firstgid").AsInt(), mapDir, src);
                    }
                    catch (Exception e)
                    {
                        int i = 0;
                        throw;
                    }
                }).ToList();
            var layers = map.Elements(XName.Get("layer")).ToList();
            for (var i = 0; i < layers.Count; i++)
                Layers.Add(new TmxLayer(i, layers[i]));
        }
    }
}
