const int latchPin = 8;
const int clockPin = 13;
const int dataPin = 11;
const int yellowPin = 6;

byte led_data = 0x00;

int serial_data[] = { 0x00, 0x00, 0x00, 0x00 };

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
		if (Serial.available() > 3) {
			serial_data[0] = Serial.read();
			serial_data[1] = Serial.read();
			serial_data[2] = Serial.read();
			serial_data[2] = serial_data[2] * 256 + Serial.read();
		}
		display_data();
	}
}

void display_data() {
	display(serial_data[0]);
	digitalWrite(yellowPin, serial_data[1]);
}

void display(byte data) {
	digitalWrite(latchPin, LOW);
	shiftOut(dataPin, clockPin, MSBFIRST, data);
	digitalWrite(latchPin, HIGH);
}