package manager

import (
	"strings"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func RunSession() {

	sessionOutPipe := BuildSessionOutPipe()
	fileInPipe := BuildFileInPipe()
	webApiPipe := BuildWebApiInPipe()


	for {
		rawLine, err := sessionOutPipe.ReadBytes('\n')
		line := string(rawLine)
		if err == nil {
			if strings.HasPrefix(line, sharedConstants.DataInCommand) {
				go fileInPipe.WriteString(sharedConstants.WriteToFileCommand + strings.TrimPrefix(line, sharedConstants.DataInCommand))
			} else if strings.HasPrefix(line, sharedConstants.ReadFromFileCommand) {
				message := strings.TrimSuffix(strings.TrimPrefix(line, sharedConstants.ReadFromFileCommand), "\n")
				go fileInPipe.WriteString(sharedConstants.ReadFromFileCommand + message + "\n")
			} else if strings.HasPrefix(line, sharedConstants.OutputFromFileCommand) {
				go webApiPipe.WriteString(strings.TrimPrefix(line, sharedConstants.OutputFromFileCommand))
			} else {
				go webApiPipe.WriteString("Unknown command\n")
			}
		}
	}
}