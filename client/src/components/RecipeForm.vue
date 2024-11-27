<script setup>
import { recipesService } from '@/services/RecipesService.js';
import Pop from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { ref } from 'vue';

const editableRecipeData = ref({
  title: '',
  instructions: '',
  img: '',
  category: ''
})

async function createRecipe() {
  try {
    await recipesService.createRecipe(editableRecipeData.value);
    editableRecipeData.value = {
      title: '',
      instructions: '',
      img: '',
      category: ''
    }
    Pop.toast('Recipe Successfully Created!', 'success', 'top');
    Modal.getOrCreateInstance('#recipe-form').hide();
  }
  catch (error) {
    Pop.error(error);
  }
}
</script>


<template>
  <section class="container-fluid p-0">
    <h2 class="bg-success text-light ps-2">New Recipe</h2>
    <form @submit.prevent="createRecipe()" class="row p-3">

      <div class="col-12">
        <div v-if="editableRecipeData.img">
          <img class="img-fluid w-100 recipe-form-img" :src="editableRecipeData.img">
        </div>
        <div class="mb-3">
          <label class="form-label" for="recipeImg">Image</label>
          <input v-model="editableRecipeData.img" class="form-control" type="url" name="recipeImg" id="recipeImg"
            maxlength="1000" required>
        </div>
      </div>

      <div class="col-md-7">
        <div class="mb-3">
          <label class="form-label" for="recipeTitle">Title</label>
          <input v-model="editableRecipeData.title" class="form-control" id="recipeTitle" name="recipeTitle"
            placeholder="Title..." maxlength="255" required>
        </div>
      </div>

      <div class="col-md-5">
        <div class="mb-3">
          <label for="recipeCategory" class="form-label">Category</label>
          <select v-model="editableRecipeData.category" class="form-select form-select-md" name="recipeCategory"
            id="recipeCategory" required>
            <option disabled selected value="">Select an appetizer</option>
            <option value="breakfast">Breakfast</option>
            <option value="lunch">Lunch</option>
            <option value="dinner">Dinner</option>
            <option value="snack">Snack</option>
            <option value="dessert">Dessert</option>
          </select>
        </div>
      </div>

      <div class="col-12">
        <div class="mb-3">
          <label class="form-label" for="recipeInstructions">Instructions</label>
          <textarea class="form-control" v-model="editableRecipeData.instructions" name="recipeInstructions"
            id="recipeInstructions" placeholder="Recipe Instructions..." rows="4" maxlength="1000" required></textarea>
        </div>
      </div>

      <div class="col-12">
        <div class="d-flex justify-content-end">
          <button class="btn btn-outline-dark rounded" type="button" data-bs-dismiss="modal"
            aria-label="Close">Cancel</button>
          <button class="btn btn-outline-info rounded ms-2">Submit</button>
        </div>
      </div>
    </form>
  </section>
</template>


<style lang="scss" scoped>
.recipe-form-img {
  aspect-ratio: 2/1;
  height: 300px;
  object-fit: cover;
  object-position: center;
}

textarea {
  resize: none;
}
</style>