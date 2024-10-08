// test, one joystick?(which has two digital buttons) and 4 buttons
// 2024-09-23
// all buttons are digital

#include <XInput.h>

// input joystick pins (left, right)
const uint8_t Pin_clicker1 = 9;
const uint8_t Pin_clicker2 = 8;

const uint8_t Pin_buttonA = 2;
const uint8_t Pin_buttonB = 3;
const uint8_t Pin_buttonX = 4;
const uint8_t Pin_buttonY = 5;
const uint8_t Pin_buttonMS = 7;
const uint8_t Pin_buttonLeftJoyX = A0;
const uint8_t Pin_buttonLeftJoyY = A1;

void setup()
{

	// put your setup code here, to run once:
	pinMode(Pin_clicker1, INPUT_PULLUP);
	pinMode(Pin_clicker2, INPUT_PULLUP);
	pinMode(Pin_buttonA, INPUT_PULLUP);
	pinMode(Pin_buttonB, INPUT_PULLUP);
	pinMode(Pin_buttonX, INPUT_PULLUP);
	pinMode(Pin_buttonY, INPUT_PULLUP);
	pinMode(Pin_buttonMS, INPUT_PULLUP);
	XInput.setJoystickRange(0,1023);
	XInput.begin();
}

void loop()
{
	boolean pressClicker1 = !digitalRead(Pin_clicker1);
	boolean pressClicker2 = !digitalRead(Pin_clicker2);
	boolean pressButton1 = !digitalRead(Pin_buttonA);
	boolean pressButton2 = !digitalRead(Pin_buttonB);
	boolean pressButton3 = !digitalRead(Pin_buttonX);
	boolean pressButton4 = !digitalRead(Pin_buttonY);

	boolean pressButton5 = !digitalRead(Pin_buttonMS);
	int joystickValueX = analogRead(Pin_buttonLeftJoyX);
	int joystickValueY = analogRead(Pin_buttonLeftJoyY);

	XInput.setButton(BUTTON_LB, pressClicker1); // Map Button 1 to "LB"
	XInput.setButton(BUTTON_RB, pressClicker2); // Map Button 2 to "RB"
	XInput.setButton(BUTTON_A, pressButton1);	// Map Button 3 to "A"
	XInput.setButton(BUTTON_B, pressButton2);	// Map Button 4 to "B"
	XInput.setButton(BUTTON_X, pressButton3);	// Map Button 5 to "X"
	XInput.setButton(BUTTON_Y, pressButton5);	// Map Button 6 to "Y"
	XInput.setButton(BUTTON_LB, pressButton5);
	XInput.setJoystick(JOY_LEFT, joystickValueX -100, joystickValueY);

	int rumble = XInput.getRumble();

	if (rumble > 0)
	{

		XInput.press(BUTTON_A);
		delay(1000);
		XInput.release(BUTTON_A);
		delay(1000);
	}
	else
	{
	}
}
