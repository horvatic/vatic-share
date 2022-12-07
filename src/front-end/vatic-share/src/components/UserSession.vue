<template>
  <div id="session">
    <h2>WebSocket Testing</h2> 
    <p>{{ responseMessage }}</p>
    <input v-model="requestMessage" placeholder="" />
    <button v-on:click="sendMessage()">Send Message</button>
  </div>
</template>

<script>
export default {
  name: 'UserSession',
  mounted() { 
    this.connection = new WebSocket("ws://127.0.0.1:8080")

    this.connection.onmessage = (event) => {
      console.log(event.data)
      this.responseMessage = event.data;
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
      requestMessage: null
    }
  },
  methods: {
    sendMessage() {
      this.connection.send(this.requestMessage);
    }
  }
}
</script>