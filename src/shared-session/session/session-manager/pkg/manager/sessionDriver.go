package manager

import (
	"strings"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func RunSession() {

	sessionOutFromWebApiPipe := BuildSessionOutFromWebApiPipe()
	fileInPipe := BuildFileInPipe()


	go runFileSession()

	for {
		rawLine, err := sessionOutFromWebApiPipe.ReadBytes('\n')
		if err == nil {
			line := string(rawLine)
			if strings.HasPrefix(line, sharedConstants.DataInCommand) {
				fileInPipe.WriteString(sharedConstants.WriteToFileCommand + strings.TrimPrefix(line, sharedConstants.DataInCommand))
			} else if strings.HasPrefix(line, sharedConstants.ReadFromFileCommand) {
				message := strings.TrimSuffix(strings.TrimPrefix(line, sharedConstants.ReadFromFileCommand), "\n")
				fileInPipe.WriteString(sharedConstants.ReadFromFileCommand + message + "\n")
			} 
		}
	}
}

func runFileSession() {
	webApiPipe := BuildWebApiInPipe()
	sessionOutFromFileReadPipe := BuildSessionOutFromFileReadPipe()

	for {
		rawLine, err := sessionOutFromFileReadPipe.ReadBytes('\n')
		if err == nil {
			line := string(rawLine)
			if strings.HasPrefix(line, sharedConstants.OutputFromFileCommand) {
				webApiPipe.WriteString(strings.TrimPrefix(line, sharedConstants.OutputFromFileCommand))
			} else {
				webApiPipe.WriteString("Unknown command\n")
			}
		}
	}
}