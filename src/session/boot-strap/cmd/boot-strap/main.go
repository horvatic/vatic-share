package main

import (
	"os"
	"os/exec"

	"github.com/horvatic/vatic-share/sharedConstants"
	"golang.org/x/sys/unix"
)

func main() {

	os.Remove(sharedConstants.FileInPipeName)
	os.Remove(sharedConstants.SessionInPipeName)
	os.Remove(sharedConstants.WebApiInPipeName)

	_ = unix.Mkfifo(sharedConstants.FileInPipeName, 0666)
	_ = unix.Mkfifo(sharedConstants.SessionInPipeName, 0666)
	_ = unix.Mkfifo(sharedConstants.WebApiInPipeName, 0666)

	webApi := exec.Command("./Share.Web")
	webApi.Stdout = os.Stdout
	webApi.Stderr = os.Stderr
	sessionManager := exec.Command("./session-manager")
	sessionManager.Stdout = os.Stdout
	sessionManager.Stderr = os.Stderr
	fileSession := exec.Command("./file-session")
	fileSession.Stdout = os.Stdout
	fileSession.Stderr = os.Stderr

	sessionManager.Start()
	fileSession.Start()
	webApi.Start()

	webApi.Wait()
}
