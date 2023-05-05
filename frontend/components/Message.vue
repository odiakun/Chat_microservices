<template>
    <div>
        <div class="msg-wrapper">
            <div class="msg">
                <div class="msg_information">
                    <span class="msg_ID">{{ messid }}</span>
                </div>
                <p class="msg_text"> 
                    <b-button variant="outline-light" :disabled="isDisabled" @click="deleteMessage">Delete</b-button>
                    [{{ user }}] {{ text }}
                </p>
            </div>
        </div>
    </div>
</template>

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
        mid: String
    },
    created() {
        this.hubConnection
        .start()
        .then(() => console.log("Connected to the hub"))
        .catch(er => console.log(err));

        this.hubConnection.on("MessageDeleted", (index) => {});

        this.hubConnection.on("MessageReceived", (msg) => {});
    },
    computed: {
        isDisabled() {
            return this.user !== this.$cookies.get("UserName");
        }
    },
    methods: {
        deleteMessage(){
            this.text = "Message deleted";
            this.hubConnection.invoke("DeleteMessage", this.messid);
        }
    }
};
</script>