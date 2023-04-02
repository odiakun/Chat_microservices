<template>
    <div>
    <div>
        <b-jumbotron 
        fluid=true
        bg-variant="dark"
        text-variant="white"
        header="The best web chat!" 
        header-level="4"
        lead="Be yourself while chatting with your online friends">
            <p>Log in to the web chat application</p>
        </b-jumbotron>
    </div>
    <div class="w-50 pb-1 mx-auto">
        <b-form-group 
            id="fieldset-1" 
            label="Enter your name:" 
            label-for="input-1"
            valid-feedback="Thanks">

            <b-form-input 
            id="input-1" 
            v-model="form.name" 
            placeholder="Doesn't have to be your real name."
            required >

            </b-form-input>
        </b-form-group>

        <b-form-group 
        id="fieldset-2" 
        label="Email address" 
        label-for="input-2" 
        description="We'll never share your email with anyone else."
        >
            <b-form-input
                id="input-2"
                v-model="form.email"
                type="email"
                placeholder="Enter email"
                required
            ></b-form-input>
        </b-form-group>

        <b-form-group 
        id="select-1"
        label="Gender"
        label-for="input-3"
        >
        <b-form-select
            id="input-3"
            v-model="form.gender"
            :options="genders"
            required
        ></b-form-select>
    </b-form-group>

        <b-button variant="dark" size="lg" @click="Log()">Enter the chat </b-button>

    </div>
    </div>
</template>


<script>
     import axios from "axios";

    export default {
        env: {
            LoginUrl: process.env.HPDS_LOGIN_URL
        },
        data(){
            return{
                form:{
                    name:"",
                    email:"",
                    gender:""
                },
                genders:[{text: 'Select One', value: null}, 'Male', 'Female', 'Non-binary'],
                show: true
            }
        },
        methods:{
            Log(){
                // let url = process.env.LoginUrl + "/users";
                let url = "http://login.hpds/users";
                console.log(process.env.LoginUrl);
                console.log(process.env.HPDS_LOGIN_URL);
                console.log(url);
                let result = axios.get(url + "/" + this.form.name)
                .then((result) => {
                    alert("Username occupied");
                })
                .catch((error) =>
                {
                    var result2 = axios.post(url, {
                        Username: this.form.name,
                        Email:this.form.email,
                        Gender:this.form.gender
                    })
                .then((result2) =>
                {
                    this.$router.push('/ChatPage');
                })
                .catch((error2) =>{
                    alert(error2);
                })
                })

            }
        }

    }
</script>
