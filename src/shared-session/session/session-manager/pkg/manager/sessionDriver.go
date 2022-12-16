package manager

import (
	"fmt"

	"strings"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func RunSession() {
	go runDataFileSession()
	go runKeyDataSession()
	go runCommandDataSession()
	runBlockDataSession()
}

func runCommandDataSession() {
	sessionCommandDataOutFromWebApiPipe := BuildSessionCommandDataOutFromWebApiPipe()

	for {
		rawLine, err := sessionCommandDataOutFromWebApiPipe.ReadBytes('\n')
		if err == nil {
			line := string(rawLine)
			if strings.HasPrefix(line, sharedConstants.CommandDataInCommand) {
				message := strings.TrimSuffix(strings.TrimPrefix(line, sharedConstants.CommandDataInCommand), "\n")
				fmt.Println(message)
			}
		}
	}
}


func runBlockDataSession() {
	sessionBlockDataOutFromWebApiPipe := BuildSessionBlockDataOutFromWebApiPipe()
	fileInPipe := BuildFileInPipe()

	for {
		rawLine, err := sessionBlockDataOutFromWebApiPipe.ReadBytes('\n')
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

func runKeyDataSession() {
	sessionKeyDataOutFromWebApiPipe := BuildSessionKeyDataOutFromWebApiPipe()
	fileInPipe := BuildFileInPipe()

	for {
		rawLine, err := sessionKeyDataOutFromWebApiPipe.ReadBytes('\n')
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

func runDataFileSession() {
	webApiBlockDataPipe := BuildWebApiBlockDataInPipe()
	webApiKeyDataPipe := BuildWebApiKeyDataInPipe()
	sessionOutFromFileReadPipe := BuildSessionOutFromFileReadPipe()

	for {
		rawLine, err := sessionOutFromFileReadPipe.ReadBytes('\n')
		if err == nil {
			line := string(rawLine)
			if strings.HasPrefix(line, sharedConstants.OutputBlockDataFromFileCommand) {
				webApiBlockDataPipe.WriteString(strings.TrimPrefix(line, sharedConstants.OutputBlockDataFromFileCommand))
			} else if strings.HasPrefix(line, sharedConstants.OutputKeyDataFromFileCommand) {
				webApiKeyDataPipe.WriteString(strings.TrimPrefix(line, sharedConstants.OutputKeyDataFromFileCommand))
			} else {
				webApiBlockDataPipe.WriteString("Unknown command\n")
			}
		}
	}
}
