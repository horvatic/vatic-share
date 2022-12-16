package main

import (
	"os"
	"os/exec"

	"github.com/horvatic/vatic-share/sharedConstants"
	"golang.org/x/sys/unix"
)

func main() {

	makePipe(sharedConstants.FileInFileData)
	makePipe(sharedConstants.SessionInFileRead)
	makePipe(sharedConstants.SessionKeyDataInFileData)
	makePipe(sharedConstants.WebApiKeyDataInFileData)
	makePipe(sharedConstants.SessionBlockDataInFileData)
	makePipe(sharedConstants.WebApiBlockDataInFileData)
	makePipe(sharedConstants.SessionInCommandData)
	makePipe(sharedConstants.CommandInCommandData)
	makePipe(sharedConstants.SessionInReadCommandData)
	makePipe(sharedConstants.WebApiInCommandData)

	webApi := exec.Command("./Share.Web")
	webApi.Stdout = os.Stdout
	webApi.Stderr = os.Stderr
	sessionManager := exec.Command("./session-manager")
	sessionManager.Stdout = os.Stdout
	sessionManager.Stderr = os.Stderr
	fileSession := exec.Command("./file-session")
	fileSession.Stdout = os.Stdout
	fileSession.Stderr = os.Stderr
	commandSession := exec.Command("./command-session")
	commandSession.Stdout = os.Stdout
	commandSession.Stderr = os.Stderr

	sessionManager.Start()
	fileSession.Start()
	webApi.Start()
	commandSession.Start()

	webApi.Wait()
}

func makePipe(name string) {
	os.Remove(name)
	_ = unix.Mkfifo(name, 0666)
}