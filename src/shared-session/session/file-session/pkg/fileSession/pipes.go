package fileSession

import (
	"bufio"
	"os"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func BuildFileOutPipe() *bufio.Reader {
	fileOutPipe, err := os.OpenFile(sharedConstants.FileInFileData, os.O_CREATE, os.ModeNamedPipe)
	if err != nil {
		panic(err)
	}

	return bufio.NewReader(fileOutPipe)
}

func BuildSessionInPipe() *os.File {
	sessionInPipe, err := os.OpenFile(sharedConstants.SessionInFileRead, os.O_RDWR|os.O_CREATE|os.O_APPEND, 0777)
	if err != nil {
		panic(err)
	}

	return sessionInPipe
}
