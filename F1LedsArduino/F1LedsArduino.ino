#include <LedControlMS.h>
#define N_MAX 3        // Cuantas matrices vamos a usar
#define MATRIX 0
#define DIGIT_0 1
#define DIGIT_1 2
#define NELEMS(x)  (sizeof(x) / sizeof((x)[0]))

#define DPIN 2
#define CSPIN 3
#define CLKPIN 4
#define INTENSITY 0

byte NUMBERS[] = {
	B01111110, // 0
	B00110000, // 1
	B01101101, // 2
	B01111001, // 3
	B00110011, // 4
	B01011011, // 5
	B01011111, // 6
	B01110000, // 7
	B01111111, // 8 
	B01111011, // 9
};

const byte GEARS[][8] = {
	{
		B01111100,
		B01111110,
		B01100110,
		B01100110,
		B01111100,
		B01100110,
		B01100110,
		B01100110
	},{
		B01100110,
		B01110110,
		B01110110,
		B01111110,
		B01111110,
		B01101110,
		B01101110,
		B01100110
	},{
		B00011000,
		B00111000,
		B00111000,
		B00011000,
		B00011000,
		B00011000,
		B00011000,
		B00011000
	},{
		B00111100,
		B01111110,
		B00000110,
		B00111110,
		B01111100,
		B01100000,
		B01111110,
		B01111110
	},{
		B01111100,
		B01111110,
		B00000110,
		B01111110,
		B01111110,
		B00000110,
		B01111110,
		B01111100
	},{
		B01100110,
		B01100110,
		B01100110,
		B01111110,
		B01111110,
		B00000110,
		B00000110,
		B00000110
	},{
		B01111110,
		B01111110,
		B01100000,
		B01111100,
		B01111110,
		B00000110,
		B01111110,
		B01111100
	},{
		B00111100,
		B01111110,
		B01100000,
		B01111100,
		B01111110,
		B01100110,
		B01111110,
		B00111100
	},{
		B01111110,
		B00111110,
		B00000110,
		B00000110,
		B00000110,
		B00000110,
		B00000110,
		B00000110
	},{
		B00111100,
		B01111110,
		B01100110,
		B01111110,
		B01111110,
		B01100110,
		B01111110,
		B00111100
	},{
		B01111110,
		B01111110,
		B01100110,
		B01111110,
		B00111110,
		B00000110,
		B00000110,
		B00000110
	}
};

// unsigned int = [0...32767]
typedef struct
{
	uint32_t gear = 0;
	uint32_t kmh = 0;
	uint32_t rpm = 0;
	uint32_t lapTime = 0;
} SerialData;

SerialData data;

const size_t packet_size = 16;
char messageBuffer[packet_size];

LedControl lc = LedControl(DPIN, CLKPIN, CSPIN, N_MAX);

void setup() {
	lc.shutdown(MATRIX, false);
	lc.setIntensity(MATRIX, 0);
	lc.clearDisplay(MATRIX);

	lc.shutdown(DIGIT_0, false);
	lc.setIntensity(DIGIT_0, 1);
	lc.clearDisplay(DIGIT_0);

	lc.shutdown(DIGIT_1, false);
	lc.setIntensity(DIGIT_1, 1);
	lc.clearDisplay(DIGIT_1);

	for (int h = 1; h >= 0; h--) {
		for (size_t i = 0; i < N_MAX; i++) {
			for (size_t j = 0; j < 8; j++) {
				for (size_t k = 0; k < 8; k++) {
					lc.setLed(i, j, k, h);
					delay(2);
				}
			}
		}
	}
		
	Serial.begin(14400);
}

void loop() {
	if (Serial.available() >= packet_size) {
		Serial.readBytes(messageBuffer, packet_size);
		memcpy(&data, &messageBuffer, packet_size);
	}
	display();
}

int power(int value, int exponent) {
	return 0.5 + pow(value, exponent);
}

int count_digits(int arg) {
	return snprintf(NULL, 0, "%d", arg) - (arg < 0);
}

void printNumber(uint8_t disp, uint8_t offset, uint8_t max_digits, uint32_t number) {
	uint8_t digits = count_digits(number);
	uint8_t arr[max_digits];
	uint8_t arr_size = NELEMS(arr);

	for (size_t i = 0; i < arr_size; i++) {
		if (number == 0) {
			arr[i] = 0;
		}
		else {
			arr[i] = number % 10;
			number = number / 10;
		}
	}
	for (size_t j = 0; j < arr_size; j++) {
		byte b = NUMBERS[arr[j]];
		// if (j == 1)
		//	 b += B10000000;
		if (arr[j] == 0 && j + 1 > digits)
			lc.setRow(disp, j + offset, B00000000);
		else
			lc.setRow(disp, j + offset, b);
	}
}

void printGear(int gear) {
	for (size_t row = 0; row < 8; row++) {
		lc.setRow(MATRIX, row, GEARS[gear][row]);
	}
}

void display() {
	printGear(data.gear);
	printNumber(DIGIT_0, 0, 3, data.kmh);
	printNumber(DIGIT_1, 0, 5, data.lapTime);
}
