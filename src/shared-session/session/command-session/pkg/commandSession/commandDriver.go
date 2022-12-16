package commandSession

import (
	"fmt"

	b64 "encoding/base64"
	"os/exec"
	"strings"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func RunDriver() {

	commandOutPipe := BuildCommandOutPipe()

	for {
		rawLine, err := commandOutPipe.ReadBytes('\n')
		line := string(rawLine)
		if err == nil {
			if strings.HasPrefix(line, sharedConstants.TriggerCommandCommand) {
				message := strings.TrimSuffix(strings.TrimPrefix(line, sharedConstants.TriggerCommandCommand), "\n")
				sessionMessage := strings.SplitN(message, " ", 2)
				decode, _ := b64.StdEncoding.DecodeString(sessionMessage[1])

				userCommand := exec.Command(string(decode))
				output, _ := userCommand.CombinedOutput()

				fmt.Println(string(output))
			}
		}
	}
}