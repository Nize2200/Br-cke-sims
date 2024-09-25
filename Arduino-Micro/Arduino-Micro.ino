#include <pitches.h>

#include <XInput.h>

const int Buzzer = 1;
const uint8_t Pin_buttonA = 2;
const uint8_t Pin_buttonB = 3;
const uint8_t Pin_buttonX = 4;
const uint8_t Pin_buttonY = 5;
const uint8_t Pin_buttonDU = 6;
const uint8_t Pin_buttonDD = 7;
const uint8_t Pin_buttonDR = 8;
const uint8_t Pin_buttonDL = 9;
const uint8_t Pin_JoyX = A1;
const uint8_t Pin_JoyY = A0;

const int DEADZONE = 1;

void setup() {
    pinMode(Buzzer,OUTPUT);
    pinMode(Pin_buttonA, INPUT_PULLUP);
    pinMode(Pin_buttonB, INPUT_PULLUP);
    pinMode(Pin_buttonX, INPUT_PULLUP);
    pinMode(Pin_buttonY, INPUT_PULLUP);
    pinMode(Pin_buttonDU, INPUT_PULLUP);
    pinMode(Pin_buttonDD, INPUT_PULLUP);
    pinMode(Pin_buttonDR, INPUT_PULLUP);
    pinMode(Pin_buttonDL, INPUT_PULLUP);
    XInput.setRange(JOY_LEFT, 0, 1023);
    XInput.begin();
}

void loop() {
    boolean pressButtonA = !digitalRead(Pin_buttonA);
    boolean pressButtonB = digitalRead(Pin_buttonB);
    boolean pressButtonX = digitalRead(Pin_buttonX);
    boolean pressButtonY = !digitalRead(Pin_buttonY);
    boolean pressButtonDU = !digitalRead(Pin_buttonDU);
    boolean pressButtonDD = !digitalRead(Pin_buttonDD);
    boolean pressButtonDR = !digitalRead(Pin_buttonDR);
    boolean pressButtonDL = !digitalRead(Pin_buttonDL);

    XInput.setButton(BUTTON_A, pressButtonA);   // Map Button 3 to "A"
   // XInput.setButton(BUTTON_B, pressButtonB);   // Map Button 4 to "B"
    //XInput.setButton(BUTTON_X, pressButtonX);   // Map Button 5 to "X" X SKA BLI LEFT B SKA BLI RIGHT
    XInput.setButton(BUTTON_Y, pressButtonY);   // Map Button 6 to "Y"
    XInput.setButton(DPAD_UP,pressButtonDU);
    XInput.setButton(DPAD_DOWN,pressButtonDD);
    XInput.setButton(DPAD_RIGHT,pressButtonDR);
    XInput.setButton(DPAD_LEFT,pressButtonDL);
    int16_t leftXAxis = analogRead(Pin_JoyX);
    int16_t leftYAxis = analogRead(Pin_JoyY);

    XInput.setJoystick(JOY_RIGHT,0,0,pressButtonB,pressButtonX);
   // XInput.setTrigger(TRIGGER_RIGHT, pressButton1);

    // Apply deadzone
    //if (abs(leftXAxis) < DEADZONE) leftXAxis = 1023/2;
    //if (abs(leftYAxis) < DEADZONE) leftYAxis = 1023/2;
  
  
  XInput.setJoystickX(JOY_LEFT, leftXAxis, false);
  XInput.setJoystickY(JOY_LEFT, leftYAxis, true);
    uint16_t rumble = XInput.getRumble();



    if(pressButtonDU){
      tone(Buzzer,NOTE_D4);
    }
    else {
      noTone(Buzzer);
    }
    
    if(pressButtonDD){
      tone(Buzzer,NOTE_F4);
    }
    else {
      noTone(Buzzer);
    }
    
    if(pressButtonDR){
      tone(Buzzer,NOTE_G4);
    }
    else {
      noTone(Buzzer);
    }
    
    if(pressButtonDL){
      tone(Buzzer,NOTE_GS4);
    }
    else {
      noTone(Buzzer);
    }
    if (rumble > 0) {

    }
}
