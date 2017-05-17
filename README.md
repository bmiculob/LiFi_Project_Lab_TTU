# LiFi_Project_Lab_TTU
This a git repository containing source code as well as schematics for the Li-Fi project at Texas Tech.
Sponsored by Dr. Zhaoyang Fan
# System Operating Instructions.
1.	Adjust the transmitter and the receiver with the lens so that the light is focused on the photodiode on the receiver side.
	*	Transmitter LED is powered with 12 V
	*	Turn the transmitter microcontroller off so that the LED remains on when powered with 12 V
2.	Keep only one microcontroller plugged into the laptop whether it be the transmitter or receiver.
3.	Got the the command prompt and type the command "mode" to determine which COM port it is and write it down or memorize it.
4.	Do the same for the other microcontroller.
5.	Open both the transmitter and receiver UI.
6.	Configure the transmitter values so that the Serial Port and Baudrate (200000) are set. All other values should remain default.
7.	Configure the receiver values so that the Serial Port and Baudrate (230414) are set. All other values should remain default.
8.	Try the test signal and see that the receiver detects it.
	*	If the indicator doesn’t work then save the data to file and check it.
9.	For transmitting a file chose a file on the transmitter side by clicking the browse button.
10.	On the receiver side choose a file path to save by clicking the browse button.
	*	Every time the path is changed the UART connection must toggle from closed state to open state.
11.	Once both UI’s are configured click the transmit button on the transmitter side to transmit the file.
12.	Once it is verified that the receiver is done transmitting close the UART connection and check if the file transmitted successfully.
# Schematics
The jumpers are referenced from the chip side of the tiva microcontroller.
## Transmitter
![alt text][Transmitter]
## Receiver
![alt text][Receiver]

[Transmitter]: https://github.com/soukoba/LiFi_Project_Lab_TTU/blob/master/TransmitterSchematic.PNG
[Receiver]: https://github.com/soukoba/LiFi_Project_Lab_TTU/blob/master/ReceiverSchematic.PNG
