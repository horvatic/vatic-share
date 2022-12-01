package fileSession

import (
	"bufio"
	"os"

	"golang.org/x/sys/unix"
	"github.com/horvatic/vatic-share/sharedConstants"
)

func BuildPipes() (sessionInPipe *bufio.Reader, fileInPipeName *os.File) {
	writeFile, _ := os.OpenFile(controlInPipe, os.O_RDWR|os.O_CREATE|os.O_APPEND, 0777)
	readFile, _ := os.OpenFile(fileWorkerInPipe, os.O_CREATE, os.ModeNamedPipe)
	reader := bufio.NewReader(readFile)

	return writeFile, readFile
}
