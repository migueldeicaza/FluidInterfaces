using System;
using CoreGraphics;


public static class CGPointExtensions {
	public static nfloat Distance (this CGPoint self, CGPoint to)
	{
		return NMath.Sqrt (NMath.Pow (to.X - self.X, 2) + NMath.Pow (to.Y - self.Y, 2));
	}
}
