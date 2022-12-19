<template>
  <div id="session" class="container">
    <h2>Vatic Share</h2>
    <div class="d-flex">
      <div class="p-2 w-100 vh-100 flex-grow-1">
        <textarea class="w-100 h-75" v-model="fileResponseMessage" @keydown="onPressFileKey" readonly/>
        <input v-model="currentWorkingFileName" readonly/>
        <input v-model="fileName" @keyup.enter="onPressFileName"/>
        <button @click="saveFile">Save</button>
      </div>
      <div class="p-2 vh-100 flex-shrink-1">
        <div class="d-flex vh-100 flex-column">
          <div class="p-2 h-100">
            <textarea class="w-100 h-75" v-model="messageResponseMessage" readonly/>
            <input v-model="messageRequestMessage" @keyup.enter="onPressMessageEnter"/>
          </div>
          <div class="p-2 h-100">
            <textarea class="w-100 h-50" v-model="commandResponseMessage" readonly/>
            <input v-model="commandRequestMessage" @keyup.enter="onPressCommandEnter"/>
          </div>
        </div>
      </div>
    </div>
  </div>

</template>

<script>
export default {
  name: 'UserSession',
  mounted() { 
    this.connection = new WebSocket("ws://127.0.0.1:8080")
    this.connection.onmessage = (event) => {
      if(event.data.startsWith("filedata ")) {
        let fileMessage = event.data.slice(9)
        if(fileMessage == '\b') {
          this.fileResponseMessage = this.fileResponseMessage.slice(0, -2) + '|'
        } else {
          this.fileResponseMessage = this.fileResponseMessage.slice(0, -1) + fileMessage + '|'
        }
      } else if (event.data.startsWith("message ")) {
        let messageData = event.data.slice(8)
        this.messageResponseMessage = this.messageResponseMessage + messageData + '\n'
      } else if (event.data.startsWith("commanddata ")) {
        this.commandResponseMessage = event.data.slice(12)
      }
    }
    this.connection.onopen = (event) => {
      console.log(event)
      console.log("Successfully connected to the websocket server")
    }
    return
  },
  data: () => {
    return {
      connection: null,
      fileResponseMessage: '',
      fileName: '',
      messageResponseMessage: '',
      messageRequestMessage: '',
      currentWorkingFileName: 'No File Selected',
      commandResponseMessage: '',
      commandRequestMessage: ''
    }
  },
  methods: {
    onPressFileKey(e) {
      if(e.key == 'Enter') {
        this.connection.send("filekey " +'\n')
      } else if(e.key == 'Backspace') {
        this.connection.send("filekey " +'\b')
      } else if(e.key == 'Shift') {
          return
      } else {
        this.connection.send("filekey " + e.key);
      }
    },
    onPressMessageEnter() {
      this.connection.send("message " + this.messageRequestMessage)
      this.messageRequestMessage = ''
    },
    onPressFileName() {
      this.fileResponseMessage = '' + '|'
      this.currentWorkingFileName = this.fileName
      this.connection.send("filename " + this.fileName)
      this.fileName = ''
    }, onPressCommandEnter() {
      this.connection.send("command " + this.commandRequestMessage)
      this.commandRequestMessage = '';
    },
    saveFile() {
      this.connection.send("save " + this.fileName)
    }
  }
}
</script>
