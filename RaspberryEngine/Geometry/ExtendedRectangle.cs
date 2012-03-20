using System;
using Microsoft.Xna.Framework;

namespace RaspberryEngine.Geometry
{
	public class ExtendedRectangle
	{
		
		public ExtendedRectangle (Rectangle rectangle)
		{
			new ExtendedRectangle(rectangle, 0);
		}
		
		public ExtendedRectangle (Rectangle rectangle, float initialRotation)
		{
			// if origin is not specified, assume its in the center of the rectangle
			new ExtendedRectangle(rectangle, initialRotation, new Vector2 ((int)rectangle.Width/2, (int)rectangle.Height/2));
		}
		
        public ExtendedRectangle (Rectangle rectangle, float initialRotation, Vector2 origin)
		{
			
			Rotation = initialRotation;
			Origin = origin;		
			
			
			CollisionRectangle = rectangle;
			Rectangle tmp = rectangle;
			
		
			tmp.X -= (int)origin.X;
			tmp.Y -= (int)origin.Y;
			
			UpperLeft = new Vector2 (tmp.Left, tmp.Top);
			UpperRight = new Vector2 (tmp.Right, tmp.Top);
			LowerLeft = new Vector2 (tmp.Left, tmp.Bottom);
			LowerRight = new Vector2 (tmp.Right, tmp.Bottom);
		
		
		}
		
		   public void Rotate (Vector2 origin, float rotation)
		{
			UpperLeft = RotateVector (UpperLeft, origin, rotation);
			UpperRight = RotateVector (UpperRight, origin, rotation);
			LowerLeft = RotateVector (LowerLeft, origin, rotation);
			LowerRight = RotateVector (LowerRight, origin, rotation);
		}

		public void RotateInverse (Vector2 origin, float rotation)
		{
			UpperLeft = RotateVectorInverse (UpperLeft, origin, rotation);
			UpperRight = RotateVectorInverse (UpperRight, origin, rotation);
			LowerLeft = RotateVectorInverse (LowerLeft, origin, rotation);
			LowerRight = RotateVectorInverse (LowerRight, origin, rotation);
		}

		public void AddVector (Vector2 vector)
		{
			UpperLeft += vector;
			UpperRight += vector;
			LowerLeft += vector;
			LowerRight += vector;
		}

		public float MinX ()
		{
			float[] temp = { UpperLeft.X, UpperRight.X, LowerLeft.X, LowerRight.X };
			return Min (temp);
		}

		public float MaxX ()
		{
			float[] temp = { UpperLeft.X, UpperRight.X, LowerLeft.X, LowerRight.X };
			return Max (temp);
		}

		public float MinY ()
		{
			float[] temp = { UpperLeft.Y, UpperRight.Y, LowerLeft.Y, LowerRight.Y };
			return Min (temp);
		}

		public float MaxY ()
		{
			float[] temp = { UpperLeft.Y, UpperRight.Y, LowerLeft.Y, LowerRight.Y };
			return Max (temp);
		}
		
		#region Collision functions 
		
	
		
		protected Vector2[] SortCornersClockwise (Vector2[] corners)
		{
			
			
			// Sort points by Y-value
			Array.Sort (corners, delegate (Vector2 point1, Vector2 point2) {
				return point1.Y.CompareTo (point2.Y);	
			});
			
			// the first and second now contains the ones with highest values
			if (corners [0].X > corners [1].X) {
				// swap
				Vector2 tmp = corners [0];
				corners [0] = corners [1];
				corners [1] = tmp;
			}
								
			if (corners [2].X > corners [3].X) {
				// swap
				Vector2 tmp = corners [2];
				corners [2] = corners [3];
				corners [3] = tmp;
			}
				
			return corners;
			
			
		}
		
		public Vector2[] RotatedCorners {
			get {
				Vector2 theOriginA = this.Origin;
				
				ExtendedRectangle rectA = new ExtendedRectangle (this.CollisionRectangle, this.Rotation, this.Origin);
				theOriginA += rectA.UpperLeft;
				rectA.Rotate (theOriginA, Rotation);
				
				
				Vector2[] corners = new Vector2[] {
					rectA.UpperLeft,
					rectA.UpperRight,
					rectA.LowerLeft,
					rectA.LowerRight
				};
				
				return corners;
			}
		}
		
		public Vector2[] SortedRotatedCorners {
			get {
				return SortCornersClockwise(RotatedCorners);
			}
		}
		
		
		
		
		
		public Vector2 MinXYPoint {
			get {
				
				ExtendedRectangle rectA = new ExtendedRectangle (this.CollisionRectangle, this.Rotation, this.Origin);
				rectA.Origin += rectA.UpperLeft;
				rectA.Rotate (rectA.Origin, Rotation);
					
				return new Vector2(rectA.MinX (), rectA.MinY());
			}
		}
		public Vector2 MaxXYPoint {
			get {
				
				ExtendedRectangle rectA = new ExtendedRectangle (this.CollisionRectangle, this.Rotation, this.Origin);
				rectA.Origin += rectA.UpperLeft;
				rectA.Rotate (rectA.Origin, Rotation);
					
				return new Vector2 (rectA.MaxX (), rectA.MaxY ());
			}
		}
		
		public Vector2 RotatePoint(Vector2 point) {
			Vector2 origin = this.Origin;
			origin += this.UpperLeft;
					
			return RotateVectorInverse (point, origin, this.Rotation);
					
		}
		
		public bool Contains (Vector2 point)
		{
			//if (Rotation != 0) {
			Rectangle notRotated = new Rectangle (CollisionRectangle.X- (int)Origin.X, CollisionRectangle.Y- (int)Origin.Y, CollisionRectangle.Width, CollisionRectangle.Height);
			
		
			
			return notRotated.Contains (RotatePoint (point));
			
			/*Vector2 theOriginA = this.Origin;
				
			ExtendedRectangle rectA = new ExtendedRectangle (this.CollisionRectangle, this.Rotation, this.Origin);
			theOriginA += rectA.UpperLeft;
			rectA.Rotate (theOriginA, Rotation);*/
			//rectA.AddVector (-theOriginA);
			//return point.X > rectA.MinX () && point.X < rectA.MaxX () && point.Y < rectA.MaxY () && point.Y > rectA.MinY ();
				
			// 0 = UL, 1 = UR, 2=LL, 3=LR
			/*Vector2[] corners = rectA.SortedRotatedCorners;
			Vector2 ul = corners [0];
			Vector2 ur = corners [1];
			Vector2 ll = corners [2];
			Vector2 lr = corners [3];
				
			Console.WriteLine ("Using corners: ");
			Console.WriteLine (point.X + " > " + ul.X + " && " + point.X + " < " + lr.X + " && " + point.Y + " < " + ll.Y + " && " + point.Y + " > " + ur.Y);
			*/	
			
			/*Vector2 rotatedPoint = RotateVectorInverse (point, theOriginA, Rotation);
			return this.CollisionRectangle.Contains (rotatedPoint);
			*/
			
			
			//return point.X > ul.X && point.X < lr.X && point.Y < ll.Y && point.Y > ur.Y;
			// Leta rätt på centrum av rektanglen och sortera ta reda på graderna. Sortera sedan från över och under
			// Den som är över 180 grader utifrån mittenpunkten. Stega sedan ifrån lower left, 
			// 
			//}
			//return this.CollisionRectangle.Contains (point);
			//return this.CollisionRectangle.Contains (new ExtendedRectangle (new Rectangle (point.X, point.Y, 1, 1)));
			//return false;
			//Vector2 theOriginA = this.Origin;
			//ExtendedRectangle rectA = this;
			//theOriginA += rectA.UpperLeft;
			
			
			
			//rectA.Rotate (this.Origin, this.Rotation);
			//rectA.AddVector (-this.Origin);
			
			//originA += rectA.UpperLeft;
			
			//rectA.Rotate (originA, this.Rotation);
			//rectA.RotateInverse (originA, this.Rotation);
			
			//rectA.AddVector (-originA);
			
			//Console.WriteLine (string.Format ("{0} > {1} && {0} < {2} && {3} > {4} && {3} < {5}", point.X, UpperLeft.X, LowerRight.X, point.Y, LowerLeft.Y, UpperRight.Y));
			
		}
		/*
		 * bool Inisde( x, y, l, r, b, t )//x,y are the point, l,r,b,t are the extents of the rectangle{   return x > l && x < r && y > b && y < t;}
		 * */
		
		
		
		public bool Intersects (ExtendedRectangle rectangle)
		{
			return Check (this.CollisionRectangle, this.Origin,this.Rotation, rectangle.CollisionRectangle,rectangle.Origin,rectangle.Rotation);
		}
		
		public bool Check (Rectangle theRectangleA, Vector2 theOriginA, float theRotationA, Rectangle theRectangleB, Vector2 theOriginB, float theRotationB)
		{
		
			
			ExtendedRectangle rectA = new ExtendedRectangle (theRectangleA, theRotationA, theOriginA);
			ExtendedRectangle rectB = new ExtendedRectangle (theRectangleB, theRotationB, theOriginB);

			theOriginA += rectA.UpperLeft;
			theOriginB += rectB.UpperLeft;
			
			rectB.Rotate (theOriginB, theRotationB);
			rectB.RotateInverse (theOriginA, theRotationA);

			rectA.AddVector (-theOriginA);
			rectB.AddVector (-theOriginA);

			if ((rectB.MinX () > rectA.MaxX ()) || (rectB.MaxX () < rectA.MinX ()) // x-axis of A
                || (rectB.MinY () > rectA.MaxY ()) || (rectB.MaxY () < rectA.MinY ()) // y-axis of A
                || (!CheckAxisCollision (rectA, rectB, rectB.UpperLeft - rectB.UpperRight)) // x-axis of B
                || (!CheckAxisCollision (rectA, rectB, rectB.UpperLeft - rectB.LowerLeft))) // y-axis of B
				return false;

			return true;
		}

        private bool CheckAxisCollision(ExtendedRectangle rectA, ExtendedRectangle rectB, Vector2 aAxis)
        {
            int[] aRectangleAScalars = {
                GenerateScalar(rectB.UpperLeft, aAxis),
                GenerateScalar(rectB.UpperRight, aAxis),
                GenerateScalar(rectB.LowerLeft, aAxis),
                GenerateScalar(rectB.LowerRight, aAxis)
            };

            int[] aRectangleBScalars = {
                GenerateScalar(rectA.UpperLeft, aAxis),
                GenerateScalar(rectA.UpperRight, aAxis),
                GenerateScalar(rectA.LowerLeft, aAxis),
                GenerateScalar(rectA.LowerRight, aAxis)
            };

            int aRectangleAMinimum = Min(aRectangleAScalars);
            int aRectangleAMaximum = Max(aRectangleAScalars);
            int aRectangleBMinimum = Min(aRectangleBScalars);
            int aRectangleBMaximum = Max(aRectangleBScalars);

            //If we have overlaps between the Rectangles (i.e. Min of B is less than Max of A)
            //then we are detecting a collision between the rectangles on this Axis
            if ((aRectangleBMinimum <= aRectangleAMaximum
                    && aRectangleBMaximum >= aRectangleAMaximum)
                || (aRectangleAMinimum <= aRectangleBMaximum
                    && aRectangleAMaximum >= aRectangleBMaximum))
                return true;

            return false;
        }

        private static int GenerateScalar(Vector2 theRectangleCorner, Vector2 theAxis)
        {
            // create projection of corner onto axis
            float aDivisionResult = Vector2.Dot(theRectangleCorner, theAxis) / Vector2.Dot(theAxis, theAxis);
            Vector2 aCornerProjected = aDivisionResult * theAxis;

            // return scalar of projection
            return (int)Vector2.Dot(theAxis, aCornerProjected);
        }
		
		#endregion
		
		#region Helper functions
		public Vector2 RotateVector (Vector2 vector, Vector2 origin, float angle)
		{
			return Vector2.Transform (vector, Matrix.CreateTranslation (-origin.X, -origin.Y, 0) * Matrix.CreateRotationZ (angle) * Matrix.CreateTranslation (origin.X, origin.Y, 0));
		}

		public Vector2 RotateVectorInverse (Vector2 vector, Vector2 origin, float angle)
		{
			return Vector2.Transform (vector, Matrix.Invert (Matrix.CreateTranslation (-origin.X, -origin.Y, 0) * Matrix.CreateRotationZ (angle) * Matrix.CreateTranslation (origin.X, origin.Y, 0)));
		}

		public float Min (float[] array)
		{
			float min = float.MaxValue;

			for (int i = 0; i < array.Length; i++) {
				if (array [i] < min)
					min = array [i];
			}

			return min;
		}

		public float Max (float[] array)
		{
			float max = float.MinValue;

			for (int i = 0; i < array.Length; i++) {
				if (array [i] > max)
					max = array [i];
			}

			return max;
		}

		public int Min (int[] array)
		{
			int min = int.MaxValue;

			for (int i = 0; i < array.Length; i++) {
				if (array [i] < min)
					min = array [i];
			}

			return min;
		}

		public  int Max (int[] array)
		{
			int max = int.MinValue;

			for (int i = 0; i < array.Length; i++) {
				if (array [i] > max)
					max = array [i];
			}

			return max;
		}
		#endregion
		
		public Vector2 UpperLeft;
		public Vector2 UpperRight;
		public Vector2 LowerLeft;
		public Vector2 LowerRight;
	
		public Rectangle CollisionRectangle {
			get;
			set;
		}
		
		public float Rotation {
			get;
			set;
		}

		public Vector2 Origin {
			get;
			set;
		}
		
	}
}
