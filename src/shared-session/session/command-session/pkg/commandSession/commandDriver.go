package commandSession

import (
	b64 "encoding/base64"
	"os/exec"
	"strings"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func RunDriver() {

	commandInPipe := BuildCommandInPipe()
	commandOutPipe := BuildCommandOutPipe()

	for {
		rawLine, err := commandInPipe.ReadBytes('\n')
		line := string(rawLine)
		if err == nil {
			if strings.HasPrefix(line, sharedConstants.TriggerCommandCommand) {
				message := strings.TrimSuffix(strings.TrimPrefix(line, sharedConstants.TriggerCommandCommand), "\n")
				sessionMessage := strings.SplitN(message, " ", 2)
				decode, _ := b64.StdEncoding.DecodeString(sessionMessage[1])
				cmd := strings.SplitN(string(decode), " ", -1)

				if len(cmd) == 1 {
					userCommand := exec.Command(cmd[0])
					output, _ := userCommand.CombinedOutput()
					commandOutPipe.WriteString(sessionMessage[0] + " " + b64.StdEncoding.EncodeToString(output) + "\n")
				} else {
					userCommand := exec.Command(cmd[0], cmd[1:]...)
					output, _ := userCommand.CombinedOutput()
					commandOutPipe.WriteString(sessionMessage[0] + " " + b64.StdEncoding.EncodeToString(output) + "\n")
				}
			}
		}
	}
}