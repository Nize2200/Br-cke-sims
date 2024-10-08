# A simple Windows Service
that records current time every second while the program is running.

## Installation
You can install the program via cmd (administator mode) with command:
sc create service_test1 binPath=””

## Start
You can start the program at cmd with command:
sc start service_test1

## Start
To stop the program:
sc stop service_test1

## For more info about sc commands
check the link below:
https://learn.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2012-r2-and-2012/cc754599(v=ws.11)

The main code is in the [Service1.sc] file.