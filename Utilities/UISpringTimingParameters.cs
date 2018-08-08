using System;
using UIKit;
using CoreGraphics;

public static class UISpringTiming {
	/// A design-friendly way to create a spring timing curve.
	/// damping: The 'bounciness' of the animation. Value must be between 0 and 1.
	/// response: The 'speed' of the animation.
	/// initialVelocity: The vector describing the starting motion of the property. Use CGVector.Zero if not desired
	public static UISpringTimingParameters MakeTimingParameters (nfloat damping, nfloat response, CGVector initialVelocity)
	{
		var stiffness = NMath.Pow (2 * NMath.PI / response, 2);
		var damp = 4 * NMath.PI * damping / response;

		return new UISpringTimingParameters (mass: 1, stiffness: stiffness, damping: damp, velocity: initialVelocity);
	}

	public static UISpringTimingParameters MakeTimingParameters (nfloat damping, nfloat response) => MakeTimingParameters (damping, response, new CGVector (0, 0));
}