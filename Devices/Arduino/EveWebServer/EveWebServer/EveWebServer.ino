#include <DigiFi.h>
DigiFi wifi;

uint8_t pinR = 2, pinG = 3, pinB = 4;
byte output_r = 0, output_g = 0, output_b = 0;

void setup()
{
  Serial.begin(57600); 
  Serial2.begin(57600);
  wifi.begin(57600);

  //DigiX trick - since we are on serial over USB wait for character to be entered in serial terminal
  while(!Serial.available()){
    Serial.println("Enter any key to begin");
    delay(1000);
  }

  Serial.println("Starting");

  while (wifi.ready() != 1)
  {
    Serial.println("Error connecting to network");
    delay(15000);
  }  

  Serial.println("Connected to wifi!");
  Serial.print("Server running at: ");
  String address = wifi.server(8080);//sets up server and returns IP
  Serial.println(address); 


  pinMode(pinR, OUTPUT);
  pinMode(pinG, OUTPUT);
  pinMode(pinB, OUTPUT);  
//  wifi.close();
}

void loop()
{
  if ( wifi.serverRequest())
  {
    Serial.print("Request for: ");
    Serial.println(wifi.serverRequestPath());
    String path = wifi.serverRequestPath();
    if (path != "/") {
      if (path.startsWith("/setrgb")) 
      {
        int dataIndex = path.indexOf("?");
        int redIndex = path.indexOf("r", dataIndex);
        int greenIndex = path.indexOf("g", dataIndex);
        int blueIndex = path.indexOf("b", dataIndex);
        
        int red = path.substring(redIndex + 2, greenIndex - 1).toInt();
        int green = path.substring(greenIndex + 2, blueIndex - 1).toInt();
        int blue = path.substring(blueIndex + 2).toInt();
        
        Serial.print(red, HEX);
        Serial.print(green, HEX);
        Serial.print(blue, HEX);
        
        wifi.serverResponse("<html><body style='background-color:#" + String(red, HEX) + String(green, HEX) + String(blue, HEX) + "'></body></html>");
        
        Serial2.write((byte)red);
        Serial2.write((byte)green);
        Serial2.write((byte)blue);        
      }
      else wifi.serverResponse("404 Not Found",404);         
    }
    else wifi.serverResponse("<html><body><h1>EVE @ home</h1></body></html>"); 
  }
  delay(10);  
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
