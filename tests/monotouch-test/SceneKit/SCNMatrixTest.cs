//
// Unit tests for SCNMatrix4
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

#if !__WATCHOS__

#nullable enable

#if !MONOMAC
#define PFLOAT_SINGLE
#endif

using System;
using System.Runtime.InteropServices;

using CoreAnimation;
using Foundation;
using SceneKit;
using OpenTK;

using NUnit.Framework;

#if PFLOAT_SINGLE
using pfloat = System.Single;
#else
using pfloat = System.Runtime.InteropServices.NFloat;
#endif

namespace MonoTouchFixtures.SceneKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SCNMatrix4Test {
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
		static NFloat OneThird = new NFloat (1.0 / 3.0);
		static NFloat OneFifteenth = new NFloat (1.0 / 15.0);
		static NFloat TwoThirds = new NFloat (2.0 / 3.0);
		static NFloat TwoFifteenths = new NFloat (2.0 / 15.0); // 0.1333333333..
		static NFloat SqrtTwo = new NFloat (Math.Sqrt (2));
		static NFloat SqrtTwoHalved = new NFloat (Math.Sqrt (2) / 2);
		static NFloat SqrtThree = new NFloat (Math.Sqrt (3));
		static NFloat SqrtThreeHalved = new NFloat (Math.Sqrt (3) / 2);
		static NFloat SqrtThreeInverted = new NFloat (1 / Math.Sqrt (3));
		static NFloat SqrtSix = new NFloat (Math.Sqrt (6));
		static NFloat SqrtSixInverted = new NFloat  (1 / Math.Sqrt (6));
		static NFloat SqrtTwelve = new NFloat (Math.Sqrt (12)); // 3.464102
		static NFloat OhPointFive = new NFloat (0.5);
#else
		static pfloat OneThird = (pfloat) (1.0 / 3.0);
		static pfloat OneFifteenth = (pfloat) (1.0 / 15.0);
		static pfloat TwoThirds = (pfloat) (2.0 / 3.0);
		static pfloat TwoFifteenths = (pfloat) (2.0 / 15.0); // 0.1333333333..
		static pfloat SqrtTwo = (pfloat) (Math.Sqrt (2));
		static pfloat SqrtTwoHalved = (pfloat) (Math.Sqrt (2) / 2);
		static pfloat SqrtThree = (pfloat) (Math.Sqrt (3));
		static pfloat SqrtThreeHalved = (pfloat) (Math.Sqrt (3) / 2);
		static pfloat SqrtThreeInverted = (pfloat) (1 / Math.Sqrt (3));
		static pfloat SqrtSix = (pfloat) (Math.Sqrt (6));
		static pfloat SqrtSixInverted = (pfloat)  (1 / Math.Sqrt (6));
		static pfloat SqrtTwelve = (pfloat) (Math.Sqrt (12)); // 3.464102
		static pfloat OhPointFive = (pfloat) 0.5;
#endif

#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
		public static bool CloseEnough (NFloat a, NFloat b, double epsilon = 0.00001)
		{
			return CloseEnough (a.Value, b.Value, epsilon);
		}
#endif

		public static bool CloseEnough (double a, double b, double epsilon = 0.00001)
		{
			const double MinNormal = 2.2250738585072014E-308d;
			var absA = Math.Abs (a);
			var absB = Math.Abs (b);
			var diff = Math.Abs (a - b);

			if (a == b) {
				return true;
			} else if (a == 0 || b == 0 || absA + absB < MinNormal) {
				// a or b is zero or both are extremely close to it
				// relative error is less meaningful here
				return diff < (epsilon * MinNormal);
			} else { // use relative error
				return diff / (absA + absB) < epsilon;
			}
		}

		void AssertEqual (SCNMatrix4 matrix, string message,
			int m11, int m12, int m13, int m14,
			int m21, int m22, int m23, int m24,
			int m31, int m32, int m33, int m34,
			int m41, int m42, int m43, int m44
		)
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			AssertEqual (matrix, message,
				new NFloat (m11), new NFloat (m12), new NFloat (m13), new NFloat (m14),
				new NFloat (m21), new NFloat (m22), new NFloat (m23), new NFloat (m24),
				new NFloat (m31), new NFloat (m32), new NFloat (m33), new NFloat (m34),
				new NFloat (m41), new NFloat (m42), new NFloat (m43), new NFloat (m44));
#else
			AssertEqual (matrix, message,
				(pfloat) m11, (pfloat) m12, (pfloat) m13, (pfloat) m14,
				(pfloat) m21, (pfloat) m22, (pfloat) m23, (pfloat) m24,
				(pfloat) m31, (pfloat) m32, (pfloat) m33, (pfloat) m34,
				(pfloat) m41, (pfloat) m42, (pfloat) m43, (pfloat) m44);
#endif
		}

		void AssertEqual (SCNMatrix4 matrix, string message,
			double m11, double m12, double m13, double m14,
			double m21, double m22, double m23, double m24,
			double m31, double m32, double m33, double m34,
			double m41, double m42, double m43, double m44
		)
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			AssertEqual (matrix, message,
				new NFloat (m11), new NFloat (m12), new NFloat (m13), new NFloat (m14),
				new NFloat (m21), new NFloat (m22), new NFloat (m23), new NFloat (m24),
				new NFloat (m31), new NFloat (m32), new NFloat (m33), new NFloat (m34),
				new NFloat (m41), new NFloat (m42), new NFloat (m43), new NFloat (m44));
#else
			AssertEqual (matrix, message,
				(pfloat) m11, (pfloat) m12, (pfloat) m13, (pfloat) m14,
				(pfloat) m21, (pfloat) m22, (pfloat) m23, (pfloat) m24,
				(pfloat) m31, (pfloat) m32, (pfloat) m33, (pfloat) m34,
				(pfloat) m41, (pfloat) m42, (pfloat) m43, (pfloat) m44);
#endif
		}

		void AssertEqual (SCNMatrix4 matrix, string message,
			pfloat m11, pfloat m12, pfloat m13, pfloat m14,
			pfloat m21, pfloat m22, pfloat m23, pfloat m24,
			pfloat m31, pfloat m32, pfloat m33, pfloat m34,
			pfloat m41, pfloat m42, pfloat m43, pfloat m44
		)
		{
			if (CloseEnough (m11, matrix.M11) && CloseEnough (m12, matrix.M12) && CloseEnough (m13, matrix.M13) && CloseEnough (m14, matrix.M14) &&
				CloseEnough (m21, matrix.M21) && CloseEnough (m22, matrix.M22) && CloseEnough (m23, matrix.M23) && CloseEnough (m24, matrix.M24) &&
				CloseEnough (m31, matrix.M31) && CloseEnough (m32, matrix.M32) && CloseEnough (m33, matrix.M33) && CloseEnough (m34, matrix.M34) &&
				CloseEnough (m41, matrix.M41) && CloseEnough (m42, matrix.M42) && CloseEnough (m43, matrix.M43) && CloseEnough (m44, matrix.M44))
				return;

			var actualString = matrix.ToString ();

			var row1 = $"({m11}, {m12}, {m13}, {m14})";
			var row2 = $"({m21}, {m22}, {m23}, {m24})";
			var row3 = $"({m31}, {m32}, {m33}, {m34})";
			var row4 = $"({m41}, {m42}, {m43}, {m44})";
			var expectedString = $"{row1}\n{row2}\n{row3}\n{row4}";
			Assert.Fail ($"Expected matrix:\n{expectedString}\nActual matrix:\n{actualString}\n{message}");
		}

		void AssertEqual (SCNVector4 vector, string message, pfloat m1, pfloat m2, pfloat m3, pfloat m4)
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			if (m1.Value == vector.X.Value && m2.Value == vector.Y.Value && m3.Value == vector.Z.Value && m4.Value == vector.W.Value)
#else
			if (m1 == vector.X && m2 == vector.Y && m3 == vector.Z && m4 == vector.W)
#endif
				return;

			var expectedString = vector.ToString ();
			var actualString = $"({m1}, {m2}, {m3}, {m4})";

			Assert.Fail ($"Expected vector:\n{expectedString}\nActual vector:\n{actualString}\n{message}");
		}

		void AssertEqual (SCNVector4 vector, string message, double m1, double m2, double m3, double m4)
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			AssertEqual (vector, message, new NFloat (m1), new NFloat (m2), new NFloat (m3), new NFloat (m4));
#else
			AssertEqual (vector, message, (pfloat) m1, (pfloat) m2, (pfloat) m3, (pfloat) m4);
#endif
		}

		[Test]
		public void Identity ()
		{
			var matrix = SCNMatrix4.Identity;
			AssertEqual (matrix, "Identity",
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
		}

		[Test]
		public void Constructor_RowVectors ()
		{
			var matrix = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			AssertEqual (matrix, "Constructor",
				11, 12, 13, 14,
				21, 22, 23, 24,
				31, 32, 33, 34,
				41, 42, 43, 44);
		}

		[Test]
		public void Constructor_Elements ()
		{
			var matrix = new SCNMatrix4 (
				11, 12, 13, 14,
				21, 22, 23, 24,
				31, 32, 33, 34,
				41, 42, 43, 44);
			AssertEqual (matrix, "Constructor",
				11, 12, 13, 14,
				21, 22, 23, 24,
				31, 32, 33, 34,
				41, 42, 43, 44);
		}

#if !WATCH
		[Test]
		public void Constructor_CATransform3d ()
		{
			var transform = new CATransform3D () {
#if NO_NFLOAT_OPERATORS
				M11 = new NFloat (11),
				M12 = new NFloat (12),
				M13 = new NFloat (13),
				M14 = new NFloat (14),
				M21 = new NFloat (21),
				M22 = new NFloat (22),
				M23 = new NFloat (23),
				M24 = new NFloat (24),
				M31 = new NFloat (31),
				M32 = new NFloat (32),
				M33 = new NFloat (33),
				M34 = new NFloat (34),
				M41 = new NFloat (41),
				M42 = new NFloat (42),
				M43 = new NFloat (43),
				M44 = new NFloat (44),
#else
				M11 = 11,
				M12 = 12,
				M13 = 13,
				M14 = 14,
				M21 = 21,
				M22 = 22,
				M23 = 23,
				M24 = 24,
				M31 = 31,
				M32 = 32,
				M33 = 33,
				M34 = 34,
				M41 = 41,
				M42 = 42,
				M43 = 43,
				M44 = 44,
#endif
			};
			var matrix = new SCNMatrix4 (transform);
			AssertEqual (matrix, "Constructor",
				11, 12, 13, 14,
				21, 22, 23, 24,
				31, 32, 33, 34,
				41, 42, 43, 44);
		}
#endif

		[Test]
		public void Determinant ()
		{
			var matrix = new SCNMatrix4 (
				3, 5, 8, 9,
				5, 3, 5, 8,
				9, 6, 4, 2,
				4, 6, 9, 8);
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			Assert.AreEqual (new NFloat (-165), matrix.Determinant, "Determinant");
#else
			Assert.AreEqual ((pfloat) (-165), matrix.Determinant, "Determinant");
#endif
		}


		[Test]
		public void Rows ()
		{
			var matrix = new SCNMatrix4 (
				11, 12, 13, 14,
				21, 22, 23, 24,
				31, 32, 33, 34,
				41, 42, 43, 44);
			AssertEqual (matrix.Row0, "Row0", 11, 12, 13, 14);
			AssertEqual (matrix.Row1, "Row1", 21, 22, 23, 24);
			AssertEqual (matrix.Row2, "Row2", 31, 32, 33, 34);
			AssertEqual (matrix.Row3, "Row3", 41, 42, 43, 44);
		}

		[Test]
		public void Elements ()
		{
			var matrix = new SCNMatrix4 (
				11, 12, 13, 14,
				21, 22, 23, 24,
				31, 32, 33, 34,
				41, 42, 43, 44);
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			Assert.AreEqual (new NFloat (11), matrix.M11, "M11");
			Assert.AreEqual (new NFloat (12), matrix.M12, "M12");
			Assert.AreEqual (new NFloat (13), matrix.M13, "M13");
			Assert.AreEqual (new NFloat (14), matrix.M14, "M14");
			Assert.AreEqual (new NFloat (21), matrix.M21, "M21");
			Assert.AreEqual (new NFloat (22), matrix.M22, "M22");
			Assert.AreEqual (new NFloat (23), matrix.M23, "M23");
			Assert.AreEqual (new NFloat (24), matrix.M24, "M24");
			Assert.AreEqual (new NFloat (31), matrix.M31, "M31");
			Assert.AreEqual (new NFloat (32), matrix.M32, "M32");
			Assert.AreEqual (new NFloat (33), matrix.M33, "M33");
			Assert.AreEqual (new NFloat (34), matrix.M34, "M34");
			Assert.AreEqual (new NFloat (41), matrix.M41, "M41");
			Assert.AreEqual (new NFloat (42), matrix.M42, "M42");
			Assert.AreEqual (new NFloat (43), matrix.M43, "M43");
			Assert.AreEqual (new NFloat (44), matrix.M44, "M44");
#else
			Assert.AreEqual ((pfloat) 11, matrix.M11, "M11");
			Assert.AreEqual ((pfloat) 12, matrix.M12, "M12");
			Assert.AreEqual ((pfloat) 13, matrix.M13, "M13");
			Assert.AreEqual ((pfloat) 14, matrix.M14, "M14");
			Assert.AreEqual ((pfloat) 21, matrix.M21, "M21");
			Assert.AreEqual ((pfloat) 22, matrix.M22, "M22");
			Assert.AreEqual ((pfloat) 23, matrix.M23, "M23");
			Assert.AreEqual ((pfloat) 24, matrix.M24, "M24");
			Assert.AreEqual ((pfloat) 31, matrix.M31, "M31");
			Assert.AreEqual ((pfloat) 32, matrix.M32, "M32");
			Assert.AreEqual ((pfloat) 33, matrix.M33, "M33");
			Assert.AreEqual ((pfloat) 34, matrix.M34, "M34");
			Assert.AreEqual ((pfloat) 41, matrix.M41, "M41");
			Assert.AreEqual ((pfloat) 42, matrix.M42, "M42");
			Assert.AreEqual ((pfloat) 43, matrix.M43, "M43");
			Assert.AreEqual ((pfloat) 44, matrix.M44, "M44");
#endif
		}

#if NET // The legacy Invert implementation seems very wrong, so only verify .NET behavior
		[Test]
		public void Invert ()
		{
			var matrix = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			var ex = Assert.Throws<InvalidOperationException> (() => matrix.Invert (), "Singular matrix");
			Assert.That (ex.Message, Does.Contain ("Matrix is singular and cannot be inverted"), "Singular matrix message");

			matrix = new SCNMatrix4 (
				3, 5, 8, 9,
				5, 3, 5, 8,
				9, 6, 4, 2,
				4, 6, 9, 8);
			matrix.Invert ();

			AssertEqual (matrix, "Invert",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				new NFloat (-0.6181818181818182), new NFloat (0.3151515151515151), new NFloat (-0.030303030303030304), new NFloat (0.3878787878787879),
				new NFloat (1.6363636363636365), new NFloat (-0.696969696969697), new NFloat (0.3939393939393939), new NFloat (-1.2424242424242424),
				new NFloat (-1.3818181818181818), new NFloat (0.3515151515151515), new NFloat (-0.30303030303030304), new NFloat (1.2787878787878788),
				new NFloat (0.6363636363636364), new NFloat (-0.030303030303030304), new NFloat (0.06060606060606061), new NFloat (-0.5757575757575758));
#else
				(pfloat) (-0.6181818181818182), (pfloat) (0.3151515151515151), (pfloat) (-0.030303030303030304), (pfloat) (0.3878787878787879),
				(pfloat) (1.6363636363636365), (pfloat) (-0.696969696969697), (pfloat) (0.3939393939393939), (pfloat) (-1.2424242424242424),
				(pfloat) (-1.3818181818181818), (pfloat) (0.3515151515151515), (pfloat) (-0.30303030303030304), (pfloat) (1.2787878787878788),
				(pfloat) (0.6363636363636364), (pfloat) (-0.030303030303030304), (pfloat) (0.06060606060606061), (pfloat) (-0.5757575757575758));
#endif
		}
#endif

		[Test]
		public void Transpose ()
		{
			var matrix = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			matrix.Transpose ();
			AssertEqual (matrix, "Transpose",
				11, 21, 31, 41,
				12, 22, 32, 42,
				13, 23, 33, 43,
				14, 24, 34, 44);
		}

		[Test]
		public void CreateFromColumns ()
		{
			var matrix = SCNMatrix4.CreateFromColumns (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			AssertEqual (matrix, "CreateFromColumns",
				11, 21, 31, 41,
				12, 22, 32, 42,
				13, 23, 33, 43,
				14, 24, 34, 44);
		}

		[Test]
		public void CreateFromColumns_Out ()
		{
			SCNMatrix4.CreateFromColumns (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44),
			out var matrix);
			AssertEqual (matrix, "CreateFromColumns",
				11, 21, 31, 41,
				12, 22, 32, 42,
				13, 23, 33, 43,
				14, 24, 34, 44);
		}

		[Test]
		public void CreateFromAxisAngle_pfloat_Out ()
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			SCNMatrix4.CreateFromAxisAngle (new SCNVector3 (2, 2, 2), new NFloat (Math.PI / 3), out var matrix);
#else
			SCNMatrix4.CreateFromAxisAngle (new SCNVector3 (2, 2, 2), (pfloat) (Math.PI / 3), out var matrix);
#endif
			AssertEqual (matrix, "CreateFromAxisAngle",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				TwoThirds, TwoThirds, new NFloat (-OneThird.Value), new NFloat (0),
				new NFloat (-OneThird.Value), TwoThirds, TwoThirds, new NFloat (0),
				TwoThirds, new NFloat (-OneThird.Value), TwoThirds, new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (0), new NFloat (1));
#else
				TwoThirds, TwoThirds, -OneThird, 0,
				-OneThird, TwoThirds, TwoThirds, 0,
				TwoThirds, -OneThird, TwoThirds, 0,
				0, 0, 0, 1);
#endif
		}

		[Test]
		public void CreateFromAxisAngle_float_Out ()
		{
			SCNMatrix4.CreateFromAxisAngle (new Vector3 (2, 2, 2), (float) (Math.PI / 3), out var matrix);
			AssertEqual (matrix, "CreateFromAxisAngle",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				TwoThirds, TwoThirds, new NFloat (-OneThird.Value), new NFloat (0),
				new NFloat (-OneThird.Value), TwoThirds, TwoThirds, new NFloat (0),
				TwoThirds, new NFloat (-OneThird.Value), TwoThirds, new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (0), new NFloat (1));
#else
				TwoThirds, TwoThirds, -OneThird, 0,
				-OneThird, TwoThirds, TwoThirds, 0,
				TwoThirds, -OneThird, TwoThirds, 0,
				0, 0, 0, 1);
#endif
		}

		[Test]
		public void CreateFromAxisAngle_double_Out ()
		{
			SCNMatrix4.CreateFromAxisAngle (new Vector3d (2, 2, 2), (double) (Math.PI / 3), out var matrix);
			AssertEqual (matrix, "CreateFromAxisAngle",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				TwoThirds, TwoThirds, new NFloat (-OneThird.Value), new NFloat (0),
				new NFloat (-OneThird.Value), TwoThirds, TwoThirds, new NFloat (0),
				TwoThirds, new NFloat (-OneThird.Value), TwoThirds, new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (0), new NFloat (1));
#else
				TwoThirds, TwoThirds, -OneThird, 0,
				-OneThird, TwoThirds, TwoThirds, 0,
				TwoThirds, -OneThird, TwoThirds, 0,
				0, 0, 0, 1);
#endif
		}

		[Test]
		public void CreateFromAxisAngle ()
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			var matrix = SCNMatrix4.CreateFromAxisAngle (new SCNVector3 (2, 2, 2), new NFloat (Math.PI / 3));
#else
			var matrix = SCNMatrix4.CreateFromAxisAngle (new SCNVector3 (2, 2, 2), (pfloat) (Math.PI / 3));
#endif
			AssertEqual (matrix, "CreateFromAxisAngle",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				TwoThirds, TwoThirds, new NFloat (-OneThird.Value), new NFloat (0),
				new NFloat (-OneThird.Value), TwoThirds, TwoThirds, new NFloat (0),
				TwoThirds, new NFloat (-OneThird.Value), TwoThirds, new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (0), new NFloat (1));
#else
				TwoThirds, TwoThirds, -OneThird, 0,
				-OneThird, TwoThirds, TwoThirds, 0,
				TwoThirds, -OneThird, TwoThirds, 0,
				0, 0, 0, 1);
#endif
		}

		[Test]
		public void CreateRotationX_Out ()
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			SCNMatrix4.CreateRotationX (new NFloat (Math.PI / 3), out var matrix);
#else
			SCNMatrix4.CreateRotationX ((pfloat) (Math.PI / 3), out var matrix);
#endif
			AssertEqual (matrix, "CreateRotationX",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				new NFloat (1), new NFloat (0), new NFloat (0), new NFloat (0),
				new NFloat (0), OhPointFive, SqrtThreeHalved, new NFloat (0),
				new NFloat (0), new NFloat (-SqrtThreeHalved.Value), OhPointFive, new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (0), new NFloat (1));
#else
				1, 0, 0, 0,
				0, OhPointFive, SqrtThreeHalved, 0,
				0, -SqrtThreeHalved, OhPointFive, 0,
				0, 0, 0, 1);
#endif
		}

		[Test]
		public void CreateRotationX ()
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			var matrix = SCNMatrix4.CreateRotationX (new NFloat (Math.PI / 3));
#else
			var matrix = SCNMatrix4.CreateRotationX ((pfloat) (Math.PI / 3));
#endif
			AssertEqual (matrix, "CreateRotationX",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				new NFloat (1), new NFloat (0), new NFloat (0), new NFloat (0),
				new NFloat (0), OhPointFive, SqrtThreeHalved, new NFloat (0),
				new NFloat (0), new NFloat (-SqrtThreeHalved.Value), OhPointFive, new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (0), new NFloat (1));
#else
				1, 0, 0, 0,
				0, OhPointFive, SqrtThreeHalved, 0,
				0, -SqrtThreeHalved, OhPointFive, 0,
				0, 0, 0, 1);
#endif
		}

		[Test]
		public void CreateRotationY_Out ()
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			SCNMatrix4.CreateRotationY (new NFloat (Math.PI / 3), out var matrix);
#else
			SCNMatrix4.CreateRotationY ((pfloat) (Math.PI / 3), out var matrix);
#endif
			AssertEqual (matrix, "CreateRotationY",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				OhPointFive, new NFloat (0), new NFloat (-SqrtThreeHalved.Value), new NFloat (0),
				new NFloat (0), new NFloat (1), new NFloat (0), new NFloat (0),
				SqrtThreeHalved, new NFloat (0), OhPointFive, new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (0), new NFloat (1));
#else
				OhPointFive, 0, -SqrtThreeHalved, 0,
				0, 1, 0, 0,
				SqrtThreeHalved, 0, OhPointFive, 0,
				0, 0, 0, 1);
#endif
		}

		[Test]
		public void CreateRotationY ()
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			var matrix = SCNMatrix4.CreateRotationY (new NFloat (Math.PI / 3));
#else
			var matrix = SCNMatrix4.CreateRotationY ((pfloat) (Math.PI / 3));
#endif
			AssertEqual (matrix, "CreateRotationY",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				OhPointFive, new NFloat (0), new NFloat (-SqrtThreeHalved.Value), new NFloat (0),
				new NFloat (0), new NFloat (1), new NFloat (0), new NFloat (0),
				SqrtThreeHalved, new NFloat (0), OhPointFive, new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (0), new NFloat (1));
#else
				OhPointFive, 0, -SqrtThreeHalved, 0,
				0, 1, 0, 0,
				SqrtThreeHalved, 0, OhPointFive, 0,
				0, 0, 0, 1);
#endif
		}

		[Test]
		public void CreateRotationZ_Out ()
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			SCNMatrix4.CreateRotationZ (new NFloat (Math.PI / 3), out var matrix);
#else
			SCNMatrix4.CreateRotationZ ((pfloat) (Math.PI / 3), out var matrix);
#endif
			AssertEqual (matrix, "CreateRotationZ",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				OhPointFive, SqrtThreeHalved, new NFloat (0), new NFloat (0),
				new NFloat (-SqrtThreeHalved.Value), OhPointFive, new NFloat (0), new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (1), new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (0), new NFloat (1));
#else
				OhPointFive, SqrtThreeHalved, 0, 0,
				-SqrtThreeHalved, OhPointFive, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
#endif
		}

		[Test]
		public void CreateRotationZ ()
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			var matrix = SCNMatrix4.CreateRotationZ (new NFloat (Math.PI / 3));
#else
			var matrix = SCNMatrix4.CreateRotationZ ((pfloat) (Math.PI / 3));
#endif
			AssertEqual (matrix, "CreateRotationZ",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				OhPointFive, SqrtThreeHalved, new NFloat (0), new NFloat (0),
				new NFloat (-SqrtThreeHalved.Value), OhPointFive, new NFloat (0), new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (1), new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (0), new NFloat (1));
#else
				OhPointFive, SqrtThreeHalved, 0, 0,
				-SqrtThreeHalved, OhPointFive, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
#endif
		}

		[Test]
		public void CreateTranslation_Out ()
		{
			SCNMatrix4.CreateTranslation (1, 2, 3, out var matrix);
			AssertEqual (matrix, "CreateTranslation",
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				1, 2, 3, 1);
		}

		[Test]
		public void CreateTranslation_Vector_Out ()
		{
			var translation = new SCNVector3 (1, 2, 3);
			SCNMatrix4.CreateTranslation (ref translation, out var matrix);
			AssertEqual (matrix, "CreateTranslation",
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				1, 2, 3, 1);
		}

		[Test]
		public void CreateTranslation ()
		{
			var matrix = SCNMatrix4.CreateTranslation (1, 2, 3);
			AssertEqual (matrix, "CreateTranslation",
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				1, 2, 3, 1);
		}

		[Test]
		public void CreateTranslation_Vector ()
		{
			var translation = new SCNVector3 (1, 2, 3);
			var matrix = SCNMatrix4.CreateTranslation (translation);
			AssertEqual (matrix, "CreateTranslation",
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				1, 2, 3, 1);
		}

		[Test]
		public void CreateOrthographic_Out ()
		{
			SCNMatrix4.CreateOrthographic (1, 2, 3, 4, out var matrix);
			AssertEqual (matrix, "CreateOrthographic",
				2, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, -2, 0,
				0, 0, -7, 1);
		}

		[Test]
		public void CreateOrthographic ()
		{
			var matrix = SCNMatrix4.CreateOrthographic (1, 2, 3, 4);
			AssertEqual (matrix, "CreateOrthographic",
				2, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, -2, 0,
				0, 0, -7, 1);
		}

		[Test]
		public void CreateOrthographicOffCenter_Out ()
		{
			SCNMatrix4.CreateOrthographicOffCenter (1, 2, 3, 4, 5, 6, out var matrix);
			AssertEqual (matrix, "CreateOrthographicOffCenter",
				2, 0, 0, 0,
				0, 2, 0, 0,
				0, 0, -2, 0,
				-3, -7, -11, 1);
		}

		[Test]
		public void CreateOrthographicOffCenter ()
		{
			var matrix = SCNMatrix4.CreateOrthographicOffCenter (1, 2, 3, 4, 5, 6);
			AssertEqual (matrix, "CreateOrthographicOffCenter",
				2, 0, 0, 0,
				0, 2, 0, 0,
				0, 0, -2, 0,
				-3, -7, -11, 1);
		}

		[Test]
		public void CreatePerspectiveFieldOfView_Out ()
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			SCNMatrix4.CreatePerspectiveFieldOfView (new NFloat (Math.PI / 3), new NFloat (2), new NFloat (3), new NFloat (4), out var matrix);
#else
			SCNMatrix4.CreatePerspectiveFieldOfView ((pfloat) (Math.PI / 3), 2, 3, 4, out var matrix);
#endif
			AssertEqual (matrix, "CreatePerspectiveFieldOfView",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				SqrtThreeHalved, new NFloat (0), new NFloat (0), new NFloat (0),
				new NFloat (0), SqrtThree, new NFloat (0), new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (-7), new NFloat (-1),
				new NFloat (0), new NFloat (0), new NFloat (-24), new NFloat (0));
#else
				SqrtThreeHalved, 0, 0, 0,
				0, SqrtThree, 0, 0,
				0, 0, -7, -1,
				0, 0, -24, 0);
#endif
		}

		[Test]
		public void CreatePerspectiveFieldOfView ()
		{
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
			var matrix = SCNMatrix4.CreatePerspectiveFieldOfView (new NFloat (Math.PI / 3), new NFloat (2), new NFloat (3), new NFloat (4));
#else
			var matrix = SCNMatrix4.CreatePerspectiveFieldOfView ((pfloat) (Math.PI / 3), 2, 3, 4);
#endif
			AssertEqual (matrix, "CreatePerspectiveFieldOfView",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				SqrtThreeHalved, new NFloat (0), new NFloat (0), new NFloat (0),
				new NFloat (0), SqrtThree, new NFloat (0), new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (-7), new NFloat (-1),
				new NFloat (0), new NFloat (0), new NFloat (-24), new NFloat (0));
#else
				SqrtThreeHalved, 0, 0, 0,
				0, SqrtThree, 0, 0,
				0, 0, -7, -1,
				0, 0, -24, 0);
#endif
		}

		[Test]
		public void CreatePerspectiveOffCenter_Out ()
		{
			SCNMatrix4.CreatePerspectiveOffCenter (1, 2, 3, 4, 5, 6, out var matrix);
			AssertEqual (matrix, "CreatePerspectiveOffCenter",
				10, 0, 0, 0,
				0, 10, 0, 0,
				3, 7, -11, -1,
				0, 0, -60, 0);
		}

		[Test]
		public void CreatePerspectiveOffCenter ()
		{
			var matrix = SCNMatrix4.CreatePerspectiveOffCenter (1, 2, 3, 4, 5, 6);
			AssertEqual (matrix, "CreatePerspectiveOffCenter",
				10, 0, 0, 0,
				0, 10, 0, 0,
				3, 7, -11, -1,
				0, 0, -60, 0);
		}

		[Test]
		public void Scale ()
		{
			var matrix = SCNMatrix4.Scale (2);
			AssertEqual (matrix, "CreateScale",
				2, 0, 0, 0,
				0, 2, 0, 0,
				0, 0, 2, 0,
				0, 0, 0, 1);
		}

		[Test]
		public void Scale_Vector ()
		{
			var matrix = SCNMatrix4.Scale (new SCNVector3 (1, 2, 3));
			AssertEqual (matrix, "CreateScale",
				1, 0, 0, 0,
				0, 2, 0, 0,
				0, 0, 3, 0,
				0, 0, 0, 1);
		}

		[Test]
		public void Scale_3 ()
		{
			var matrix = SCNMatrix4.Scale (1, 2, 3);
			AssertEqual (matrix, "CreateScale",
				1, 0, 0, 0,
				0, 2, 0, 0,
				0, 0, 3, 0,
				0, 0, 0, 1);
		}

		[Test]
		public void Rotate ()
		{
			var quaternion = new Quaternion (1, 2, 3, 4);
			var matrix = SCNMatrix4.Rotate (quaternion);
			AssertEqual (matrix, "Rotate",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				TwoFifteenths, new NFloat (7 * TwoFifteenths.Value), new NFloat (-OneThird.Value), new NFloat (0),
				new NFloat (-TwoThirds.Value), OneThird, TwoThirds, new NFloat (0),
				new NFloat (11 * OneFifteenth.Value), TwoFifteenths, TwoThirds, new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (0), new NFloat (1));
#else
				TwoFifteenths, 7 * TwoFifteenths, -OneThird, 0,
				-TwoThirds, OneThird, TwoThirds, 0,
				11 * OneFifteenth, TwoFifteenths, TwoThirds, 0,
				0, 0, 0, 1);
#endif
		}

		[Test]
		public void Rotate_d ()
		{
			var quaternion = new Quaterniond (1, 2, 3, 4);
			var matrix = SCNMatrix4.Rotate (quaternion);
			AssertEqual (matrix, "Rotate",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				TwoFifteenths, new NFloat (7 * TwoFifteenths.Value), new NFloat (-OneThird.Value), new NFloat (0),
				new NFloat (-TwoThirds.Value), OneThird, TwoThirds, new NFloat (0),
				new NFloat (11 * OneFifteenth.Value), TwoFifteenths, TwoThirds, new NFloat (0),
				new NFloat (0), new NFloat (0), new NFloat (0), new NFloat (1));
#else
				TwoFifteenths, 7 * TwoFifteenths, -OneThird, 0,
				-TwoThirds, OneThird, TwoThirds, 0,
				11 * OneFifteenth, TwoFifteenths, TwoThirds, 0,
				0, 0, 0, 1);
#endif
		}

		[Test]
		public void LookAt_Vectors ()
		{
			var matrix = SCNMatrix4.LookAt (new SCNVector3 (1, 2, 3), new SCNVector3 (4, 5, 6), new SCNVector3 (7, 8, 9));
			AssertEqual (matrix, "LookAt",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				SqrtSixInverted, new NFloat (-SqrtTwoHalved.Value), new NFloat (-SqrtThreeInverted.Value), new NFloat (0),
				new NFloat (-2 * SqrtSixInverted.Value), new NFloat (0), new NFloat (-SqrtThreeInverted.Value), new NFloat (0),
				SqrtSixInverted, SqrtTwoHalved, new NFloat (-SqrtThreeInverted.Value), new NFloat (0),
				new NFloat (0), new NFloat (-SqrtTwo.Value), SqrtTwelve, new NFloat (1));
#else
				SqrtSixInverted, -SqrtTwoHalved, -SqrtThreeInverted, 0,
				-2 * SqrtSixInverted, 0, -SqrtThreeInverted, 0,
				SqrtSixInverted, SqrtTwoHalved, -SqrtThreeInverted, 0,
				0, -SqrtTwo, SqrtTwelve, 1);
#endif
		}

		[Test]
		public void LookAt_Elements ()
		{
			var matrix = SCNMatrix4.LookAt (1, 2, 3, 4, 5, 6, 7, 8, 9);
			AssertEqual (matrix, "LookAt",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				SqrtSixInverted, new NFloat (-SqrtTwoHalved.Value), new NFloat (-SqrtThreeInverted.Value), new NFloat (0),
				new NFloat (-2 * SqrtSixInverted.Value), new NFloat (0), new NFloat (-SqrtThreeInverted.Value), new NFloat (0),
				SqrtSixInverted, SqrtTwoHalved, new NFloat (-SqrtThreeInverted.Value), new NFloat (0),
				new NFloat (0), new NFloat (-SqrtTwo.Value), SqrtTwelve, new NFloat (1));
#else
				SqrtSixInverted, -SqrtTwoHalved, -SqrtThreeInverted, 0,
				-2 * SqrtSixInverted, 0, -SqrtThreeInverted, 0,
				SqrtSixInverted, SqrtTwoHalved, -SqrtThreeInverted, 0,
				0, -SqrtTwo, SqrtTwelve, 1);
#endif
		}

		[Test]
		public void Mult ()
		{
			var a = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			var b = new SCNMatrix4 (
				new SCNVector4 (911, 912, 913, 914),
				new SCNVector4 (921, 922, 923, 924),
				new SCNVector4 (931, 932, 933, 934),
				new SCNVector4 (941, 942, 943, 944));
			var matrix = SCNMatrix4.Mult (a, b);
			AssertEqual (matrix, "Mult",
				46350, 46400, 46450, 46500,
				83390, 83480, 83570, 83660,
				120430, 120560, 120690, 120820,
				157470, 157640, 157810, 157980);
		}

		[Test]
		public void Mult_ByRef ()
		{
			var a = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			var b = new SCNMatrix4 (
				new SCNVector4 (911, 912, 913, 914),
				new SCNVector4 (921, 922, 923, 924),
				new SCNVector4 (931, 932, 933, 934),
				new SCNVector4 (941, 942, 943, 944));
			SCNMatrix4.Mult (ref a, ref b, out var matrix);
			AssertEqual (matrix, "Mult",
				46350, 46400, 46450, 46500,
				83390, 83480, 83570, 83660,
				120430, 120560, 120690, 120820,
				157470, 157640, 157810, 157980);
		}

#if NET // The legacy Invert implementation seems very wrong, so only verify .NET behavior
		[Test]
		public void Static_Invert ()
		{
			var a = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			var ex = Assert.Throws<InvalidOperationException> (() => SCNMatrix4.Invert (a), "Singular matrix");
			Assert.That (ex.Message, Does.Contain ("Matrix is singular and cannot be inverted"), "Singular matrix message");

			a = new SCNMatrix4 (
				3, 5, 8, 9,
				5, 3, 5, 8,
				9, 6, 4, 2,
				4, 6, 9, 8);

			var matrix = SCNMatrix4.Invert (a);

			AssertEqual (matrix, "Invert",
#if NO_NFLOAT_OPERATORS && !PFLOAT_SINGLE
				new NFloat (-0.6181818181818182), new NFloat (0.3151515151515151), new NFloat (-0.030303030303030304), new NFloat (0.3878787878787879),
				new NFloat (1.6363636363636365), new NFloat (-0.696969696969697), new NFloat (0.3939393939393939), new NFloat (-1.2424242424242424),
				new NFloat (-1.3818181818181818), new NFloat (0.3515151515151515), new NFloat (-0.30303030303030304), new NFloat (1.2787878787878788),
				new NFloat (0.6363636363636364), new NFloat (-0.030303030303030304), new NFloat (0.06060606060606061), new NFloat (-0.5757575757575758));
#else
				(pfloat) (-0.6181818181818182), (pfloat) (0.3151515151515151), (pfloat) (-0.030303030303030304), (pfloat) (0.3878787878787879),
				(pfloat) (1.6363636363636365), (pfloat) (-0.696969696969697), (pfloat) (0.3939393939393939), (pfloat) (-1.2424242424242424),
				(pfloat) (-1.3818181818181818), (pfloat) (0.3515151515151515), (pfloat) (-0.30303030303030304), (pfloat) (1.2787878787878788),
				(pfloat) (0.6363636363636364), (pfloat) (-0.030303030303030304), (pfloat) (0.06060606060606061), (pfloat) (-0.5757575757575758));
#endif
		}
#endif

		[Test]
		public void Static_Transpose ()
		{
			var a = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			var matrix = SCNMatrix4.Transpose (a);
			AssertEqual (matrix, "Transpose",
				11, 21, 31, 41,
				12, 22, 32, 42,
				13, 23, 33, 43,
				14, 24, 34, 44);
		}

		[Test]
		public void Static_Transpose_ByRef ()
		{
			var a = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			SCNMatrix4.Transpose (ref a, out var matrix);
			AssertEqual (matrix, "Transpose",
				11, 21, 31, 41,
				12, 22, 32, 42,
				13, 23, 33, 43,
				14, 24, 34, 44);
		}

		[Test]
		public void Operator_Multiply ()
		{
			var a = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			var b = new SCNMatrix4 (
				new SCNVector4 (911, 912, 913, 914),
				new SCNVector4 (921, 922, 923, 924),
				new SCNVector4 (931, 932, 933, 934),
				new SCNVector4 (941, 942, 943, 944));
			var matrix = a * b;
			AssertEqual (matrix, "*",
				46350, 46400, 46450, 46500,
				83390, 83480, 83570, 83660,
				120430, 120560, 120690, 120820,
				157470, 157640, 157810, 157980);
		}

		[Test]
		public void Operator_Equals ()
		{
			var a = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			var b = new SCNMatrix4 (
				new SCNVector4 (911, 912, 913, 914),
				new SCNVector4 (921, 922, 923, 924),
				new SCNVector4 (931, 932, 933, 934),
				new SCNVector4 (941, 942, 943, 944));
			Assert.IsFalse (a == b, "Equals");
		}

		[Test]
		public void Operator_NotEquals ()
		{
			var a = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			var b = new SCNMatrix4 (
				new SCNVector4 (911, 912, 913, 914),
				new SCNVector4 (921, 922, 923, 924),
				new SCNVector4 (931, 932, 933, 934),
				new SCNVector4 (941, 942, 943, 944));
			Assert.IsTrue (a != b, "NotEquals");
		}

		[Test]
		public void ToString ()
		{
			var matrix = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			Assert.AreEqual ("(11, 12, 13, 14)\n(21, 22, 23, 24)\n(31, 32, 33, 34)\n(41, 42, 43, 44)", matrix.ToString (), "ToString");
		}

		[Test]
		public void Object_Equals ()
		{
			var a = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			var b = new SCNMatrix4 (
				new SCNVector4 (911, 912, 913, 914),
				new SCNVector4 (921, 922, 923, 924),
				new SCNVector4 (931, 932, 933, 934),
				new SCNVector4 (941, 942, 943, 944));
			Assert.IsFalse (((object) a).Equals (b), "object.Equals");
		}

		[Test]
		public void IEquatable_Equals ()
		{
			var a = new SCNMatrix4 (
				new SCNVector4 (11, 12, 13, 14),
				new SCNVector4 (21, 22, 23, 24),
				new SCNVector4 (31, 32, 33, 34),
				new SCNVector4 (41, 42, 43, 44));
			var b = new SCNMatrix4 (
				new SCNVector4 (911, 912, 913, 914),
				new SCNVector4 (921, 922, 923, 924),
				new SCNVector4 (931, 932, 933, 934),
				new SCNVector4 (941, 942, 943, 944));
			Assert.IsFalse (((IEquatable<SCNMatrix4>) a).Equals (b), "object.Equals");
		}
	}
}
#endif // !__WATCHOS__
