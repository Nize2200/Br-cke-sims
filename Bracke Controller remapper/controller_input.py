import inputs
import vgamepad as vg

# Initialize virtual Xbox controller
gamepad = vg.VX360Gamepad()

# Default mapping current controller buttons to virtual Xbox buttons
button_map = {
    "BTN_SOUTH": vg.XUSB_BUTTON.XUSB_GAMEPAD_A,  # A button
    "BTN_EAST": vg.XUSB_BUTTON.XUSB_GAMEPAD_B,   # B button
    "BTN_NORTH": vg.XUSB_BUTTON.XUSB_GAMEPAD_X,  # X button
    "BTN_WEST": vg.XUSB_BUTTON.XUSB_GAMEPAD_Y,   # Y button
    "BTN_TL": vg.XUSB_BUTTON.XUSB_GAMEPAD_LEFT_SHOULDER,  # Left bumper
    "BTN_TR": vg.XUSB_BUTTON.XUSB_GAMEPAD_RIGHT_SHOULDER, # Right bumper
    "BTN_SELECT": vg.XUSB_BUTTON.XUSB_GAMEPAD_BACK,       # Back button
    "BTN_START": vg.XUSB_BUTTON.XUSB_GAMEPAD_START,       # Start button
    "BTN_THUMBL": vg.XUSB_BUTTON.XUSB_GAMEPAD_LEFT_THUMB, # Left thumbstick button
    "BTN_THUMBR": vg.XUSB_BUTTON.XUSB_GAMEPAD_RIGHT_THUMB,# Right thumbstick button
    "ABS_HAT0X": vg.XUSB_BUTTON.XUSB_GAMEPAD_DPAD_LEFT,   # D-pad left/right
    "ABS_HAT0Y": vg.XUSB_BUTTON.XUSB_GAMEPAD_DPAD_UP,     # D-pad up/down
}

# Mapping for triggers
trigger_map = {
    "ABS_Z": "left_trigger",    # Left trigger
    "ABS_RZ": "right_trigger",  # Right trigger
}

# Deadzone threshold for joysticks and triggers
JOYSTICK_DEADZONE = 0
TRIGGER_DEADZONE = 0

# Stores which virtual button is selected for remapping
selected_virtual_button = None

# Global variables for labels
virtual_button_labels = {}
current_button_label = None

def set_virtual_button_labels(labels):
    global virtual_button_labels
    virtual_button_labels = labels

def set_current_button_label(label):
    global current_button_label
    current_button_label = label

# Function to handle controller inputs
def handle_controller_input():
    global selected_virtual_button

    while True:
        events = inputs.get_gamepad()
        for event in events:
            print(f"Event: {event.ev_type}, Code: {event.code}, State: {event.state}")  # Debugging output
            if event.ev_type == 'Key' or event.ev_type == 'Absolute':
                # Skip joystick and trigger events within the deadzone
                if event.code.startswith("ABS_"):
                    if event.code in ["ABS_X", "ABS_Y", "ABS_RX", "ABS_RY"]:
                        if abs(event.state) < JOYSTICK_DEADZONE:
                            # Ensure joystick returns to neutral state
                            if event.code in ["ABS_X", "ABS_Y"]:
                                gamepad.left_joystick(x_value=0, y_value=0)
                            elif event.code in ["ABS_RX", "ABS_RY"]:
                                gamepad.right_joystick(x_value=0, y_value=0)
                            continue
                    elif event.code in ["ABS_Z", "ABS_RZ"]:
                        if event.state < TRIGGER_DEADZONE:
                            continue

                # Update the current button label
                update_current_button_label(event.code if event.state != 0 else "None")

                if selected_virtual_button is not None:
                    # Map the pressed button to the selected virtual Xbox button
                    if event.state != 0:  # Button pressed
                        if event.code in trigger_map:
                            trigger_map[event.code] = selected_virtual_button
                        else:
                            button_map[event.code] = selected_virtual_button
                        update_button_label(selected_virtual_button, f"Mapped to {event.code}")
                        selected_virtual_button = None  # Reset remap selection
                        break

                if event.code in button_map:
                    virtual_button = button_map[event.code]
                    if event.state != 0:  # Button pressed
                        gamepad.press_button(button=virtual_button)
                    elif event.state == 0:  # Button released
                        gamepad.release_button(button=virtual_button)
                elif event.code in trigger_map:
                    if event.code == "ABS_Z":  # Left trigger
                        gamepad.left_trigger(value=event.state)
                    elif event.code == "ABS_RZ":  # Right trigger
                        gamepad.right_trigger(value=event.state)
                elif event.code == "ABS_HAT0X":
                    if event.state == -1:  # D-pad left
                        gamepad.press_button(button=vg.XUSB_BUTTON.XUSB_GAMEPAD_DPAD_LEFT)
                    elif event.state == 1:  # D-pad right
                        gamepad.press_button(button=vg.XUSB_BUTTON.XUSB_GAMEPAD_DPAD_RIGHT)
                    else:  # D-pad released
                        gamepad.release_button(button=vg.XUSB_BUTTON.XUSB_GAMEPAD_DPAD_LEFT)
                        gamepad.release_button(button=vg.XUSB_BUTTON.XUSB_GAMEPAD_DPAD_RIGHT)
                elif event.code == "ABS_HAT0Y":
                    if event.state == -1:  # D-pad up
                        gamepad.press_button(button=vg.XUSB_BUTTON.XUSB_GAMEPAD_DPAD_UP)
                    elif event.state == 1:  # D-pad down
                        gamepad.press_button(button=vg.XUSB_BUTTON.XUSB_GAMEPAD_DPAD_DOWN)
                    else:  # D-pad released
                        gamepad.release_button(button=vg.XUSB_BUTTON.XUSB_GAMEPAD_DPAD_UP)
                        gamepad.release_button(button=vg.XUSB_BUTTON.XUSB_GAMEPAD_DPAD_DOWN)
                elif event.code == "ABS_Y" or event.code == "ABS_X":  # Left joystick Y-axis
                    if abs(event.state) < JOYSTICK_DEADZONE:
                        gamepad.left_joystick(y_value=0, x_value=0)
                    else:
                        gamepad.left_joystick(y_value=event.state, x_value=event.state)

                elif event.code == "ABS_RY" or event.code == "ABS_RX":  # Right joystick Y-axis
                    if abs(event.state) < JOYSTICK_DEADZONE:
                        gamepad.right_joystick(y_value=0, x_value=0)
                    else:
                        gamepad.right_joystick(y_value=event.state, x_value=event.state)

                gamepad.update()

# Function to update the GUI label for a virtual button
def update_button_label(virtual_button, text):
    if virtual_button in virtual_button_labels:
        virtual_button_labels[virtual_button].config(text=text)

# Function to update the current button label
def update_current_button_label(button):
    current_button_label.config(text=f"Current Button: {button}")

# Function to set up remapping for a selected virtual button
def remap_button(virtual_button):
    global selected_virtual_button
    selected_virtual_button = virtual_button
    update_button_label(virtual_button, "Press a controller button to remap...")
def update_current_virutal_button_label(virtual_button):
    update_button_label(virtual_button, "Current Virual Button Label")

