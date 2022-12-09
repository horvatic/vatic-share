package manager

import (
	"bufio"
	"os"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func BuildSessionOutFromFileReadPipe() *bufio.Reader {
	sessionOutPipe, err := os.OpenFile(sharedConstants.SessionInPipeNameForFileRead, os.O_CREATE, os.ModeNamedPipe)
	if err != nil {
		panic(err)
	}

	return bufio.NewReader(sessionOutPipe)
}

func BuildSessionOutFromWebApiPipe() *bufio.Reader {
	sessionOutPipe, err := os.OpenFile(sharedConstants.SessionInPipeNameForWebApi, os.O_CREATE, os.ModeNamedPipe)
	if err != nil {
		panic(err)
	}

	return bufio.NewReader(sessionOutPipe)
}

func BuildFileInPipe() *os.File {
	fileInPipe, err := os.OpenFile(sharedConstants.FileInPipeName, os.O_RDWR|os.O_CREATE|os.O_APPEND, 0777)
	if err != nil {
		panic(err)
	}

	return fileInPipe
}

func BuildWebApiInPipe() *os.File {
	webApiInPipe, err := os.OpenFile(sharedConstants.WebApiInPipeName, os.O_RDWR|os.O_CREATE|os.O_APPEND, 0777)
	if err != nil {
		panic(err)
	}

	return webApiInPipe
}
