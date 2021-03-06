﻿using System;
using UIKit;

public class Color {
	public static UIColor FromHex (int hex, float alpha = 1)
	{
		var r = ((hex & 0xff0000) >> 16) / 255f;
		var g = ((hex & 0xff00) >> 8) / 255f;
		var b = ((hex & 0xff)) / 255f;
		return new UIColor ((nfloat)r, (nfloat)g, (nfloat)b, alpha);
	}
}
