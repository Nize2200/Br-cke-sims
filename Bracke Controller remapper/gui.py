import tkinter as tk
from tkinter import ttk
from controller_input import remap_button, set_virtual_button_labels, set_current_button_label, trigger_map, button_map
from profile_management import save_profile, load_profile

def create_gui(root):
    root.title("Controller Remapper")

    # Create a frame for the scrollbar
    main_frame = ttk.Frame(root, padding="10")
    main_frame.pack(fill=tk.BOTH, expand=True)

    # Create a canvas for the scrollbar
    canvas = tk.Canvas(main_frame)
    canvas.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)

    # Add a scrollbar to the canvas
    scrollbar = ttk.Scrollbar(main_frame, orient=tk.VERTICAL, command=canvas.yview)
    scrollbar.pack(side=tk.RIGHT, fill=tk.Y)

    # Configure the canvas
    canvas.configure(yscrollcommand=scrollbar.set)
    canvas.bind('<Configure>', lambda e: canvas.configure(scrollregion=canvas.bbox("all")))

    # Create another frame inside the canvas
    inner_frame = ttk.Frame(canvas)
    canvas.create_window((0, 0), window=inner_frame, anchor="nw")

    # Create a label to show the name of the virtual controller
    name_label = ttk.Label(inner_frame, text="Bracke grip modifier", font=('Helvetica', 16, 'bold'))
    name_label.pack(pady=10)

    # Create a label to show the program is running
    status_label = ttk.Label(inner_frame, text="Select what button on your profile to remap:", font=('Helvetica', 14))
    status_label.pack(pady=20)

    # Dictionary to store the button labels for the virtual Xbox controller
    virtual_button_labels = {}
    # Create buttons for each of the virtual Xbox buttons
    name_label = ttk.Label(inner_frame, text="Buttons", font=('Helvetica', 16, 'bold'))
    name_label.pack(pady=10)

    for button_name, virtual_button in button_map.items():
        button_label = ttk.Label(inner_frame, text=f"Profile {button_name}: Not mapped", font=('Helvetica', 12))
        button_label.pack(pady=5)

        remap_btn = ttk.Button(inner_frame, text=f"Remap Virtual {button_name}", command=lambda vb=virtual_button: remap_button(vb))
        remap_btn.pack(pady=5)

        # Store the label so it can be updated later
        virtual_button_labels[virtual_button] = button_label

    # Create buttons for each of the virtual Xbox triggers
    name_label = ttk.Label(inner_frame, text="Triggers", font=('Helvetica', 16, 'bold'))
    name_label.pack(pady=10)

    for trigger_name, virtual_trigger in trigger_map.items():
        trigger_label = ttk.Label(inner_frame, text=f"Profile  {trigger_name}: Not mapped", font=('Helvetica', 12))
        trigger_label.pack(pady=5)

        remap_btn = ttk.Button(inner_frame, text=f"Remap Virtual {trigger_name}", command=lambda vt=virtual_trigger: remap_button(vt))
        remap_btn.pack(pady=5)

        # Store the label so it can be updated later
        virtual_button_labels[virtual_trigger] = trigger_label

    # Create a label to display the current button being pressed
    name_label = ttk.Label(inner_frame, text="Current button", font=('Helvetica', 16, 'bold'))
    name_label.pack(pady=10)

    current_button_label = ttk.Label(inner_frame, text="Current Button: None", font=('Helvetica', 12))
    current_button_label.pack(pady=20)

    viewport_label = ttk.Label(inner_frame, text="Virtual Button", font=('Helvetica', 12))
    viewport_label.pack(pady=20)

    set_virtual_button_labels(virtual_button_labels)
    set_current_button_label(current_button_label)

    # Create entry and buttons for saving/loading profiles
    profile_entry = ttk.Entry(inner_frame, font=('Helvetica', 12))
    profile_entry.pack(pady=5)

    save_btn = ttk.Button(inner_frame, text="Save Profile", command=lambda: save_profile(profile_entry.get(), status_label))
    save_btn.pack(pady=5)

    load_btn = ttk.Button(inner_frame, text="Load Profile", command=lambda: load_profile(profile_entry.get(), status_label))
    load_btn.pack(pady=5)

    return status_label
