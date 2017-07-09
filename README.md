# Encrypted-Messenger-Application

## Overview:
Quick 2 week C# project for Uni. A messaging application was created using Client/Server contracts. These contracts provided a mechanism to pass data between clients using separate Messengers. Encryption was performed before reaching the Server to insure the integrity of the message would never be accessible through hacking the Server. Decryption was performed done on the recipient side, again reassuring the safety of its content. 

## Setup:
There are two optinons to set up.

### To test:

- Open `ChattingServer.exe` from `ChattingServer/bin/Debug`

- and then Open `Messenger.exe` from `Messenger/Bin/Debug`

### To edit:

- Run `Messenger.sln` from Visual Studio

## To Use:

#### Getting Started
-	Open the `ChattingServer.exe` Application file on a work station that will not be switched off at any point during this process
-	Wait until the `ChattingServer.exe` console is ready. This can be established by the black console window opening and displaying the text “Server is running…” 
-	Open the “Messenger” Application file on a computer which access the same local network as the computer running the server
-	To Login - use the active textbox at the top and press login

#### Usage:
- Once the user has logged in it is possible to send group messages. Otherwise, select your own username from the username options on the left hand side of the screen
-	Select the user that you wish to message from the online user options on the left
- Check the Important check box send the message as usual
-	Close the “Messenger” window with the “X” button at the top right of the application
-	Only close the “Server” if all users have successfully logged out
-	To access previous conversations log into the Messenger application with the same username used when previous conversations were held. Then select the user you wish to read previous messages from

