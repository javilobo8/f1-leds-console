#pragma once

int power(int value, int exponent) {
	return 0.5 + pow(value, exponent);
}

int countDigits(int arg) {
	return snprintf(NULL, 0, "%d", arg) - (arg < 0);
}

void rotateByteArray(const byte* source, byte* dest, size_t length)
{
	/* for each bit position starting from first */
	for (size_t j = 0; j < 8; ++j)
	{
		/* this is the mask of the i-th bit in source data */
		const unsigned char mask = 1 << j;

		/* for each row in source data (which will become column) */
		for (size_t i = 0; i < length; ++i)
		{
			/* if j-th bit of i-th row set */
			if (source[i] & mask)
				/* then set i-th bit of j-th row */
				dest[j] |= 1 << i;
		}
	}
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