<template>
    <v-footer class="footer text-center d-flex flex-column">
        <v-divider></v-divider>
        <div class="text-center">
            &copy; {{ new Date().getFullYear() }} Car Notes
        </div>
    </v-footer>
</template>

<script>
export default {
    name: 'FooterComponent'
}
</script>

<style lang="less" scoped>
.footer {
    padding: 2em 0;
    flex-grow: 0;
}
</style>
