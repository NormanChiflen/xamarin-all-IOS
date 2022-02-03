
using System;
using System.Diagnostics;

using Foundation;
using ObjCRuntime;

#if NET
using MatrixFloat3x3 = global::CoreGraphics.NMatrix3;
#else
using OpenTK;
using MatrixFloat3x3 = global::OpenTK.NMatrix3;
#endif

using NUnit.Framework;

namespace MonoTouchFixtures.Simd
{
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MatrixFloat3x3Test
	{
		[Test]
		public void Identity ()
		{
			var identity = new MatrixFloat3x3 {
				R0C0 = 1f,
				R1C1 = 1f,
				R2C2 = 1f,
			};
			Asserts.AreEqual (identity, MatrixFloat3x3.Identity, "identity");
#if !NET
			Asserts.AreEqual (Matrix3.Identity, MatrixFloat3x3.Identity, "opentk identity");
#endif
		}

#if !NET //we no longer have OpentK.Matrix3 for comparison
		[Test]
		public void ElementConstructor ()
		{
			var expected = GetTestMatrix ();
			var actual = new MatrixFloat3x3 (expected.R0C0, expected.R0C1, expected.R0C2,
											 expected.R1C0, expected.R1C1, expected.R1C2,
											 expected.R2C0, expected.R2C1, expected.R2C2);
			Asserts.AreEqual (expected, actual, "ctor 1");

		}

		[Test]
		public void Determinant ()
		{
			var expected = GetTestMatrix ();
			var actual = (MatrixFloat3x3) expected;
			Assert.AreEqual (expected.Determinant, actual.Determinant, 0.000001f, "determinant\n" + actual);
		}

		[Test]
		public void Elements ()
		{
			var expected = GetTestMatrix ();
			var actual = (MatrixFloat3x3) expected;

			Assert.AreEqual (expected.R0C0, actual.R0C0, "R0C0 getter");
			Assert.AreEqual (expected.R0C1, actual.R0C1, "R0C1 getter");
			Assert.AreEqual (expected.R0C2, actual.R0C2, "R0C2 getter");
			Assert.AreEqual (expected.R1C0, actual.R1C0, "R1C0 getter");
			Assert.AreEqual (expected.R1C1, actual.R1C1, "R1C1 getter");
			Assert.AreEqual (expected.R1C2, actual.R1C2, "R1C2 getter");
			Assert.AreEqual (expected.R2C0, actual.R2C0, "R2C0 getter");
			Assert.AreEqual (expected.R2C1, actual.R2C1, "R2C1 getter");
			Assert.AreEqual (expected.R2C2, actual.R2C2, "R2C2 getter");

			var newExpected = GetTestMatrix ();
			actual.R0C0 = newExpected.R0C0;
			actual.R0C1 = newExpected.R0C1;
			actual.R0C2 = newExpected.R0C2;
			actual.R1C0 = newExpected.R1C0;
			actual.R1C1 = newExpected.R1C1;
			actual.R1C2 = newExpected.R1C2;
			actual.R2C0 = newExpected.R2C0;
			actual.R2C1 = newExpected.R2C1;
			actual.R2C2 = newExpected.R2C2;
			Assert.AreEqual (newExpected.R0C0, actual.R0C0, "R0C0 setter");
			Assert.AreEqual (newExpected.R0C1, actual.R0C1, "R0C1 setter");
			Assert.AreEqual (newExpected.R0C2, actual.R0C2, "R0C2 setter");
			Assert.AreEqual (newExpected.R1C0, actual.R1C0, "R1C0 setter");
			Assert.AreEqual (newExpected.R1C1, actual.R1C1, "R1C1 setter");
			Assert.AreEqual (newExpected.R1C2, actual.R1C2, "R1C2 setter");
			Assert.AreEqual (newExpected.R2C0, actual.R2C0, "R2C0 setter");
			Assert.AreEqual (newExpected.R2C1, actual.R2C1, "R2C1 setter");
			Assert.AreEqual (newExpected.R2C2, actual.R2C2, "R2C2 setter");
		}

		[Test]
		public void TransposeInstance ()
		{
			var expected = GetTestMatrix ();
			var actual = (MatrixFloat3x3) expected;

			expected.Transpose ();
			actual.Transpose ();

			Asserts.AreEqual (expected, actual, "transpose");
		}

		[Test]
		public void TransposeStatic ()
		{
			var input = GetTestMatrix ();
			var inputSimd = (MatrixFloat3x3) input;

			Matrix3 expected;
			Matrix3.Transpose (ref input, out expected);
			var actual = MatrixFloat3x3.Transpose (inputSimd);

			Asserts.AreEqual (expected, actual, "transpose");

			input = GetTestMatrix ();
			inputSimd = (MatrixFloat3x3) input;
			Matrix3.Transpose (ref input, out expected);
			MatrixFloat3x3.Transpose (ref inputSimd, out actual);
			Asserts.AreEqual (expected, actual, "transpose out/ref");
		}

		[Test]
		public void TransposeStatic_ByRef ()
		{
			var input = GetTestMatrix ();
			var inputSimd = (MatrixFloat3x3) input;

			Matrix3 expected;
			MatrixFloat3x3 actual;

			Matrix3.Transpose (ref input, out expected);
			MatrixFloat3x3.Transpose (ref inputSimd, out actual);
			Asserts.AreEqual (expected, actual, "transpose out/ref");
		}

		[Test]
		public void Multiply ()
		{
			var inputL = GetTestMatrix ();
			var inputR = GetTestMatrix ();
			var inputSimdL = (MatrixFloat3x3) inputL;
			var inputSimdR = (MatrixFloat3x3) inputR;
			Matrix3 expected;
			Matrix3.Multiply (ref inputR, ref inputL, out expected); // OpenTK.Matrix3 got left/right mixed up...
			var actual = MatrixFloat3x3.Multiply (inputSimdL, inputSimdR);

			Asserts.AreEqual (expected, actual, "multiply");
		}

		[Test]
		public void Multiply_ByRef ()
		{
			var inputL = GetTestMatrix ();
			var inputR = GetTestMatrix ();
			var inputSimdL = (MatrixFloat3x3) inputL;
			var inputSimdR = (MatrixFloat3x3) inputR;
			Matrix3 expected;
			MatrixFloat3x3 actual;

			Matrix3.Multiply (ref inputR, ref inputL, out expected); // OpenTK.Matrix3 got left/right mixed up...
			MatrixFloat3x3.Multiply (ref inputSimdL, ref inputSimdR, out actual);

			Asserts.AreEqual (expected, actual, "multiply");
		}


		[Test]
		public void Multiply_Operator ()
		{
			var inputL = GetTestMatrix ();
			var inputR = GetTestMatrix ();
			var inputSimdL = (MatrixFloat3x3) inputL;
			var inputSimdR = (MatrixFloat3x3) inputR;
			Matrix3 expected;
			Matrix3.Multiply (ref inputR, ref inputL, out expected); // OpenTK.Matrix3 got left/right mixed up...
			var actual = inputSimdL * inputSimdR;

			Asserts.AreEqual (expected, actual, "multiply");
		}

		[Test]
		public void Equality_Operator ()
		{
			var inputL = GetTestMatrix ();
			var inputR = GetTestMatrix ();
			var inputSimdL = (MatrixFloat3x3) inputL;
			var inputSimdR = (MatrixFloat3x3) inputR;

			// matrices are different
			Assert.AreEqual (inputL.Equals (inputR), inputSimdL == inputSimdR, "inequality");
			Assert.IsFalse (inputL.Equals (inputR), "inequality 2 expected");
			Assert.IsFalse (inputSimdL == inputSimdR, "inequality 2 actual");

			inputL = inputR;
			inputSimdL = inputSimdR;
			// matrices are identical
			Assert.AreEqual (inputL.Equals (inputR), inputSimdL == inputSimdR, "equality");
			Assert.IsTrue (inputL.Equals (inputR), "equality 2 expected");
			Assert.IsTrue (inputSimdL == inputSimdR, "equality 2 actual");

			Assert.IsTrue (MatrixFloat3x3.Identity == (MatrixFloat3x3) Matrix3.Identity, "identity equality");
		}

		[Test]
		public void Inequality_Operator ()
		{
			var inputL = GetTestMatrix ();
			var inputR = GetTestMatrix ();
			var inputSimdL = (MatrixFloat3x3) inputL;
			var inputSimdR = (MatrixFloat3x3) inputR;

			// matrices are different
			Assert.AreEqual (!inputL.Equals (inputR), inputSimdL != inputSimdR, "inequality");
			Assert.IsTrue (!inputL.Equals (inputR), "inequality 2 expected");
			Assert.IsTrue (inputSimdL != inputSimdR, "inequality 2 actual");

			inputL = inputR;
			inputSimdL = inputSimdR;
			// matrices are identical
			Assert.AreEqual (!inputL.Equals (inputR), inputSimdL != inputSimdR, "equality");
			Assert.IsFalse (!inputL.Equals (inputR), "equality 2 expected");
			Assert.IsFalse (inputSimdL != inputSimdR, "equality 2 actual");

			Assert.IsFalse (MatrixFloat3x3.Identity != (MatrixFloat3x3) Matrix3.Identity, "identity equality");
		}

		[Test]
		public void Explicit_Operator_ToMatrix3 ()
		{
			var expected = (MatrixFloat3x3) GetTestMatrix ();
			var actual = (Matrix3) expected;

			Asserts.AreEqual (expected, actual, "tomatrix4");

			actual = (Matrix3) MatrixFloat3x3.Identity;
			Asserts.AreEqual (MatrixFloat3x3.Identity, actual, "tomatrix4 identity");
			Asserts.AreEqual (Matrix3.Identity, actual, "tomatrix4 identity2");
		}

		[Test]
		public void Explicit_Operator_FromMatrix3 ()
		{
			var expected = GetTestMatrix ();
			var actual = (MatrixFloat3x3) expected;

			Asserts.AreEqual (expected, actual, "frommatrix4");

			actual = (MatrixFloat3x3) Matrix3.Identity;
			Asserts.AreEqual (MatrixFloat3x3.Identity, actual, "tomatrix4 identity");
			Asserts.AreEqual (Matrix3.Identity, actual, "tomatrix4 identity2");
		}
#endif // !NET
		[Test]
		public void ToStringTest ()
		{
			var actual = new MatrixFloat3x3 (1, 2, 3, 4, 5, 6, 7, 8, 9);

			Assert.AreEqual ("(1, 2, 3)\n(4, 5, 6)\n(7, 8, 9)", actual.ToString (), "tostring");
		}

		// GetHashCode doesn't have to be identical, so no need to test

#if !NET
		[Test]
		public void Equals_Object ()
		{
			var expectedA = GetTestMatrix ();
			var expectedB = GetTestMatrix ();
			var actualA = (MatrixFloat3x3) expectedA;
			var actualB = (MatrixFloat3x3) expectedB;

			Assert.IsTrue (actualA.Equals ((object) actualA), "self");
			Assert.IsFalse (actualA.Equals ((object) actualB), "other");
			Assert.IsFalse (actualA.Equals (null), "null");
			Assert.IsFalse (actualA.Equals (expectedA), "other type");
		}

		[Test]
		public void Equals_Matrix ()
		{
			var expectedA = GetTestMatrix ();
			var expectedB = GetTestMatrix ();
			var actualA = (MatrixFloat3x3) expectedA;
			var actualB = (MatrixFloat3x3) expectedB;

			Assert.IsTrue (actualA.Equals (actualA), "self");
			Assert.IsFalse (actualA.Equals (actualB), "other");
		}

		// A collection of test matrices.
		//
		// I initially tried randomly generating test matrices, but it turns out
		// there are accumulative computational differences in the different algorithms
		// between Matrix3 and MatrixFloat3x3. Since the differences are accumulative,
		// I couldn't find a minimal sensible delta values when comparing 
		// matrices.
		//
		// So I just serialized a few matrices that were randomly generated, and
		// these have been tested to not produce accumulative computational differences.
		// 
		static Matrix3 [] test_matrices = new [] {
			new Matrix3 (3, 5, 7, 11, 13, 17, 19, 23, 29),
			new Matrix3 (5, 7, 11, 13, 17, 19, 23, 29, 31),
			new Matrix3 (7, 11, 13, 17, 19, 23, 29, 31, 37),
			new Matrix3 (0.1532144f, 0.5451511f, 0.2004739f, 0.8351463f, 0.9884372f, 0.1313103f, 0.3327205f, 0.01164342f, 0.6563147f),
			new Matrix3 (0.7717745f, 0.559364f, 0.00918373f, 0.6579159f, 0.123461f, 0.9993145f, 0.5487496f, 0.2823398f, 0.9710717f),
			new Matrix3 (0.2023053f, 0.4701468f, 0.6618567f, 0.7685714f, 0.8561344f, 0.009231919f, 0.6150167f, 0.7542298f, 0.550727f),
			new Matrix3 (9.799572E+08f, 1.64794E+09f, 1.117296E+09f, 1.239858E+09f, 6.389504E+07f, 1.172175E+09f, 1.399567E+09f, 1.187143E+09f, 3.729208E+07f),
			new Matrix3 (1.102396E+09f, 3.082477E+08f, 1.126484E+09f, 5.022931E+08f, 1.966322E+09f, 1.1814E+09f, 8.464673E+08f, 1.940651E+09f, 1.229937E+09f),
			new Matrix3 (2.263112E+08f, 8.79644E+08f, 1.303282E+09f, 1.654159E+09f, 3.705524E+08f, 1.984941E+09f, 2.175935E+07f, 4.633518E+08f, 1.801243E+09f),
			new Matrix3 (0.4904693f, 0.841727f, 0.2294401f, 0.5736054f, 0.5406881f, 0.2172498f, 0.1261143f, 0.6736677f, 0.4570194f),
			new Matrix3 (0.1252193f, 0.08986127f, 0.3407605f, 0.9144857f, 0.340791f, 0.2192288f, 0.5144276f, 0.01813344f, 0.07687104f),
			new Matrix3 (8.176959E+08f, 1.386156E+09f, 5.956444E+08f, 4.210506E+08f, 1.212676E+09f, 4.131035E+08f, 1.032453E+09f, 2.074689E+08f, 1.536594E+09f),
			new Matrix3 (0.006755914f, 0.07464754f, 0.287938f, 0.3724834f, 0.1496783f, 0.6224982f, 0.7150125f, 0.5554719f, 0.4638171f),
		};
	
		static int counter;
		internal static Matrix3 GetTestMatrix ()
		{
			counter++;
			if (counter == test_matrices.Length)
				counter = 0;
			return test_matrices [counter];
		}
#endif // !NET
	}
}
