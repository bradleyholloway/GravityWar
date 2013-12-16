using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using BradleyXboxUtils;
using Microsoft.Xna.Framework;

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
        public void generate(int n)
        {
            generate(n, false);
        }
        public void generate(int n, bool orderly)
        {
            if(orderly)
            {
            int x, y;
            y = (int) Math.Ceiling(Math.Sqrt(n * height / width));
            x = (int) Math.Ceiling((double)n / y);
            for (int a = 0; a < x; a++)
            {
                for (int b = 0; b < y; b++)
                {
                    getPlanet(new Vector2((float)(a+.5)/x*width, (float)(b+.5)/y*height));
                }
            }
            }
            else
            {
                for(int a = 0; a<n; a++)
                {
                    getPlanet();
                }
            }

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
                if (r.Next(10) == 0)
                {
                    double radius = Math.Min(width, height) / 20;
                    double density = 50;
                    Texture2D image = images.ElementAt<Texture2D>(1);//images.Count-1);
                    double x = r.NextDouble() * (width - 2 * radius) + radius;
                    double y = r.NextDouble() * (height - 2 * radius) + radius;
                    temp = new Planet(new Microsoft.Xna.Framework.Vector2((int)x, (int)y), image, radius, density);
                }
                else
                {

                    double radius = r.NextDouble() * (Math.Min(width, height) / 20) + Math.Min(width, height) / 20;
                    double density = r.NextDouble() * .5 + .5;
                    Texture2D image = images.ElementAt<Texture2D>(0);//r.Next(images.Count-1));
                    double x = r.NextDouble() * (width - 2 * radius) + radius;
                    double y = r.NextDouble() * (height - 2 * radius) + radius;
                    temp = new Planet(new Microsoft.Xna.Framework.Vector2((int)x, (int)y), image, radius, density);
                }
            } 
            while (Planets.collides(temp));
            Planets.Add(temp);
            return temp;
        }
        public Planet getPlanet(Vector2 location)
        {
            Planet temp;
            do
            {
                double radius = r.NextDouble() * (Math.Min(width, height) / 20) + Math.Min(width, height) / 20;
                double density = r.NextDouble() * .5 + .5;
                Texture2D image = images.ElementAt<Texture2D>(r.Next(images.Count));
                
                temp = new Planet(location, image, radius, density);
            }
            while (Planets.collides(temp));
            Planets.Add(temp);
            return temp;
        }
    }
}
