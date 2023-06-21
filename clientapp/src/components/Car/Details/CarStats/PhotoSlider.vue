<template>
    <splide :options="sliderOptions">
        <splide-slide v-for="(item, index) in photos" :key="index">
            <img class="slider-image" :src="require(`@/assets/car/profile/photos/${item}.jpg`)" alt="Car Photo" />
        </splide-slide>
    </splide>
</template>

<script>
import '@splidejs/vue-splide/css';
import { Splide, SplideSlide } from '@splidejs/vue-splide';
// import { AutoScroll } from '@splidejs/splide-extension-auto-scroll';
export default {
    name: 'PhotoSlider',
    components: {
        Splide,
        SplideSlide
    },
    data() {
        return {
            photos: [16, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15],
            sliderOptions: {
                height: '10em',
                type: 'loop',
                drag: 'free',
                autoWidth: true,
                autoplay: true
            }
        }
    }
}
</script>

<style land="less" scoped>
.slider-image {
    margin: 2px 1px 0;
    max-height: 100%;
}
</style>
