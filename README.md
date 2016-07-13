# KeyboordUsage
A keyboard statistics logger that respects your privacy and is configurable to any keyboard in any language.

<img src="keyboordusage.png">

Top features

  * No communication with external servers - your typing is saved only on your computer
  * Configurable keyboard layout (just edit a json-file to customize language, placement of keys etc)
  * Showing the ratio between producing text and deleting text (*productive ratio*)
  * Showing the ratio between editing and navigating text (*navigation ratio*)
  * Currently supported keyboards: QWERTY, DVORAK, (more to come)
  * Tracking meta-keys such as ALT, ALTgr, Control, Windows, Menu, ...


## 1. Installation

Just [download](https://github.com/kbilsted/KeyboordUsage/releases) and extract the zip file and run `KeyboordUsage.exe`

You can also make the program start whenever windows starts. See eg http://tunecomp.net/add-app-to-startup/



## 2. How to define/tweak a keyboard layout
The application is very open to new keyboard key configurations (e.g. numeric keypad, AZERTY, COLEMAK, ...). All you need to do is to go to the folder with the `KeyboordUsage.exe` from here dig into the folder `Configuration/Keyboard` (browse the .json at https://github.com/kbilsted/KeyboordUsage/tree/master/code/Configuration/Keyboard). Make a copy of any of the `.json` files and start editing. After editing simply restart the application and your new configuration has been picked up. 

If you make something great please share by making a pull request or opening an issue.



## 3. About me
I blog at http://http://firstclassthoughts.co.uk/ and have a ton of open source repositories at http://github.com/kbilsted/

