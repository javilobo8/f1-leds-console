#include <LedControlMS.h>
#define N_MAX 3       // Cuantas matrices vamos a usar
#define MATRIX 0
#define DIGIT_0 1
#define DIGIT_1 2
#define NELEMS(x)  (sizeof(x) / sizeof((x)[0]))

#define DPIN 2
#define CSPIN 3
#define CLKPIN 4

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
	} };

typedef struct
{
	unsigned int rpm = 0;
	unsigned int drs = 0;
	unsigned int gear = 0;
	unsigned int kmh = 0;
	unsigned int lapTime = 0;
} SerialData;

SerialData data;

char messageBuffer[10];

LedControl lc = LedControl(DPIN, CLKPIN, CSPIN, N_MAX);

void setup() {
	for (int i = 0; i < N_MAX; i++) {
		lc.shutdown(i, false);
		lc.setIntensity(i, 1);
		lc.clearDisplay(i);
	}
	Serial.begin(9600);
}

void loop() {
	while (true) {
		if (Serial.available() >= 10) {
			Serial.readBytes(messageBuffer, 10);
			memcpy(&data, &messageBuffer, 10);
		}
		display();
	}
}

int power(int value, int exponent) {
	return 0.5 + pow(value, exponent);
}

int count_digits(int arg) {
	return snprintf(NULL, 0, "%d", arg) - (arg < 0);
}

void printNumber(int disp, int offset, int max_digits, int num) {
	int number = num;
	int arr[max_digits];
	int arr_size = NELEMS(arr);

	for (int i = 0; i < arr_size; i++) {
		if (number == 0) {
			arr[i] = 0;
		}
		else {
			arr[i] = number % 10;
			number = number / 10;
		}
	}
	for (int j = 0; j < arr_size; j++) {
		lc.setRow(disp, j + offset, NUMBERS[arr[j]]);
	}
}

void printGear(int gear) {
	for (int row = 0; row < 8; row++) {
		lc.setRow(MATRIX, row, GEARS[gear][row]);
	}
}

void display() {
	printGear(data.gear);
	printNumber(1, 0, 3, (int)data.kmh);
	// printNumber(2, 0, 6, (int)data.lapTime);
}
