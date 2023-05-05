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
            this.online = type === 'Online';
        },
        handler: function handler(event) {
            const url = "http://login.hpds" + "/users" + "/" + this.$cookies.get("UserName");
            let result = axios.delete(url, {
                userName: this.$cookies.get("UserName")
            })
            .then((result) => {
                console.log(result);
            })
            .catch((err) => {
                console.log(err)
            });
        }
    },
    computed: {
        ...mapState(["user", "messages"])
    },
    watch: {
        onLine(v) {
            if(v) {

            }else {
                const url = "http://login.hpds" + "/users" + "/" + this.$cookies.get("UserName");
                let result = axios.delete(url, {
                    userName: this.$cookies.get("UserName")
                })
                .then((result) => {
                    console.log(result);
                })
                .catch((err) => {
                    console.log(err)
                });
                alert("Connection lost. Log again.");
                this.$router.push("");
            }
        }
    },
    mounted() {
        window.addEventListener('Online', this.updateOnlineStatus);
        window.addEventListener('Offline', this.updateOnlineStatus);
    },
    beforeDestroy() {
        window.removeEventListener('Online', this.updateOnlineStatus);
        window.removeEventListener('Offline', this.updateOnlineStatus);
    },
    created() {
        window.addEventListener('BeforeUnload', this.handler);
        document.addEventListener('BeforeUnload', this.handler);

        let url = "http://chat.hpds" + "/messages";
        let result = axios.get(url)
        .then((result) => {
            let data = result.data;
            for(let i=0;i<data.length;i++){
                this.$store.commit('newMessage', data[i]);
            }
        })
    }
};


</script>