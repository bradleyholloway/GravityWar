using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using BradleyXboxUtils;

namespace Gravity_War
{
    class PlanetGenerator
    {
        private int width;
        private int height;
        private List<Texture2D> images;
        private static Random r = new Random();
        public PlanetGenerator(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        public void clearImages()
        {
            images = new List<Texture2D>();
        }

        public void loadImage(Texture2D image)
        {
            images.Add(image);
        }
        public Planet getPlanet()
        {
            Planet temp;
            do
            {
                double radius = r.NextDouble() * (Math.Min(width, height) / 20);
                double density = r.NextDouble();
                Texture2D image = images.ElementAt<Texture2D>(r.Next(images.Count));
                double x = r.NextDouble() * (width - 2 * radius) + radius;
                double y = r.NextDouble() * (height - 2 * radius) + radius;
                temp = new Planet(new Microsoft.Xna.Framework.Vector2((int)x, (int)y), image, radius, density);
            } while (Planets.collides(temp));
            Planets.Add(temp);
            return temp;
        }        
    }
}
