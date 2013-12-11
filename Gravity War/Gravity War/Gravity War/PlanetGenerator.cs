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
        private List<Planet> planets;
        private List<Texture2D> images;
        private static Random r = new Random();
        public PlanetGenerator(int width, int height)
        {
            this.width = width;
            this.height = height;
            planets = new List<Planet>();
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
            } while (collides(temp));
            planets.Add(temp);
            return temp;
        }

        public Boolean collides(Planet p1)
        {
            foreach (Planet p2 in planets)
            {
                if (UTIL.distance(p1.getLocation(), p2.getLocation()) < p1.getRadius() + p2.getRadius())
                    return true;
            }

            return false;
        }

        public void clear()
        {
            planets = new List<Planet>();
        }
    }
}
