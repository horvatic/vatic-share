<template>
  <div id="session">
    <h2>WebSocket Testing</h2> 
    <textarea v-model="responseMessage" @keydown="onPress" readonly/>
  </div>
</template>

<script>
export default {
  name: 'UserSession',
  mounted() { 
    this.connection = new WebSocket("ws://127.0.0.1:8080")
    this.connection.onmessage = (event) => {
      this.responseMessage = this.responseMessage.slice(0, -1) + event.data + '|';
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
      responseMessage: '',
    }
  },
  methods: {
    onPress(e) {
      if(e.key == 'Enter') {
        this.connection.send('\n');
      } else if(e.key == 'Shift') {
        return
      } else {
        this.connection.send(e.key);
      }
    }
  }
}
</script>

<style>
  textarea {
    width: 1250px;
    height: 750px;
  }
</style>