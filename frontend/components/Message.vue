<template>
    <div>
        <div class="msg-wrapper">
            <div class="msg">
                <div class="msg_information">
                    <span class="msg_ID"></span>
                </div>
                <p class="msg_text"> 
                    <b-button variant="outline-light" :disabled="isDisabled" @click="deleteMessage">Delete</b-button>
                    [{{ user }}] {{ text }}
                <img v-if="image" v-bind:src="image" class="resized-image"/>
                </p>
            </div>
        </div>
    </div>
</template>

<style scoped>
.resized-image {
  max-width: 256px;
  max-height: 256px;
}
</style>

<script>
import Message from '../utils/Message'
import {mapState, mapMutations} from "vuex";
import chat from './WebSocket'
const signalR = require('@microsoft/signalr');

export default {
    props: {
        messid: String,
        user: String,
        timestamp: Number,
        text: String,
        mid: String,
        image: String
    },
    created() {
        this.hubConnection = chat.createHub(this.$config.ENTRANCE_URL + "/chat");
        
        this.hubConnection
        .start()
        .then()
        .catch(er => console.log(err));

        this.hubConnection.on("MessageDeleted", (index) => {});
        this.hubConnection.on("MessageReceived", (msg) => {});
        this.hubConnection.on("History", (data) => {});
        this.hubConnection.on("UserAdded", (username) => {});
        this.hubConnection.on("SomeoneTyping",() =>{});
        this.hubConnection.on("UserNotFound", (username) => {});
        this.hubConnection.on("UserFound", (username) => {});
        this.hubConnection.on("ImageAdded", (url) => {});
        this.hubConnection.on("ImageHistory", (images) => {});
        this.hubConnection.on("ImageDeleted", (messId) => {});
    },
    computed: {
        isDisabled() {
            return this.user !== this.$cookies.get("UserName");
        }
    },
    methods: {
        deleteMessage(){
            this.text = "Message deleted";
            this.hubConnection.invoke("DeleteMessage", this.mid);
            this.hubConnection.invoke("DeleteImage", this.mid);
            this.image = null;
        }
    }
};
</script>