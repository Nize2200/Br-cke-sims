import json
import os
from controller_input import button_map, trigger_map, update_button_label


def save_profile(profile_name, status_label):
    if profile_name:
        with open(f"{profile_name}.json", "w") as f:
            json.dump({"buttons": button_map, "triggers": trigger_map}, f)
        status_label.config(text=f"Profile '{profile_name}' saved!")


def load_profile(profile_name, status_label):
    if profile_name and os.path.exists(f"{profile_name}.json"):
        with open(f"{profile_name}.json", "r") as f:
            data = json.load(f)
            global button_map, trigger_map
            button_map = data["buttons"]
            trigger_map = data["triggers"]
        update_all_button_labels()
        status_label.config(text=f"Profile '{profile_name}' loaded!")

# Function to update all button labels after loading a profile
def update_all_button_labels():
    for event_code, virtual_button in button_map.items():
        update_button_label(virtual_button, f"Mapped to {event_code}")
    for event_code, virtual_button in trigger_map.items():
        update_button_label(virtual_button, f"Mapped to {event_code}")
