import tkinter as tk
import psycopg2
import psycopg2.extras
import json
import logging
import os

# Configure logging
logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')

# Database connection parameters
db_params = {
    'host': 'localhost',
    'port': 5432,
    'user': 'postgres',
    'password': os.getenv('DB_PASSWORD', 'Nirre123'),  # Use environment variable for password
    'dbname': 'seeeds'
}


# Function to fetch the latest controller state from the database
def fetch_latest_state():
    try:
        # Establish connection using context manager
        with psycopg2.connect(**db_params) as conn:
            # Use RealDictCursor to get results as dictionaries
            with conn.cursor(cursor_factory=psycopg2.extras.RealDictCursor) as cursor:
                cursor.execute("SELECT state_ FROM xinput_list ORDER BY id DESC LIMIT 1")
                result = cursor.fetchone()
                if result and 'state_' in result:
                    state_data = result['state_']
                    logging.debug(f"Fetched state_data: {state_data} (type: {type(state_data)})")

                    # Check the type of state_data
                    if isinstance(state_data, (str, bytes, bytearray)):
                        # If it's a string, parse it as JSON
                        try:
                            return json.loads(state_data)
                        except json.JSONDecodeError as jde:
                            logging.error(f"JSON decoding error: {jde}")
                            return None
                    elif isinstance(state_data, dict):
                        # Already a dict
                        return state_data
                    else:
                        logging.error(f"Unexpected data type for state_: {type(state_data)}")
                        return None
                else:
                    logging.warning("No data found in the database.")
                    return None
    except Exception as e:
        logging.error(f"Error fetching data from database: {e}")
        return None


# Function to update the GUI with the latest controller state
def update_gui():
    try:
        logging.debug("update_gui called.")
        state = fetch_latest_state()
        if state:
            logging.debug(f"Updating GUI with state: {state}")
            buttons_var.set(f"Buttons: {state.get('Buttons', 'N/A')}")
            left_trigger_var.set(f"Left Trigger: {state.get('LeftTrigger', 'N/A')}")
            right_trigger_var.set(f"Right Trigger: {state.get('RightTrigger', 'N/A')}")
            thumb_lx_var.set(f"ThumbLX: {state.get('LeftThumbX', 'N/A')}")
            thumb_ly_var.set(f"ThumbLY: {state.get('LeftThumbY', 'N/A')}")
            thumb_rx_var.set(f"ThumbRX: {state.get('RightThumbX', 'N/A')}")
            thumb_ry_var.set(f"ThumbRY: {state.get('RightThumbY', 'N/A')}")
        else:
            logging.warning("State is None, setting all GUI variables to 'N/A'.")
            buttons_var.set("Buttons: N/A")
            left_trigger_var.set("Left Trigger: N/A")
            right_trigger_var.set("Right Trigger: N/A")
            thumb_lx_var.set("ThumbLX: N/A")
            thumb_ly_var.set("ThumbLY: N/A")
            thumb_rx_var.set("ThumbRX: N/A")
            thumb_ry_var.set("ThumbRY: N/A")
    except Exception as e:
        logging.error(f"Exception in update_gui: {e}")
    finally:
        root.after(10, update_gui)  # Schedule next update


# Create the main window
root = tk.Tk()
root.title("Controller State")

# Create StringVar variables to hold the state values
buttons_var = tk.StringVar(value="Buttons: Loading...")
left_trigger_var = tk.StringVar(value="Left Trigger: Loading...")
right_trigger_var = tk.StringVar(value="Right Trigger: Loading...")
thumb_lx_var = tk.StringVar(value="ThumbLX: Loading...")
thumb_ly_var = tk.StringVar(value="ThumbLY: Loading...")
thumb_rx_var = tk.StringVar(value="ThumbRX: Loading...")
thumb_ry_var = tk.StringVar(value="ThumbRY: Loading...")

# Create labels to display the state values
tk.Label(root, textvariable=buttons_var, font=("Arial", 14)).pack(pady=5)
tk.Label(root, textvariable=left_trigger_var, font=("Arial", 14)).pack(pady=5)
tk.Label(root, textvariable=right_trigger_var, font=("Arial", 14)).pack(pady=5)
tk.Label(root, textvariable=thumb_lx_var, font=("Arial", 14)).pack(pady=5)
tk.Label(root, textvariable=thumb_ly_var, font=("Arial", 14)).pack(pady=5)
tk.Label(root, textvariable=thumb_rx_var, font=("Arial", 14)).pack(pady=5)
tk.Label(root, textvariable=thumb_ry_var, font=("Arial", 14)).pack(pady=5)

# Start updating the GUI
update_gui()

# Run the main loop
root.mainloop()
