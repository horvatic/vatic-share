package manager

import (
	"strings"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func RunSession() {
	go runDataFileSession()
	go runKeyDataSession()
	go runCommandDataSession()
	go runCommandResultsSession()
	runBlockDataSession()
}

func runCommandResultsSession() {
	sessionComamndDataIn := BuildInPipe(sharedConstants.SessionInReadCommandData)
	webApiOut := BuildOutPipe(sharedConstants.WebApiInCommandData)

	for {
		rawLine, err := sessionComamndDataIn.ReadBytes('\n')
		if err == nil {
			line := string(rawLine)
			webApiOut.WriteString(strings.TrimSuffix(line, "\n") +  "\n")
		}
	}

}

func runCommandDataSession() {
	sessionCommandDataOutFromWebApiPipe := BuildInPipe(sharedConstants.SessionInCommandData)
	commandInPipe := BuildOutPipe(sharedConstants.CommandInCommandData)

	for {
		rawLine, err := sessionCommandDataOutFromWebApiPipe.ReadBytes('\n')
		if err == nil {
			line := string(rawLine)
			if strings.HasPrefix(line, sharedConstants.CommandDataInCommand) {
				message := strings.TrimSuffix(strings.TrimPrefix(line, sharedConstants.CommandDataInCommand), "\n")
				commandInPipe.WriteString(sharedConstants.TriggerCommandCommand + message +  "\n")
			}
		}
	}
}


func runBlockDataSession() {
	sessionBlockDataOutFromWebApiPipe := BuildInPipe(sharedConstants.SessionBlockDataInFileData)
	fileInPipe := BuildOutPipe(sharedConstants.FileInFileData)

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
	sessionKeyDataOutFromWebApiPipe := BuildInPipe(sharedConstants.SessionKeyDataInFileData)
	fileInPipe := BuildOutPipe(sharedConstants.FileInFileData)

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
	webApiBlockDataPipe := BuildOutPipe(sharedConstants.WebApiBlockDataInFileData)
	webApiKeyDataPipe := BuildOutPipe(sharedConstants.WebApiKeyDataInFileData)
	sessionOutFromFileReadPipe := BuildInPipe(sharedConstants.SessionInFileRead)

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
