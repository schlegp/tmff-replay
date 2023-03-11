# tmff-replay
Tool for replaying USB packets recorded from wireshark on your Thrustmaster FFB-Wheel

# Usage
- Record a session with wireshark
- Filter for "usb.dst == "x.y.1" where x ist the correct USB bus of your wheel and y is the id of your wheel on that bus
- Select a packet and select HID Data as a Column (right click on HID Data in the lower left)
- Export the packets as csv
- Direct your operating system to use libusb as the driver for your wheel (on Windows the easiest way to do this is to use Zadig)
- Build the solution
- Run `ReplayTool 0xb696 filename.csv` where 0xb696 is the ProductId of your wheel and filename.csv is the file you just exported
- Feel the same forces on your wheel as you did while driving
