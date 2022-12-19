package commandSession

import (
	"bufio"
	"os"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func BuildCommandInPipe() *bufio.Reader {
	commandInPipe, err := os.OpenFile(sharedConstants.CommandInCommandData, os.O_CREATE, os.ModeNamedPipe)
	if err != nil {
		panic(err)
	}

	return bufio.NewReader(commandInPipe)
}

func BuildCommandOutPipe() *os.File {
	commandOutPipe, err := os.OpenFile(sharedConstants.SessionInReadCommandData, os.O_RDWR|os.O_CREATE|os.O_APPEND, 0777)
	if err != nil {
		panic(err)
	}

	return commandOutPipe
}
