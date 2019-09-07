using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public struct Coordinates : IEquatable<Coordinates>
{
    public int x;
    public int y;

    public Coordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool Equals(Coordinates other)
    {
        if (other.x == x && other.y == y)
        {
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return x + y * 997;
    }
}

