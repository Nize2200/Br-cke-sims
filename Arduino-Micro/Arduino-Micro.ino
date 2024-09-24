#include <XInput.h>

const uint8_t Pin_ButtonA = 3;
const uint8_t Pin_ButtonB = 2;
const uint8_t Pin_LED = LED_BUILTIN;
const uint8_t Pin_JoystickXaxis = A0;
const uint8_t Pin_JoystickYaxis = A1;
const int AnalogMax = 1023; // 10-bit resolution
const uint8_t Pin_BigRumble = A3;

int CenterXValue = 0;
int CenterYValue = 0;

void setup()
{
	pinMode(Pin_ButtonA, INPUT_PULLUP);
	pinMode(Pin_ButtonB, INPUT_PULLUP);
	XInput.setRange(JOY_LEFT, 0, AnalogMax);
	XInput.begin();
}

void loop()
{

	uint16_t rumble = XInput.getRumble();
	boolean pressA = !digitalRead(Pin_ButtonA);
	boolean pressB = !digitalRead(Pin_ButtonB);
  int joystickValue = analogRead(Pin_JoystickXaxis);
	XInput.setJoystick(JOY_LEFT, joystickValue, AnalogMax / 2);
	XInput.setButton(BUTTON_A, pressA);
	XInput.setButton(BUTTON_B, pressB);

	if (rumble > 0)
	{
		XInput.press(BUTTON_A);
		delay(1000);
		XInput.release(BUTTON_A);
		delay(1000);
	}

	else
	{
		analogWrite(Pin_BigRumble, 0);
	}
}
