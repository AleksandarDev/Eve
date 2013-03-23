#include <Servo.h>
#include <Wire.h>
#include <Firmata.h>
#include <RF24Network.h>
#include <RF24.h>
#include <SPI.h>
#include <Timer.h>
#include <stdlib.h> // for malloc and free
#include "EveDevice.h"

#define		DEBUG // Remove for release
#define		DeviceID				0

//
// Pins
//
#define		OnBoardLED	13
#define		RadioCE		48
#define		RadioCSN	49

// 
// Definitions
//
void UpdateServices();

void HandleNetworkMessage();
void HandleCommandMessage();
void HandleDataMessage();
void HandleCommand();
void ClearCurrentCommand();

void HandleCommandConnectionRequest();
void HandleCommandConnectionCheckConfirm();

void BlinkOnBoard(unsigned long);
bool SendCommand(int16_t, unsigned long, unsigned int);
bool SendData(int16_t, byte[], unsigned int);


struct NodeConnection {
	// state (byte)
	// 0		connected state
	// 1-7	reserved
	uint16_t id;
	byte state;
	unsigned long lastChecked;
};

struct CommandMessage {
	unsigned long command;
	unsigned long localTime;
	unsigned int dataPackets;
};

//
// Variables
//
RF24 radio(RadioCE, RadioCSN);
RF24Network network(radio);
unsigned int packetsRecieved = 0;
unsigned int packetsSent = 0;
Timer timer;

NodeConnection nodesConnected[25];
byte numNodesConnected = 0;

RF24NetworkHeader currentCommandHeader;
CommandMessage currentCommand;
unsigned int currentCommandDataRecieved;
byte* currentCommandData;
boolean isCurrentCommandReady;

struct SetColorMessage {
	byte r, g, b;
};

void setup(void) {
	// Initiate serial communication
	Serial.begin(SerialBaud);
	Serial.print("BaseDevice firmware version=");
	Serial.println(FirmwareVersion, DEC);
	Serial.println("--------------------");

	// Initiate radio communication
	SPI.begin();
	radio.begin();
	network.begin(CommunicationChannel, DeviceID);

	// Setup pins
	pinMode(OnBoardLED, OUTPUT);

	// Setup services
	//timer.every(CheckConnectionPeriod, CheckConnections);
}

byte pwm = 0;
void loop(void) {
	UpdateServices();
	
	// Read network message if available
	while(network.available()) {
		RF24NetworkHeader header;
		SetColorMessage data;
		network.read(header, &data, sizeof(SetColorMessage));
		Serial.print("Recieved R:"); Serial.print(data.r, HEX); Serial.print(" G:"); Serial.print(data.g); Serial.print(" B:"); Serial.println(data.b);
		BlinkOnBoard(20);
	}
	
	// Execute command if available
	/*if (isCurrentCommandReady)
		HandleCommand();*/

	while(Serial.available() >= 3) {
		Serial.print("Sending...");
		RF24NetworkHeader header(1);
		SetColorMessage data = { Serial.read(), Serial.read(), Serial.read() }; 
		bool success = network.write(header, &data, sizeof(SetColorMessage));
		if (success) Serial.println("ok");
		else Serial.println("fail");
		Serial.print("Sending R: "); Serial.print(data.r); Serial.print("G: "); Serial.print(data.g); Serial.print("B: ");
		Serial.println(data.b);
	}

	/*RF24NetworkHeader h(1);
	byte d = 0xE0;
	network.write(h, &d, sizeof(byte));*/
}


/*****************************************************************************
 * 
 * Checks connection to all available nodes
 *
 *****************************************************************************/
//void CheckConnections() {
//	// Send confirm request for all nodes
//	for(byte index = 0; index < numNodesConnected; index++) {
//		SendCommand(nodesConnected[index].id, CommandConnectionCheckRequest, 0);
//		Serial.println(millis() - nodesConnected[index].lastChecked, DEC);
//		// Check if node is connected
//		if (millis() - nodesConnected[index].lastChecked > ConnectionTimeout) {
//			// Sets first bit to 0 (not connected)
//			nodesConnected[index].state &= ~1; 
//
//#ifdef DEBUG
//			Serial.print("Couldn't confirm connection to device ");
//			Serial.println(nodesConnected[index].id);
//#endif
//		}
//	}
//}


/*****************************************************************************
 * 
 * Updates all needed services
 *
 *****************************************************************************/
void UpdateServices() {
	network.update();
	timer.update();
}


/*****************************************************************************
 * 
 * Handles recieving message 
 * Determines whether message is command or data
 *
 *****************************************************************************/
void HandleNetworkMessage() {
	if (currentCommand.command == 0)
		HandleCommandMessage();
	else HandleDataMessage();

	BlinkOnBoard(50);
}


/*****************************************************************************
 * 
 * Handles command message on network
 *
 *****************************************************************************/
void HandleCommandMessage() {
	network.read(currentCommandHeader, &currentCommand, sizeof(CommandMessage));
	isCurrentCommandReady = true;

	if (currentCommand.dataPackets > 0) {
		currentCommandData = (byte*)malloc(currentCommand.dataPackets);
		currentCommandDataRecieved = 0;
		isCurrentCommandReady = false;
	}

#ifdef DEBUG
	Serial.print("Got message from ");
	Serial.print(currentCommandHeader.from_node, OCT);
	Serial.print(" an command ");
	Serial.println(currentCommand.command, HEX);
	
	if (!isCurrentCommandReady) 
		Serial.println("Waiting for data packets...");
#endif
}


/*****************************************************************************
 * 
 * Handles data message recieved from network
 *
 *****************************************************************************/
void HandleDataMessage() {
	// Check if all data packes are recieved
	if (currentCommandDataRecieved < currentCommand.dataPackets) {
		RF24NetworkHeader header;
		network.read(header, &currentCommandData[currentCommandDataRecieved++], sizeof(byte));
	}
	else {
		isCurrentCommandReady = true;

#ifdef DEBUG
		Serial.println("All data packets recieved.");
#endif
	}
}


/*****************************************************************************
 * 
 * Handle currenty loaded command
 *
 *****************************************************************************/
void HandleCommand() {
	switch (currentCommand.command) {
		case CommandConnectionRequest: HandleCommandConnectionRequest(); break;
		case CommandConnectionCheckConfirm: HandleCommandConnectionCheckConfirm(); break;
		default:
#ifdef DEBUG
			Serial.print("Unknown Command code ");
			Serial.println(currentCommand.command, HEX);
#endif
			break;
	}

	if (currentCommand.command == CommandConnectionCheckConfirm) 
		HandleCommandConnectionRequest();

	// After command was handled
	ClearCurrentCommand();
}


/*****************************************************************************
 * 
 * Handles connection request command from node 
 *
 * Checks whether connection isn't made already or maximum number of
 * connections is made already
 *
 *****************************************************************************/
void HandleCommandConnectionRequest() {
#ifdef DEBUG
	Serial.print("Handling connection request from ");
	Serial.println(currentCommandHeader.from_node, OCT);
#endif

	// Check if new nodes can be added and if node with same ID already exists
	bool canAccept = numNodesConnected < 25;
	for(byte index = 0; index < numNodesConnected; index++) {
		if (nodesConnected[index].id == currentCommandHeader.from_node &&
			nodesConnected[index].state & 0x1 != 0) {
			canAccept = false;
		}
	}

	// Send decline command if node can't be added
	if (!canAccept) {
		SendCommand(currentCommandHeader.from_node, CommandConnectionDeclined, 0);

#ifdef DEBUG
		Serial.print("Device connection request declined ");
		Serial.println(currentCommandHeader.from_node, OCT);
#endif
	}

	// Send connection accepted command
	SendCommand(currentCommandHeader.from_node, CommandConnectionAccepted, 0);

	// Insert node to last available place
	nodesConnected[numNodesConnected].id = currentCommandHeader.from_node;
	nodesConnected[numNodesConnected].state |= 1;
	nodesConnected[numNodesConnected].lastChecked = millis();

	numNodesConnected++;

#ifdef DEBUG
	Serial.print("Device added to connected nodes ");
	Serial.println(currentCommandHeader.from_node, OCT);
#endif
}


/*****************************************************************************
 * 
 * Handles connection check confirm command
 *
 *****************************************************************************/
void HandleCommandConnectionCheckConfirm() {
	for(byte index = 0; index < numNodesConnected; index++) {
		if (nodesConnected[index].id == currentCommandHeader.from_node) {
			nodesConnected[index].lastChecked = millis();

#ifdef DEBUG
			Serial.print("Connection confirmed for device ");
			Serial.println(nodesConnected[index].id, OCT);
#endif
			break;
		}
	}
}


/*****************************************************************************
 * 
 * Clears current command variables and all data
 *
 *****************************************************************************/
void ClearCurrentCommand() {
	currentCommand.command = 0;
	currentCommandDataRecieved = 0;
	isCurrentCommandReady = false;
	free(currentCommandData);
}


/*****************************************************************************
 * 
 * Helper method for sending command messages
 *
 *****************************************************************************/
bool SendCommand(int16_t reciever, unsigned long command, unsigned int dataPackets) {
	RF24NetworkHeader header(reciever);
	CommandMessage message = { command, millis(), dataPackets };
	return network.write(header, &message, sizeof(CommandMessage));
}


/*****************************************************************************
 * 
 * Helper method for sending data messages
 *
 *****************************************************************************/
bool SendData(int16_t reciever, byte data[], unsigned int numDataPackets) {
	RF24NetworkHeader header(reciever);
	for(unsigned int index = 0; index < numDataPackets; index++) {
		if (!network.write(header, &data[index], sizeof(byte)))
			return false;
	}
	return true;
}


/*****************************************************************************
 * 
 * Helper method for blinking on board LED
 *
 *****************************************************************************/
void BlinkOnBoard(unsigned long t) {
#ifdef DEBUG
	timer.pulse(OnBoardLED, t, LOW);
#endif
}