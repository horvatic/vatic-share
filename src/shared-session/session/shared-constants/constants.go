package sharedConstants

//Pipe Command Process
const SessionInCommandData = "/tmp/sessionInCommandData"
const CommandInCommandData = "/tmp/commandInCommandData"
const SessionInReadCommandData = "/tmp/sessionInReadCommandData"
const WebApiInCommandData = "/tmp/webApiInCommandData"

// Pipes File Process
const FileInFileData = "/tmp/fileInFileData"
const SessionInFileRead = "/tmp/sessionInFileRead"

// Pipe WebApi Process
const SessionKeyDataInFileData = "/tmp/sessionKeyDataInFileData"
const WebApiKeyDataInFileData = "/tmp/webApiKeyDataInFileData"
const SessionBlockDataInFileData = "/tmp/sessionInPipeForWebApiBlockData"
const WebApiBlockDataInFileData = "/tmp/webApiBlockDataInFileData"

// Commands
const DataInCommand = "datain "
const CommandDataInCommand = "commanddata "
const WriteToFileCommand = "writefile "
const TriggerCommandCommand = "triggercommand "
const ReadFromFileCommand = "read "
const OutputBlockDataFromFileCommand = "blockoutput "
const OutputKeyDataFromFileCommand = "keyoutput "
