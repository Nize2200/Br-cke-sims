#include <X360ControllerLEDs.h>

#include <pitches.h>

#include <XInput.h>
const uint8_t Pin_buttonLB = 0;
const uint8_t Pin_buttonRB = 1;
const uint8_t Pin_buttonA = 4;
const uint8_t Pin_buttonB = 5;
const uint8_t Pin_buttonX = 2;
const uint8_t Pin_buttonY = 3;
const uint8_t Pin_buttonDU = 7; //DU = D-pad Upp
const uint8_t Pin_buttonDD = 8; //DD = D-pad Down
const uint8_t Pin_buttonDR = 9; //DR = D-pad Rright
const uint8_t Pin_buttonDL = 6; //DL = D-pad Left


const uint8_t Pin_JoyX = A1;
const uint8_t Pin_JoyY = A0;
const uint8_t Pin_JoyRightX = A2;
const uint8_t Pin_JoyRightY = A3;


const uint8_t Pin_LED_1 = 10;
const uint8_t Pin_LED_2 = 11;
const uint8_t Pin_LED_3 = 12;
const uint8_t Pin_LED_4 = 13;
const int DEADZONE = 1;
#define BUZZER_PIN 1


// DOOM intro
int melody[] = {
  NOTE_E2, NOTE_E2, NOTE_E3, NOTE_E2, NOTE_E2, NOTE_D3, NOTE_E2, NOTE_E2, 
  NOTE_C3, NOTE_E2, NOTE_E2, NOTE_AS2, NOTE_E2, NOTE_E2, NOTE_B2, NOTE_C3,
  NOTE_E2, NOTE_E2, NOTE_E3, NOTE_E2, NOTE_E2, NOTE_D3, NOTE_E2, NOTE_E2,
  NOTE_C3, NOTE_E2, NOTE_E2, NOTE_AS2,
  
  NOTE_E2, NOTE_E2, NOTE_E3, NOTE_E2, NOTE_E2, NOTE_D3, NOTE_E2, NOTE_E2, 
  NOTE_C3, NOTE_E2, NOTE_E2, NOTE_AS2, NOTE_E2, NOTE_E2, NOTE_B2, NOTE_C3,
  NOTE_E2, NOTE_E2, NOTE_E3, NOTE_E2, NOTE_E2, NOTE_D3, NOTE_E2, NOTE_E2,
  NOTE_C3, NOTE_E2, NOTE_E2, NOTE_AS2,
  
};

int durations[] = {
  8, 8, 8, 8, 8, 8, 8, 8, 
  8, 8, 8, 8, 8, 8, 8, 8,
  8, 8, 8, 8, 8, 8, 8, 8,
  8, 8, 8, 2,
  
  8, 8, 8, 8, 8, 8, 8, 8, 
  8, 8, 8, 8, 8, 8, 8, 8,
  8, 8, 8, 8, 8, 8, 8, 8,
  8, 8, 8, 2,
  
};


void setup() {
  
    pinMode(Pin_buttonRB, INPUT_PULLUP);
    pinMode(Pin_buttonLB, INPUT_PULLUP);
    pinMode(Pin_buttonA, INPUT_PULLUP);
    pinMode(Pin_buttonB, INPUT_PULLUP);
    pinMode(Pin_buttonX, INPUT_PULLUP);
    pinMode(Pin_buttonY, INPUT_PULLUP);
    pinMode(Pin_buttonDU, INPUT_PULLUP);
    pinMode(Pin_buttonDD, INPUT_PULLUP);
    pinMode(Pin_buttonDR, INPUT_PULLUP);
    pinMode(Pin_buttonDL, INPUT_PULLUP);
    pinMode(Pin_LED_1,OUTPUT);
    pinMode(Pin_LED_1,OUTPUT);  
    pinMode(Pin_LED_1,OUTPUT);  
    pinMode(Pin_LED_1,OUTPUT);

    XInput.setRange(JOY_LEFT, 0, 1023);
    XInput.setRange(JOY_RIGHT, 0 ,1023);
    XInput.begin();
}

void loop() {

  int size = sizeof(durations) / sizeof(int);

  int playerIndex = XInput.getPlayer();

  digitalWrite(Pin_LED_1, LOW);
  digitalWrite(Pin_LED_2, LOW);
  digitalWrite(Pin_LED_3, LOW);
  digitalWrite(Pin_LED_4, LOW);


 
  // Turn on the LED corresponding to the player index
  switch (playerIndex) {
    case 0:
      digitalWrite(Pin_LED_1, HIGH);
      break;
    case 1:
      digitalWrite(Pin_LED_2, HIGH);
      break;
    case 2:
      digitalWrite(Pin_LED_3, HIGH);
      break;
    case 3:
      digitalWrite(Pin_LED_4, HIGH);
      break;
  }
    boolean pressButtonRB = !digitalRead(Pin_buttonRB);
    boolean pressButtonLB = !digitalRead(Pin_buttonLB);
    boolean pressButtonA = !digitalRead(Pin_buttonA);
    boolean pressButtonB = !digitalRead(Pin_buttonB);
    boolean pressButtonX = !digitalRead(Pin_buttonX);
    boolean pressButtonY = !digitalRead(Pin_buttonY);
    boolean pressButtonDU = !digitalRead(Pin_buttonDU);
    boolean pressButtonDD = !digitalRead(Pin_buttonDD);
    boolean pressButtonDR = !digitalRead(Pin_buttonDR);
    boolean pressButtonDL = !digitalRead(Pin_buttonDL);

    
    //XInput.setButton(BUTTON_LB,pressButtonLB);
    //XInput.setButton(BUTTON_RB,pressButtonRB);
    XInput.setButton(BUTTON_A, pressButtonA);   
    XInput.setButton(BUTTON_B, pressButtonB);   
    XInput.setButton(BUTTON_X, pressButtonX); 
    XInput.setButton(BUTTON_Y, pressButtonY);   
    XInput.setButton(DPAD_UP,pressButtonDU);
    XInput.setButton(DPAD_DOWN,pressButtonDD);
    XInput.setButton(DPAD_RIGHT,pressButtonDR);
    XInput.setButton(DPAD_LEFT,pressButtonDL);
    int16_t leftXAxis = analogRead(Pin_JoyX);
    int16_t leftYAxis = analogRead(Pin_JoyY);
    int16_t rightXAxis = analogRead(Pin_JoyRightX);
    int16_t rightYAxis = analogRead(Pin_JoyRightY);
   
    

    // Apply deadzone
    //if (abs(leftXAxis) < DEADZONE) leftXAxis = 1023/2;
    //if (abs(leftYAxis) < DEADZONE) leftYAxis = 1023/2;
  

    if(pressButtonRB) {

      XInput.press(TRIGGER_RIGHT);
    }
    else {

      XInput.release(TRIGGER_RIGHT);
    }
    
    if(pressButtonLB) {

      XInput.press(TRIGGER_LEFT);
    }
    else {

      XInput.release(TRIGGER_LEFT);
    }
  XInput.setJoystickX(JOY_LEFT, leftXAxis, false);
  XInput.setJoystickY(JOY_LEFT, leftYAxis, true); 
  XInput.setJoystickX(JOY_RIGHT, rightYAxis, true); // Joystik is mounted wrong way on joystick , makesure we check x and y
  XInput.setJoystickY(JOY_RIGHT, rightXAxis, true);
    uint16_t rumble = XInput.getRumble();


    // if we press a button and want something to happen
    if(pressButtonDU){

      digitalWrite(Pin_LED_4, HIGH);
      digitalWrite(Pin_LED_3, HIGH);
     }
    
    
    
    if (rumble > 0) {
       

    }
    

}

