package fileSession

import (
	"fmt"
)

func RunDriver() {
	fmt.Println("Running File Manager...")
	
	fileOutPipe, sessionInPipe := BuildPipes()

	sessionInPipe.WriteString("Hello From File Session\n")
	line, _ := fileOutPipe.ReadBytes('\n')
	fmt.Print("Message from Session Manager: " + string(line))
}