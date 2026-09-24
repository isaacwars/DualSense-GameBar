# DualSense GameBar Listener 🎮

A native Windows (System Tray) application written in C# that allows you to instantly open the **Xbox Game Bar** using the **PlayStation Button** on a DualSense (PS5) or DualSense Edge controller.

Unlike heavy emulators (like DS4Windows or Steam Input) or AutoHotkey scripts, this program is incredibly lightweight, consumes ~0% CPU, and communicates directly with the Windows COM API to summon the Game Bar without simulating keyboard shortcuts.

## Features ✨
* **100% Background Execution:** No annoying windows. It lives quietly in your System Tray.
* **USB & Bluetooth Support:** Works flawlessly regardless of how you connect your controller thanks to native DirectInput integration.
* **Extremely Lightweight:** Written in pure WinForms. The monitoring thread sleeps for 2 seconds when no controller is detected, and polls efficiently at 33Hz when a controller is active.
* **No Keyboard Macros:** Uses `ApplicationActivationManager` (COM) to launch the UWP Xbox Game Bar application directly, bypassing the Windows Foreground Lock by injecting a phantom F24 keystroke.
* **Plug and Play:** Supports real-time disconnections and reconnections. It instantly recognizes your controller without needing to restart the app.

## How to Install 📦
If you just want to use the application, download the installer from the **Releases** tab of this repository and install it like any other standard Windows program.

## How to Compile from Source 🛠️
You don't need to install Visual Studio. Your Windows PC already has the necessary compiler built-in.

1. Clone or download this repository.
2. Replace `icon.ico` with your preferred icon (optional).
3. Double-click the `compilar.bat` file.
4. Done! A new `DualSenseGameBar.exe` will be generated.


## Technical Details (Auditing) 🔬
* The PS button is recognized via bitmasks `4096` and `8192` (Buttons 13 and 14 in DirectInput).
* A secondary STA thread is executed when invoking the COM API to prevent the main loop from hanging if the Game Bar freezes.
* It uses a global `Mutex` to prevent multiple instances of the application from running simultaneously.
