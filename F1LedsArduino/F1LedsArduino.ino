#include <Adafruit_NeoPixel.h>
#include <math.h> 
#include "LedControlCustom.h"
#include "Constants.h"
#include "Utils.h"

#define NELEMS(x) (sizeof(x) / sizeof((x)[0]))

// Serial
#define BAUDRATE 115200

// MAX7219
#define MATRIX_DEVICES 1
#define MATRIX_0 0

#define MATRIX_DPIN 2
#define MATRIX_CSPIN 3
#define MATRIX_CLKPIN 4
#define MATRIX_INTENSITY 1

#define DIGIT_DEVICES 2 // 2
#define DIGIT_DPIN 8
#define DIGIT_CSPIN 9
#define DIGIT_CLKPIN 10
#define DIGIT_INTENSITY 1
#define DIGIT_0 0
#define DIGIT_1 1

// NeoPixel Stick
#define NP_PIN 6
#define NUM_LEDS 8
#define NUM_LEDS_MULTP 8
#define NEO_MAX_BRIGHTNESS 255

LedControl led_matrix = LedControl(MATRIX_DPIN, MATRIX_CLKPIN, MATRIX_CSPIN, 1);
LedControl led_digit = LedControl(DIGIT_DPIN, DIGIT_CLKPIN, DIGIT_CSPIN, 2);
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
	
	for (int h = 1; h >= 0; h--) {
		for (size_t i = 0; i < MATRIX_DEVICES; i++) {
			led_matrix.shutdown(i, false);
			led_matrix.setIntensity(i, MATRIX_INTENSITY);
			led_matrix.clearDisplay(i);
			for (size_t j = 0; j < 8; j++) {
				for (size_t k = 0; k < 8; k++) {
					led_matrix.setLed(i, j, k, h);
					delay(2);
				}
			}
		}

		for (size_t i = 0; i < DIGIT_DEVICES; i++) {
			led_digit.shutdown(i, false);
			led_digit.setIntensity(i, DIGIT_INTENSITY);
			led_digit.clearDisplay(i);
			for (size_t j = 0; j < 8; j++) {
				for (size_t k = 0; k < 8; k++) {
					led_digit.setLed(i, j, k, h);
					delay(2);
				}
			}
		}
	}
	
	// Setup NeoPixel Strip
	strip.begin();
	strip.setBrightness(NEO_MAX_BRIGHTNESS / 2);
	strip.clear();
	strip.show();

	for (uint32_t i = NUM_LEDS; i < NUM_LEDS * NUM_LEDS_MULTP + 1; i++) {
		data.rpm = i;
		display();
		delay(10);
	}
	data.rpm = 0;
	display();

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
			led_digit.setRow(disp, j + offset, B00000000);
		else
			led_digit.setRow(disp, j + offset, b);
	}
}

void printGear(int gear) {
	for (int i = 0; i < 8; i++) {
		led_matrix.setRow(MATRIX_0, i, GEARS[gear][i]);
	}
}

void printLEDStrip() {
	int led_position = data.rpm / NUM_LEDS_MULTP;
	for (int current_led = 0; current_led < NUM_LEDS; current_led++) {
		switch (current_led) {
		case 0:
			if (data.drsAllowed == 1 && data.drs == 0) {
				strip.setPixelColor(current_led, C_YELLOW);
				break;
			} else if (data.drs == 1) {
				strip.setPixelColor(current_led, C_MAGENTA);
				break;
			}
		default:
			if (current_led < led_position) {
				strip.setPixelColor(current_led, LED_COLORS[current_led]);

				// Set next led brightness
				if (current_led == led_position - 1) {
					int mod = ceil(data.rpm % NUM_LEDS_MULTP);
					if (mod != 0) {
						int nex_pos = current_led + 1;
						int b = (int)(mod * NEO_MAX_BRIGHTNESS / NUM_LEDS);
						strip.setPixelColor(nex_pos, changeBrightness(LED_COLORS[nex_pos], b));
					}
				}
			} else if (current_led == 0 || current_led > led_position) {
				strip.setPixelColor(current_led, 0);
			}	
		}
	}
	strip.show();
}

void printTime(uint8_t disp, uint32_t ms) {
	uint8_t arr[6];

	uint32_t minutes = (uint32_t)(ms / 1000 / 60);
	uint32_t seconds = (uint32_t)((ms / 1000) % 60);
	uint32_t milliseconds = (uint32_t)(ms % 1000);

	uint32_t number = (minutes * 100000) + (seconds * 1000) + milliseconds;

	for (size_t i = 0; i < 6; i++) {
		if (number == 0) {
			arr[i] = 0;
		}
		else {
			arr[i] = number % 10;
			number = number / 10;
		}
	}

	for (size_t j = 0; j < 6; j++) {
		byte b = NUMBERS[arr[j]];
		if (j == 3 || j == 5)
			b += B10000000;
		led_digit.setRow(disp, j, b);
	}
}

void display() {
	printLEDStrip();
	printGear(data.gear);
	printTime(DIGIT_0, data.lapTime);
	printNumber(DIGIT_1, 0, 3, data.kmh, 0);
}


