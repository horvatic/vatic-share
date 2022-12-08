package fileSession

import (
	"strings"
	b64 "encoding/base64"
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
				decode, _ := b64.StdEncoding.DecodeString(sessionMessage[1])
				fileStore[sessionMessage[0]] = fileStore[sessionMessage[0]] + string(decode)
				go sessionInPipe.WriteString(sharedConstants.OutputFromFileCommand + sessionMessage[1] + "\n")
			} else if strings.HasPrefix(line, sharedConstants.ReadFromFileCommand)  {
				message := strings.TrimSuffix(strings.TrimPrefix(line, sharedConstants.ReadFromFileCommand), "\n")
				encode := b64.StdEncoding.EncodeToString([]byte(fileStore[message]))
				go sessionInPipe.WriteString(sharedConstants.OutputFromFileCommand + encode + "\n")
			} else {
				go sessionInPipe.WriteString("Unknown command\n")
			}
		}
	}
}
