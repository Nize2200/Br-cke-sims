import tkinter as tk
import threading
from controller_input import handle_controller_input
from gui import create_gui

def main():
    root = tk.Tk()
    status_label = create_gui(root)

    # Start a separate thread to capture controller inputs
    def start_controller_thread():
        thread = threading.Thread(target=handle_controller_input)
        thread.daemon = True  # Ensures the thread will close when the main program exits
        thread.start()

    # Start the controller input thread
    start_controller_thread()

    # Run the application
    root.mainloop()

if __name__ == "__main__":
    main()
