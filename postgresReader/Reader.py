import tkinter as tk
import psycopg2
import psycopg2.extras
import json
import logging
import os
import traceback

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


# Function to fetch the latest 4 controller states from the database
def fetch_latest_states():
    try:
        with psycopg2.connect(**db_params) as conn:
            with conn.cursor(cursor_factory=psycopg2.extras.RealDictCursor) as cursor:
                cursor.execute("SELECT index_, state_ FROM xinput_list ORDER BY index_ DESC LIMIT 4")
                results = cursor.fetchall()
                states = []
                for result in results:
                    index = result['index_']
                    state_data = result['state_']

                    logging.debug(f"Fetched state_data for index {index}: {state_data} (type: {type(state_data)})")

                    # Check the type of state_data, assume it's JSON stored as a string
                    try:
                        state = json.loads(state_data) if isinstance(state_data,
                                                                     (str, bytes, bytearray)) else state_data
                        states.append({'index': index, 'state': state})
                    except json.JSONDecodeError as jde:
                        logging.error(f"JSON decoding error for index {index}: {jde}")
                        states.append({'index': index, 'state': None})

                return states
    except Exception as e:
        logging.error(f"Error fetching data from database: {e}")
        logging.error(traceback.format_exc())
        return []


# Function to update the GUI with the latest controller states
def update_gui():
    try:
        logging.debug("update_gui called.")
        states = fetch_latest_states()

        for i in range(4):
            if i < len(states):
                index = states[i]['index']
                state = states[i]['state']

                index_vars[i].set(f"Index: {index}")

                if state:
                    buttons_vars[i].set(f"Buttons: {state.get('Buttons', 'N/A')}")
                    left_trigger_vars[i].set(f"Left Trigger: {state.get('LeftTrigger', 'N/A')}")
                    right_trigger_vars[i].set(f"Right Trigger: {state.get('RightTrigger', 'N/A')}")
                    thumb_lx_vars[i].set(f"ThumbLX: {state.get('LeftThumbX', 'N/A')}")
                    thumb_ly_vars[i].set(f"ThumbLY: {state.get('LeftThumbY', 'N/A')}")
                    thumb_rx_vars[i].set(f"ThumbRX: {state.get('RightThumbX', 'N/A')}")
                    thumb_ry_vars[i].set(f"ThumbRY: {state.get('RightThumbY', 'N/A')}")
                else:
                    # If state is None, display N/A
                    buttons_vars[i].set("Buttons: N/A")
                    left_trigger_vars[i].set("Left Trigger: N/A")
                    right_trigger_vars[i].set("Right Trigger: N/A")
                    thumb_lx_vars[i].set("ThumbLX: N/A")
                    thumb_ly_vars[i].set("ThumbLY: N/A")
                    thumb_rx_vars[i].set("ThumbRX: N/A")
                    thumb_ry_vars[i].set("ThumbRY: N/A")
            else:
                # If there's no state available for this index, set all to 'N/A'
                index_vars[i].set(f"Index: N/A")
                buttons_vars[i].set("Buttons: N/A")
                left_trigger_vars[i].set("Left Trigger: N/A")
                right_trigger_vars[i].set("Right Trigger: N/A")
                thumb_lx_vars[i].set("ThumbLX: N/A")
                thumb_ly_vars[i].set("ThumbLY: N/A")
                thumb_rx_vars[i].set("ThumbRX: N/A")
                thumb_ry_vars[i].set("ThumbRY: N/A")
    except Exception as e:
        logging.error(f"Exception in update_gui: {e}")
        logging.error(traceback.format_exc())
    finally:
        root.after(100, update_gui)  # Schedule the next update in 1 second


# Create the main window
root = tk.Tk()
root.title("Controller State")

# Create StringVar variables to hold the state values for each controller index
index_vars = [tk.StringVar(value=f"Index {i}: Loading...") for i in range(4)]
buttons_vars = [tk.StringVar(value=f"Buttons {i}: Loading...") for i in range(4)]
left_trigger_vars = [tk.StringVar(value=f"Left Trigger {i}: Loading...") for i in range(4)]
right_trigger_vars = [tk.StringVar(value=f"Right Trigger {i}: Loading...") for i in range(4)]
thumb_lx_vars = [tk.StringVar(value=f"ThumbLX {i}: Loading...") for i in range(4)]
thumb_ly_vars = [tk.StringVar(value=f"ThumbLY {i}: Loading...") for i in range(4)]
thumb_rx_vars = [tk.StringVar(value=f"ThumbRX {i}: Loading...") for i in range(4)]
thumb_ry_vars = [tk.StringVar(value=f"ThumbRY {i}: Loading...") for i in range(4)]

# Create labels to display the state values for each index in a grid layout (left to right)
for i in range(4):
    tk.Label(root, textvariable=index_vars[i], font=("Arial", 14)).grid(row=0, column=i, padx=10, pady=10)
    tk.Label(root, textvariable=buttons_vars[i], font=("Arial", 14)).grid(row=1, column=i, padx=10, pady=10)
    tk.Label(root, textvariable=left_trigger_vars[i], font=("Arial", 14)).grid(row=2, column=i, padx=10, pady=10)
    tk.Label(root, textvariable=right_trigger_vars[i], font=("Arial", 14)).grid(row=3, column=i, padx=10, pady=10)
    tk.Label(root, textvariable=thumb_lx_vars[i], font=("Arial", 14)).grid(row=4, column=i, padx=10, pady=10)
    tk.Label(root, textvariable=thumb_ly_vars[i], font=("Arial", 14)).grid(row=5, column=i, padx=10, pady=10)
    tk.Label(root, textvariable=thumb_rx_vars[i], font=("Arial", 14)).grid(row=6, column=i, padx=10, pady=10)
    tk.Label(root, textvariable=thumb_ry_vars[i], font=("Arial", 14)).grid(row=7, column=i, padx=10, pady=10)

# Start updating the GUI
update_gui()

# Run the main loop
root.mainloop()
