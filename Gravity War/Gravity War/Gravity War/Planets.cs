using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BradleyXboxUtils;

namespace Gravity_War
{
    class Planets
    {
        private static List<Planet> planets = new List<Planet>();

        public static void Add(Planet p)
        {
            planets.Add(p);
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
    }
}
