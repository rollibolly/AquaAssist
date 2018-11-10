// Adafruit Motor shield library
// copyright Adafruit Industries LLC, 2009
// this code is public domain, enjoy!

#include "AFMotor.h"

AF_DCMotor motor_1(1);
AF_DCMotor motor_2(2);
AF_DCMotor motor_3(3);
AF_DCMotor motor_4(4);

#define MOTOR_CODE 'M'
#define MOTOR_1_ID '1'
#define MOTOR_2_ID '2'
#define MOTOR_3_ID '3'
#define MOTOR_4_ID '4'

void setup() {
  Serial.begin(9600);           // set up Serial library at 9600 bps
  Serial.println("Setup...");

  // turn on motor
  motor_1.setSpeed(255);
  motor_2.setSpeed(255);
  motor_3.setSpeed(255);
  motor_4.setSpeed(255);
 
  motor_1.run(RELEASE);
  motor_2.run(RELEASE);
  motor_3.run(RELEASE);
  motor_4.run(RELEASE);
}

byte waitForNextByte(){
  while (Serial.available() == 0) {
    //blocking execution
  }
  return Serial.read();
}

void loop() {
  byte incomingByte;
  
  // send data only when you receive data:
  if (Serial.available() > 0) {
          // read the incoming byte:
          incomingByte = Serial.read();

          // say what you got:
          Serial.print("I received: ");
          Serial.println(incomingByte, DEC);
          if (incomingByte == MOTOR_CODE) {
            //A motor command will follow
            Serial.println("Motor Comand...");  
            byte motorId = waitForNextByte();

            // Building dosing time from 3 digits
            unsigned int dosingQuantity = (waitForNextByte() - '0') * 10; // if digit is 3 => 30
            dosingQuantity = (dosingQuantity + waitForNextByte() - '0') * 10; // 320
            dosingQuantity = (dosingQuantity + waitForNextByte() - '0') * 10; // 3210
            dosingQuantity = (dosingQuantity + waitForNextByte() - '0') * 10; // 32130
            dosingQuantity = (dosingQuantity + waitForNextByte() - '0'); // 32101

            Serial.print("Dosing time: ");
            Serial.println(dosingQuantity);

            switch(motorId){
              case MOTOR_1_ID:
                Serial.println("M1 dosing...");
                motor_1.run(BACKWARD);
                //motor_1.setSpeed(255);  
                delay(dosingQuantity);
                motor_1.run(RELEASE);
                break;
              case MOTOR_2_ID:
                Serial.println("M2 dosing...");
                motor_2.run(BACKWARD);
                //motor_2.setSpeed(255);  
                delay(dosingQuantity);
                motor_2.run(RELEASE);
                break;
              case MOTOR_3_ID:
                Serial.println("M3 dosing...");
                motor_3.run(BACKWARD);
                //motor_3.setSpeed(255);  
                delay(dosingQuantity);
                motor_3.run(RELEASE);
                break;
              case MOTOR_4_ID:
                Serial.println("M4 dosing...");
                motor_4.run(BACKWARD);
                //motor_4.setSpeed(255);  
                delay(dosingQuantity);
                motor_4.run(RELEASE);
                break;              
            }                       
            Serial.println("Dosing done.");            
          }
  }

  /*
  motor.run(BACKWARD);
  motor.setSpeed(255);  

  Serial.print("tech");
  motor.run(RELEASE);
  delay(1000);*/
} 
