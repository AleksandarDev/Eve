#ifndef EveDevice_h
#define EveDevice_h

#define		FirmwareVersion		       1
#define		SerialBaud		       57600
#define		CommunicationChannel	       100

//
// Messages
//
#define		CheckInRange			0xE0
#define		SendRequest			0xF0

//
// Commands
//
#define		CommandConnectionRequest	0x0001
#define		CommandConnectionAccepted	0x0002
#define		CommandConnectionDeclined	0x0003
#define		CommandConnectionCheckRequest	0x0004
#define		CommandConnectionCheckConfirm	0x0005

#endif
