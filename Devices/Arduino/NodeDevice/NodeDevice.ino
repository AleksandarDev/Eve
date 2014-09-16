#include <RF24Network.h>
#include <RF24.h>
#include <SPI.h>
#include <Timer.h>

//
// Eve Device
//
#define		EveFirmata
#define		DeviceID	1
#define		BaseDevice	0
#define		FirmwareVersion 1
#define		SerialBaud	57600
#define		CommunicationChannel 90

//
// Reserved Pins
//
#define		OnBoardLED	8
#define		RadioCE		10
#define		RadioCSN	9

//
// Custom firmata Commands
//
#define		CommandConnectionRequest		0x0001
#define		CommandConnectionAccepted		0x0002
#define		CommandConnectionDeclined		0x0003
#define		CommandConnectionCheckRequest	0x0004
#define		CommandConnectionCheckConfirm	0x0005


/*******************************************************************************************
 *
 * VARIABLES
 *
 ******************************************************************************************/
//
// Eve Device Variables
//
RF24 radio(RadioCE, RadioCSN);
RF24Network network(radio);
unsigned int packetsRecieved = 0;
unsigned int packetsSent = 0;
unsigned int lastConnectionCheck;
Timer timer;

/*******************************************************************************************
 *
 * METHOD DECLARATIONS
 *
 ******************************************************************************************/
// Eve functions
void UpdateServices();
void BlinkOnBoard(unsigned long);
void SendCommand(unsigned long, unsigned int);
void Solid(uint8_t, uint8_t, uint8_t, byte, byte, byte);
void DoFade();
void SetFade(byte, byte, byte, int);

bool isFading = false;
uint8_t pinR = 3, pinG = 5, pinB = 6;
float fadeStartR, fadeStartG, fadeStartB;
float grad_r, grad_g, grad_b;
byte output_r = 0, output_g = 0, output_b = 0;
int fadeCounter = 0, fadeT = 0;

struct SetColorMessage {
	byte r, g, b;
};

void TestSend() {
	Serial.print("Sending...");
	RF24NetworkHeader header(0);
	SetColorMessage data = { 255, 0, 128 }; 
	bool success = network.write(header, &data, sizeof(SetColorMessage));
	if (success) Serial.println("ok");
	else Serial.println("fail");
}

/*******************************************************************************************
 *
 * METHODS
 *
 ******************************************************************************************/
void setup(void) {
	// Initiate serial communication
	Serial.begin(SerialBaud);
	Serial.println();
	Serial.println("--------------------------------------------------------------------------------");
	Serial.print("    NodeDevice firmware version=");
	Serial.println(FirmwareVersion, DEC);
	Serial.println("--------------------------------------------------------------------------------");
	BlinkOnBoard(50);

	// Initiate radio communication
	SPI.begin();
	radio.begin();
	network.begin(CommunicationChannel, DeviceID);

	// Setup pins
	pinMode(OnBoardLED, OUTPUT);

	timer.every(1, DoFade, (void*)0);
	//timer.every(1000, TestSend);
}

void loop(void) {
	UpdateServices();

	if (network.available()) {
		BlinkOnBoard(20);
		RF24NetworkHeader header;
		SetColorMessage data;
		network.read(header, &data, sizeof(SetColorMessage));
		Serial.print(" R:");
		Serial.print(data.r, HEX);
		Serial.print(" G:");
		Serial.print(data.g, HEX);
		Serial.print(" B:");
		Serial.println(data.b, HEX);
		SetFade(data.r, data.g, data.b, 10000);
	}
}

void UpdateServices() {
	network.update();
	timer.update();
}

void BlinkOnBoard(unsigned long t) {
	timer.pulse(OnBoardLED, t, LOW);
}

//function holds RGB values for time t milliseconds
void Solid(byte r, byte g, byte b) {
	//output
	analogWrite(pinR, r);
	analogWrite(pinG, g);
	analogWrite(pinB, b);

	output_r = r;
	output_g = g;
	output_b = b;
}

//function fades between two RGB values over fade time period t
//maximum value of fade time = 30 seconds before gradient values
//get too small for floating point math to work? replace floats
//with doubles to remedy this?
void DoFade(void* context) {
	if (isFading) {
		fadeCounter++;
		if (fadeCounter > fadeT) {
			fadeCounter = fadeT = 0;
			isFading = false;
		}
		else {
			output_r = fadeStartR + grad_r * fadeCounter;
			output_g = fadeStartG + grad_g * fadeCounter;
			output_b = fadeStartB + grad_b * fadeCounter;
	
			//output
			analogWrite(pinR, (int)output_r);
			analogWrite(pinG, (int)output_g);
			analogWrite(pinB, (int)output_b);
	
			//hold at this colour set for 1ms
			//delay(1);
		}
	}
}

void SetFade(byte r2, byte g2, byte b2, int t) {
	fadeT = t;
	fadeCounter = 0;
	isFading = true;

	fadeStartR = (float)output_r;
	fadeStartG = (float)output_g;
	fadeStartB = (float)output_b;

	//calculate rates of change of R, G, and B values
	grad_r = ((float)r2 - fadeStartR) / fadeT;
	grad_g = ((float)g2 - fadeStartG) / fadeT;
	grad_b = ((float)b2 - fadeStartB) / fadeT;
}
