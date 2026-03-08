using System;
using UnityEngine;

[Serializable]
public struct HexCoordinates : IEquatable<HexCoordinates>
{
    public int q;
    public int r;

    public HexCoordinates(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    // cube coordinate (computed)
    public int S => -q - r;

    // operator +
    public static HexCoordinates operator +(HexCoordinates a, HexCoordinates b)
    {
        return new HexCoordinates(a.q + b.q, a.r + b.r);
    }

    // operator -
    public static HexCoordinates operator -(HexCoordinates a, HexCoordinates b)
    {
        return new HexCoordinates(a.q - b.q, a.r - b.r);
    }

    // equality
    public bool Equals(HexCoordinates other)
    {
        return q == other.q && r == other.r;
    }

    public override bool Equals(object obj)
    {
        return obj is HexCoordinates other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(q, r);
    }

    public static bool operator ==(HexCoordinates a, HexCoordinates b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(HexCoordinates a, HexCoordinates b)
    {
        return !a.Equals(b);
    }

    public override string ToString()
    {
        return $"({q},{r})";
    }
    public Vector3 ToPosition()
    {
        float x = q * (HexMetrics.innerRadius * 2f);

        if ((r & 1) == 1)
        {
            x += HexMetrics.innerRadius;
        }

        float z = r * (HexMetrics.outerRadius * 1.5f);

        return new Vector3(x, 0f, z);
    }
}