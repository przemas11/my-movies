<script setup>
import MovieList from "./components/MovieList.vue";

export default {
  components: {
    MovieList,
  },
};
</script>

<template>
  <div id="app">
    <MovieList />

    <MountingPortal mountTo="#modal-target">
      <PortalTarget name="modal-target" />
    </MountingPortal>
  </div>
</template>

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
