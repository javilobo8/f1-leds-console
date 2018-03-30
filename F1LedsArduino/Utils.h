#pragma once

int power(int value, int exponent) {
	return 0.5 + pow(value, exponent);
}

int countDigits(int arg) {
	return snprintf(NULL, 0, "%d", arg) - (arg < 0);
}

uint32_t changeBrightness(uint32_t c, int brightness) {
	uint8_t
		r = (uint8_t)(c >> 16),
		g = (uint8_t)(c >> 8),
		b = (uint8_t)c;
	r = (r * brightness) >> 8;
	g = (g * brightness) >> 8;
	b = (b * brightness) >> 8;
	uint32_t rgb = r;
	rgb = (rgb << 8) + g;
	rgb = (rgb << 8) + b;
	return rgb;
}