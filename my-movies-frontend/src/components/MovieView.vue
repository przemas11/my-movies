<template>
  <Transition
    name="modal"
    @before-enter="beforeOpenAnimation()"
    @after-leave="afterCloseAnimation()"
  >
    <div v-if="show" class="modal-mask">
      <div class="modal-wrapper">
        <div class="modal-container">
          <div class="modal-section">
            <div id="header">
              <h1>{{ this.viewTitle }}</h1>
            </div>
          </div>

          <div class="modal-section">
            <div id="body">
              <form>
                <div
                  class="form-group"
                  :class="{ invalid: $v.title.$invalid && !formInputsDisabled }"
                >
                  <label for="movieTitle"><h4>Title</h4></label>
                  <input
                    type="text"
                    class="form-control"
                    id="movieTitle"
                    placeholder="Enter the title (max 200 characters)"
                    v-model.trim="title"
                    :disabled="formInputsDisabled"
                  />
                  <div
                    class="error"
                    v-if="!$v.title.required && !formInputsDisabled"
                  >
                    Title is required
                  </div>
                  <div
                    class="error"
                    v-if="!$v.title.maxLength && !formInputsDisabled"
                  >
                    Title must have no more than
                    {{ $v.title.$params.maxLength.max }} letters.
                  </div>
                </div>

                <div
                  class="form-group"
                  :class="{
                    invalid: $v.releaseYear.$invalid && !formInputsDisabled,
                  }"
                >
                  <label for="movieReleaseYear"><h4>Release year</h4></label>
                  <input
                    type="text"
                    class="form-control"
                    id="movieReleaseYear"
                    placeholder="Enter the release year (1900-2100, optional)"
                    v-model="releaseYear"
                    :disabled="formInputsDisabled"
                  />
                  <div
                    class="error"
                    v-if="!$v.releaseYear.numeric && !formInputsDisabled"
                  >
                    Must be a number
                  </div>
                  <div
                    class="error"
                    v-if="!$v.releaseYear.between && !formInputsDisabled"
                  >
                    Must be between {{ $v.releaseYear.$params.between.min }} and
                    {{ $v.releaseYear.$params.between.max }}
                  </div>
                </div>
              </form>
            </div>
          </div>

          <div class="modal-section">
            <div id="footer">
              <button
                type="button"
                id="button-confirm"
                class="modal-default-button btn btn-success mx-1 mb-1"
                @click="onClickButtonConfirm()"
                :disabled="formButtonsDisabled"
              >
                {{ this.viewConfirmButtonTitle }}
              </button>
              <button
                type="button"
                id="button-danger"
                class="modal-default-button btn btn-danger mx-1 mb-1"
                @click="onClickButtonCancel()"
                :disabled="formButtonsDisabled"
                v-if="this.viewCancelButtonTitle != null"
              >
                {{ this.viewCancelButtonTitle }}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </Transition>
</template>

<script>
import axios from "axios";
import {
  required,
  maxLength,
  numeric,
  between,
} from "vuelidate/lib/validators";

const apiUrl = import.meta.env.VITE_VUE_API_URL;
const viewModes = Object.freeze([
  {
    viewModeName: "NEW",
    viewTitle: "New movie",
    viewConfirmButtonTitle: "Create",
    viewCancelButtonTitle: "Cancel",
  },
  {
    viewModeName: "EDIT",
    viewTitle: "Edit movie",
    viewConfirmButtonTitle: "Modify",
    viewCancelButtonTitle: "Cancel",
  },
  {
    viewModeName: "PREVIEW",
    viewTitle: "Movie preview",
    viewConfirmButtonTitle: "Close",
    viewCancelButtonTitle: null,
  },
]);

export default {
  props: {
    viewMode: String,
    show: Boolean,
    movie: Object,
  },
  data() {
    return {
      title: "",
      releaseYear: null,
      formInputsDisabled: false,
      formButtonsDisabled: false,
    };
  },
  validations: {
    title: {
      required,
      maxLength: maxLength(200),
    },
    releaseYear: {
      numeric,
      between: between(1900, 2100),
    },
  },

  computed: {
    currentModeIndex: function () {
      // check if given mode is allowed
      let modeIndex = viewModes.findIndex(
        (mode) => mode.viewModeName == this.viewMode
      );
      if (modeIndex < 0) {
        // fallback to the default mode if viewMode prop is not correct
        return 0; // viewMode = NEW
      }

      return modeIndex;
    },
    // computed mode-dependent properties
    viewModeName: function () {
      return viewModes[this.currentModeIndex].viewModeName;
    },
    viewTitle: function () {
      return viewModes[this.currentModeIndex].viewTitle;
    },
    viewConfirmButtonTitle: function () {
      return viewModes[this.currentModeIndex].viewConfirmButtonTitle;
    },
    viewCancelButtonTitle: function () {
      return viewModes[this.currentModeIndex].viewCancelButtonTitle;
    },
  },
  methods: {
    async onClickButtonConfirm() {
      if (this.viewModeName === "PREVIEW") {
        this.closeModal();
      } else {
        // form validation
        if (this.$v.$invalid) {
          alert("Please fill the form correctly");
          return;
        }

        // prepare request body
        let requestBody = {
          Title: this.title,
          ReleaseYear: this.releaseYear,
        };

        this.formButtonsDisabled = true; // lock the buttons

        // try to send the data
        try {
          if (this.viewModeName === "NEW") {
            // create
            await axios.post(`${apiUrl}/movies`, requestBody);
          } else if (this.viewModeName === "EDIT") {
            // modify
            await axios.put(`${apiUrl}/movies/${this.movie.Id}`, requestBody);
          }
          alert("Operation has completed successfully");
          this.closeModal();
        } catch (error) {
          alert("Operation has failed\n" + error);
        }

        this.formButtonsDisabled = false; // unlock the buttons
      }
    },

    onClickButtonCancel() {
      // always close the modal
      this.closeModal();
    },

    closeModal() {
      // emit close event
      this.$emit("close");
    },

    beforeOpenAnimation() {
      // if movie exists -> copy properties values
      if (this.movie != null) {
        this.title = this.movie.Title;
        this.releaseYear = this.movie.ReleaseYear;
      }

      // replace null year with unknown text
      if (this.viewModeName == "PREVIEW") {
        this.formInputsDisabled = true;
        if (this.releaseYear == null) this.releaseYear = "unknown";
      }
    },

    afterCloseAnimation() {
      // reset input data
      Object.assign(this.$data, this.$options.data.call(this));
    },
  },
};
</script>

<style lang="scss" scoped>
.modal-mask {
  position: fixed;
  z-index: 9999;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
  transition: opacity 0.3s ease;
}

.modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.modal-container {
  max-width: 40rem;
  margin: 0px auto;
  padding: 20px 30px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  transition: all 0.3s ease;
}

.modal-section {
  text-align: center;

  #body {
    margin: 20px 0;

    input {
      text-align: center;
    }
  }
}

.invalid {
  .form-control {
    border-color: #ff6449;
  }

  .error {
    color: #ff6449;
  }
}

.modal-enter-from,
.modal-leave-to {
  opacity: 0;
}

.modal-enter-from .modal-container,
.modal-leave-to .modal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}
</style>