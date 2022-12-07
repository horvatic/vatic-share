package fileSession

import (
	"strings"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func RunDriver() {

	fileOutPipe := BuildFileOutPipe()
	sessionInPipe := BuildSessionInPipe()

	fileStore := make(map[string]string)

	for {
		rawLine, err := fileOutPipe.ReadBytes('\n')
		line := string(rawLine)
		if err == nil {
			if strings.HasPrefix(line, sharedConstants.WriteToFileCommand) {
				message := strings.TrimSuffix(strings.TrimPrefix(line, sharedConstants.WriteToFileCommand), "\n")
				sessionMessage := strings.SplitN(message, " ", 2)
				fileStore[sessionMessage[0]] = sessionMessage[1]
			} else if strings.HasPrefix(line, sharedConstants.ReadFromFileCommand)  {
				message := strings.TrimSuffix(strings.TrimPrefix(line, sharedConstants.ReadFromFileCommand), "\n")
				go sessionInPipe.WriteString(sharedConstants.OutputFromFileCommand + fileStore[message] + "\n")
			} else {
				go sessionInPipe.WriteString("Unknown command\n")
			}
		}
	}
}
