const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
    transpileDependencies: true,
    devServer: {
        proxy: 'https://localhost:7140'
    },
    pages: {
        index: {
            entry: 'src/main.js',
            title: 'Car Notes'
        }
    }
})
