<script setup>
import { AppState } from '@/AppState.js';
import ModalWrapper from '@/components/ModalWrapper.vue';
import Navbar from '@/components/Navbar.vue';
import RecipeDetails from '@/components/RecipeDetails.vue';
import { favoritesService } from '@/services/FavoritesService.js';
import { recipesService } from '@/services/RecipesService.js';
import Pop from '@/utils/Pop.js';
import { computed, onMounted, ref, watch } from 'vue';

const account = computed(() => AppState.account);
const recipes = computed(() => AppState.recipes);
const recipeFavorites = computed(() => AppState.recipeFavorites);

onMounted(() => {
  getAllRecipes();
});

const selectedRecipeFilter = ref(null);

watch(account, getPersonalFavoriteRecipes);
watch(() => AppState.recipes, () => {
  selectedRecipeFilter.value = recipes.value;
}, { immediate: true })

async function getAllRecipes() {
  try {
    await recipesService.getAllRecipes();
  }
  catch (error) {
    Pop.error(error);
  }
}

async function getPersonalFavoriteRecipes() {
  try {
    await favoritesService.getPersonalFavoriteRecipes();
  }
  catch (e) {
    Pop.error(e);
  }
}


async function deleteRecipe(recipeId) {
  try {
    const confirm = await Pop.confirm("You sure want to delete this recipe?");
    if (!confirm) return;
    await recipesService.deleteRecipe(recipeId);
    Pop.toast('Recipe successfully deleted', 'success', 'top');
  }
  catch (error) {
    Pop.error(error);
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

const filterRecipes = (option) => {
  if (option === '1') {
    selectedRecipeFilter.value = AppState.recipes;
  }
  else if (option === '2') {
    selectedRecipeFilter.value = AppState.recipes.filter(recipe => recipe.creatorId === account.value.id);
  }
  else if (option === '3') {
    selectedRecipeFilter.value = AppState.recipes.filter(recipe => AppState.recipeFavorites.some(fave => fave.id === recipe.id))
  }
}

const selectedRecipe = ref(null);
function openRecipeDetailsModal(recipe) {
  selectedRecipe.value = recipe;
}

</script>

<template>
  <header class="container-fluid hero">
    <div class="row">
      <Navbar />
      <div class="col-12">
        <div class="motto text-center text-light my-3">
          <h1>All Spice</h1>
          <p class="fw-bold fs-4">A Taste Above the Rest</p>
        </div>
      </div>
      <div class="col-12">
        <div class="nav-links">
          <button type="button" @click="filterRecipes('1')" class="ms-0 ms-md-3 btn btn-outline-dark rounded">All
            Recipes</button>
          <button type="button" @click="filterRecipes('2')" class="mx-0 mx-md-5 btn btn-outline-dark rounded"
            :disabled="!account">My
            Recipes</button>
          <button type="button" @click="filterRecipes('3')" class="me-0 me-md-3 btn btn-outline-dark rounded"
            :disabled="!account">Favorites</button>
        </div>
      </div>
    </div>
  </header>

  <section class="container-fluid">
    <div class="row justify-content-center mt-5">
      <div class="col-11">
        <div class="row">
          <div v-for="recipe in selectedRecipeFilter" :key="recipe.id" class="col-md-4">
            <div class="recipe-structure position-relative card text-bg-dark mb-3">
              <button @click="openRecipeDetailsModal(recipe)"
                class="position-absolute top-0 start-0 w-100 h-100 special-btn" type="button" data-bs-toggle="modal"
                data-bs-target="#recipe-details"></button>
              <img class="recipe-img" :src="recipe.imgUrl" :alt="recipe.title">
              <div class="card-img-overlay">
                <div class="h-100 d-flex flex-column justify-content-between">
                  <div class="d-flex justify-content-between">
                    <div class="d-flex">
                      <button v-if="account?.id === recipe.creatorId" @click="deleteRecipe(recipe.id)"
                        class="recipe-delete-btn" type="button" title="Delete recipe"><i class="fa-solid fa-trash fa-lg"
                          style="color: #ff0000;"></i></button>
                      <h5 class="recipe-details-design card-title fw-bold rounded-pill text-capitalize">
                        {{
                          recipe.category }}
                      </h5>
                    </div>

                    <div v-if="account" class="favorite-recipe-structure">
                      <button v-if="recipeFavorites.some(fave => fave.id === recipe.id)"
                        @click="deleteFavoriteRecipe(recipe.id)" class="favorite-recipe-design"
                        :title="`Favorited ${recipe.title}`" type="button"><i class="fa-solid fa-heart"
                          style="color: #ff0000;"></i></button>
                      <button v-else @click="favoriteRecipe(recipe.id)" class="favorite-recipe-design" type="button"
                        title="Not yet favorited recipe"><i class="fa-regular fa-heart"></i></button>
                    </div>
                  </div>
                  <p class="recipe-details-design card-text fs-5 fw-bold rounded">{{ recipe.title }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>

  <ModalWrapper id="recipe-details">
    <RecipeDetails v-if="selectedRecipe" :recipeProp="selectedRecipe" @closed-update-modal="selectedRecipe = null" />
  </ModalWrapper>
  <button v-if="account" class="create-recipe-design" type="button" data-bs-toggle="modal"
    data-bs-target="#recipe-form"><i class="fa-solid fa-plus fa-2xl"></i></button>
</template>

<style scoped lang="scss">
.hero {
  background-image: url('https://cdn.vox-cdn.com/thumbor/QrVUJ17x9BSv_CpJ2KsjgqICWSM=/0x0:1306x871/1200x900/filters:focal(549x332:757x540)/cdn.vox-cdn.com/uploads/chorus_image/image/63268564/Mastros.0.jpg');
  background-size: cover;
  background-position: center;
}

.motto {
  max-width: fit-content;
  margin: 0 auto;
  padding: 1rem;
  background-color: rgba(0, 0, 0, 30%);
  border-radius: 10px;
}

.nav-links {
  display: flex;
  flex-wrap: wrap;
  max-width: fit-content;
  margin: 0 auto;
  padding: 1rem;
  background-color: azure;
  border-radius: 10px;
  box-shadow: 0 0 9px black;
  position: relative;
  top: 21px;
}

.recipe-structure {
  height: 400px;
  box-shadow: 0 0 9px rgba(0, 0, 0, 40%);
}

.recipe-delete-btn {
  background: none;
  border: none;
  z-index: 1;
  align-self: center;
  margin-bottom: .5rem;
}

.special-btn {
  cursor: pointer;
  z-index: 1;
  background: none;
  border: none;
}

.recipe-img {
  height: 100%;
  object-fit: cover;
  object-position: center;
}

.recipe-details-design {
  text-shadow: 1px 1px 7px rgba(240, 255, 255, 0.792);
  background-color: rgba(0, 0, 0, 40%);
  padding: .2rem .7rem .2rem .7rem;
}

.favorite-recipe-structure {
  z-index: 2;
}

.favorite-recipe-design {
  background-color: rgba(0, 0, 0, 60%);
  border-radius: 50%;
  padding: 0 .3rem 0 .3rem;
}

.create-recipe-design {
  background-color: #527360;
  border: none;
  border-radius: 50%;
  padding: 1rem;
  position: fixed;
  font-size: 30px;
  bottom: 70px;
  right: 70px;
  transition: all .4s ease-in-out;
  z-index: 3;

  &:hover {
    box-shadow: 0 0 30px rgba(106, 230, 114, 0.7);
  }
}

@media (width <=576px) {
  .nav-links {
    flex-direction: column;

    & :nth-child(2) {
      margin-top: .3rem;
      margin-bottom: .3rem;
    }
  }
}
</style>
