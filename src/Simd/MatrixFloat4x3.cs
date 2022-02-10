//
// Authors:
//     Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright (c) 2017 Microsoft Inc
//

//
// This represents the native matrix_float4x3 type (3 rows and 4 columns)
//

using System;
using System.Runtime.InteropServices;

#if NET
using VectorFloat4=global::System.Numerics.Vector4;
#else
using VectorFloat4=global::OpenTK.Vector4;
#endif

// This type does not come from the CoreGraphics framework; it's defined in /usr/include/simd/matrix_types.h
#if NET
namespace CoreGraphics
#else
namespace OpenTK
#endif
{
	[StructLayout (LayoutKind.Sequential)]
	public struct NMatrix4x3 : IEquatable<NMatrix4x3>
	{
		public float M11;
		public float M21;
		public float M31;
		float dummy1;

		public float M12;
		public float M22;
		public float M32;
		float dummy2;

		public float M13;
		public float M23;
		public float M33;
		float dummy3;

		public float M14;
		public float M24;
		public float M34;
		float dummy4;

		public NMatrix4x3 (
			float m11, float m12, float m13, float m14,
			float m21, float m22, float m23, float m24,
			float m31, float m32, float m33, float m34)
		{
			M11 = m11;
			M21 = m21;
			M31 = m31;
			M12 = m12;
			M22 = m22;
			M32 = m32;
			M13 = m13;
			M23 = m23;
			M33 = m33;
			M14 = m14;
			M24 = m24;
			M34 = m34;
			dummy1 = 0;
			dummy2 = 0;
			dummy3 = 0;
			dummy4 = 0;
		}

		public NVector3 Column0 {
			get {
				return new NVector3 (M11, M21, M31);
			}
			set {
				M11 = value.X;
				M21 = value.Y;
				M31 = value.Z;
			}
		}

		public NVector3 Column1 {
			get {
				return new NVector3 (M12, M22, M32);
			}
			set {
				M12 = value.X;
				M22 = value.Y;
				M32 = value.Z;
			}
		}

		public NVector3 Column2 {
			get {
				return new NVector3 (M13, M23, M33);
			}
			set {
				M13 = value.X;
				M23 = value.Y;
				M33 = value.Z;
			}
		}

		public NVector3 Column3 {
			get {
				return new NVector3 (M14, M24, M34);
			}
			set {
				M14 = value.X;
				M24 = value.Y;
				M34 = value.Z;
			}
		}

		public VectorFloat4 Row0 {
			get {
				return new VectorFloat4 (M11, M12, M13, M14);
			}
			set {
				M11 = value.X;
				M12 = value.Y;
				M13 = value.Z;
				M14 = value.W;
			}
		}

		public VectorFloat4 Row1 {
			get {
				return new VectorFloat4 (M21, M22, M23, M24);
			}
			set {
				M21 = value.X;
				M22 = value.Y;
				M23 = value.Z;
				M24 = value.W;
			}
		}

		public VectorFloat4 Row2 {
			get {
				return new VectorFloat4 (M31, M32, M33, M34);
			}
			set {
				M31 = value.X;
				M32 = value.Y;
				M33 = value.Z;
				M34 = value.W;
			}
		}

		public static bool operator == (NMatrix4x3 left, NMatrix4x3 right)
		{
			return left.Equals (right);
		}

		public static bool operator != (NMatrix4x3 left, NMatrix4x3 right)
		{
			return !left.Equals (right);
		}

		public override string ToString ()
		{
			return
				$"({M11}, {M12}, {M13}, {M14})\n" +
				$"({M21}, {M22}, {M23}, {M24})\n" +
				$"({M31}, {M32}, {M33}, {M34})";
		}

		public override int GetHashCode ()
		{
			return
				M11.GetHashCode () ^ M12.GetHashCode () ^ M13.GetHashCode () ^ M14.GetHashCode () ^
				M21.GetHashCode () ^ M22.GetHashCode () ^ M23.GetHashCode () ^ M24.GetHashCode () ^
				M31.GetHashCode () ^ M32.GetHashCode () ^ M33.GetHashCode () ^ M34.GetHashCode ();
		}

		public override bool Equals (object obj)
		{
			if (!(obj is NMatrix4x3))
				return false;

			return Equals ((NMatrix4x3) obj);
		}

		public bool Equals (NMatrix4x3 other)
		{
			return
				M11 == other.M11 && M12 == other.M12 && M13 == other.M13 && M14 == other.M14 &&
				M21 == other.M21 && M22 == other.M22 && M23 == other.M23 && M24 == other.M24 &&
				M31 == other.M31 && M32 == other.M32 && M33 == other.M33 && M34 == other.M34;
		}
	}
}
