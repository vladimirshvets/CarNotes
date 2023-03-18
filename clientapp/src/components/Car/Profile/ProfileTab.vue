<template>
    <v-icon>{{ icon }}</v-icon>
    <span>{{ title }}</span>
</template>

<script>
export default {
    name: 'ProfileTab',
    props: [
        'icon',
        'title'
    ]
}
</script>