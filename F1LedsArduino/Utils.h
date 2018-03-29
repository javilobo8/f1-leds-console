#pragma once

int power(int value, int exponent) {
	return 0.5 + pow(value, exponent);
}

int countDigits(int arg) {
	return snprintf(NULL, 0, "%d", arg) - (arg < 0);
}