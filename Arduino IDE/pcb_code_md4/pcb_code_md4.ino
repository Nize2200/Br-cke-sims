// Xinput configuration code for the customized PCB for MD4

#include <XInput.h>

// digital buttons
const uint8_t Pin_FuncButton1 = 10; 
const uint8_t Pin_FuncButton2 = 30;
const uint8_t Pin_buttonA = 17; // Will be D-pad down
const uint8_t Pin_buttonB = 11; // will be D-pad right
const uint8_t Pin_buttonX = 7; // will be D-pad left
const uint8_t Pin_buttonY = 0; // Will be D-pad UP
 
// analog thumbstick
const uint8_t Pin_JoyX = A1;
const uint8_t Pin_JoyY = A0;


// motor: already installed in the controller, but we had some connection issue via PCB
// so we recommend you shoudld find the right pin numbers for the motors...
// const uint8_t Pin_rumble1 = 13;
// const uint8_t Pin_rumble2 = 13;

const int DEADZONE = 1;
 
 
void setup() {
  Serial.begin(9600);
    pinMode(Pin_FuncButton1, INPUT_PULLUP);
    pinMode(Pin_FuncButton2, INPUT_PULLUP);
    pinMode(Pin_buttonA, INPUT_PULLUP);
    pinMode(Pin_buttonB, INPUT_PULLUP);
    pinMode(Pin_buttonX, INPUT_PULLUP);
    pinMode(Pin_buttonY, INPUT_PULLUP);
    pinMode(Pin_rumble1, OUTPUT);
    pinMode(Pin_rumble2, OUTPUT);
 
    XInput.setRange(JOY_LEFT, 225, 850);
    XInput.begin();
}
 
void loop() {
 
    boolean funcButton1 = !digitalRead(Pin_FuncButton1);
    boolean funcButton2 = !digitalRead(Pin_FuncButton2);
    boolean pressButtonA = !digitalRead(Pin_buttonA);
    boolean pressButtonB = !digitalRead(Pin_buttonB);
    boolean pressButtonX = !digitalRead(Pin_buttonX);
    boolean pressButtonY = !digitalRead(Pin_buttonY);
    uint16_t rumble = XInput.getRumble();
 
    int16_t leftXAxis = analogRead(Pin_JoyX);
    int16_t leftYAxis = analogRead(Pin_JoyY);
   
 // by combining funcButton1 with digital buttons (X,Y,A,B)
 // you can use them as dpad
    if(funcButton1) {
 
    XInput.setButton(BUTTON_A, pressButtonA);  
    XInput.setButton(BUTTON_B, pressButtonB);  
    XInput.setButton(BUTTON_X, pressButtonX);
    XInput.setButton(BUTTON_Y, pressButtonY);  
    }
    else {
 
    XInput.setButton(DPAD_DOWN,pressButtonA);
    XInput.setButton(DPAD_RIGHT,pressButtonB);
    XInput.setButton(DPAD_LEFT,pressButtonX);
    XInput.setButton(DPAD_UP,pressButtonY);
   
    }
   
    if(funcButton2) {
 
      XInput.press(TRIGGER_LEFT);
    }
    else {
 
      XInput.release(TRIGGER_LEFT);
    }
   
 
XInput.setJoystickX(JOY_LEFT, leftXAxis, false);
XInput.setJoystickY(JOY_LEFT, leftYAxis, true);
 
   
    if (rumble > 0) {
      analogWrite(Pin_rumble1, 255);
      analogWrite(Pin_rumble2, 255);
    }
   
 
 
 
 
 
}
 
 