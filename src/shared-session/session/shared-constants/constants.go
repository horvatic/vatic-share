package sharedConstants

// Pipes File Process
const FileInPipeName = "/tmp/fileInPipe"
const SessionInPipeNameForFileRead = "/tmp/sessionInPipeForFileRead"

// Pipe WebApi Process
const SessionKeyDataInPipeNameForWebApi = "/tmp/sessionInPipeForWebApiKeyData"
const WebApiKeyDataInPipeName = "/tmp/webApiInPipeKeyData"
const SessionBlockDataInPipeNameForWebApi = "/tmp/sessionInPipeForWebApiBlockData"
const WebApiBlockDataInPipeName = "/tmp/webApiInPipeBlockData"

// Commands
const DataInCommand = "datain "
const WriteToFileCommand = "writefile "
const ReadFromFileCommand = "read "
const OutputBlockDataFromFileCommand = "blockoutput "
const OutputKeyDataFromFileCommand = "keyoutput "
