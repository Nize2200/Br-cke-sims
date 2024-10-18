import pygame
from pygame.locals import *

# Initialize pygame
pygame.init()

# Initialize the joystick
pygame.joystick.init()
joystick = pygame.joystick.Joystick(0)
joystick.init()

# Define a mapping for the buttons
button_mapping = {
    0: "Ax01",  # Button A
    1: "Bx02",  # Button B
    2: "Xx03",  # Button X
    3: "Yx04",  # Button Y
}

running = True
while running:
    for event in pygame.event.get():
        if event.type == JOYBUTTONDOWN:
            button = event.button
            if button in button_mapping:
                print(f"Button {button} pressed, command: {button_mapping[button]}")
        elif event.type == QUIT:
            running = False

# Quit pygame
pygame.quit()
