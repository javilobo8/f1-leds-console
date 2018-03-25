const int latchPin = 8;
const int clockPin = 13;
const int dataPin = 11;
const int yellowPin = 6;

byte led_data = 0x00;
const byte only_blue = 0xE0;
const byte only_red = 0x1F;

int serial_data[] = { 0, 0 };

int power(int value, int exponent) {
	return 0.5 + pow(value, exponent);
}

void initialize() {
	display(0x00);
	digitalWrite(yellowPin, LOW);
}

void setup() {
	pinMode(latchPin, OUTPUT);
	pinMode(clockPin, OUTPUT);
	pinMode(dataPin, OUTPUT);
	pinMode(yellowPin, OUTPUT);

	Serial.begin(9600);
	initialize();
}

void loop() {
	while (true) {
		if (Serial.available() > 1) {
			serial_data[0] = Serial.read();
			serial_data[1] = Serial.read();
		}
		display(serial_data[0]);
		digitalWrite(yellowPin, serial_data[1] ? HIGH : LOW);
	}
}

void display(byte data) {
	digitalWrite(latchPin, LOW);
	shiftOut(dataPin, clockPin, MSBFIRST, data);
	digitalWrite(latchPin, HIGH);
}