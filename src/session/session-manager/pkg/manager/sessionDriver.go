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
				fileInPipe.WriteString(sharedConstants.WriteToFileCommand + strings.TrimPrefix(line, sharedConstants.DataInCommand))
			} else if strings.HasPrefix(line, sharedConstants.ReadFromFileCommand) {
				fileInPipe.WriteString(sharedConstants.ReadFromFileCommand + "\n")
			} else if strings.HasPrefix(line, sharedConstants.OutputFromFileCommand) {
				webApiPipe.WriteString(strings.TrimPrefix(line, sharedConstants.OutputFromFileCommand))
			} else {
				webApiPipe.WriteString("Unknown command\n")
			}
		}
	}
}