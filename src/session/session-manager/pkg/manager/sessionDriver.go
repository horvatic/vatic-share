package manager

import (
	"fmt"
	"os"
)

func RunSession() {
	fmt.Println("Running Session Manager...")

	sessionOutPipe := BuildSessionOutPipe()
	fileInPipe := BuildFileInPipe()
	webApiPipe := BuildWebApiInPipe()

	writeToFileSession(fileInPipe)

	for {
		line, err := sessionOutPipe.ReadBytes('\n')
		if err == nil {
			webApiPipe.WriteString("Hello From Session Manager\n")
			fmt.Print("Message: " + string(line))
		}
	}
}

func writeToFileSession(fileInPipe *os.File) {
	fileInPipe.WriteString("Hello From Session Manager\n")
}
