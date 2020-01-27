using Microsoft.Xna.Framework;
using System;

namespace Romi.Standard.UIControls
{
    public struct InputSignal
    {
        public bool Triggered;

        public void Clear()
        {
            Triggered = false;
        }

        public override bool Equals(object obj)
        {
            if (obj is InputSignal iss)
            {
                return iss.Triggered == Triggered;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Triggered.GetHashCode() * 1677716;
        }

        public static bool operator ==(InputSignal left, InputSignal right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(InputSignal left, InputSignal right)
        {
            return !(left == right);
        }
    }
}
