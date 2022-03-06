import { createVuePlugin } from 'vite-plugin-vue2'
import { defineConfig } from 'vite';

export default defineConfig({
  plugins: [
    createVuePlugin(/* options */)
  ],
  server: {
    port: 3000,
    proxy: {
      "/api": {
        target: "http://localhost:5000",
        changeOrigin: true,
        secure: false,
      }
    },
  },
  define: {
    'process.env': process.env
  }
});