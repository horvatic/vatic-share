<template>
  <div id="session">
    <h2>WebSocket Testing</h2> 
    <textarea v-model="fileResponseMessage" @keydown="onPressFileKey" readonly/>
    <textarea v-model="messageResponseMessage" readonly/>
    <input v-model="messageRequestMessage" @keyup.enter="onPressMessageEnter"/>
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
        console.log(event.data)
        let messageData = event.data.slice(8)
        this.messageResponseMessage = this.messageResponseMessage + messageData + '\n'
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
      messageResponseMessage: '',
      messageRequestMessage: ''
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
    }
  }
}
</script>

<style>
  textarea {
    width: 1250px;
    height: 550px;
  }
</style>