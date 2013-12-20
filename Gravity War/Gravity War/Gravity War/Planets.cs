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
        public static Planet collide(Vector2 location)
        {
            foreach (Planet p in planets)
            {
                if (UTIL.distance(location, p.getLocation()) < p.getRadius())
                    return p;
            }
            return null;
        }
        
        public static Vector2 getGravityField(Vector2 location)
        {
            Vector2 sum = Vector2.Zero;
            foreach (Planet p in planets)
            {
                if(!p.getLocation().Equals(location))
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
        public static void colide()
        {
            for (int a = 0; a < planets.Count-1; a++)
            {
                for (int b = a+1; b < planets.Count; b++)
                {
                    if(planets.ElementAt<Planet>(a).getRadius() + planets.ElementAt<Planet>(b).getRadius() >= UTIL.distance(planets.ElementAt<Planet>(a).getLocation(), planets.ElementAt<Planet>(b).getLocation()))
                    {
                        Planet p1 = planets.ElementAt<Planet>(b);
                        planets.RemoveAt(b);
                        Planet p2 = planets.ElementAt<Planet>(a);
                        planets.RemoveAt(a);
                        planets.Add(new Planet(p1, p2));
                        b--;
                    }
                }
            }
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
