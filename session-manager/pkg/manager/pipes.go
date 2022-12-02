package manager

import (
	"bufio"
	"os"

	"github.com/horvatic/vatic-share/sharedConstants"
)

func BuildPipes() (sessionOutPipe *bufio.Reader, fileInPipe *os.File) {
	readFile, _ := os.OpenFile(sharedConstants.SessionInPipeName, os.O_CREATE, os.ModeNamedPipe)
	writeFile, _ := os.OpenFile(sharedConstants.FileInPipeName, os.O_RDWR|os.O_CREATE|os.O_APPEND, 0777)

	return bufio.NewReader(readFile), writeFile
}
