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
      this.responseMessage = event.data + '|';
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
      responseMessage: 'no data',
    }
  },
  methods: {
    onPress(e) {
      if(e.key == 'Enter') {
        this.connection.send('\n');
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