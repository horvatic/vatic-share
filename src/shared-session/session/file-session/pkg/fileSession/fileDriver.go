package fileSession

import (
	b64 "encoding/base64"
	"github.com/horvatic/vatic-share/sharedConstants"
	"strings"
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
				if string(decode) == "\b" {
					currentSessionMessage := fileStore[sessionMessage[0]]
					if currentSessionMessage != "" {
						fileStore[sessionMessage[0]] = currentSessionMessage[:len(currentSessionMessage)-1]
					}
				} else {
					fileStore[sessionMessage[0]] = fileStore[sessionMessage[0]] + string(decode)
				}
				sessionInPipe.WriteString(sharedConstants.OutputKeyDataFromFileCommand + sessionMessage[0] + " " + sessionMessage[1] + "\n")
			} else if strings.HasPrefix(line, sharedConstants.ReadFromFileCommand) {
				filename := strings.TrimSuffix(strings.TrimPrefix(line, sharedConstants.ReadFromFileCommand), "\n")
				encode := b64.StdEncoding.EncodeToString([]byte(fileStore[filename]))
				sessionInPipe.WriteString(sharedConstants.OutputBlockDataFromFileCommand + filename + " " + encode + "\n")
			} else {
				sessionInPipe.WriteString("Unknown command\n")
			}
		}
	}
}
