using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BradleyXboxUtils;

namespace Gravity_War
{
    class Planet
    {
        private Vector2 location;
        private double radius;
        private float scale;
        private Texture2D image;
        private double density;
        private double mass;
        private static double G = 6.67384 * Math.Pow(10,-11);

        public Planet(Vector2 location, Texture2D image, double radius, double density)
        {
            this.location = location;
            this.radius = radius;
            this.image = image;
            this.scale = (float)((double)image.Width / 2 / radius);
            this.density = density;
            this.mass = density * Math.PI * radius * radius;
        }
        public double getGravityField(Vector2 location)
        {
            return mass*G/Math.Pow(UTIL.distance(location,this.location),2);
        }
        public Texture2D getImage()
        {
            return image;
        }
        public Vector2 getLocation()
        {
            return location;
        }
        public float getScale()
        {
            return scale;
        }
    }
}
