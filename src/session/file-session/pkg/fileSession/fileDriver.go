package fileSession

import (
	"fmt"
)

func RunDriver() {
	fmt.Println("Running File Manager...")

	fileOutPipe := BuildFileOutPipe()
	sessionInPipe := BuildSessionInPipe()

	sessionInPipe.WriteString("Hello From File Session\n")

	for {
		line, err := fileOutPipe.ReadBytes('\n')
		if err == nil {
			fmt.Print("Message: " + string(line))
		}
	}
}
