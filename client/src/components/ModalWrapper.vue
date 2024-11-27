<script setup>

defineProps({ modalId: String })

</script>


<template>
  <div class="modal fade" tabindex="-1" aria-hidden="true" :id="modalId">
    <div class="modal-dialog modal-dialog-centered modal-xl">
      <div class="modal-content">
        <div class="modal-body">
          <slot></slot>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.modal-content {
  --bs-modal-padding: 0;
}
</style>