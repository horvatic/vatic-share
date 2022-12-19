package manager

import (
	"bufio"
	"os"
)

func BuildInPipe(name string) *bufio.Reader {
	outPipe, err := os.OpenFile(name, os.O_CREATE, os.ModeNamedPipe)
	if err != nil {
		panic(err)
	}

	return bufio.NewReader(outPipe)
}

func BuildOutPipe(name string) *os.File {
	inPipe, err := os.OpenFile(name, os.O_RDWR|os.O_CREATE|os.O_APPEND, 0777)
	if err != nil {
		panic(err)
	}

	return inPipe
}
