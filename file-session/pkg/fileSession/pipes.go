package fileSession

import (
	"bufio"
	"os"
	
	"github.com/horvatic/vatic-share/sharedConstants"
)

func BuildPipes() (fileOutPipe *bufio.Reader, sessionInPipe *os.File) {
	writeFile, _ := os.OpenFile(sharedConstants.SessionInPipeName, os.O_RDWR|os.O_CREATE|os.O_APPEND, 0777)
	readFile, _ := os.OpenFile(sharedConstants.FileInPipeName, os.O_CREATE, os.ModeNamedPipe)

	return bufio.NewReader(readFile), writeFile
}
