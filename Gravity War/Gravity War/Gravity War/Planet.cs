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
        private Vector2 origin;
        private static double G = 6.67384 * Math.Pow(10, -11) * 1000000000;

        public Planet(Vector2 location, Texture2D image, double radius, double density)
        {
            this.location = location;
            this.radius = radius;
            this.image = image;
            this.scale = (float)((double)2 * radius/image.Width);
            this.density = density;
            this.mass = density * Math.PI * radius * radius *radius *4 / 3;
            this.origin = new Vector2((float)image.Width/2, (float)image.Height/2);
        }
        public Vector2 getGravityField(Vector2 location)
        {
            return UTIL.magD(mass*G/Math.Pow(UTIL.distance(location,this.location),2), UTIL.getDirectionTward(location,this.location));
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
        public double getRadius()
        {
            return radius;
        }
        public Vector2 getOrigin()
        {
            return origin;
        }
    }
}
