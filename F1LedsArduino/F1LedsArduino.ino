#include <Adafruit_NeoPixel.h>
#include <math.h> 
#include "LedControlCustom.h"
#include "Constants.h"
#include "Utils.h"

// Serial
#define BAUDRATE 115200

// NeoPixel Stick
#define NP_PIN 6
#define NUM_LEDS 16
#define NEO_MAX_BRIGHTNESS 32

uint32_t LEDS[16] = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUM_LEDS, NP_PIN, NEO_GRB + NEO_KHZ800);

typedef struct
{
	byte led;
	uint32_t led_color;
} ColorData;
ColorData color_data;
const size_t packet_size = 8; //sizeof(ColorData);
char messageBuffer[packet_size];

void setup() {
	// Setup NeoPixel Strip
	strip.begin();
	strip.setBrightness(NEO_MAX_BRIGHTNESS);
	strip.clear();
	strip.show();
	display();

	Serial.begin(BAUDRATE);
	Serial.print("ColorData size: ");
	Serial.println(packet_size);
}

void loop() {
	if (Serial.available() >= packet_size) {
		Serial.readBytes(messageBuffer, packet_size);
		memcpy(&color_data, &messageBuffer, packet_size);
		//Serial.print("color_data.led ");
		//Serial.println(color_data.led);
		//Serial.print("color_data.led_color ");
		//Serial.println(color_data.led_color);
		update();
		display();
	}
}

void update() {
	LEDS[color_data.led] = color_data.led_color;
}

void display() {
	printLEDStrip();
}

void printLEDStrip() {
	for (size_t i = 0; i < 16; i++) {
		strip.setPixelColor(i, LEDS[i]);
	}
	strip.show();
}


