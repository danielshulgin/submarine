using System;

public class Connection : IEquatable<Connection>
{
    public Room first;
    public Room second;

    public Connection(Room first, Room second)
    {
        this.first = first;
        this.second = second;
    }

    public bool Equals(Connection other)
    {
        return (other.first.Equals(first)) && (other.second.Equals(second)) || ((other.second.Equals(first)) && (other.first.Equals(second)));
    }
}

