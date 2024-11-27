<script setup>
import { AppState } from '@/AppState.js';
import { Recipe } from '@/models/Recipe.js';
import { favoritesService } from '@/services/FavoritesService.js';
import { ingredientsService } from '@/services/IngredientsService.js';
import { recipesService } from '@/services/RecipesService.js';
import Pop from '@/utils/Pop.js';
import { computed, ref, watch } from 'vue';

const props = defineProps({
  recipeProp: { type: Recipe, required: true }
})

const emit = defineEmits(['closed-update-modal'])

const newIngredientData = ref({
  name: '',
  quantity: '',
  recipeId: 0
});

const updatableRecipeData = ref({
  title: '',
  instructions: ''
});

watch(() => props.recipeProp, () => {
  getIngredientsForRecipe();
  setupUpdateRecipeData();
}, { immediate: true })

const account = computed(() => AppState.account);
const recipeFavorites = computed(() => AppState.recipeFavorites);
const activeIngredients = computed(() => AppState.activeRecipeIngredients);

const editRecipe = ref(false);
const toggler = () => {
  editRecipe.value = !editRecipe.value;
}

const modalElement = document.getElementById('recipe-details');
modalElement.addEventListener('hide.bs.modal', () => {
  updatableRecipeData.value = {
    title: '',
    instructions: ''
  }
  newIngredientData.value = {
    name: '',
    quantity: '',
    recipeId: 0
  }

  if (editRecipe.value === true) {
    editRecipe.value = !editRecipe.value;
  }

  emit('closed-update-modal');
});


async function updateRecipe(recipeId) {
  const confirm = await Pop.confirm("Confirm Changes?");
  if (!confirm) return;
  await recipesService.updateRecipe(recipeId, updatableRecipeData.value);
  Pop.toast('Successfully updated recipe!', 'success', 'top');
  updatableRecipeData.value = {
    title: '',
    instructions: ''
  }
  editRecipe.value = false;
}

async function createIngredient(recipeId) {
  try {
    newIngredientData.value.recipeId = recipeId;
    await ingredientsService.createIngredient(newIngredientData.value);
    Pop.toast("New ingredient added successfully", 'success', 'top');
    newIngredientData.value = {
      name: '',
      quantity: '',
      recipeId: 0
    }
  }
  catch (e) {
    Pop.error(e);
  }
}

async function getIngredientsForRecipe() {
  await ingredientsService.getIngredientsForRecipe(props.recipeProp);
}

async function deleteIngredient(ingredientId) {
  try {
    const confirm = await Pop.confirm('You sure you want to delete this ingredient?');
    if (!confirm) return;
    await ingredientsService.deleteIngredient(ingredientId);
    Pop.toast("Ingredient successfully deleted", 'success', 'top');
  }
  catch (e) {
    Pop.error(e);
  }
}

async function favoriteRecipe(recipeId) {
  try {
    await favoritesService.favoriteRecipe(recipeId);
    Pop.toast("Recipe Favorited!", "success", "top");
  }
  catch (e) {
    Pop.error(e);
  }
}

async function deleteFavoriteRecipe(recipeId) {
  try {
    const recipeFavorite = recipeFavorites.value.find(fave => fave.id === recipeId);
    await favoritesService.deleteFavoriteRecipe(recipeFavorite.favoriteId);
    Pop.toast("Recipe Unfavorited!", "success", "top");
  }
  catch (e) {
    Pop.error(e);
  }
}

function setupUpdateRecipeData() {
  updatableRecipeData.value = {
    title: props.recipeProp?.title,
    instructions: props.recipeProp?.instructions
  }
}
</script>

<template>
  <section class="container-fluid p-0">
    <section class="row">
      <div class="col-md-5 h-100">
        <div class="position-relative">
          <img class="recipe-img" :src="recipeProp.imgUrl" :alt="recipeProp.title">
          <div v-if="account" class="favorite-recipe-structure">
            <button v-if="recipeFavorites.some(fave => fave.id === recipeProp.id)"
              @click="deleteFavoriteRecipe(recipeProp.id)" class="favorite-recipe-design"
              :title="`Favorited ${recipeProp.title}`" type="button"><i class="fa-solid fa-heart"
                style="color: #ff0000;"></i></button>
            <button v-else @click="favoriteRecipe(recipeProp.id)" class="favorite-recipe-design" type="button"
              title="Not yet favorited recipe"><i class="fa-regular fa-heart"></i></button>
          </div>
        </div>
      </div>
      <div class="col-md-7 h-100">
        <div v-if="account?.id === recipeProp.creatorId" class="d-flex justify-content-between">
          <button @click="toggler()" type="button" class="recipe-update-btn p-1" title="Edit recipe"><i
              v-if="editRecipe" class="fa-solid fa-x fa-lg" style="color: #74C0FC;"></i><i v-else
              class="fa-solid fa-pen fa-lg" style="color: #3ca5fb;"></i></button>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>

        </div>

        <section class="row">
          <div class="col-12">
            <div class="mb-1">
              <h3 v-if="!editRecipe">{{ recipeProp.title }} <span class="fs-5 text-capitalize">{{
                recipeProp.category
              }}</span>
              </h3>
            </div>
          </div>
          <div class="col-md-6">
            <form @submit.prevent="updateRecipe(recipeProp.id)" class="p-1">
              <h5 class="bg-success text-center text-light">Recipe Steps</h5>
              <div v-if="editRecipe" class="d-flex">
                <input v-model="updatableRecipeData.title" class="form-control w-50 mb-2" placeholder="Title...">
                <p class="fs-5 text-capitalize w-50 m-0 text-center">{{ recipeProp.category }}</p>
              </div>
              <div class="recipe-steps">
                <p v-if="!editRecipe">{{ recipeProp.instructions }}</p>
                <textarea v-else v-model="updatableRecipeData.instructions" class="form-control" rows="9"
                  name="recipeInstructions" id="recipeInstructions" placeholder="Instructions..."></textarea>
              </div>
              <div v-if="editRecipe" class="text-center mt-3">
                <button class="btn btn-outline-dark rounded-pill px-3">Update {{ recipeProp.title }}</button>
              </div>
            </form>
          </div>
          <div class="col-md-6">
            <form @submit.prevent="createIngredient(recipeProp.id)" class="p-1">
              <h5 class="bg-success text-center text-light">Ingredients</h5>
              <p v-if="!activeIngredients.length">There seems to be no ingredients very odd add something perhaps if
                you are the creator of this recipe...
              </p>
              <ul>
                <div v-for="ingredient in activeIngredients" :key="ingredient.id" class="d-flex mb-1">
                  <button @click="deleteIngredient(ingredient.id)" class="delete-ingredient-btn"
                    v-if="account?.id === recipeProp.creatorId" title="Delete ingredient" type="button"><i
                      class="fa-solid fa-x" style="color: #ff0000;"></i></button>
                  <li>{{ ingredient.quantity }} | {{
                    ingredient.name }}</li>
                </div>
              </ul>
              <div v-if="editRecipe" class="mb-3">
                <input v-model="newIngredientData.quantity" maxlength="255" class="form-control mb-1"
                  name="ingredientQuantity" id="ingredientQuantity" placeholder="Quantity" required>
                <div class="input-group">
                  <input v-model="newIngredientData.name" class="form-control position-relative"
                    placeholder="Add a ingredient...">
                  <button class="add position-absolute end-0" title="Add ingredient"><i
                      class="fa-solid fa-plus"></i></button>
                </div>
              </div>
            </form>
          </div>
        </section>
      </div>
    </section>
  </section>
</template>


<style lang="scss" scoped>
.favorite-recipe-structure {
  position: absolute;
  top: 0;
  right: 0;
  margin: .3rem .3rem 0 0;
  z-index: 2;
}

.recipe-steps {
  max-height: 300px;
  overflow-y: auto;
}

ul {
  list-style-type: none;
  padding-left: 0;
}

.recipe-img {
  aspect-ratio: 1/1;
  width: 100%;
  object-fit: cover;
  object-position: center;
}

.recipe-update-btn {
  background: none;
  border: none;
  transition: all .3s ease;

  & :hover {
    box-shadow: 1px 1px 10px rgb(53, 87, 223);
  }
}

.favorite-recipe-design {
  background-color: rgba(0, 0, 0, 40%);
  border-radius: 50%;
}

.add {
  background: none;
  border: none;
  top: 5px;
  z-index: 30;
  margin-right: .3rem;
}

textarea {
  resize: none;
}

.delete-ingredient-btn {
  background: none;
  border: none;
}
</style>