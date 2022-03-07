<template>
  <div>
    <header class="d-flex justify-content-center py-3">
      <h1>My movies</h1>
    </header>

    <button type="button" class="btn btn-primary" @click="addNewMovie()">
      Add new movie
    </button>
    <table
      class="table justify-content-center my-4"
      aria-label="table of movies"
    >
      <thead>
        <tr>
          <th scope="col"></th>
          <th scope="col">Title</th>
          <th scope="col">Release year</th>
          <th scope="col">Options</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(movie, index) in this.moviesArray" :key="movie.Id">
          <th scope="row">{{ index + 1 }}</th>
          <td>{{ movie.Title }}</td>
          <td>{{ movie.ReleaseYear || "unknown" }}</td>
          <td>
            <button
              type="button"
              class="btn btn-success btn-sm mx-1 mb-1"
              @click="previewMovie(movie.Id)"
            >
              Preview
            </button>
            <button
              type="button"
              class="btn btn-info btn-sm mx-1 mb-1"
              @click="editMovie(movie.Id)"
            >
              Edit
            </button>
            <button
              type="button"
              class="btn btn-danger btn-sm mx-1 mb-1"
              @click="deleteMovie(movie)"
            >
              Delete
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <portal to="modal-target">
      <MovieView
        :show="showMovieView"
        :viewMode="movieViewMode"
        :movie="selectedMovie"
        @close="onCloseMovieViewModal"
      />
    </portal>
  </div>
</template>

<script>
import MovieView from "./MovieView.vue";
import axios from "axios";
const apiUrl = import.meta.env.VITE_VUE_API_URL;

export default {
  components: {
    MovieView,
  },
  data() {
    return {
      moviesArray: [],
      selectedMovie: {},
      movieViewMode: "PREVIEW",
      showMovieView: false,
    };
  },
  methods: {
    async getAllMovies() {
      try {
        const response = await axios.get(`${apiUrl}/movies`);
        this.moviesArray = response.data;
      } catch (error) {
        alert("Loading movies list failed\n" + error);
      }
    },

    addNewMovie() {
      this.openMovieViewModal("NEW");
    },

    previewMovie(id) {
      if (this.idCheck(id)) {
        this.openMovieViewModal("PREVIEW", id);
      }
    },

    editMovie(id) {
      if (this.idCheck(id)) {
        this.openMovieViewModal("EDIT", id);
      }
    },

    openMovieViewModal(viewMode, movieId) {
      this.selectedMovie = this.moviesArray.find((mov) => mov.Id == movieId);
      this.movieViewMode = viewMode;
      this.showMovieView = true;
    },

    onCloseMovieViewModal() {
      this.showMovieView = false;
      this.selectedMovie = {};

      this.getAllMovies(); // reload movies list
    },

    async deleteMovie(movie) {
      if (this.idCheck(movie.Id) === false) return;

      if (
        confirm(
          "Are you sure you want to delete this movie?\n" +
            `Title: ${movie.Title}\n` +
            `Release year: ${movie.ReleaseYear || "unknown"}`
        ) === false
      )
        return;

      // delete movie if id is correct and user confirms
      try {
        await axios.delete(`${apiUrl}/movies/${movie.Id}`);
      } catch (error) {
        alert("Movie deletion failed\n" + error);
      }

      this.getAllMovies(); // reload movies list
    },

    idCheck(id) {
      if (id == null || isNaN(id)) {
        alert("Error: Movie ID is incorrect");
        return false;
      }
      return true;
    },
  },

  beforeMount() {
    this.getAllMovies(); // reload movies list
  },
};
</script>

<style lang="scss" scoped>
h1 {
  font-size: 5rem;
}
.table {
  width: auto;
  margin: auto;

  & tr {
    line-height: 30px;
    min-height: 30px;
    height: 30px;

    & > td {
      overflow-wrap: anywhere;
    }
  }
}
</style>