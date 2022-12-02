package manager

import (
	"fmt"
)

func RunSession() {
	fmt.Println("Running Session Manager...")
	
	sessionOutPipe, fileInPipe := BuildPipes()

	line, err := sessionOutPipe.ReadBytes('\n')
	fmt.Print("Message from File Manager: " + string(line))
	fileInPipe.WriteString("Hello From Session Manager\n")
	for {
		line, err = sessionOutPipe.ReadBytes('\n')
		if err == nil {
			fmt.Println("Waitng for Web API")
			fmt.Print("Message from Web Api: " + string(line))
			fmt.Println("Hit for Web Hit")
		}
	}
}