<template>
    <div>
        <b-input-group class="mb-2">
            <b-form-input
                id="message"
                type="text"
                v-model="message"
                required
                @keydown.enter.prevent="send"
                placeholder="Type message">
            </b-form-input>
        </b-input-group>
    </div>
</template>

<script src="~/lib/signalr/signalr.js"></script>
<script src="https://cdn.jsdelivr.net/npm/vue/dist/vue.js"></script>


<script>
import chat from './WebSocket'
import Message from '../utils/Message'
import {snowflakeGenerator} from 'snowflake-id-js'; //snowflake id generation
import { mapState, mapMutations } from 'vuex';

const signalR = require('@microsoft/signalr');
const generator = snowflakeGenerator(512);
export default {
    data: () => ({
        message: "",
    }),
    created(){
        this.user = this.$cookies.get("UserName");

        if(this.user == ""){
            this.$router.push("");
        }
        this.hubConnection = chat.createHub(this.$config.ENTRANCE_URL + "/chat");

        this.hubConnection
        .start()
        .then(()=>console.log("Connected to the hub"))
        .catch(err => console.log(err));

        this.hubConnection.on("MessageReceived", (msg) => {
            this.appendMsgToChat(msg);
        });
        this.hubConnection.on("MessageDeleted",(index) => {
            this.$store.commit("DeleteMessage", index);
        })
        this.hubConnection.on("History", (data) => {});
        this.hubConnection.on("UserAdded", (username) => {});
    },
    computed: {
        ...mapState(["user"])
    },
    methods: {
        ...mapMutations(["newMessage"]),
        appendMsgToChat(msg) {
            console.log("Firing up appendMsgToChat method")
            if(this.$store.getters.IsUnique(msg.mid) == 0) {
                this.$store.commit("newMessage", msg)
                console.log("commited a new message to messages")
            }
        },
        send() {
            console.log("fire up send method")
            if(this.message.length > 0){
                console.log("sent the message");
                let msgId = generator.next().value;
                const messag = new Message(msgId,msgId,this.$cookies.get("UserName"),this.message);
                this.hubConnection.invoke("SendMessage", messag);
                this.message = "";
            }
        },
    }
};
</script>