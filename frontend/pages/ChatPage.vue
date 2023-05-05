<template>
    <ChatAll />
</template>

<script>
import Vue from 'vue'
import ChatAll from "../components/ChatAll.vue"
import MessageForm from "../components/MessageForm.vue"

export default Vue.extend({
    name: "ChatPage",
    components: {ChatAll, MessageForm}
})
</script>