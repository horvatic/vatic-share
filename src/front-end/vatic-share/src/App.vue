<template>
  <UserSession />
</template>

<script>
import UserSession from './components/UserSession.vue'
export default {
  name: 'App',
  components: {
    UserSession
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>