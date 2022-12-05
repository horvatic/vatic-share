package fileSession

import (
	"strings"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func RunDriver() {

	fileOutPipe := BuildFileOutPipe()
	sessionInPipe := BuildSessionInPipe()

	message := ""

	for {
		rawLine, err := fileOutPipe.ReadBytes('\n')
		line := string(rawLine)
		if err == nil {
			if strings.HasPrefix(line, sharedConstants.WriteToFileCommand) {
				message = strings.TrimSuffix(strings.TrimPrefix(line, sharedConstants.WriteToFileCommand), "\n")
			} else if strings.HasPrefix(line, sharedConstants.ReadFromFileCommand)  {
				go sessionInPipe.WriteString(sharedConstants.OutputFromFileCommand + message + "\n")
			} else {
				go sessionInPipe.WriteString("Unknown command\n")
			}
		}
	}
}
