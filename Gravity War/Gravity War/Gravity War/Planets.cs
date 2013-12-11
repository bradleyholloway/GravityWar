using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BradleyXboxUtils;
using Microsoft.Xna.Framework;

namespace Gravity_War
{
    class Planets
    {
        private static List<Planet> planets = new List<Planet>();

        public static void Add(Planet p)
        {
            planets.Add(p);
        }
        public static Boolean collides(Vector2 location)
        {
            foreach (Planet p in planets)
            {
                if (UTIL.distance(location, p.getLocation()) < p.getRadius())
                    return true;
            }
            return false;
        }
        
        public static Vector2 getGravityField(Vector2 location)
        {
            Vector2 sum = Vector2.Zero;
            foreach (Planet p in planets)
            {
                sum += p.getGravityField(location);
            }

            return sum;
        }
        
        public static Boolean collides(Planet p1)
        {
            foreach (Planet p2 in planets)
            {
                if (UTIL.distance(p1.getLocation(), p2.getLocation()) < p1.getRadius() + p2.getRadius())
                    return true;
            }

            return false;
        }
        public static void clear()
        {
            planets = new List<Planet>();
        }

        public static List<Planet> getPlanets()
        {
            return planets;
        }
    }
}
