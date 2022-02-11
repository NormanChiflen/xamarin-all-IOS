//
// VectorInt3.cs:
//     This represents the native vector_int3 type, which is 16 bytes.
//
//
// Authors:
//     Stephane Delcroix <stephane@delcroix.org> 
//
// Copyright (c) 2021 Microsoft Inc
//

#nullable enable

using System;
using System.Runtime.InteropServices;

// This type does not come from the CoreGraphics framework; it's defined in /usr/include/simd/vector_types.h
#if NET
namespace CoreGraphics
{
	[StructLayout (LayoutKind.Sequential)]
	public struct NVector3i : IEquatable<NVector3i>
	{
		public int X;
		public int Y;
		public int Z;

		public NVector3i (int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static bool operator == (NVector3i left, NVector3i right)
		{
			return left.Equals (right);
		}

		public static bool operator != (NVector3i left, NVector3i right)
		{
			return !left.Equals (right);
		}

		public override string ToString ()
		{
			return $"({X}, {Y}, {Z})";
		}

		public override int GetHashCode ()
		{
			return X.GetHashCode () ^ Y.GetHashCode () ^ Z.GetHashCode ();
		}

		public override bool Equals (object? obj)
		{
			if (!(obj is NVector3i vector))
				return false;

			return Equals (vector);
		}

		public bool Equals (NVector3i other)
		{
			return X == other.X && Y == other.Y && Z == other.Z;
		}

		public static readonly NVector3i Zero = new NVector3i(0, 0, 0);
	}
}
#endif //!NET
