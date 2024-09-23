// test, one joystick?(which has two digital buttons) and 4 buttons
// 2024-09-23
// all buttons are digital

#include <XInput.h>

// input joystick pins (left, right)
const uint8_t Pin_clicker1 = 9;
const uint8_t Pin_clicker2 = 8;

// input 4 button pins
const uint8_t Pin_button1 = 2;
const uint8_t Pin_button2 = 3;
const uint8_t Pin_button3 = 4;
const uint8_t Pin_button4 = 5;

/*
boolean lastButtonState1 = HIGH;
boolean lastButtonState2 = HIGH;
boolean lastButtonState3 = HIGH;
boolean lastButtonState4 = HIGH;
boolean lastButtonState5 = HIGH;
boolean lastButtonState6 = HIGH;
*/

void setup() {


  // put your setup code here, to run once:
  pinMode(Pin_clicker1, INPUT_PULLUP);
  pinMode(Pin_clicker2, INPUT_PULLUP);
  pinMode(Pin_button1, INPUT_PULLUP);
  pinMode(Pin_button2, INPUT_PULLUP);
  pinMode(Pin_button3, INPUT_PULLUP);
  pinMode(Pin_button4, INPUT_PULLUP);

  XInput.begin();

}

void loop() {
  // put your main code here, to run repeatedly:
  boolean pressClicker1 = !digitalRead(Pin_clicker1);
  boolean pressClicker2 = !digitalRead(Pin_clicker2);
  boolean pressButton1 = !digitalRead(Pin_button1);
  boolean pressButton2 = !digitalRead(Pin_button2);
  boolean pressButton3 = !digitalRead(Pin_button3);
  boolean pressButton4 = !digitalRead(Pin_button4);


  // Update XInput button states to simulate controller inputs
  XInput.setButton(BUTTON_LB, pressClicker1);  // Map Button 1 to "LB"
  XInput.setButton(BUTTON_RB, pressClicker2);  // Map Button 2 to "RB"
  XInput.setButton(BUTTON_A, pressButton1);  // Map Button 3 to "A"
  XInput.setButton(BUTTON_B, pressButton2);  // Map Button 4 to "B"
  XInput.setButton(BUTTON_X, pressButton3); // Map Button 5 to "X"
  XInput.setButton(BUTTON_Y, pressButton4); // Map Button 6 to "Y"

  // Add a small delay to avoid bouncing issues
  delay(50);
}
