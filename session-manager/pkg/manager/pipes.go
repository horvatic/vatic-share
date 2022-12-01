package manager

import (
	"bufio"
	"os"

	"golang.org/x/sys/unix"
	"github.com/horvatic/vatic-share/sharedConstants"
)

func BuildPipes() (sessionOutPipe *bufio.Reader, fileInPipe *os.File) {
	os.Remove(sharedConstants.FileInPipeName)
	os.Remove(sharedConstants.SessionInPipeName)

	_ = unix.Mkfifo(sharedConstants.FileInPipeName, 0666)
	_ = unix.Mkfifo(sharedConstants.SessionInPipeName, 0666)

	readFile, _ := os.OpenFile(sharedConstants.SessionInPipeName, os.O_CREATE, os.ModeNamedPipe)
	writeFile, _ := os.OpenFile(sharedConstants.FileInPipeName, os.O_RDWR|os.O_CREATE|os.O_APPEND, 0777)

	return bufio.NewReader(readFile), writeFile
}
