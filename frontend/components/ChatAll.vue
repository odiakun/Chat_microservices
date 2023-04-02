<template>
    <div>
        <div>
            <b-form-input id="message" type="text" v-model="message" required @keyup.native.enter="send" placeholder="Type your message here..."></b-form-input>
        </div>
    </div>
</template>

<script>
    export default {
        data(){
            return{
                message: ""
            }
        },
        methods: {
            send(){
                console.log(this.message);
                this.message = "";
            }
        }
    }
</script>