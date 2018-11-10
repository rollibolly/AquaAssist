#include <DallasTemperature.h>

#include <math.h>

int lightSensorPin = A0;
int tempSensorPin = A1;
int lightSensorValue = 0;
int tempSensorValue = 0;
int relayStat = 0;

// Relay control variables
const int relayPin_0 = 2;
const int relayPin_1 = 4;
const int relayPin_2 = 7;

String commandString = "";

double Thermistor(int RawADC)
{
  double Temp;
  Temp = log(10000.0 * ((1024.0 / RawADC - 1)));
  Temp = 1 / (0.001129148 + (0.000234125 + (0.0000000876741 * Temp * Temp )) * Temp );
  Temp = Temp - 273.15;            // Convert Kelvin to Celcius
  //Temp = (Temp * 9.0)/ 5.0 + 32.0; // Convert Celcius to Fahrenheit
  return Temp;
}

void modifyRelayState(int relayNo, bool state){
  int newState;
  if (state){
    newState = LOW;  
  } else {
    newState = HIGH;  
  }
  switch (relayNo){
    case 0:
      digitalWrite(relayPin_0, newState);
      break;
    case 1:
      digitalWrite(relayPin_1, newState);
      break;
    case 2:
      digitalWrite(relayPin_2, newState);
      break;
    default:
      return;
  }
  if (state){
    relayStat = bitSet(relayStat, relayNo);
  } else {
    relayStat = bitClear(relayStat, relayNo);
  }
}

void setup() {
  Serial.begin(9600);
  
  pinMode(relayPin_0, OUTPUT);
  pinMode(relayPin_1, OUTPUT);
  pinMode(relayPin_2, OUTPUT);

  modifyRelayState(0, true);
  modifyRelayState(1, true);
  modifyRelayState(2, true);
}

void loop() {

  /*
  1000 0000 get light sensore value
  0100 0000 get temperature sensore value
  0010 0000 relay 0 on
  0001 0000 relay 1 on
  0000 1000 relay 2 on

  00111000 - 8 no sensor data
  
  */
  while (Serial.available()){
    
    char command = Serial.read();
    char buff[100];
    
    
    
    // Send light sensor current value
    lightSensorValue = 1023 - analogRead(lightSensorPin)/10.23;
          
    // Send temperature sensor current value
    tempSensorValue = analogRead(tempSensorPin);
   float temperatureC = Thermistor(tempSensorValue);
  char str_temp[6];

  /* 4 is mininum width, 2 is precision; float value is copied onto str_temp*/
  dtostrf(temperatureC, 4, 2, str_temp);
    
    bool value;
    value = bitRead(command, 3);
    if (value) {
      // Modify relay stats        
      value = bitRead(command, 0);
      
      if (value){
        // Turn on relay 1
        modifyRelayState(0, true);
      } else {
        // Turn of relay 1  
        modifyRelayState(0, false);
      }
      value = bitRead(command, 1);
      if (value){
        // Turn on relay 2
        modifyRelayState(1, true);
      } else {
        // Turn of relay 2  
        modifyRelayState(1, false);
      }
      value = bitRead(command, 2);
      if (value){
        // Turn on relay 3
        modifyRelayState(2, true);
      } else {
        // Turn of relay 3  
        modifyRelayState(2, false);
      }
    }
    sprintf(buff, "<MSG><COMMAND:%d><LIGHT:%d><TEMP:%s><RELAY:%d></MSG>", command, lightSensorValue, str_temp, relayStat);
    Serial.println(buff);
  }  
}
