package main
import (
	"os"
    "os/exec"
	"fmt"

	"golang.org/x/sys/unix"
	"github.com/horvatic/vatic-share/sharedConstants"
)

func main() {

	os.Remove(sharedConstants.FileInPipeName)
	os.Remove(sharedConstants.SessionInPipeName)

	_ = unix.Mkfifo(sharedConstants.FileInPipeName, 0666)
	_ = unix.Mkfifo(sharedConstants.SessionInPipeName, 0666)

	sessionManager := exec.Command("./session-manager")
	sessionManager.Stdout = os.Stdout
    sessionManager.Stderr = os.Stderr
	fileSession := exec.Command("./file-session")
	fileSession.Stdout = os.Stdout
    fileSession.Stderr = os.Stderr

	sessionManager.Start()
	fileSession.Start()

	sessionManager.Wait()

	fmt.Println("Done")
}
