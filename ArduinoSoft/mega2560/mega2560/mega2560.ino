/*************************************************************************************

  Mark Bramwell, July 2010

  This program will test the LCD panel and the buttons.When you push the button on the shield，
  the screen will show the corresponding one.
 
  Connection: Plug the LCD Keypad to the UNO(or other controllers)

**************************************************************************************/

#include "LiquidCrystal.h"

LiquidCrystal lcd(8, 9, 4, 5, 6, 7);           // select the pins used on the LCD panel

// define some values used by the panel and buttons
int lcd_key     = 0;
int adc_key_in  = 0;

byte page = 0;
byte oldPage = -1;
byte relaysState;
byte buttonDown = 0;

byte relays[8] = {52, 50, 48, 46, 44, 42, 40, 38};

#define DEBUG 1

#define btnRIGHT  0
#define btnUP     1
#define btnDOWN   2
#define btnLEFT   3
#define btnSELECT 4
#define btnNONE   5

#define MESSAGE_CODE 'T'
#define RELAY_CODE 'R'
#define INFO_CODE 'I'
#define PAGE_COUNT 3
#define ON '+'
#define OFF '-'

int read_LCD_buttons(){               // read the buttons
    adc_key_in = analogRead(0);       // read the value from the sensor 

    // my buttons when read are centered at these valies: 0, 144, 329, 504, 741
    // we add approx 50 to those values and check to see if we are close
    // We make this the 1st option for speed reasons since it will be the most likely result

    if (adc_key_in > 1000) return btnNONE; 

    // For V1.1 us this threshold
    if (adc_key_in < 50)   return btnRIGHT;  
    if (adc_key_in < 250)  return btnUP; 
    if (adc_key_in < 450)  return btnDOWN; 
    if (adc_key_in < 650)  return btnLEFT; 
    if (adc_key_in < 850)  return btnSELECT;  

    return btnNONE;                // when all others fail, return this.
}

byte waitForNextByte(){
  while (Serial.available() == 0) {
    //blocking execution
  }
  return Serial.read();
} 

void nextPage(){
  if (page == PAGE_COUNT - 1)
    page = 0;
  else
    page = page + 1;
}
void prevPage(){
  if (page == 0)
    page = PAGE_COUNT - 1;
  else
    page = page - 1;
}

void clearScreen(){
  lcd.setCursor(0,0);
  lcd.print("                ");  
  lcd.setCursor(0,1);
  lcd.print("                ");
}

/*
Changes a Relay state between HIGH/LOW and adjusts the relayStates byte
The function is triggers a screen refresh on the LCD
*/
void changeRelayState(byte relay){
  byte currentState = bitRead(relaysState, relay);
  if (currentState == 0){    
    digitalWrite(relays[relay], HIGH);
    bitSet(relaysState, relay);
  } else {
    digitalWrite(relays[relay], LOW);
    bitClear(relaysState, relay);
  }
  oldPage = -1;
}

void setup(){
  Serial.begin(9600);           // set up Serial library at 9600 bps
  
  
   lcd.begin(16, 2);               // start the library
   lcd.setCursor(0,0);             // set the LCD cursor   position 
   lcd.print("Push the buttons");  // print a simple message on the LCD 

  // Set Relay pins to OUTPUT  
  for (int i = 0; i < 8; i++)
     pinMode(relays[i], OUTPUT);

  Serial.println("Setup done");
}
 
void loop(){

  if (Serial.available() > 0){
    byte initial = Serial.read();    
    if (initial == MESSAGE_CODE){     // We got a message
      
      lcd.setCursor(0,0);
      for (int i = 0; i < 16; i++){   // Display first line
        char c = waitForNextByte();        
        lcd.print(c);
      }
      lcd.setCursor(0,1);
      for (int i = 0; i < 16; i++){   // Display second line
        char c = waitForNextByte();
        lcd.print(c);
      }
    } else if (initial == RELAY_CODE){
      byte relayNum = waitForNextByte() - '0';
      if (relayNum < 8 && relayNum >= 0) {
        byte desiredState = waitForNextByte();        
        relayNum = 7 - relayNum;

        bool rState = bitRead(relaysState, relayNum);
        if ((rState && desiredState == OFF) || (!rState && desiredState == ON)){
          changeRelayState(relayNum);                        
        }
        Serial.println(relaysState, BIN);
      }
    } else if (initial == INFO_CODE){
      Serial.print("Relays:");
      Serial.println(relaysState, BIN);
    }
  }
  
  switch(page){
    case 0:
      if (oldPage != page){        
        oldPage = page;
        clearScreen();
        lcd.setCursor(0,0);
        lcd.print("Relay#: 01234567");
        lcd.setCursor(0,1);
        lcd.print("State:  ");
        lcd.print(bitRead(relaysState, 7)?'+':'-');
        lcd.print(bitRead(relaysState, 6)?'+':'-');
        lcd.print(bitRead(relaysState, 5)?'+':'-');
        lcd.print(bitRead(relaysState, 4)?'+':'-');
        lcd.print(bitRead(relaysState, 3)?'+':'-');
        lcd.print(bitRead(relaysState, 2)?'+':'-');
        lcd.print(bitRead(relaysState, 1)?'+':'-');
        lcd.print(bitRead(relaysState, 0)?'+':'-');
      }
      break;
    case 1:
      if (oldPage != page){        
        oldPage = page;
        clearScreen();
        lcd.setCursor(0,0);
        lcd.print("PAGE 2");
      }
      break;
    case 2:      
      if (oldPage != page){        
        oldPage = page;
        clearScreen();
        lcd.setCursor(0,0);
        lcd.print("PAGE 3");
      }
      break;
  }

  lcd_key = read_LCD_buttons();   // read the buttons
  
   switch (lcd_key){               // depending on which button was pushed, we perform an action

       case btnRIGHT:{             //  push button "RIGHT" and show the word on the screen                      
          
          break;
       }
       case btnLEFT:{          
          
          break;
       }    
       case btnUP:{
          if (buttonDown == 0){
             prevPage();
             buttonDown = 1;
          }
          break;
       }
       case btnDOWN:{
          if (buttonDown == 0){
            nextPage();
            buttonDown = 1;
          }
          break;
       }
       case btnSELECT:{          
          break;
       }
       case btnNONE:{          
            buttonDown = 0;
          break;
       }
   }
}
