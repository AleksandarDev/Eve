#include <Timer.h>

Timer timer;
uint8_t pinR = 10, pinG = 11, pinB = 9;
bool isFading = false;
float fadeStartR, fadeStartG, fadeStartB;
float grad_r, grad_g, grad_b;
byte output_r = 0, output_g = 0, output_b = 0;
int fadeCounter = 0, fadeT = 0;

void SetFade(byte, byte, byte, int);
void Solid(uint8_t, uint8_t, uint8_t, byte, byte, byte);


void setup() {
  // put your setup code here, to run once:
  Serial.begin(57600); 

  pinMode(pinR, OUTPUT);
  pinMode(pinG, OUTPUT);
  pinMode(pinB, OUTPUT); 
  
  timer.every(1, DoFade);
}

void loop() {
  timer.update();
  
  // put your main code here, to run repeatedly:
  while(Serial.available() >= 3) {
    byte rInput = Serial.read();
    byte gInput = Serial.read();
    byte bInput = Serial.read();    
    SetFade(rInput, gInput, bInput, 2000);
    Serial.print(rInput, HEX);
    Serial.print(gInput, HEX);
    Serial.print(bInput, HEX);
  }
}

//function holds RGB values for time t milliseconds
void Solid(byte r, byte g, byte b) {
	//output
	analogWrite(pinR, 255-r);
	analogWrite(pinG, 255-g);
	analogWrite(pinB, 255-b);

	output_r = r;
	output_g = g;
	output_b = b;
}

//function fades between two RGB values over fade time period t
//maximum value of fade time = 30 seconds before gradient values
//get too small for floating point math to work? replace floats
//with doubles to remedy this?
void DoFade() {
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
