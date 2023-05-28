<template>
    <div>
        <b-input-group class="mb-2">

            <b-input-group-prepend>{{this.$cookies.get("UserName")}}</b-input-group-prepend>

            <b-form-input
                id="message"
                type="text"
                v-model="message"
                required
                @keydown.enter.prevent="send"
                @input ="isTyping"
                placeholder="Type message">
            </b-form-input>

            <b-form-file
            ref="file-input"
                    id="file"
                    v-model="file"
                    :state="Boolean(file)"
                    @keyup.native.enter="send"
                    placeholder="Choose a file or drop it here..."
                    drop-placeholder="Drop file here..."
                    accept="image/jpeg, image/png, image/jpg"
            ></b-form-file>

        </b-input-group>

        <div v-if="Chatting" class="d-flex align-items-center">
            <strong>Someone is typing      </strong>
            <b-spinner small label = "Small Spinner" type = "grow" variant = "info"></b-spinner>
        </div>
        <div v-else>
            <strong></strong>
        </div>
    </div>
</template>

<script src="~/lib/signalr/signalr.js"></script>
<script src="https://cdn.jsdelivr.net/npm/vue/dist/vue.js"></script>


<script>
import chat from './WebSocket'
import Message from '../utils/Message'
import ImageMessage from "../utils/ImageMessage"
import CombinedMessage from "../utils/CombinedMessage"
import {snowflakeGenerator} from 'snowflake-id-js'; //snowflake id generation
import { mapState, mapMutations } from 'vuex';

const signalR = require('@microsoft/signalr');
const generator = snowflakeGenerator(512);
export default {
    data: () => ({
        message: "",
        Chatting: false,
        file: null,
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

        this.hubConnection.on("SomeoneTyping",(name) =>{
            if (name != this.$cookies.get("UserName"))
            {
            if (this.Chatting ==  false)
            {
            this.Chatting = true;
            setTimeout(() => {
                this.Chatting = false;
            }, 700);
            }
            }
        });

        this.hubConnection.on("MessageReceived", (msg) => {
            this.appendMsgToChat(msg);
        });
        this.hubConnection.on("MessageDeleted",(index) => {
            this.$store.commit("DeleteMessage", index);
        })

        this.hubConnection.on("ImageAdded", (img) => {
            this.appendImgToChat(img);
        });

        this.hubConnection.on("ImageDeleted", (messId) => {
            this.$store.commit("DeleteMessage", messId);
        });


        this.hubConnection.on("ImageHistory", (images) => {});
        this.hubConnection.on("History", (data) => {});
        this.hubConnection.on("UserAdded", (username) => {});
        this.hubConnection.on("UserNotFound", (username) => {});
        this.hubConnection.on("UserFound", (username) => {});
    },
    computed: {
        ...mapState(["user"])
    },
    methods: {
        ...mapMutations(["newMessage", "replaceMessage"]),
        appendMsgToChat(msg) {
            var count = this.$store.getters.IsUnique(msg.mid)
            if(count == 0) {
                var message = new CombinedMessage(msg.mid,msg.mid,msg.user,msg.text,null)
                this.$store.commit("newMessage", message)
            }
            else if(count == 1){
                var message = this.$store.getters.getMessage(msg.mid);
                if(message.text != msg.text)
                {
                var modifiedMessage = new CombinedMessage(message.mid,message.mid,message.user,msg.text,message.url);
                this.$store.commit("replaceMessage", modifiedMessage)
                }
            }
        },
        appendImgToChat(msg) {
            var count = this.$store.getters.IsUnique(msg.mid)
            if(count == 0) {
                var message = new CombinedMessage(msg.mid,msg.mid,msg.user,"",msg.url)
                this.$store.commit("newMessage", message)
            }
            else if(count == 1){
                var message = this.$store.getters.getMessage(msg.mid);
                if(message.image != msg.url)
                {
                var modifiedMessage = new CombinedMessage(message.mid,message.mid,message.user,message.text,msg.url);
                this.$store.commit("replaceMessage", modifiedMessage)
                }
            }
        },
        send() {
            var file = document.querySelector('input[type=file]').files[0];
            let msgId = generator.next().value;
            if(this.message.length > 0){
                const messag = new Message(msgId,msgId,this.$cookies.get("UserName"),this.message);
                this.hubConnection.invoke("SendMessage", messag);
                this.message = "";
            }
            if(file){
                const reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onloadend = () => {
                var base64 = reader.result;
                base64 = base64.split(",")[1];
                const image_message = new ImageMessage(msgId,msgId,this.$cookies.get("UserName"),base64)
                this.hubConnection.invoke("AddImage", image_message)
                this.$refs['file-input'].reset()
                }
            }
        },
        isTyping()
        {
            this.hubConnection.invoke("Typing", this.$cookies.get("UserName"));
        }
    }
};
</script>