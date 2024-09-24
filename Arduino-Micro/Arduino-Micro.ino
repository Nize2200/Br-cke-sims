#include <XInput.h>

const uint8_t Pin_clicker1 = 9;
const uint8_t Pin_clicker2 = 8;

const uint8_t Pin_buttonA = 2;
const uint8_t Pin_buttonB = 3;
const uint8_t Pin_buttonX = 4;
const uint8_t Pin_buttonY = 5;
const uint8_t Pin_buttonL3 = 7;



const int DEADZONE = 1;

void setup() {
    pinMode(Pin_clicker1, INPUT_PULLUP);
    pinMode(Pin_clicker2, INPUT_PULLUP);
    pinMode(Pin_buttonA, INPUT_PULLUP);
    pinMode(Pin_buttonB, INPUT_PULLUP);
    pinMode(Pin_buttonX, INPUT_PULLUP);
    pinMode(Pin_buttonY, INPUT_PULLUP);
    pinMode(Pin_buttonL3, INPUT_PULLUP);
    XInput.begin();
}

void loop() {
    boolean pressClicker1 = !digitalRead(Pin_clicker1);
    boolean pressClicker2 = !digitalRead(Pin_clicker2);
    boolean pressButton1 = !digitalRead(Pin_buttonA);
    boolean pressButton2 = !digitalRead(Pin_buttonB);
    boolean pressButton3 = !digitalRead(Pin_buttonX);
    boolean pressButton4 = !digitalRead(Pin_buttonY);
    boolean pressButton5 = !digitalRead(Pin_buttonL3);

    XInput.setButton(BUTTON_LB, pressClicker1); // Map Button 1 to "LB"
    XInput.setButton(BUTTON_RB, pressClicker2); // Map Button 2 to "RB"
    XInput.setButton(BUTTON_A, pressButton1);   // Map Button 3 to "A"
    XInput.setButton(BUTTON_B, pressButton2);   // Map Button 4 to "B"
    XInput.setButton(BUTTON_X, pressButton3);   // Map Button 5 to "X"
    XInput.setButton(BUTTON_Y, pressButton4);   // Map Button 6 to "Y"
    XInput.setButton(BUTTON_L3, pressButton5);

    int16_t leftXAxis = (analogRead(A0) - 512) *64;
    int16_t leftYAxis = (analogRead(A1) - 512) *64;

    // Apply deadzone
    if (abs(leftXAxis) < DEADZONE) leftXAxis = 1023/2;
    if (abs(leftYAxis) < DEADZONE) leftYAxis = 1023/2;
  
  
  XInput.setJoystick(JOY_RIGHT, leftXAxis,1023/2);
  XInput.setJoystickY(JOY_LEFT, leftYAxis, true);
    uint16_t rumble = XInput.getRumble();

    if (rumble > 0) {
      
    }
}
