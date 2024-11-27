<script setup>
import Login from './Login.vue';

</script>

<template>
  <nav class="px-3 col-12">
    <div class="d-flex justify-content-end align-items-center w-100 mb-3">
      <!-- NOTE Scrapped -->
      <!-- <form class="me-3">
        <div class="input-group">
          <input type="search" class="form-control position-relative" placeholder="Search Recipe">
          <button class="search position-absolute end-0" type="button"><i class="fa-brands fa-searchengin fa-lg"
              style="color: #d7a700;"></i></button>
        </div>
      </form> -->
      <Login />
    </div>
  </nav>
</template>

<style scoped>
.search {
  background: none;
  border: none;
  top: 5px;
  z-index: 30;
}
</style>
