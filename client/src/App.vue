<script setup>
import ModalWrapper from './components/ModalWrapper.vue';
import RecipeForm from './components/RecipeForm.vue';

</script>

<template>
  <main class="p-3">
    <router-view />
  </main>
  <ModalWrapper id="recipe-form">
    <RecipeForm />
  </ModalWrapper>
</template>

<style lang="scss">
@import '@/assets/scss/main.scss';

:root {
  --main-height: calc(100vh - 32px - 64px);
}
</style>
