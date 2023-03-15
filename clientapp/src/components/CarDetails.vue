<template>
  <h2>Car Page</h2>
  <div>Car Id: {{  $route.params.id }}</div>
  <v-row>
    <v-col
      cols="12"
      offset-sm="3"
    >
      <v-card height="400px">
        <v-card-title class="bg-blue">
          <span class="text-h5">Active Tab Name?</span>

          <v-spacer></v-spacer>

          <router-link :to="{ name: 'carProfile' }">Profile</router-link> |
          <router-link :to="{ name: 'carRefuelings' }">Refuelings</router-link> |
          <router-link :to="{ name: 'carWashings' }">Washings</router-link>
          
        </v-card-title>

        <v-card-text>
          <router-view></router-view> 
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>
