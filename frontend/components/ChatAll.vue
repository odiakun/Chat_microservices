<template>
    <div class="chat-wrapper">
        <div class="chat_form">
            <MessageForm />
        </div>

        <div class="chat" ref="chat">
            <Message
       v-for="(misag) in messages.slice().reverse()"

       :key="misag.messid"
       :user="misag.user"
       :text="misag.text"
       :timestamp="misag.timestamp"
        :messid="misag.messid"
        :mid="misag.mid"
     />
        </div>
        
    </div>
</template>

<script src="~/lib/signalr/signalr.js"></script>
<script src="https://cdn.jsdelivr.net/npm/vue/dist/vue.js"></script>

<script>
import Message from '@/components/Message';
import MessageForm from '@/components/MessageForm';
import { mapState, mapMutations } from 'vuex';
import axios from "axios";
import chat from './WebSocket'

export default {
    components: {
        Message,
        MessageForm
    },
    head() {
        return {
            title: 'Chat Room'
        };
    },
    methods: {
        ...mapMutations(["newMessage"]),
        updateOnlineStatus(e) {
            const {type} = e;
            this.online = type === 'online';
        },
        handler: function handler(event) {
            if (this.hubConnection && this.hubConnection.state === 'Connected') {
            this.hubConnection.invoke("DeleteUser", this.$cookies.get("UserName"));
            }
        }
    },
    computed: {
        ...mapState(["user", "messages"])
    },
    watch: {
        onLine(v) {
            if(v) {

            }else {
                if (this.hubConnection && this.hubConnection.state === 'Connected') {
                this.hubConnection.invoke("DeleteUser", this.$cookies.get("UserName"));
                }
                alert("Connection lost. Log again.");
                this.$router.push("");
            }
        }
    },
    mounted() {
        window.addEventListener('online', this.updateOnlineStatus);
        window.addEventListener('offline', this.updateOnlineStatus);
    },
    beforeDestroy() {
        window.removeEventListener('online', this.updateOnlineStatus);
        window.removeEventListener('offline', this.updateOnlineStatus);
    },
    created() {
        window.addEventListener('beforeunload', this.handler);
        document.addEventListener('beforeunload', this.handler);

        this.hubConnection = chat.createHub(this.$config.ENTRANCE_URL + "/chat");

        this.hubConnection
        .start()
        .then(() => this.hubConnection.invoke("GetHistory"))
        .catch(err => console.log(err));

        var control = 0;

        this.hubConnection.on("History", (data) => {
            if (control == 0)
            {
                for(let i=0;i<data.length;i++)
                {
                    this.$store.commit('newMessage', data[i]);
                }
                control = 1;
            }
        })
        this.hubConnection.on("UserAdded", (username) => {});
        this.hubConnection.on("MessageDeleted", (index) => {});
        this.hubConnection.on("MessageReceived", (msg) => {});
    }
};


</script>