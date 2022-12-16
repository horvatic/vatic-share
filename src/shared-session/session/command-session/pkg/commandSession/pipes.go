package commandSession

import (
	"bufio"
	"os"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func BuildCommandOutPipe() *bufio.Reader {
	commandOutPipe, err := os.OpenFile(sharedConstants.CommandInCommandData, os.O_CREATE, os.ModeNamedPipe)
	if err != nil {
		panic(err)
	}

	return bufio.NewReader(commandOutPipe)
}
