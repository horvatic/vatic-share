package main

import (
	"os"
	"os/exec"

	"github.com/horvatic/vatic-share/sharedConstants"
	"golang.org/x/sys/unix"
)

func main() {

	os.Remove(sharedConstants.FileInPipeName)
	os.Remove(sharedConstants.SessionInPipeNameForFileRead)
	os.Remove(sharedConstants.SessionKeyDataInPipeNameForWebApi)
	os.Remove(sharedConstants.WebApiKeyDataInPipeName)
	os.Remove(sharedConstants.SessionBlockDataInPipeNameForWebApi)
	os.Remove(sharedConstants.WebApiBlockDataInPipeName)

	_ = unix.Mkfifo(sharedConstants.FileInPipeName, 0666)
	_ = unix.Mkfifo(sharedConstants.SessionInPipeNameForFileRead, 0666)
	_ = unix.Mkfifo(sharedConstants.SessionKeyDataInPipeNameForWebApi, 0666)
	_ = unix.Mkfifo(sharedConstants.WebApiKeyDataInPipeName, 0666)
	_ = unix.Mkfifo(sharedConstants.SessionBlockDataInPipeNameForWebApi, 0666)
	_ = unix.Mkfifo(sharedConstants.WebApiBlockDataInPipeName, 0666)

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
