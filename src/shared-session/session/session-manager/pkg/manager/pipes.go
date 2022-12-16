package manager

import (
	"bufio"
	"os"

	"github.com/horvatic/vatic-share/sharedConstants"
)

// Command
func BuildSessionCommandDataOutFromWebApiPipe() *bufio.Reader {
	sessionOutPipe, err := os.OpenFile(sharedConstants.SessionInPipeForWebApiCommandData, os.O_CREATE, os.ModeNamedPipe)
	if err != nil {
		panic(err)
	}

	return bufio.NewReader(sessionOutPipe)
}

// File
func BuildSessionOutFromFileReadPipe() *bufio.Reader {
	sessionOutPipe, err := os.OpenFile(sharedConstants.SessionInPipeNameForFileRead, os.O_CREATE, os.ModeNamedPipe)
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

func BuildSessionKeyDataOutFromWebApiPipe() *bufio.Reader {
	sessionOutPipe, err := os.OpenFile(sharedConstants.SessionKeyDataInPipeNameForWebApi, os.O_CREATE, os.ModeNamedPipe)
	if err != nil {
		panic(err)
	}

	return bufio.NewReader(sessionOutPipe)
}

func BuildWebApiKeyDataInPipe() *os.File {
	webApiInPipe, err := os.OpenFile(sharedConstants.WebApiKeyDataInPipeName, os.O_RDWR|os.O_CREATE|os.O_APPEND, 0777)
	if err != nil {
		panic(err)
	}

	return webApiInPipe
}

func BuildSessionBlockDataOutFromWebApiPipe() *bufio.Reader {
	sessionOutPipe, err := os.OpenFile(sharedConstants.SessionBlockDataInPipeNameForWebApi, os.O_CREATE, os.ModeNamedPipe)
	if err != nil {
		panic(err)
	}

	return bufio.NewReader(sessionOutPipe)
}

func BuildWebApiBlockDataInPipe() *os.File {
	webApiInPipe, err := os.OpenFile(sharedConstants.WebApiBlockDataInPipeName, os.O_RDWR|os.O_CREATE|os.O_APPEND, 0777)
	if err != nil {
		panic(err)
	}

	return webApiInPipe
}
