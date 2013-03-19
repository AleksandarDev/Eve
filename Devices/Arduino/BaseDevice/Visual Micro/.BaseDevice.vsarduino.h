//Board = Arduino Mega 2560 or Mega ADK
#define ARDUINO 101
#define __AVR_ATmega2560__
#define F_CPU 16000000L
#define __AVR__
#define __cplusplus
#define __attribute__(x)
#define __inline__
#define __asm__(x)
#define __extension__
#define __ATTR_PURE__
#define __ATTR_CONST__
#define __inline__
#define __asm__ 
#define __volatile__
#define __builtin_va_list
#define __builtin_va_start
#define __builtin_va_end
#define __DOXYGEN__
#define prog_void
#define PGM_VOID_P int
#define NOINLINE __attribute__((noinline))

typedef unsigned char byte;
extern "C" void __cxa_pure_virtual() {}

void setup(void);
void loop(void);
void UpdateServices();
void HandleNetworkMessage();
void HandleCommandMessage();
void HandleDataMessage();
void HandleCommand();
void HandleCommandConnectionRequest();
void HandleCommandConnectionCheckConfirm();
void ClearCurrentCommand();
bool SendCommand(int16_t reciever, unsigned long command, unsigned int dataPackets);
bool SendData(int16_t reciever, byte data[], unsigned int numDataPackets);
void BlinkOnBoard(unsigned long t);

#include "C:\Program Files\arduino-1.0.1\hardware\arduino\variants\mega\pins_arduino.h" 
#include "C:\Program Files\arduino-1.0.1\hardware\arduino\cores\arduino\arduino.h"
#include "C:\Users\Aleksandar\Documents\Arduino\BaseDevice\BaseDevice.ino"
#include "C:\Users\Aleksandar\Documents\Arduino\BaseDevice\EveDevice.h"
