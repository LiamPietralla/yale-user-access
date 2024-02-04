// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  modules: [ '@nuxtjs/tailwindcss' ],
  devtools: { enabled: true },
  runtimeConfig: {
    public: {
      apiBaseUrl: 'https://localhost:7069'
    }
  }
})
