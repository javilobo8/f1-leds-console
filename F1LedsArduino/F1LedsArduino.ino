#include <Adafruit_NeoPixel.h>
#include <math.h> 
#include "LedControlCustom.h"
#include "Constants.h"
#include "Utils.h"

// Serial
#define BAUDRATE 115200

// MAX7219
#define N_MAX 3
#define MATRIX 0
#define DIGIT_0 1
#define DIGIT_1 2
#define NELEMS(x) (sizeof(x) / sizeof((x)[0]))

#define DPIN 2
#define CSPIN 3
#define CLKPIN 4
#define MAX7219_INTENSITY 15

// NeoPixel Stick
#define NP_PIN 6
#define NUM_LEDS 8
#define NUM_LEDS_MULTP 8
#define NEO_MAX_BRIGHTNESS 255

LedControl lc = LedControl(DPIN, CLKPIN, CSPIN, N_MAX);
Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUM_LEDS, NP_PIN, NEO_GRB + NEO_KHZ800);

typedef struct
{
	uint32_t gear = 0;
	uint32_t kmh = 0;
	uint32_t rpm = 0;
	uint32_t lapTime = 0;
	uint32_t drsAllowed = 0;
	uint32_t drs = 0;
} SerialData;

SerialData data;

void setup() {
	strip.begin();
	strip.setBrightness(NEO_MAX_BRIGHTNESS / 2);
	strip.clear();
	strip.show();

	lc.shutdown(MATRIX, false);
	lc.setIntensity(MATRIX, MAX7219_INTENSITY);
	lc.clearDisplay(MATRIX);

	lc.shutdown(DIGIT_0, false);
	lc.setIntensity(DIGIT_0, MAX7219_INTENSITY);
	lc.clearDisplay(DIGIT_0);

	lc.shutdown(DIGIT_1, false);
	lc.setIntensity(DIGIT_1, MAX7219_INTENSITY);
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
	
	// Test
	for (size_t i = 0; i < NUM_LEDS * NUM_LEDS_MULTP + 1; i++) {
		data.rpm = i;
		display();
		delay(20);
	}

	Serial.begin(BAUDRATE);
}

const size_t packet_size = 24;
char messageBuffer[packet_size];

void loop() {
	if (Serial.available() >= packet_size) {
		Serial.readBytes(messageBuffer, packet_size);
		memcpy(&data, &messageBuffer, packet_size);
		display();
	}
}

void printNumber(uint8_t disp, uint8_t offset, uint8_t max_digits, uint32_t number, byte dp) {
	uint8_t digits = countDigits(number);
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
		if (dp && dp == j)
		  b += B10000000;
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

void printLEDStrip(uint32_t rpm, uint32_t drsAllowed, uint32_t drs) {
	int pos = rpm / NUM_LEDS_MULTP;
	for (int i = 0; i < NUM_LEDS; i++) {
		switch (i) {
		case 0:
			if (drsAllowed == 1 && drs == 0) {
				strip.setPixelColor(i, C_YELLOW);
				break;
			} else if (drs == 1) {
				strip.setPixelColor(i, C_MAGENTA);
				break;
			}
		default:
			if (i < pos) {
				strip.setPixelColor(i, LED_COLORS[i]);

				// Set next led brightness
				if (i == pos - 1) {
					int mod = ceil(rpm % NUM_LEDS_MULTP);
					if (mod != 0) {
						int nex_pos = i + 1;
						int b = (int)(mod * NEO_MAX_BRIGHTNESS / NUM_LEDS);
						strip.setPixelColor(nex_pos, changeBrightness(LED_COLORS[nex_pos], b));
					}
				}
			} else if (i == 0 || i > pos) {
				strip.setPixelColor(i, 0);
			}	
		}
	}
	strip.show();
}

void display() {
	printLEDStrip(data.rpm, data.drsAllowed, data.drs);
	printGear(data.gear);
	printNumber(DIGIT_0, 0, 3, data.kmh, 0);
	// printNumber(DIGIT_1, 0, 6, data.lapTime, 1);
}