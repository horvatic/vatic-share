package manager

import (
	"fmt"
)

func RunSession() {
	fmt.Println("Running Session Manager...")
	
	sessionOutPipe, fileInPipe := BuildPipes()

	line, _ := sessionOutPipe.ReadBytes('\n')
	fmt.Print("Message from File Manager: " + string(line))
	fileInPipe.WriteString("Hello From Session Manager\n")
}